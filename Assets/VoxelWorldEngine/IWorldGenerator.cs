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
    public abstract class IWorldGenerator : MonoBehaviour
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
        //****************************************************************
        public abstract BlockType ComputeHeightNoise(float x, float y, float z);
        public abstract BlockType ComputeDensityNoise(float x, float y, float z);
    }
}
