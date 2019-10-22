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
            //TODO: Check the surrounding chunks as well
            Chunk chunk = null;

            //Find in which chunk is the point
            //Vector3 global_pos = new Vector3(pos.x + x, pos.y + y, pos.z + z);
            Vector3 chunk_key = new Vector3(pos.x + CheckLimits((int)pos.x, Chunk.XSize), pos.y + CheckLimits((int)pos.y, Chunk.YSize), pos.z + CheckLimits((int)pos.z, Chunk.ZSize));

            //If the chunk doesn't exist, return true (AIR)
            m_chunks.TryGetValue(chunk_key, out chunk);
            if (chunk == null) return true;

            //NO NEED: WAIT FOR VERIFICATION...
            //Check array limits
            //if (x >= chunk.Blocks.GetLength(0) ||
            //    y >= chunk.Blocks.GetLength(1) ||
            //    z >= chunk.Blocks.GetLength(2) ||
            //    x < 0 || y < 0 || z < 0
            //    )
            //{
            //    return true;
            //}

            return Block.IsTransparent(chunk.Blocks[x, y, z]);
        }

        public static int CheckLimits(int value, int limit)
        {
            if (value - limit < 0)
            {
                return -1;
            }
            if (value + limit >= limit)
            {
                return 1;
            }

            return 0;
        }

        bool CheckSurroundings_old(int x, int y, int z)
        {
            int wx = x + (int)this.transform.position.x;
            int wy = y + (int)this.transform.position.y;
            int wz = z + (int)this.transform.position.z;
            //Debug.Log((int)this.transform.position.x);

            //Check array limits
            if (wx >= World.WorldBlocks.GetLength(0) ||
                wy >= World.WorldBlocks.GetLength(1) ||
                wz >= World.WorldBlocks.GetLength(2) ||
                wx < 0 || wy < 0 || wz < 0
                )
            {
                return true;
            }

            if (World.WorldBlocks[wx, wy, wz] == BLOCK.NULL)
                return true;

            return false;
        }
    }
}