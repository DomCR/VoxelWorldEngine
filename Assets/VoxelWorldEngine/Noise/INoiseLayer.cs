using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Noise
{
    public interface INoiseLayer 
    {
        BlockType Compute2D(Vector3 pos);
        BlockType Compute3D(Vector3 pos);
    }
}
