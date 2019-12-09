using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Attributes
{
    public class OreAttributes
    {
        public string Name { get; set; }
        public BlockType OreBlock { get; set; }

        public float XWidth { get; set; }
        public float YWidth { get; set; }
        public float ZWidth { get; set; }
        public int HeightLimit { get; set; }

        public float Density { get; set; }
        public bool Inverted { get; set; }

        public float Frequency { get; set; }
        public int Octaves { get; set; }
        public float Lacunarity { get; set; }
        public float Persistence { get; set; }
        public int Dimensions { get; set; }
        public NoiseMethodType NoiseType { get; set; }

        public OreAttributes()
        {
            Dimensions = 3;
        }

        public OreAttributes(OreAttributes att)
        {
            Name = att.Name;
            OreBlock = att.OreBlock;

            XWidth = att.XWidth;
            YWidth = att.YWidth;
            ZWidth = att.ZWidth;

            Density = att.Density;
            Inverted = att.Inverted;

            HeightLimit = att.HeightLimit;
            Frequency = att.Frequency;
            Octaves = att.Octaves;
            Lacunarity = att.Lacunarity;
            Persistence = att.Persistence;
            Dimensions = att.Dimensions;
            NoiseType = att.NoiseType;
        }
        public OreAttributes(SerializedOreAttributes att)
        {
            Name = att.Name;
            OreBlock = att.OreBlock;

            XWidth = att.XWidth;
            YWidth = att.YWidth;
            ZWidth = att.ZWidth;

            Density = att.Density;
            Inverted = att.Inverted;

            HeightLimit = att.HeightLimit;
            Frequency = att.Frequency;
            Octaves = att.Octaves;
            Lacunarity = att.Lacunarity;
            Persistence = att.Persistence;
            Dimensions = att.Dimensions;
            NoiseType = att.NoiseType;
        }
    }
}
