using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Biomes;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Utils;
using VoxelWorldEngine.Noise;
using VoxelWorldEngine.Noise.RawNoise;

namespace VoxelWorldEngine
{
    [Serializable]
    public abstract class WorldGenerator : MonoBehaviour
    {
        public GameObject ChunkPrefab;

        public WGDebugOptions debug;

        [Space()]
        public int Seed;
        public bool UseSeed;

        [Space()]
        public int ChunksX;
        public int ChunksY;
        public int ChunksZ;

        [Space()]
        //Array of all the world biomes available
        public SerializedBiomeAttributes[] BiomeData;

        #region Goes to the biome
        [Header("Generic noise properties")]
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float WidthMagnitude = 125;
        [Range(0, 999f)]
        [Tooltip("Wave height of the noise")]
        public float HeightMagnitude = 200;
        #endregion

        [Tooltip("Minimum world height")]
        public int WorldHeight = 0;

        protected Vector3 m_position;
        protected Dictionary<Vector3, Chunk> m_chunks;
        protected BiomeAttributes[] m_worldBiomes;

        #region Behaviour methods
        // Start is called before the first frame update
        void Start()
        {
            //Initialize variables
            m_position = this.transform.position;
            m_chunks = new Dictionary<Vector3, Chunk>();

            //Feed the world biome attributes to be used outside the main thread
            m_worldBiomes = new BiomeAttributes[BiomeData.Length];
            for (int i = 0; i < BiomeData.Length; i++)
            {
                m_worldBiomes[i] = new BiomeAttributes(BiomeData[i]);
            }
            //Clear the none needed data
            Array.Clear(BiomeData, 0, BiomeData.Length);

            //Generate the world
            GenerateWorld();
        }
        // Update is called once per frame
        void Update()
        {
            if (debug.IsActive)
            {
                debugActions();
            }
        }
        #endregion
        //****************************************************************
        void debugActions()
        {
            if (debug.StateHasChanged())
            {
                foreach (Chunk chunk in m_chunks.Values)
                {
                    chunk.State = debug.ChunkState;
                }
            }
        }
        //****************************************************************
        /// <summary>
        /// Create the world chunks 
        /// </summary>
        /// <remarks>
        /// World generation steps:
        /// * Height map
        /// * Strata
        /// * Caves (holes by now)
        /// * Ores
        /// * Vegetation
        /// </remarks>
        void GenerateWorld()
        {
            for (int x = 0; x < ChunksX; x++)
            {
                for (int y = 0; y < ChunksY; y++)
                {
                    for (int z = 0; z < ChunksZ; z++)
                    {
                        //Vector3 pos = new Vector3(x * Chunk.XSize, y * Chunk.YSize, z * Chunk.ZSize);
                        Vector3 pos = new Vector3(x * Chunk.XSize, y * Chunk.YSize, z * Chunk.ZSize);
                        //Add the position of this world
                        pos += this.transform.position;

                        GameObject tmp_chunk = Instantiate(ChunkPrefab, pos, new Quaternion(0, 0, 0, 0), this.transform);
                        m_chunks.Add(pos, tmp_chunk.GetComponent<Chunk>());
                    }
                }
            }
        }
        /// <summary>
        /// Get the block in a designated coordinates
        /// </summary>
        /// <param name="pos">Chunk position</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public BlockType GetBlock(Vector3 pos, float x, float y, float z)
        {
            return GetBlock(pos.x + x, pos.y + y, pos.z + z);
        }
        public BlockType GetBlock(float x, float y, float z)
        {

            if (isWorldEdge(new Vector3(x, y, z)))
                return BlockType.NULL;

            Vector3 chunk_key = new Vector3(
                (int)(x / Chunk.XSize) * Chunk.XSize,
                (int)(y / Chunk.YSize) * Chunk.YSize,
                (int)(z / Chunk.ZSize) * Chunk.ZSize);

            //Get the chunk where the block is
            m_chunks.TryGetValue(chunk_key, out Chunk chunk);

            if (chunk != null)
                return chunk.Blocks[
                  (int)(x - chunk.Position.x),
                  (int)(y - chunk.Position.y),
                  (int)(z - chunk.Position.z)];
            else
                //There is no chunk, limit found
                return BlockType.NULL;
        }
        public BlockType ComputeHeightNoise(Vector3 pos)
        {
            BlockType block = BlockType.NULL;

            //Add the seed to the position
            Vector3 seedPos = new Vector3();

            if (UseSeed)
                seedPos = new Vector3(pos.x + Seed, pos.y, pos.z + Seed);
            else
                seedPos = pos;

            //Set the bottom edge blocks 
            block = setBedRock(seedPos);
            if (block == BlockType.BEDROCK)
                return block;

            //TODO: Calculate the biomes present in the current point
            //TODO: Solve the biome lerp by quantifing the biome presence
            float bio = Mathf.PerlinNoise(pos.x / 100, pos.z / 100);
            BiomeAttributes bioAtt = new BiomeAttributes();
            if (bio > 0.5f)
                bioAtt = m_worldBiomes[0];
            else
                bioAtt = m_worldBiomes[1];

            return HeightNoise(pos, bioAtt);

            //block = HeightNoise(seedPos);

            //return block;
        }
        public BlockType[] ComputeStrataNoise(Vector3 pos)
        {
            return StrataNoise(pos);
        }
        public BlockType ComputeDensityNoise(Vector3 pos)
        {
            return DensityNoise(pos);
        }
        //****************************************************************
        /// <summary>
        /// Set the bedrock at the limit of the world
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected BlockType setBedRock(Vector3 pos)
        {
            //Set the bottom edge blocks 
            int height_endBlock = (int)(Mathf.PerlinNoise(
                pos.x * 0.4f,
                pos.z * 0.4f) * 5) + 1;
            if (pos.y < height_endBlock)
                return BlockType.BEDROCK;
            else
                return BlockType.NULL;
        }
        protected bool isWorldEdge(Vector3 pos)
        {
            if (pos.x < m_position.x ||
                pos.y < m_position.y ||
                pos.z < m_position.z ||
                (int)pos.x >= ChunksX * Chunk.XSize + m_position.x ||
                (int)pos.y >= ChunksY * Chunk.YSize + m_position.y ||
                (int)pos.z >= ChunksZ * Chunk.ZSize + m_position.z)
                return true;

            return false;
        }
        //****************************************************************
        protected BlockType HeightNoise(Vector3 pos, BiomeAttributes attr)
        {
            NoiseMethod_delegate method = NoiseMap.NoiseMethods[(int)attr.NoiseType][attr.Dimensions - 1];
            float sample = (NoiseMap.Sum(method, pos / WidthMagnitude, attr.Frequency, attr.Octaves, attr.Lacunarity, attr.Persistence) + 1) / 2;

            //Apply the height magnitude
            //float h = Mathf.PerlinNoise(pos.x / (WidthMagnitude), pos.z / (WidthMagnitude)) * 200;
            //Debug.Log(h);
            //TODO: Control the height, cannot generate mountains without increase the depht
            sample *= HeightMagnitude;
            //sample *= h;

            //Set the minimum height of the world
            sample += WorldHeight;

            if (pos.y < WorldHeight)
                return BlockType.SAND;
            if (pos.y < sample)
                if (debug.IsActive)
                    return attr.DebugBlock;
                else
                    return BlockType.STONE;

            return BlockType.NULL;
        }
        //****************************************************************
        protected abstract BlockType[] StrataNoise(Vector3 pos);
        protected abstract BlockType HeightNoise(Vector3 pos);
        protected abstract BlockType DensityNoise(Vector3 pos);
    }
}
