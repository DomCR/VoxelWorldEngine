using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Attributes
{
    [CreateAssetMenu(fileName = "Ore Spawn Attributes", menuName = "Voxel World/Ore Att")]
    public class SerializedOreAttributes : ScriptableObject
    {
        public string Name = "Default";
        public BlockType OreBlock;

        [Space]
        [Header("Generic noise properties")]
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float XWidth = 100;
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float YWidth = 100;
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float ZWidth = 100;

        [Range(0, 999f)]
        [Tooltip("Upper limit spawn")]
        public int HeightLimit = 0;

        [Space]
        [Header("Density noise properties")]
        [Range(0, 1.0f)]
        public float Density = 0.45f;
        public bool Inverted = false;

        [Space]
        [Header("Spawn noise properties")]
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
    }
}
