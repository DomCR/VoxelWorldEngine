using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise.RawNoise;

namespace VoxelWorldEngine.Noise
{
    [Obsolete]
    public class NoiseLayer : INoiseLayer
    {
        public string Name;
        public BlockType Block;
        
        [Space]
        [Header("Height map noise properties")]
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

        [Space()]
        [Header("Density noise properties")]
        [Range(0, 1.0f)]
        public float Density = 0.45f;
        public bool Inverted = false;

        [Space]
        [Range(1, 3)]
        [Tooltip("Noise dimensions, (x,z) as a 2Dplane, y is the up axis.")]
        public int Dimensions = 3;
        [Tooltip("Method to apply.")]
        public NoiseMethodType_Obs NoiseType;

        public BlockType Compute2D(Vector3 pos)
        {
            //TODO: implement inferior limit of the noise Ex: Dirt

            //TODO: Compute the height noise, return block type if correct

            throw new NotImplementedException();
        }

        public BlockType Compute3D(Vector3 pos)
        {
            //TODO: Implement the height limit of the noise (use a noise layer ?)

            //TODO: Compute the noise, return the ore block or null if is a cave

            throw new NotImplementedException();
        }
    }
}
