using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise;

namespace VoxelWorldEngine
{
    public class Chunk : MonoBehaviour
    {
        public BlockType[,,] Blocks;
        public const int XSize = 16;
        public const int YSize = 256;
        public const int ZSize = 16;

        public ChunkState State = ChunkState.HeightMapGeneration;

        public GameObject[] WorldObjects;

        public Vector3 Position { get; private set; }
        //*************************************************************
        private List<Vector3> m_mesh_vertices = new List<Vector3>();
        private List<Vector3> m_collider_vertices = new List<Vector3>();
        private List<int> m_mesh_triangles = new List<int>();
        private List<int> m_collider_triangles = new List<int>();
        private List<Color> m_colors = new List<Color>();
        private List<Vector2> m_uv = new List<Vector2>();
        private int m_faceCount = 0;

        private Mesh m_mesh;
        private MeshCollider m_collider;

        //TODO: implement the world class
        private WorldGenerator m_parent;

        //Thread variables
        Thread m_chunkThread;
        //TODO: implement thread pool to control the pc resources
        private static List<Thread> m_threadPool = new List<Thread>();
        private const int k_threadLimit = 8;
        //*************************************************************
        #region Behaviour methods
        void Awake()
        {
            //Initialize the block array
            Blocks = new BlockType[XSize, YSize, ZSize];

            //Get the gameobject components
            m_mesh = this.GetComponent<MeshFilter>().mesh;
            m_collider = this.GetComponent<MeshCollider>();

            //Get the parent (world)
            m_parent = this.GetComponentInParent<WorldGenerator>();

            #region Debug utils
            if (m_parent.debug.IsActive)
                State = m_parent.debug.ChunkState;
            else
                State = ChunkState.HeightMapGeneration;
            #endregion

            //THREAD TEST: Generate the current chunk
            #region Thread variable ref
            Position = transform.position;
            #endregion
        }
        void Update()
        {
            switch (State)
            {
                //No action taken states
                case ChunkState.Idle:
                case ChunkState.Updating:
                    break;
                case ChunkState.HeightMapGeneration:
                    m_chunkThread = new Thread(new ThreadStart(GenerateHeightMap));
                    //m_chunkThread = new Thread(new ThreadStart(GenerateHeightMap));
                    //m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.CaveGeneration:
                    m_chunkThread = new Thread(new ThreadStart(GenerateHoles));
                    //m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.NeedFaceUpdate:
                    m_chunkThread = new Thread(new ThreadStart(UpdateFaces));
                    //m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.NeedMeshUpdate:
                    UpdateMesh();
                    break;
                case ChunkState.OreGeneration:
                    break;
                case ChunkState.StrataGeneration:
                    m_chunkThread = new Thread(new ThreadStart(GenerateStrata));
                    //m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.DensityMapGeneration:
                    m_chunkThread = new Thread(new ThreadStart(GenerateDensityMap));
                    //m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.VegetationGeneration:
                    //m_chunkThread = new Thread(new ThreadStart(GenerateVegetation));
                    //m_threadPool.Add(m_chunkThread);
                    //m_chunkThread.Start();
                    GenerateVegetation();
                    break;
                default:
                    break;
            }

            #region WIP: Thread Management
            ////Set the active threads limit
            //int tactive;
            //if ((tactive = m_threadPool.Where(o => o.IsAlive).Count()) < k_threadLimit)
            //    foreach (Thread th in m_threadPool)
            //    {
            //        if (tactive < k_threadLimit)
            //        {
            //            switch (th.ThreadState)
            //            {
            //                case System.Threading.ThreadState.Aborted:
            //                    break;
            //                case System.Threading.ThreadState.AbortRequested:
            //                    break;
            //                case System.Threading.ThreadState.Background:
            //                    break;
            //                case System.Threading.ThreadState.Running:
            //                    break;
            //                case System.Threading.ThreadState.Stopped:
            //                    break;
            //                case System.Threading.ThreadState.StopRequested:
            //                    break;
            //                case System.Threading.ThreadState.Suspended:
            //                    break;
            //                case System.Threading.ThreadState.SuspendRequested:
            //                    break;
            //                case System.Threading.ThreadState.Unstarted:
            //                    th.Start();
            //                    tactive++;
            //                    break;
            //                case System.Threading.ThreadState.WaitSleepJoin:
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //    } 
            #endregion
        }
        #endregion
        //*************************************************************
        /// <summary>
        /// Generate the chunk mesh
        /// </summary>
        void GenerateHeightMap()
        {
            //Update the chunk state
            State = ChunkState.Updating;

            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    for (int z = 0; z < ZSize; z++)
                    {
                        Vector3 currBlockPos = new Vector3(
                            x + Position.x,
                            y + Position.y,
                            z + Position.z);

                        Blocks[x, y, z] = m_parent.ComputeHeightNoise(currBlockPos);
                    }
                }
            }

