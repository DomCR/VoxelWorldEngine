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

        public Vector3 Position { get; private set; }
        //*************************************************************
        private List<Vector3> m_vertices = new List<Vector3>();
        private List<int> m_triangles = new List<int>();
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
                State = m_parent.debug.InitialState;
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
                    m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.CaveGeneration:
                    m_chunkThread = new Thread(new ThreadStart(GenerateHoles));
                    m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.NeedFaceUpdate:
                    m_chunkThread = new Thread(new ThreadStart(UpdateFaces));
                    m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.NeedMeshUpdate:
                    UpdateMesh();
                    break;
                case ChunkState.OreGeneration:
                    break;
                case ChunkState.StrataGeneration:
                    m_chunkThread = new Thread(new ThreadStart(GenerateStrata));
                    m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
                    break;
                case ChunkState.DensityMapGeneration:
                    m_chunkThread = new Thread(new ThreadStart(GenerateDensityMap));
                    m_threadPool.Add(m_chunkThread);
                    m_chunkThread.Start();
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

                        #region Face generation
                        //Set the visible faces
                        if (m_parent.GetBlock(currBlockPos.x + 1, currBlockPos.y, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.EastFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            setFaceTexture(x, y, z, FaceType.East);
                            m_faceCount++;
                        }
                        if (m_parent.GetBlock(currBlockPos.x, currBlockPos.y + 1, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.TopFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            setFaceTexture(x, y, z, FaceType.Top);
                            m_faceCount++;
                        }
                        if (m_parent.GetBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z + 1) == BlockType.NULL)
                        {
                            Block.NorthFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            setFaceTexture(x, y, z, FaceType.North);
                            m_faceCount++;
                        }
                        if (m_parent.GetBlock(currBlockPos.x - 1, currBlockPos.y, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.WestFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            setFaceTexture(x, y, z, FaceType.West);
                            m_faceCount++;
                        }
                        if (m_parent.GetBlock(currBlockPos.x, currBlockPos.y - 1, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.BottomFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            setFaceTexture(x, y, z, FaceType.Bottom);
                            m_faceCount++;
                        }
                        if (m_parent.GetBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z - 1) == BlockType.NULL)
                        {
                            Block.SouthFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            setFaceTexture(x, y, z, FaceType.South);
                            m_faceCount++;
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
            m_mesh.vertices = m_vertices.ToArray();
            m_mesh.uv = m_uv.ToArray();
            m_mesh.triangles = m_triangles.ToArray();
            m_mesh.RecalculateNormals();
            m_mesh.Optimize();

            //Setup the collider
            m_collider.sharedMesh = m_mesh;

            //Clear the memory
            m_vertices.Clear();
            m_uv.Clear();
            m_triangles.Clear();
            m_colors.Clear();
            m_faceCount = 0;

            State = ChunkState.Idle;
        }
        //*************************************************************
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
                    break;
                default:
                    break;
            }

            //m_uv.AddRange(Block.GetTexture(Blocks[x, y, z]));
            m_uv.AddRange(Block.GetTexture(map));
        }
    }
}
