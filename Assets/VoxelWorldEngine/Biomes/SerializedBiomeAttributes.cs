using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Biomes
{
    [CreateAssetMenu(fileName = "Biome Attributes", menuName = "Voxel World/Biome Att")]
    public class SerializedBiomeAttributes : ScriptableObject
    {
        public string Name = "Default";
        public BlockType DebugBlock;

        [Space]
        [Header("Generic noise properties")]
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float WidthMagnitude = 100;
        [Range(0, 999f)]
        [Tooltip("Wave height of the noise")]
        public float HeightMagnitude = 200;

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
        [Space]
        [Range(1, 3)]

        [Tooltip("Noise dimensions, (x,z) as a 2Dplane, y is the up axis.")]
        public int Dimensions = 3;
        [Tooltip("Method to apply.")]
        public NoiseMethodType NoiseType;

        [Space]
        [Header("Biome spawn qualifications")]
        [Range(0f, 1f)]
        public float Temperature;
        [Range(0f, 1f)]
        public float Height;

        [Space(10)]
        [Header("Density noise properties")]
        [Range(0, 1.0f)]
        public float Density = 0.45f;
        public bool Inverted = false;
        [Space]
        public float Frequency3D = 2;
        [Range(1, 8)]
        [Tooltip("Number of iterations for the noise, each octave is a new noise sum.")]
        public int Octaves3D = 1;
        [Range(1f, 4f)]
        [Tooltip("Phase between the different noise frequencies when the sum is applied.")]
        public float Lacunarity3D = 1;
        [Range(0f, 1f)]
        [Tooltip("Multiplier for the noise sum, decrease each noise sum")]
        public float Persistence3D = 0.5f;
        public float Width3D = 50f;
        [Space]
        [Range(1, 3)]
        [Tooltip("Noise dimensions, (x,z) as a 2Dplane, y is the up axis.")]
        public int DimensionsDensity = 3;
        [Tooltip("Method to apply.")]
        public NoiseMethodType NoiseTypeDensity;
    }
}