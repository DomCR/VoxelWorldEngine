using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise;
using VoxelWorldEngine.Noise.RawNoise;

namespace VoxelWorldEngine
{
    [Serializable]
    public abstract class WorldGenerator : MonoBehaviour
    {
        public GameObject ChunkPrefab;

        [Space()]
        public int ChunksX;
        public int ChunksY;
        public int ChunksZ;

        protected Dictionary<Vector3, Chunk> m_chunks;

        #region Behaviour methods
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
        #endregion
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
                (int)(x / Chunk.XSize)*Chunk.XSize, 
                (int)(y / Chunk.YSize)*Chunk.YSize, 
                (int)(z / Chunk.ZSize)*Chunk.ZSize);

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
        /// Get the block of the current world initialization
        /// Once the world is generated use GetBlock()
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        [Obsolete("Use GetBlock instead")]
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
        [Obsolete("Use GetBlock instead")]
        public BlockType GetWorldBlock(Vector3 pos)
        {
            return BlockType.NULL;
        }
        public BlockType ComputeHeightNoise(Vector3 pos)
        {
            BlockType block = BlockType.NULL;

            //Set the bottom edge blocks 
            block = setBedRock(pos);
            if (block == BlockType.BEDROCK)
                return block;

            block = HeightNoise(pos);

            return block;
        }
        public BlockType ComputeDensityNoise(Vector3 pos)
        {
            return DensityNoise(pos);
        }
        //****************************************************************
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
            if (pos.z < 0 || pos.y < 0 || pos.x < 0 ||
                (int)pos.x >= ChunksX * Chunk.XSize ||
                (int)pos.y >= ChunksY * Chunk.YSize ||
                (int)pos.z >= ChunksZ * Chunk.ZSize)
                return true;

            return false;
        }
        //****************************************************************
        protected abstract BlockType HeightNoise(Vector3 pos);
        protected abstract BlockType DensityNoise(Vector3 pos);
    }
}
