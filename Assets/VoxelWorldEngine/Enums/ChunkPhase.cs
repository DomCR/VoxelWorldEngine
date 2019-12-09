using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldEngine.Enums
{
    public enum ChunkPhase
    {
        Init,

        HeightMap,
        Strata,
        Caves,
        OreGeneration,
        Vegetation,

        Generated
    }
}
