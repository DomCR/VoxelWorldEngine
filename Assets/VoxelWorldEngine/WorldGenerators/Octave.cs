using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise.RawNoise;

namespace VoxelWorldEngine
{
    public class Octave : WorldGenerator
    {
        [Space()]
        //Noise setup
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float WidthMagnitude = 125;
        [Range(0, 999f)]
        [Tooltip("Wave height of the noise")]
        public float HeightMagnitude = 200;
        [Tooltip("Minimum height under the noise")]
        public int MinHeight = 0;
        [Space()]
        [Range(0, 999f)]
        [Tooltip("Global 3d noise scale")]
        public float NoiseScale = 25;
        [Range(0, 1.0f)]
        public float Density = 0.45f;
        [Space]
        public float Frequency = 4;
        [Range(1, 8)]
        public int Octaves = 1;
        [Range(1f, 4f)]
        public float Lacunarity = 2f;
        [Range(0f, 1f)]
        public float Persistence = 0.5f;
        [Space]
        [Range(1, 3)]
        public int dimensions = 3;
        public NoiseMethodType NoiseType;
        //****************************************************************
        protected override BlockType DensityNoise(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        protected override BlockType HeightNoise(Vector3 pos)
        {
            NoiseMethod_delegate method = NoiseMap.noiseMethods[(int)NoiseType][dimensions - 1];
            float sample = NoiseMap.Sum(method, pos / WidthMagnitude, Frequency, Octaves, Lacunarity, Persistence) + 1;
            sample *= HeightMagnitude;

            if (pos.y < sample)
                return BlockType.STONE;

            return BlockType.NULL;
        }
    }
}
