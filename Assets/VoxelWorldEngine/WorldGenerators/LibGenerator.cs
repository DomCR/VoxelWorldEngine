using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Attributes;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise;

namespace VoxelWorldEngine.WorldGenerators
{
    public class LibGenerator : WorldGenerator
    {
        protected override BlockType DensityNoise(Vector3 pos)
        {
            throw new NotImplementedException();
        }
        protected override BlockType HeightNoise(Vector3 pos)
        {


            throw new NotImplementedException();
        }
        protected override BlockType HeightNoise(Vector3 pos, BiomeAttributes attr)
        {
            //float sample = NoiseLibrary.Perlin();

            throw new NotImplementedException();
        }

        protected override BlockType OreNoise(Vector3 pos, OreAttributes attr)
        {
            throw new NotImplementedException();
        }

        protected override BlockType[] StrataNoise(Vector3 pos)
        {
            throw new NotImplementedException();
        }
    }
}
