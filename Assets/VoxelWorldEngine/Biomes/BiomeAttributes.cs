using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Biomes
{
    public class BiomeAttributes
    {
        public string Name { get; set; }
        public BlockType DebugBlock { get; set; }

        public float WidthMagnitude { get; set; }
        public float HeightMagnitude { get; set; }
        public float Frequency { get; set; }
        public int Octaves { get; set; }
        public float Lacunarity { get; set; }
        public float Persistence { get; set; }
        public int Dimensions { get; set; }
        public NoiseMethodType NoiseType { get; set; }
        public float Density { get; set; }
        public bool Inverted { get; set; }
        public float Frequency3D { get; set; }
        public int Octaves3D { get; set; }
        public float Lacunarity3D { get; set; }
        public float Persistence3D { get; set; }
        public float Width3D { get; set; }
        public int DimensionsDensity { get; set; }
        public NoiseMethodType NoiseTypeDensity { get; set; }

        public BiomeAttributes()
        {

        }

        public BiomeAttributes(SerializedBiomeAttributes att)
        {
            Name = att.Name;
            DebugBlock = att.DebugBlock;

            WidthMagnitude = att.WidthMagnitude;
            HeightMagnitude = att.HeightMagnitude;
            Frequency = att.Frequency;
            Octaves = att.Octaves;
            Lacunarity = att.Lacunarity;
            Persistence = att.Persistence;
            Dimensions = att.Dimensions;
            NoiseType = att.NoiseType;
            Density = att.Density;
            Inverted = att.Inverted;
            Frequency3D = att.Frequency3D;
            Octaves3D = att.Octaves3D;
            Lacunarity3D = att.Lacunarity3D;
            Persistence3D = att.Persistence3D;
            Width3D = att.Width3D;
            DimensionsDensity = att.DimensionsDensity;
            NoiseTypeDensity = att.NoiseTypeDensity;
        }
    }
}