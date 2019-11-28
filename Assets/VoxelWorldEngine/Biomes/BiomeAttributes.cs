using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Biomes
{
    public class BiomeAttributes
    {
        public const float MaxHeight = 256f;

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

        public float Temperature { get; set; }
        public float Height { get; set; }

        public BiomeAttributes()
        {
            Dimensions = 1;
            DimensionsDensity = 1;
        }
        public BiomeAttributes(BiomeAttributes att)
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
        //**************************************************************************
        public float Presence(float height, float temperature, float spawnRange)
        {
            float hDif = Mathf.Abs(height - Height);
            float tDif = Mathf.Abs(temperature - Temperature);

            float dif = (hDif + tDif) / 2f;

            if (dif < spawnRange)
                return dif;

            return 0;
        }
        public float PresenceByHeight()
        {
            throw new NotImplementedException();
        }
        public void GetBase(BiomeAttributes att)
        {
            this.DebugBlock = att.DebugBlock;
            this.Octaves = att.Octaves;
            this.Dimensions = att.Dimensions;
            this.NoiseType = att.NoiseType;
        }
        //**************************************************************************
        public static BiomeAttributes operator +(BiomeAttributes att1, BiomeAttributes att2)
        {
            BiomeAttributes biomeAtts = new BiomeAttributes(att1);

            biomeAtts.WidthMagnitude += att2.WidthMagnitude;
            biomeAtts.HeightMagnitude += att2.HeightMagnitude;
            biomeAtts.Frequency += att2.Frequency;
            biomeAtts.Lacunarity += att2.Lacunarity;
            biomeAtts.Persistence += att2.Persistence;
            biomeAtts.Density += att2.Density;
            biomeAtts.Frequency3D += att2.Frequency3D;
            biomeAtts.Lacunarity3D += att2.Lacunarity3D;
            biomeAtts.Persistence3D += att2.Persistence3D;
            biomeAtts.Width3D += att2.Width3D;

            return biomeAtts;
        }
        public static BiomeAttributes operator /(BiomeAttributes att, float prod)
        {
            BiomeAttributes biomeAtts = new BiomeAttributes(att);

            biomeAtts.WidthMagnitude /= prod;
            biomeAtts.HeightMagnitude /= prod;
            biomeAtts.Frequency /= prod;
            biomeAtts.Lacunarity /= prod;
            biomeAtts.Persistence /= prod;
            biomeAtts.Density /= prod;
            biomeAtts.Frequency3D /= prod;
            biomeAtts.Lacunarity3D /= prod;
            biomeAtts.Persistence3D /= prod;
            biomeAtts.Width3D /= prod;

            return biomeAtts;
        }
        public static BiomeAttributes operator *(BiomeAttributes att, float prod)
        {
            BiomeAttributes biomeAtts = new BiomeAttributes(att);

            biomeAtts.WidthMagnitude *= prod;
            biomeAtts.HeightMagnitude *= prod;
            biomeAtts.Frequency *= prod;
            biomeAtts.Lacunarity *= prod;
            biomeAtts.Persistence *= prod;
            biomeAtts.Density *= prod;
            biomeAtts.Frequency3D *= prod;
            biomeAtts.Lacunarity3D *= prod;
            biomeAtts.Persistence3D *= prod;
            biomeAtts.Width3D *= prod;

            return biomeAtts;
        }
    }
}