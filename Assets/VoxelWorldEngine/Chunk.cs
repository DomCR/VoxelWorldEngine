using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.NoiseVariations;

namespace VoxelWorldEngine
{
    public class Chunk : MonoBehaviour
    {
        public BlockType[,,] Blocks;
        public const int XSize = 16;
        public const int YSize = 256;
        public const int ZSize = 16;

        [Range(0, 999f)]
        [Tooltip("WaveLength of Height Map Noise")]
        public float HeightScale;
        [Range(0, 999f)]
        public float HeightMagnitude;
        [Space()]
        [Range(0, 999f)]
        public float NoiseScale;
        [Range(0, 1.0f)]
        public float Threshold = 0.5f;
        //*************************************************************
        private List<Vector3> m_vertices = new List<Vector3>();
        private List<int> m_triangles = new List<int>();
        private List<Color> m_colors = new List<Color>();
        private List<Vector2> m_uv = new List<Vector2>();
        private int m_faceCount = 0;

        private Mesh m_mesh;
        private MeshCollider m_collider;

        //*************************************************************
        #region Behaviour methods
        void Awake()
        {
            //Get the nois variables from world generator (should be from the biome)
            HeightScale = WorldGenerator.s_HeightScale;
            HeightMagnitude = WorldGenerator.s_HeightMagnitude;
            NoiseScale = WorldGenerator.s_NoiseScale;
            Threshold = WorldGenerator.s_Threshold;

            //Initialize the block array
            Blocks = new BlockType[16, 256, 16];

            //Get the gameobject components
            m_mesh = this.GetComponent<MeshFilter>().mesh;
            m_collider = this.GetComponent<MeshCollider>();

            //Generate the current chunk
            //GenerateChunk();
            GenerateMesh();
            UpdateMesh();

            //StartCoroutine(CreateChunk());
        }
        void Update()
        {

        }
        #endregion
        //*************************************************************
        /// <summary>
        /// Generate the chunk data, applying the noise, biome and height factors
        /// </summary>
        void GenerateChunk()
        {
            for (int x = 0; x < XSize; x++)
            {
                for (int z = 0; z < ZSize; z++)
                {
                    //Get the height map
                    int height = (int)(Mathf.PerlinNoise(
                        (x + this.transform.position.x) / HeightScale,
                        (z + this.transform.position.z) / HeightScale) * HeightMagnitude);

                    for (int y = 0; y < height; y++)
                    {
                        //Generate a 3D noise to create, holes and irregularities in the terrain
                        if (PerlinNoise3D.Generate_01(
                            (x + this.transform.position.x) / NoiseScale,
                            (y + this.transform.position.y) / NoiseScale,
                            (z + this.transform.position.z) / NoiseScale) >= Threshold)
                            Blocks[x, y, z] = BlockType.SOLID;
                        else
                            Blocks[x, y, z] = BlockType.NULL;
                    }
                }
            }
        }
        /// <summary>
        /// Generate the chunk mesh
        /// </summary>
        void GenerateMesh()
        {
            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    for (int z = 0; z < ZSize; z++)
                    {
                        Vector3 currBlockPos = new Vector3(
                            x + this.transform.position.x,
                            y + this.transform.position.y,
                            z + this.transform.position.z);

                        Blocks[x, y, z] = WorldGenerator.GetWorldBlock(currBlockPos);

                        //Guard: blocks to ignore
                        if (Blocks[x, y, z] == BlockType.NULL)
                            continue;

                        //Set the visible faces
                        //if (WorldGenerator.CheckSurroundings(this.transform.position, x + 1, y, z))
                        if (WorldGenerator.GetWorldBlock(currBlockPos.x + 1, currBlockPos.y, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.EastFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        //if (WorldGenerator.CheckSurroundings(this.transform.position, x, y + 1, z))
                        if (WorldGenerator.GetWorldBlock(currBlockPos.x, currBlockPos.y + 1, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.TopFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        //if (WorldGenerator.CheckSurroundings(this.transform.position, x, y, z + 1))
                        if (WorldGenerator.GetWorldBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z + 1) == BlockType.NULL)
                        {
                            Block.NorthFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        //if (WorldGenerator.CheckSurroundings(this.transform.position, x - 1, y, z))
                        if (WorldGenerator.GetWorldBlock(currBlockPos.x - 1, currBlockPos.y, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.WestFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        //if (WorldGenerator.CheckSurroundings(this.transform.position, x, y - 1, z))
                        if (WorldGenerator.GetWorldBlock(currBlockPos.x, currBlockPos.y - 1, currBlockPos.z) == BlockType.NULL)
                        {
                            Block.BottomFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        //if (WorldGenerator.CheckSurroundings(this.transform.position, x, y, z - 1))
                        if (WorldGenerator.GetWorldBlock(currBlockPos.x, currBlockPos.y, currBlockPos.z - 1) == BlockType.NULL)
                        {
                            Block.SouthFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Update the mesh parameters, vertices, triangles and uv
        /// </summary>
        public void UpdateMesh()
        {
            //Reset mesh
            m_mesh.Clear();
            m_mesh.vertices = m_vertices.ToArray();
            m_mesh.uv = m_uv.ToArray();
            m_mesh.triangles = m_triangles.ToArray();
            m_mesh.RecalculateNormals();

            //set the textures
            if (true)
            {
                m_mesh.colors = m_colors.ToArray();
            }

            //Setup the collider
            m_collider.sharedMesh = m_mesh;

            //Clear the memory
            m_vertices.Clear();
            m_uv.Clear();
            m_triangles.Clear();
            m_colors.Clear();
            m_faceCount = 0;
        }
        //*************************************************************
        private IEnumerator CreateChunk()
        {
            //TODO:Do not use coroutines to create the chunks, use threads
            //The mesh and collider assaignments must be outside the trhead,
            //Not compatible with Unity API.

            //Optimization: join generate chunk and mesh, use method, getblock() where it returns the block
            //GenerateChunk();
            GenerateMesh();
            UpdateMesh();

            yield return null;
        }
    }
}
