using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Attributes;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Utils;
using VoxelWorldEngine.Noise;

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
        public bool UseThreadControl;
        [Range(1, 256)]
        public int MaxThreads = 8;

        [Space()]
        public int ChunksX;
        public int ChunksY;
        public int ChunksZ;

        [Tooltip("Minimum world height")]
        public int MinWorldHeight = 0;

        [Space()]
        [Tooltip("Array with all the world biomes available")]
        public SerializedBiomeAttributes[] BiomeData;
        [Tooltip("Array with all the ores available in the world")]
        public SerializedOreAttributes[] OreData;

        //[Space()]
        //[Header("Game modifiers")]
        public int SectionLevel
        {
            get
            {
                return m_sectionLevel;
            }
            set
            {
                if (value < (ChunksY * Chunk.YSize - 1) && value >= 0)
                {
                    m_sectionLevel = value;
                    //Update horizontal section location
                    //UpdateWorldSection();
                }
            }
        }
        private int m_sectionLevel;

        #region Density noise properties (to delete)
        //[Header("Generic biome noise properties")]
        //[Range(0f, 999f)]
        //[Tooltip("Wave length of the biome noise")]
        //public float WidthMagnitude = 125;

        //public float HeightXNoiseGap;
        //public float HeightZNoiseGap;

        //[Space]
        //[Header("Temperature noise map properties")]
        //[Tooltip("Stablish the noise frequency by each point.")]
        //public float Frequency = 4;
        //[Range(1, 8)]
        //[Tooltip("Number of iterations for the noise, each octave is a new noise sum.")]
        //public int Octaves = 1;
        //[Range(1f, 4f)]
        //[Tooltip("Phase between the different noise frequencies when the sum is applied.")]
        //public float Lacunarity = 2f;
        //[Range(0f, 1f)]
        //[Tooltip("Multiplier for the noise sum, decrease each noise sum")]
        //public float Persistence = 0.5f;
        //[Space]
        //[Range(1, 3)]

        //[Tooltip("Noise dimensions, (x,z) as a 2Dplane, y is the up axis.")]
        //public int Dimensions = 3;
        //[Tooltip("Method to apply.")]
        //public NoiseMethodType NoiseType;
        #endregion
        //****************************************************************
        protected Vector3 m_position;
        protected Dictionary<Vector3, Chunk> m_chunks;
        protected BiomeAttributes[] m_worldBiomes;
        protected OreAttributes[] m_worldOres;
        //****************************************************************
        public List<Thread> ActiveThreads;
        //****************************************************************
        #region Behaviour methods
        // Start is called before the first frame update
        void Start()
        {
            //DEBUG
            SectionLevel = 50;

            //Initialize variables
            m_position = this.transform.position;
            m_chunks = new Dictionary<Vector3, Chunk>();
            ActiveThreads = new List<Thread>();

            //Feed the world biome attributes to be used outside the main thread
            m_worldBiomes = new BiomeAttributes[BiomeData.Length];
            for (int i = 0; i < BiomeData.Length; i++)
            {
                m_worldBiomes[i] = new BiomeAttributes(BiomeData[i]);
            }
            //Feed the world biome attributes to be used outside the main thread
            m_worldOres = new OreAttributes[OreData.Length];
            for (int i = 0; i < OreData.Length; i++)
            {
                m_worldOres[i] = new OreAttributes(OreData[i]);
            }

            //Clear the none needed data
            Array.Clear(BiomeData, 0, BiomeData.Length);
            Array.Clear(OreData, 0, OreData.Length);

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

            if (UseThreadControl)
            {
                //Chunk thread control
                threadControl();
            }
        }
        void UpdateWorldSection()
        {
            //Valudate that the world is generated
            if (m_chunks == null)
                return;

            foreach (var item in m_chunks)
            {
                Chunk tmp = item.Value;
                if (tmp.State != ChunkState.Updating)
                    tmp.State = ChunkState.NeedFaceUpdate;
            }
        }
        #endregion
        //****************************************************************
        #region World generation methods
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
                        tmp_chunk.name = "Chunk_" + (int)pos.x + "_" + (int)pos.y + "_" + (int)pos.z;
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

            #region TO FIX
            //TODO: Fix the biomes (libnoise voronoi)
            //NoiseMethod_delegate method = NoiseMap.NoiseMethods[(int)NoiseType][Dimensions - 1];
            //float temperatureNoise = (NoiseMap.Sum(method, new Vector3(basePos.x, 0, basePos.y) / WidthMagnitude, Frequency, Octaves, Lacunarity, Persistence) + 1) / 2;
            //float heightNoise = (NoiseMap.Sum(method, new Vector3(basePos.x + HeightXNoiseGap, 0, basePos.y + HeightZNoiseGap) / WidthMagnitude, Frequency, Octaves, Lacunarity, Persistence) + 1) / 2;

            //BiomeAttributes bioAtt = new BiomeAttributes();
            //bioAtt.CopyBase(m_worldBiomes.OrderBy(o => o.Presence(heightNoise, temperatureNoise, 0.1f)).First());
            //int nbiomes = 0;
            //foreach (BiomeAttributes b in m_worldBiomes)
            //{
            //    float presence = b.Presence(heightNoise, temperatureNoise, 0.7f);

            //    if (presence == 0)
            //        continue;

            //    bioAtt += b * presence;
            //    nbiomes++;
            //} 

            //bioAtt /= nbiomes;

            //float temp = (float)VoronoiMap.GetValue(basePos.x, 0, basePos.y, WidthMagnitude, Frequency, false);
            float temp = (float)(VoronoiMap.GetValue(basePos.x, 0, basePos.y, 1.5f, 0.05f, false) + 1) / 2f;
            //int bioIndex = (int)Mathf.Lerp(0, m_worldBiomes.Length, temp);
            float bioIndex = Mathf.Clamp(temp, 0f, m_worldBiomes.Length);
            bioIndex *= m_worldBiomes.Length;
            //bioIndex = Mathf.Clamp(temp, 0f, m_worldBiomes.Length);

            #endregion

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

                //col[y] = HeightNoise(pos, m_worldBiomes[(int)bioIndex]);
                col[y] = HeightNoise(pos, m_worldBiomes[0]);
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
            float bioNoise = (NoiseMap.Sum(method, pos /*/ WidthMagnitude*/, 4, 4, 2, 0.5f) + 1) / 2;


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
        public BlockType ComputeOreNoise(Vector3 basePos)
        {
            BlockType ore = BlockType.NULL;

            foreach (OreAttributes att in m_worldOres)
            {
                ore = OreNoise(basePos, att);
                if (ore != BlockType.NULL)
                    break;
            }

            if (ore == BlockType.NULL)
                return BlockType.STONE;
            else
                return ore;
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
        #endregion
        //****************************************************************
        protected abstract BlockType[] StrataNoise(Vector3 pos);
        [Obsolete]
        protected abstract BlockType HeightNoise(Vector3 pos);
        protected abstract BlockType HeightNoise(Vector3 pos, BiomeAttributes attr);
        protected abstract BlockType OreNoise(Vector3 pos, OreAttributes attr);
        protected abstract BlockType DensityNoise(Vector3 pos);
        //****************************************************************
        private void debugActions()
        {
            if (debug.StateHasChanged())
            {
                foreach (Chunk chunk in m_chunks.Values)
                {
                    chunk.State = debug.ChunkState;
                }
            }
        }
        private void threadControl()
        {
            //Set the active threads limit
            int tactive;
            if ((tactive = ActiveThreads.Where(o => o.IsAlive).Count()) < this.MaxThreads)
            {
                List<Thread> threadsToRemove = new List<Thread>();

                foreach (Thread th in ActiveThreads)
                {
                    if (tactive < this.MaxThreads)
                    {
                        switch (th.ThreadState)
                        {
                            case System.Threading.ThreadState.Aborted:
                                Debug.Log("Thread aborted");
                                break;
                            case System.Threading.ThreadState.AbortRequested:
                                break;
                            case System.Threading.ThreadState.Background:
                                Debug.Log("Thread background running");
                                break;
                            case System.Threading.ThreadState.Running:
                                //Debug.Log("Thread running");
                                break;
                            case System.Threading.ThreadState.Stopped:
                                threadsToRemove.Add(th);
                                Debug.Log("Thread stopped");
                                break;
                            case System.Threading.ThreadState.StopRequested:
                                break;
                            case System.Threading.ThreadState.Suspended:
                                break;
                            case System.Threading.ThreadState.SuspendRequested:
                                break;
                            case System.Threading.ThreadState.Unstarted:
                                th.Start();
                                tactive++;
                                Debug.Log("Generation started");
                                break;
                            case System.Threading.ThreadState.WaitSleepJoin:
                                Debug.Log("Thread waiting");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //Remove finished threads
                foreach (Thread item in threadsToRemove)
                {
                    ActiveThreads.Remove(item);
                }
            }
        }
    }
}
