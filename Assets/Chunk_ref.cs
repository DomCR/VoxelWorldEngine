using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace World.Elements
{
    public class Chunk : MonoBehaviour
    {
        //**************************************
        //*********** PUBLIC VARIABLES *********
        //**************************************
        public static BLOCK_ID[,,] ChunckData;

        public int PosX = 0;
        public int PosY = 0;
        public int PosZ = 0;

        public int DimX = 1;
        public int DimY = 1;
        public int DimZ = 1;

        //**************************************
        //********* PRIVATE VARIABLES **********
        //**************************************
        //TODO: Transform to dictionary
        private List<Block> Blocks = new List<Block>(); 
        private List<Vector3> Vertices = new List<Vector3>();
        private List<int> Triangles = new List<int>();
        private List<Color> Colors = new List<Color>();
        private List<Vector2> UV = new List<Vector2>();

        private Mesh mesh;
        private int faceCount;

        private MeshCollider chunkCollider;

        // Use this for initialization
        void Start()
        {
            ChunckData = new BLOCK_ID[DimX, DimY, DimZ];

            //Mesh
            mesh = GetComponent<MeshFilter>().mesh;

            //Collider
            chunkCollider = GetComponent<MeshCollider>();

            faceCount = 0;

            PlaceBlocks();

            UpdateMesh();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void PlaceBlocks()
        {
            //Generate world data
            for (int x = 0; x < DimX; x++)
            {
                for (int z = 0; z < DimZ; z++)
                {
                    for (int y = 0; y < DimY; y++)
                    {
                        if (WorldGenerator.WorldData[x + PosX, y + PosY, z + PosZ] > BLOCK_ID.TO_IGNORE)
                        {
                            Block tmpBlock = new Block(WorldGenerator.WorldData[x + PosX, y + PosY, z + PosZ], new Vector3(x, y, z));

                            #region FACE GENERATION
                            //Top face
                            if (CheckSurround(x, y + 1, z) < BLOCK_ID.TO_IGNORE)
                            {
                                tmpBlock.TopFace(y, z, ref faceCount);
                            }
                            //Bot face
                            if (CheckSurround(x, y - 1, z)  < BLOCK_ID.TO_IGNORE)
                            {
                                tmpBlock.BotFace(x, y, z, ref faceCount);
                            }
                            //North Face
                            if (CheckSurround(x, y, z + 1)  < BLOCK_ID.TO_IGNORE)
                            {
                                tmpBlock.NorthFace(y, z, ref faceCount);
                            }
                            //South face
                            if (CheckSurround(x, y, z - 1)  < BLOCK_ID.TO_IGNORE)
                            {
                                tmpBlock.SouthFace(x, y, z, ref faceCount);
                            }
                            //East Face
                            if (CheckSurround(x + 1, y, z)  < BLOCK_ID.TO_IGNORE)
                            {
                                tmpBlock.EastFace(x, y, z, ref faceCount);
                            }
                            //West Face
                            if (CheckSurround(x - 1, y, z)  < BLOCK_ID.TO_IGNORE)
                            {
                                tmpBlock.WestFace(x, y, z, ref faceCount);
                            }
                            #endregion

                            Blocks.Add(tmpBlock);
                        }
                    }
                }
            }
        }

        BLOCK_ID CheckSurround(int x, int y, int z)
        {
            if (x + PosX >= WorldGenerator.WorldData.GetLength(0) || x + PosX < 0
                || y + PosY >= WorldGenerator.WorldData.GetLength(1) || y + PosY < 0
                || z + PosZ >= WorldGenerator.WorldData.GetLength(2) || z + PosZ < 0)
            {
                return BLOCK_ID.NULL;
            }
            return WorldGenerator.WorldData[x + PosX, y + PosY, z + PosZ];
        }

        void UpdateMesh()
        {
            //Reset mesh
            mesh.Clear();

            //Put the block vecrtices and triangles into the chunck list
            foreach (Block block in Blocks)
            {
                Vertices.AddRange(block.Vertices);
                Triangles.AddRange(block.Triangles);

                UV.AddRange(block.UV);
                Colors.AddRange(block.Colors());
            }

            mesh.vertices = Vertices.ToArray();
            mesh.uv = UV.ToArray();   //NOT NEED WITH COLORS
            mesh.triangles = Triangles.ToArray();
            mesh.RecalculateNormals();

            mesh.colors = Colors.ToArray();

            //Set the mesh for the collider
            chunkCollider.sharedMesh = mesh;

            //Clear list and facecount
            Triangles.Clear();
            Vertices.Clear();
            Colors.Clear();
            UV.Clear();
            faceCount = 0;
        }

        //****************************************
        //************ EXTERNAL MODIFIERS ********
        //****************************************

        public void UpdateChunk()
        {
            Blocks.Clear();

            PlaceBlocks();
            UpdateMesh();
        }
    }
}