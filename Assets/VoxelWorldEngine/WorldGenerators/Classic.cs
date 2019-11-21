using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise.RawNoise;

namespace VoxelWorldEngine
{
    [Obsolete]
    public class Classic : WorldGenerator
    {
        //[Space()]
        ////Noise setup
        //[Range(0, 999f)]
        //[Tooltip("Wave length of the noise")]
        //public float WidthMagnitude = 125;
        //[Range(0, 999f)]
        //[Tooltip("Wave height of the noise")]
        //public float HeightMagnitude = 200;
        [Space()]
        [Range(0, 999f)]
        [Tooltip("Global 3d noise scale")]
        public float NoiseScale = 25;
        [Range(0, 1.0f)]
        public float Density = 0.45f;
        //****************************************************************
        /// <summary>
        /// Classic height map nosie
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected override BlockType HeightNoise(Vector3 pos)
        {
            float heightResult = 0;
            float heightLow = (noise1(pos.x * 1.3f, pos.z * 1.3f) / 6f) - 4f;
            float heightHigh = (noise2(pos.x * 1.3f, pos.z * 1.3f) / 5f) + 6f;

            if (noise3(pos.x, pos.z) / 8 > 0)
            {
                heightResult = heightLow;
            }
            else
            {
                heightResult = heightHigh;
            }

            heightResult = heightResult / 2;

            if (heightResult < 0)
            {
                heightResult = heightResult * (8 / 10);
            }

            heightResult += 30;

            float dirtThickness = noise1(pos.x, pos.z) / 24 - 4;
            float dirtTransition = heightResult;
            float stoneTransition = dirtTransition + dirtThickness;

            if (pos.y <= Mathf.Abs(stoneTransition)) return BlockType.STONE;
            else if (pos.y <= Mathf.Abs(dirtTransition)) return BlockType.DIRT;
            else return BlockType.NULL;
        }
        protected override BlockType[] StrataNoise(Vector3 pos)
        {
            throw new NotImplementedException();
        }
        protected override BlockType DensityNoise(Vector3 pos)
        {
            throw new NotImplementedException();
        }
        //****************************************************************
        private float noise1(float x, float z)
        {
            return Mathf.Pow(Mathf.PerlinNoise(x, z), 2);
        }
        private float noise2(float x, float z)
        {
            return Mathf.Pow(Mathf.PerlinNoise(x + noise1(x, z), z), 2);
        }
        private float noise3(float x, float z)
        {
            return Mathf.Pow(Mathf.PerlinNoise(x + noise2(x, z), z + noise2(x, z)), 2);
        }
    }
}
