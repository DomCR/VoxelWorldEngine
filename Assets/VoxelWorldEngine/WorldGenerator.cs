using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.NoiseVariations;

namespace VoxelWorldEngine
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject ChunkPrefab;

        [Space()]
        public int ChunksX;
        public int ChunksY;
        public int ChunksZ;

        public static int s_ChunksX;
        public static int s_ChunksY;
        public static int s_ChunksZ;

        [Space()]
        public TextureMode TextureMode;

        public static TextureMode s_textureMode;

        [Space()]
        //Noise setup
        [Range(0, 999f)]
        [Tooltip("WaveLength of Height Map Noise")]
        public float HeightScale = 125;
        [Range(0, 999f)]
        public float HeightMagnitude = 200;
        [Space()]
        [Range(0, 999f)]
        public float NoiseScale = 25;
        [Range(0, 1.0f)]
        public float Threshold = 0.45f;

        //Public static noise variables
        public static float s_HeightScale;
        public static float s_HeightMagnitude;
        public static float s_NoiseScale;
        public static float s_Threshold;

        private static Dictionary<Vector3, Chunk> m_chunks;

        // Start is called before the first frame update
        void Start()
        {
            s_ChunksX = ChunksX;
            s_ChunksY = ChunksY;
            s_ChunksZ = ChunksZ;

            s_textureMode = TextureMode;

            s_HeightScale = HeightScale;
            s_HeightMagnitude = HeightMagnitude;
            s_NoiseScale = NoiseScale;
            s_Threshold = Threshold;

            //Initialize variables
            m_chunks = new Dictionary<Vector3, Chunk>();

            //Generate the world
            GenerateWorld();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //****************************************************************
        /// <summary>
        /// Create the world chunks 
        /// </summary>
        void GenerateWorld()
        {
            for (int x = 0; x < ChunksX; x++)
            {
                for (int y = 0; y < ChunksY; y++)
                {
                    for (int z = 0; z < ChunksZ; z++)
                    {
                        Vector3 pos = new Vector3(x * Chunk.XSize, y * Chunk.YSize, z * Chunk.ZSize);
                        GameObject tmp_chunk = Instantiate(ChunkPrefab, pos, new Quaternion(0, 0, 0, 0), this.transform);
                        m_chunks.Add(pos, tmp_chunk.GetComponent<Chunk>());
                    }
                }
            }
        }
        //****************************************************************
        /// <summary>
        /// Get the block in a designated coordinates
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static BlockType GetBlock(Vector3 pos, int x, int y, int z)
        {
            Chunk chunk = null;

            //Find in which chunk is the point
            Vector3 chunk_key = new Vector3(
                pos.x + checkLimits((int)(x + pos.x), (int)pos.x, Chunk.XSize),
                pos.y + checkLimits((int)(y + pos.y), (int)pos.y, Chunk.YSize),
                pos.z + checkLimits((int)(z + pos.z), (int)pos.z, Chunk.ZSize));

            //If the chunk doesn't exist, return true (AIR)
            m_chunks.TryGetValue(chunk_key, out chunk);

            //TODO: fix the coordinate of the block
            throw new NotImplementedException();
            //return chunk.Blocks[x,y,z];
        }
        /// <summary>
        /// Get the block of the current world initialization
        /// Once the world is generated use GetBlock()
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static BlockType GetWorldBlock(float x, float y, float z)
        {
            return GetWorldBlock(new Vector3(x, y, z));
        }
        public static BlockType GetWorldBlock(Vector3 pos)
        {
            //Check edge
            if (pos.z < 0 || pos.y < 0 || pos.x < 0 ||
                (int)pos.x >= s_ChunksX * Chunk.XSize ||
                (int)pos.y >= s_ChunksY * Chunk.YSize ||
                (int)pos.z >= s_ChunksZ * Chunk.ZSize)
                return BlockType.NULL;

            //Set the bottom edge blocks 
            int height_endBlock = (int)(Mathf.PerlinNoise(pos.x, pos.y));
            if (pos.y < 3)
                return BlockType.STONE;

            //Get the height map
            int height = (int)(Mathf.PerlinNoise(
                (int)pos.x / s_HeightScale,
                (int)pos.z / s_HeightScale) * s_HeightMagnitude);

            if ((int)pos.y > height)
                return BlockType.NULL;

            //Generate a 3D noise to create, holes and irregularities in the terrain
            if (PerlinNoise3D.Generate_01(
                (int)pos.x / s_NoiseScale,
                (int)pos.y / s_NoiseScale,
                (int)pos.z / s_NoiseScale) >= s_Threshold)
                return BlockType.GRASS_TOP;
            else
                return BlockType.NULL;
        }
        //****************************************************************
        private static int checkLimits(int value, int axisPos, int limit)
        {
            int max = axisPos + limit;

            if (value < axisPos)
            {
                return -1;
            }
            if (value > max - 1)
            {
                return 1;
            }

            return 0;
        }

    }
}