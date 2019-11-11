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
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float WidthMagnitude = 125;
        [Range(0, 999f)]
        [Tooltip("Wave height of the noise")]
        public float HeightMagnitude = 200;
        [Tooltip("Minimum height under the noise")]
        public int MinHeight = 0;

        //[Space()]
        //[Range(0, 999f)]
        //[Tooltip("Global 3d noise scale")]
        //public float NoiseScale = 25;
        //[Range(0, 1.0f)]
        //public float Density = 0.45f;

        [Space]
        [Tooltip("Stablish the noise frequency by each point.")]
        public float Frequency = 4;
        [Range(1, 8)]
        [Tooltip("Number of iterations for the noise, each octave is a new noise sum.")]
        public int Octaves = 1;
        [Range(1f, 4f)]
        [Tooltip("Phase between the different noise frequencies when the sum is applied.")]
        public float Lacunarity = 2f;
        [Range(0f, 1f)]
        [Tooltip("Multiplier for the noise sum, decrease each noise sum")]
        public float Persistence = 0.5f;
        [Space]
        [Range(1, 3)]
        [Tooltip("Noise dimensions, (x,z) as a 2Dplane, y is the up axis.")]
        public int Dimensions = 3;
        [Tooltip("Method to apply.")]
        public NoiseMethodType NoiseType;
        [Space]
        public Dictionary<string, string> NoiseLayers;
        //****************************************************************
        protected override BlockType DensityNoise(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        protected override BlockType HeightNoise(Vector3 pos)
        {
            NoiseMethod_delegate method = NoiseMap.noiseMethods[(int)NoiseType][Dimensions - 1];
            float sample = NoiseMap.Sum(method, pos / WidthMagnitude, Frequency, Octaves, Lacunarity, Persistence) + 1;

            //Apply the height magnitude
            float h = Mathf.PerlinNoise(pos.x / (WidthMagnitude), pos.z / (WidthMagnitude)) * 200;
            //Debug.Log(h);
            sample *= HeightMagnitude;
            //sample *= h;

            //Set the minimum height of the world
            sample += MinHeight;

            if (pos.y < MinHeight)
                return BlockType.DIRT;
            if (pos.y < sample)
                return BlockType.STONE;

            return BlockType.NULL;
        }
        //****************************************************************
        private float computeHeightMagnitude(Vector3 pos)
        {

            throw new NotImplementedException();
        }
    }
}