            //Update the chunk state
            State = ChunkState.NeedFaceUpdate;
        }
        /// <summary>
        /// Generate the strata of the terrain (superficial dirt and grass)
        /// </summary>
        /// <remarks>
        /// WIP: by now only generates 1 block of grass at the top
        /// </remarks>
        void GenerateStrata()
        {
            //Update the chunk state
            State = ChunkState.Updating;

            for (int x = 0; x < XSize; x++)
            {
                for (int z = 0; z < ZSize; z++)
                {
                    bool grassOnTop = false;

                    for (int y = YSize - 1; y >= 0; y--)
                    {
                        //Guard: not the top block
                        if (Blocks[x, y, z] == BlockType.NULL)
                            continue;

                        //Blocks[x, y, z] = BlockType.GRASS;

                        Vector3 currBlockPos = new Vector3(
                            x + Position.x,
                            y + Position.y,
                            z + Position.z);

                        BlockType[] arr = m_parent.ComputeStrataNoise(currBlockPos);


                        for (int i = arr.Length - 1; i > 0; i--)
                        {
                            if (arr[i] != BlockType.NULL)
                                Blocks[x, i, z] = arr[i];
                        }

                        break;
                    }
                }
            }

            //Update the chunk state
            State = ChunkState.NeedFaceUpdate;
        }
        void GenerateDensityMap()
        {
            //Update the chunk state
            State = ChunkState.Updating;

            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    for (int z = 0; z < ZSize; z++)
                    {
                        //Vector3 currBlockPos = new Vector3(
                        //    x + this.transform.position.x,
                        //    y + this.transform.position.y,
                        //    z + this.transform.position.z); 
                        Vector3 currBlockPos = new Vector3(
                            x + Position.x,
                            y + Position.y,
                            z + Position.z);

                        //TODO: optimize the process by sending a column of the array
                        BlockType bl = Blocks[x, y, z] = m_parent.ComputeDensityNoise(currBlockPos);
                    }
                }
            }

            //Update the chunk state
            State = ChunkState.NeedFaceUpdate;
        }
        void GenerateHoles()
        {
            //Update the chunk state
            State = ChunkState.Updating;

            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    for (int z = 0; z < ZSize; z++)
                    {
                        //Vector3 currBlockPos = new Vector3(
                        //    x + this.transform.position.x,
                        //    y + this.transform.position.y,
                        //    z + this.transform.position.z); 
                        Vector3 currBlockPos = new Vector3(
                            x + Position.x,
                            y + Position.y,
                            z + Position.z);

                    }
                }
            }

            //Update the chunk state
            State = ChunkState.NeedFaceUpdate;
        }
        /// <summary>
        /// Spawn vegetation in the game
        /// </summary>
        /// <remarks>
        /// Spawning an object for each plant is not optim enough, consider to implement 2 vertex array
        /// 1 for the mesh and 1 for the collider, and ignore the plants in the collider
        /// Block class should be able to set the texture and the plant shape
        /// </remarks>
        void GenerateVegetation()
        {
            State = ChunkState.Updating;

            for (int x = 0; x < XSize; x++)
            {
                for (int z = 0; z < ZSize; z++)
                {
                    System.Random rand = new System.Random();
                    if (0.5 <= Mathf.PerlinNoise((x + Position.x) / 10, (z + Position.z) / 10))
                        continue;

                    for (int y = YSize - 1; y > 0; y--)
                    {
                        if (Block.IsFertile(Blocks[x, y, z]))
                        {
                            //Blocks[x, y + 1, z] = BlockType.OAKTREE_LOG;
                            //Spawn grass
                            //GameObject.Instantiate(WorldObjects[0], 
                            //    new Vector3(x + Position.x + 0.5f,
                            //        y + Position.y,
                            //        z + Position.z + 0.5f),
                            //    new Quaternion());

                            //assign blocks

                            break;
                        }
                    }
                }
            }

            State = ChunkState.NeedFaceUpdate;
        }
        /// <summary>
        /// Update the vertices to generate the faces
        /// </summary>
        void UpdateFaces()
        {
            //Update the chunk state
            State = ChunkState.Updating;

            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    for (int z = 0; z < ZSize; z++)
                    {
                        Vector3 currBlockPos = new Vector3(
                            x + Position.x,
                            y + Position.y,
                            z + Position.z);

                        //Guard: blocks to ignore
                        if (Blocks[x, y, z] == BlockType.NULL)
                            continue;

                        //TODO: in case isn't a block (grass, spider web, flowers, mushrooms...)
                        //Implement an spawn method
                        if (Block.NotBlock(Blocks[x, y, z]))
                        {
                            //Check boundaries only need 1 free side to be render
                            if (Block.IsTransparent(m_parent.GetBlock(currBlockPos.x + 1, currBlockPos.y, currBlockPos.z))
                               && Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y + 1, currBlockPos.z))
                               && Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z + 1))
                               && Block.IsTransparent(m_parent.GetBlock(currBlockPos.x - 1, currBlockPos.y, currBlockPos.z))
                               && Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y - 1, currBlockPos.z))
                               && Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z - 1)))
                            {
                                //Spawn the element and add the vertices to the mesh
                                addNotBlockToMesh(x, y, z);
                            }
                        }

                        #region Check block sides
                        //Set the visible faces
                        //TODO: Finish cleaning the code
                        if (Block.IsTransparent(m_parent.GetBlock(currBlockPos.x + 1, currBlockPos.y, currBlockPos.z)))
                        {
                            //Block.EastFace(x, y, z, m_mesh_vertices, m_mesh_triangles, m_faceCount);

                            ////Add the vertices to the mesh and the collider
                            //if(Block.IsSolid(Blocks[x, y, z]))
                            //{
                            //    //Add the vertices to the collider
                            //}
                            ////m_vertices.AddRange();

                            //setFaceTexture(x, y, z, FaceType.East);
                            //m_faceCount++;
                            addBlockToMesh(x, y, z, FaceType.East);
                        }
                        if (Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y + 1, currBlockPos.z)))
                        {
                            //Block.TopFace(x, y, z, m_mesh_vertices, m_mesh_triangles, m_faceCount);
                            //setFaceTexture(x, y, z, FaceType.Top);
                            //m_faceCount++;
                            addBlockToMesh(x, y, z, FaceType.Top);
                        }
                        if (Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z + 1)))
                        {
                            //Block.NorthFace(x, y, z, m_mesh_vertices, m_mesh_triangles, m_faceCount);
                            //setFaceTexture(x, y, z, FaceType.North);
                            //m_faceCount++;
                            addBlockToMesh(x, y, z, FaceType.North);
                        }
                        if (Block.IsTransparent(m_parent.GetBlock(currBlockPos.x - 1, currBlockPos.y, currBlockPos.z)))
                        {
                            //Block.WestFace(x, y, z, m_mesh_vertices, m_mesh_triangles, m_faceCount);
                            //setFaceTexture(x, y, z, FaceType.West);
                            //m_faceCount++;
                            addBlockToMesh(x, y, z, FaceType.West);
                        }
                        if (Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y - 1, currBlockPos.z)))
                        {
                            //Block.BottomFace(x, y, z, m_mesh_vertices, m_mesh_triangles, m_faceCount);
                            //setFaceTexture(x, y, z, FaceType.Bottom);
                            //m_faceCount++;
                            addBlockToMesh(x, y, z, FaceType.Bottom);
                        }
                        if (Block.IsTransparent(m_parent.GetBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z - 1)))
                        {
                            //Block.SouthFace(x, y, z, m_mesh_vertices, m_mesh_triangles, m_faceCount);
                            //setFaceTexture(x, y, z, FaceType.South);
                            //m_faceCount++;
                            addBlockToMesh(x, y, z, FaceType.South);
                        }
                        #endregion
                    }
                }
            }

            //Update the chunk state
            State = ChunkState.NeedMeshUpdate;
        }
        /// <summary>
        /// Update the mesh parameters, vertices, triangles and uv
        /// </summary>
        void UpdateMesh()
        {
            //Reset mesh
            m_mesh.Clear();
            m_mesh.vertices = m_mesh_vertices.ToArray();
            m_mesh.uv = m_uv.ToArray();
            m_mesh.triangles = m_mesh_triangles.ToArray();
            m_mesh.RecalculateNormals();
            m_mesh.Optimize();

            //Setup the collider
            //m_collider.sharedMesh = m_mesh;

            m_collider.sharedMesh = new Mesh();
            m_collider.sharedMesh.vertices = m_collider_vertices.ToArray();
            m_collider.sharedMesh.triangles = m_collider_triangles.ToArray();

            //Clear the memory
            m_mesh_vertices.Clear();
            m_uv.Clear();
            m_mesh_triangles.Clear();
            m_colors.Clear();
            m_faceCount = 0;

            State = ChunkState.Idle;
        }
        //*************************************************************
        private void addNotBlockToMesh(int x, int y, int z)
        {
            throw new NotImplementedException();
        }
        private void addBlockToMesh(int x, int y, int z, FaceType face)
        {

            Block.GetFace(x, y, z, out List<Vector3> tmpVert, out List<int> tmpTri, m_faceCount, face);

            //Add the vertices and triangles to the mesh and the collider
            if (Block.IsSolid(Blocks[x, y, z]))
            {
                //Add the vertices to the collider
                m_collider_vertices.AddRange(tmpVert);
                Block.AddTriangles(m_collider_triangles, (m_collider_vertices.Count / 4) - 1);
            }
            //render mesh vertex
            m_mesh_vertices.AddRange(tmpVert);
            m_mesh_triangles.AddRange(tmpTri);

            setFaceTexture(x, y, z, face);
            m_faceCount++;
        }
        private void setFaceTexture(int x, int y, int z, FaceType face)
        {
            BlockTextureMap map = (BlockTextureMap)Blocks[x, y, z];

            //Set the block face in case that is different from the side
            switch (Blocks[x, y, z])
            {
                case BlockType.GRASS:
                    switch (face)
                    {
                        case FaceType.Top:
                            map = BlockTextureMap.GRASS_TOP;
                            break;
                        case FaceType.North:
                        case FaceType.East:
                        case FaceType.South:
                        case FaceType.West:
                            map = BlockTextureMap.GRASS_SIDE;
                            break;
                        case FaceType.Bottom:
                            map = BlockTextureMap.DIRT;
                            break;
                        default:
                            break;
                    }
                    break;
                case BlockType.TNT:
                    break;
                case BlockType.OAKTREE_LOG:
                    switch (face)
                    {
                        case FaceType.North:
                        case FaceType.East:
                        case FaceType.South:
                        case FaceType.West:
                            map = BlockTextureMap.OAKTREE_SIDE;
                            break;
                        case FaceType.Top:
                        case FaceType.Bottom:
                            map = BlockTextureMap.OAKTREE_TOP;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            m_uv.AddRange(Block.GetTexture(map));
        }
    }
}
