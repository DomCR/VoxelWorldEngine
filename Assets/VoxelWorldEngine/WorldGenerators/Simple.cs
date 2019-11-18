using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise.RawNoise;

namespace VoxelWorldEngine
{
    [Obsolete]
    public class Simple : WorldGenerator
    {
        //[Space()]
        ////Noise setup
        //[Range(0, 999f)]
        //[Tooltip("Wave length of the noise")]
        //public float WidthMagnitude = 125;
        //[Range(0, 999f)]
        //[Tooltip("Wave height of the noise")]
        //public float HeightMagnitude = 200;
        //[Tooltip("Minimum height under the noise")]
        //public int MinHeight = 0;
        [Space()]
        [Range(0, 999f)]
        [Tooltip("Global 3d noise scale")]
        public float NoiseScale = 25;
        [Range(0, 1.0f)]
        public float Density = 0.45f;
        //****************************************************************
        protected override BlockType DensityNoise(Vector3 pos)
        {
            //Generate a 3D noise to create, holes and irregularities in the terrain
            if (PerlinNoise3D.Generate_01(
                (int)pos.x / NoiseScale,
                (int)pos.y / NoiseScale,
                (int)pos.z / NoiseScale) <= Density)
                return BlockType.STONE;
            else
                return BlockType.NULL;
        }
        /// <summary>
        /// Simple noise with only 1 layer
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected override BlockType HeightNoise(Vector3 pos)
        {
            #region Height layer
            //Get the height map
            int height = (int)(Mathf.PerlinNoise(
                (int)pos.x / WidthMagnitude,
                (int)pos.z / WidthMagnitude) * (HeightMagnitude)) + MinHeight;

            if ((int)pos.y > height)
                return BlockType.NULL;
            else
            {
                return BlockType.STONE;
            }
            #endregion

            #region Test 2
            //int stone = VoxelWorldEngine.Noise.RawNoise.PerlinNoise3D.ScalatedNoise(pos.x, 0, pos.z, 10, 3, 1.2f);
            //stone += VoxelWorldEngine.Noise.RawNoise.PerlinNoise3D.ScalatedNoise(pos.x, 300, pos.z, 20, 4, 0) + 10;
            //int dirt = VoxelWorldEngine.Noise.RawNoise.PerlinNoise3D.ScalatedNoise(pos.x, 100, pos.z, 50, 2, 0) + 1; //Added +1 to make sure minimum grass height is 1

            //if (pos.y <= stone)
            //{
            //    return BlockType.STONE;
            //}
            //else if (pos.y <= dirt + stone)
            //{ //Changed this line thanks to a comment
            //    return BlockType.GRASS_TOP;
            //}
            //else
            //{
            //    return BlockType.NULL;
            //}
            #endregion
        }
    }
}
