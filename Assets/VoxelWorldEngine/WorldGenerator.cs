using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine
{
    public class WorldGenerator : MonoBehaviour
    {
        public GameObject ChunkPrefab;

        [Space()]
        public int ChunksX;
        public int ChunksY;
        public int ChunksZ;

        private static Dictionary<Vector3, Chunk> m_chunks;

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
        public static bool CheckSurroundings(Vector3 pos, int x, int y, int z)
        {
            Chunk chunk = null;

            //Find in which chunk is the point
            Vector3 chunk_key = new Vector3(
                pos.x + CheckLimits((int)(x + pos.x), (int)pos.x, Chunk.XSize),
                pos.y + CheckLimits((int)(y + pos.y), (int)pos.y, Chunk.YSize),
                pos.z + CheckLimits((int)(z + pos.z), (int)pos.z, Chunk.ZSize));

            //If the chunk doesn't exist, return true (AIR)
            m_chunks.TryGetValue(chunk_key, out chunk);
            if (chunk == null) return true;

            return Block.IsTransparent(chunk.Blocks[x, y, z]);
        }

        public static int CheckLimits(int value, int axisPos, int limit)
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