using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldEngine.Enums
{
    public enum NoiseMethod
    {
        UNITY_2D,
        
        NOISE_MAP_VALUE_1D,
        NOISE_MAP_VALUE_2D,
        NOISE_MAP_VALUE_3D,
        
        NOISE_MAP_PERLIN_1D,
        NOISE_MAP_PERLIN_2D,
        NOISE_MAP_PERLIN_3D,

        NOISE_MAP_SUM
    }
}
