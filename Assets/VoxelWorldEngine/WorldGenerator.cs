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

        [Header("Biomes noise properties")]
        [Range(1, 999f)]
        [Tooltip("Wave length of the biome noise")]
        public float WidthMagnitude = 125;
        [Range(1, 999f)]
        [Tooltip("Wave height of the biome noise")]
        [Obsolete("The biome noise is 2D, no need for height")]
        public float HeightMagnitude = 200;

        [Tooltip("Minimum world height")]
        public int WorldHeight = 0;

        [Space]
        [Header("Height map noise properties")]
        [Tooltip("Stablish the noise frequency by each point.")]
        public float Frequency = 4;
        [Range(1, 8)]
        [Tooltip("Number of iterations for the noise, each octave is a new noise sum.")]
        public int Octaves = 1;
        [Range(1f, 4f)]
        [Tooltip("Phase between the different noise frequencies when the sum is applied.")]
        public float Lacunarity = 2f;
        [Range(0f, 1f)]
        [Tooltip("Multiplier for the noise sum, decrease each noise sum")]
        public float Persistence = 0.5f;
        [Space]
        [Range(1, 3)]

        [Tooltip("Noise dimensions, (x,z) as a 2Dplane, y is the up axis.")]
        public int Dimensions = 3;
        [Tooltip("Method to apply.")]
        public NoiseMethodType NoiseType;
        //****************************************************************
        protected Vector3 m_position;
        protected Dictionary<Vector3, Chunk> m_chunks;
        protected BiomeAttributes[] m_worldBiomes;
        //****************************************************************
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Apply the bedrock to all the world generators.
        /// </summary>
        /// <param name="basePos"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public BlockType[] ComputeHeightNoise(Vector2 basePos, int height)
        {
            BlockType[] col = new BlockType[Chunk.YSize];

            NoiseMethod_delegate method = NoiseMap.NoiseMethods[(int)NoiseType][Dimensions - 1];
            float bioNoise = (NoiseMap.Sum(method, new Vector3(basePos.x, 0, basePos.y) / WidthMagnitude, Frequency, Octaves, Lacunarity, Persistence) + 1) / 2;

            BiomeAttributes bioAtt = new BiomeAttributes();
            if (bioNoise > 0.5f)
            {
                bioAtt.DebugBlock = m_worldBiomes[0].DebugBlock;
                bioAtt.Octaves = m_worldBiomes[0].Octaves;
                bioAtt.Dimensions = m_worldBiomes[0].Dimensions;
                bioAtt.NoiseType = m_worldBiomes[0].NoiseType;
            }
            else
            {
                bioAtt.DebugBlock = m_worldBiomes[1].DebugBlock;
                bioAtt.Octaves = m_worldBiomes[1].Octaves;
                bioAtt.Dimensions = m_worldBiomes[1].Dimensions;
                bioAtt.NoiseType = m_worldBiomes[1].NoiseType;
            }

            float n = (1 - bioNoise);

            //if (bio > 0.5f)
            bioAtt += (m_worldBiomes[0] * bioNoise);
            //else
            bioAtt += (m_worldBiomes[1] * (1 - bioNoise));

            bioAtt /= 2;

            for (int y = 0; y < Chunk.YSize; y++)
            {
                BlockType block = BlockType.NULL;
                Vector3 pos = new Vector3(basePos.x, y + height, basePos.y);

                //Set the bottom edge blocks 
                block = setBedRock(pos);
                if (block == BlockType.BEDROCK)
                {
                    col[y] = block;
                    continue;
                }

                col[y] = HeightNoise(pos, bioAtt);
            }

            return col;
        }
        [Obsolete]
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

            //*****************************************************************
            //TODO: Calculate the biomes present in the current point
            //TODO: Solve the biome lerp by quantifing the biome presence
            
            //float bioNoise = Mathf.PerlinNoise(pos.x / 100, pos.z / 100);
            
            NoiseMethod_delegate method = NoiseMap.NoiseMethods[(int)NoiseMethodType.Perlin][2 - 1];
            float bioNoise = (NoiseMap.Sum(method, pos / WidthMagnitude, 4, 4, 2, 0.5f) + 1) / 2;


            BiomeAttributes bioAtt = new BiomeAttributes();
            if (bioNoise > 0.5f)
            {
                bioAtt.DebugBlock = m_worldBiomes[0].DebugBlock;
                bioAtt.Octaves = m_worldBiomes[0].Octaves;
                bioAtt.Dimensions = m_worldBiomes[0].Dimensions;
                bioAtt.NoiseType = m_worldBiomes[0].NoiseType;
            }
            else
            {
                bioAtt.DebugBlock = m_worldBiomes[1].DebugBlock;
                bioAtt.Octaves = m_worldBiomes[1].Octaves;
                bioAtt.Dimensions = m_worldBiomes[1].Dimensions;
                bioAtt.NoiseType = m_worldBiomes[1].NoiseType;
            }

            float n = (1 - bioNoise);

            //if (bio > 0.5f)
            bioAtt += (m_worldBiomes[0] * bioNoise);
            //else
            bioAtt += (m_worldBiomes[1] * (1 - bioNoise));

            //bioAtt = bioAtt / 2;

            //if (bioNoise > 0.5f)
            //    bioAtt = m_worldBiomes[0];
            //else
            //    bioAtt = m_worldBiomes[1];
            //*****************************************************************

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
        protected abstract BlockType[] StrataNoise(Vector3 pos);
        [Obsolete]
        protected abstract BlockType HeightNoise(Vector3 pos);
        protected abstract BlockType HeightNoise(Vector3 pos, BiomeAttributes attr);
        protected abstract BlockType DensityNoise(Vector3 pos);
    }
}
