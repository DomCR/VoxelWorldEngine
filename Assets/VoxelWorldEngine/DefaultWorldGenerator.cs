using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine
{
    public class DefaultWorldGenerator : IWorldGenerator
    {
        public override BlockType ComputeDensityNoise(float x, float y, float z)
        {
            throw new System.NotImplementedException();
        }
        public override BlockType ComputeHeightNoise(float x, float y, float z)
        {
            throw new System.NotImplementedException();
        }
    }
}
