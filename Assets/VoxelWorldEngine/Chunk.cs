using System;
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
        public BLOCK[,,] Blocks;
        public const int XSize = 16;
        public const int YSize = 256;
        public const int ZSize = 16;
        //*************************************************************
        private List<Vector3> newVertices = new List<Vector3>();
        private List<int> newTriangles = new List<int>();
        private int faceCount = 0;
        //*************************************************************
        #region Behaviour methods
        void Start()
        {
            //Initialize the block array
            Blocks = new BLOCK[16,256,16];

            //Generate the current chunk
            GenerateChunk();
            GenerateMesh();
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
                for (int y = 0; y < YSize; y++)
                {
                    for (int z = 0; z < ZSize; z++)
                    {
                        //Set the faces that aren't hidden
                        if (CheckSurroundings(x + 1, y, z))
                        {
                            Block.EastFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y + 1, z))
                        {
                            Block.TopFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y, z + 1))
                        {
                            Block.NorthFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x - 1, y, z))
                        {
                            Block.WestFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y - 1, z))
                        {
                            Block.BotFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y, z - 1))
                        {
                            Block.SouthFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                    }
                }
            }
        }

        private bool CheckSurroundings(int x, int y, int v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate the chunk mesh and textures
        /// </summary>
        void GenerateMesh()
        {
            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    for (int z = 0; z < ZSize; z++)
                    {
                        //Set the faces that aren't hidden
                        if (CheckSurroundings(x + 1, y, z))
                        {
                            Block.EastFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y + 1, z))
                        {
                            Block.TopFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y, z + 1))
                        {
                            Block.NorthFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x - 1, y, z))
                        {
                            Block.WestFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y - 1, z))
                        {
                            Block.BotFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                        if (CheckSurroundings(x, y, z - 1))
                        {
                            Block.SouthFace(x, y, z, newVertices, newTriangles, faceCount);
                            faceCount++;
                        }
                    }
                }
            }
        }
    }
}
