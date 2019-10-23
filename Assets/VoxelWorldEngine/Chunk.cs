using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine
{
    public class Chunk : MonoBehaviour
    {
        public EBlock[,,] Blocks;
        public const int XSize = 16;
        public const int YSize = 256;
        public const int ZSize = 16;
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
        void Start()
        {
            //Initialize the block array
            Blocks = new EBlock[16,256,16];

            //Get the gameobject components
            m_mesh = this.GetComponent<MeshFilter>().mesh;
            m_collider = this.GetComponent<MeshCollider>();

            //Generate the current chunk
            //GenerateChunk();
            //GenerateMesh();
            //UpdateMesh();

            StartCoroutine(CreateChunk());
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
                    for (int y = 0; y < YSize; y++)
                    {
                        //Apply Noise...
                        if(y == 1)
                        {
                            Blocks[x, y, z] = EBlock.SOLID;
                        }
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
                        //Guard: blocks to ignore
                        if (Blocks[x, y, z] == EBlock.NULL)
                            continue;

                        //Set the visible faces
                        if (WorldGenerator.CheckSurroundings(this.transform.position, x + 1, y, z))
                        {
                            Block.EastFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        if (WorldGenerator.CheckSurroundings(this.transform.position, x, y + 1, z))
                        {
                            Block.TopFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        if (WorldGenerator.CheckSurroundings(this.transform.position, x, y, z + 1))
                        {
                            Block.NorthFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        if (WorldGenerator.CheckSurroundings(this.transform.position, x - 1, y, z))
                        {
                            Block.WestFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        if (WorldGenerator.CheckSurroundings(this.transform.position, x, y - 1, z))
                        {
                            Block.BottomFace(x, y, z, m_vertices, m_triangles, m_faceCount);
                            m_faceCount++;
                        }
                        if (WorldGenerator.CheckSurroundings(this.transform.position, x, y, z - 1))
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
            GenerateChunk();
            GenerateMesh();
            UpdateMesh();

            yield return null;
        }
    }
}
