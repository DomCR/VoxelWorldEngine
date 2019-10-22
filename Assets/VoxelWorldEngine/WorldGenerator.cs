using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                        GameObject tmp_chunk = Instantiate(ChunkPrefab, pos, new Quaternion(0,0,0,0), this.transform);
                        m_chunks.Add(pos, tmp_chunk.GetComponent<Chunk>());
                    }
                }
            }
        }
        //****************************************************************
        public static bool CheckSurroundings(Vector3 pos, int x, int y, int v)
        {
            Chunk chunk = m_chunks[pos];

            throw new NotImplementedException();
        }
    }
}