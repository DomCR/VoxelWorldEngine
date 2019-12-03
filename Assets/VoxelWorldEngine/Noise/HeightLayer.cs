using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorldEngine.Noise
{
    [Obsolete]
    public class HeightLayer
    {
        public string Name;
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
        [Space]
        [Range(1, 3)]
        [Tooltip("Noise dimensions, (x,z) as a 2Dplane, y is the up axis.")]
        public int Dimensions = 3;
        [Tooltip("Method to apply.")]
        public NoiseMethodType_Obs NoiseType;


    }
}
