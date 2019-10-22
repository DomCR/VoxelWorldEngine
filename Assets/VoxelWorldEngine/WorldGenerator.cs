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

        public readonly int k = 4;

        // Start is called before the first frame update
        void Start()
        {
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
                        GameObject tmp_chunk = Instantiate(ChunkPrefab, new Vector3(x * Chunk.XSize, y * Chunk.YSize, z * Chunk.ZSize), new Quaternion(0,0,0,0), this.transform);
                    }
                }
            }
        }
    }
}