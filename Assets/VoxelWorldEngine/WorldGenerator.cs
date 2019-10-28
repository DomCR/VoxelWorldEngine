using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise;

namespace VoxelWorldEngine
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject ChunkPrefab;

        [Space()]
        public int ChunksX;
        public int ChunksY;
        public int ChunksZ;

        [Space()]
        public TextureMode TextureMode;

        [Space()]
        //Noise setup
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float WidthMagnitude = 125;
        [Range(0, 999f)]
        [Tooltip("Wave height of the noise")]
        public float HeightMagnitude = 200;
        [Space()]
        [Range(0, 999f)]
        [Tooltip("Global 3d noise scale")]
        public float NoiseScale = 25;
        [Range(0, 1.0f)]
        public float Density = 0.45f;

        [SerializeField]
        public Noise.Noise[] noises;
        public Noise.NoiseLayer[] layerNoises;

        private Dictionary<Vector3, Chunk> m_chunks;

        // Start is called before the first frame update
        void Start()
        {
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
        public BlockType GetBlock(Vector3 pos, int x, int y, int z)
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
        public BlockType GetWorldBlock(float x, float y, float z)
        {
            return GetWorldBlock(new Vector3(x, y, z));
        }
        /// <summary>
        /// Get the block of the current world initialization
        /// Once the world is generated use GetBlock()
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public BlockType GetWorldBlock(Vector3 pos)
        {
            //Check edges
            if (pos.z < 0 || pos.y < 0 || pos.x < 0 ||
                (int)pos.x >= ChunksX * Chunk.XSize ||
                (int)pos.y >= ChunksY * Chunk.YSize ||
                (int)pos.z >= ChunksZ * Chunk.ZSize)
                return BlockType.NULL;

            //Set the bottom edge blocks 
            int height_endBlock = (int)(Mathf.PerlinNoise(
                pos.x / WidthMagnitude * 50f,
                pos.z / WidthMagnitude * 50f) * 10);
            if (pos.y < height_endBlock)
                return BlockType.STONE;

            #region Height layer (example)
            //Get the height map
            int height = (int)(Mathf.PerlinNoise(
                (int)pos.x / WidthMagnitude,
                (int)pos.z / WidthMagnitude) * HeightMagnitude);

            if ((int)pos.y > height)
                return BlockType.NULL; 


            #endregion

            //Generate a 3D noise to create, holes and irregularities in the terrain
            if (PerlinNoise3D.Generate_01(
                (int)pos.x / NoiseScale,
                (int)pos.y / NoiseScale,
                (int)pos.z / NoiseScale) >= Density)
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