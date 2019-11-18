using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldEngine.Enums
{
    public enum ChunkState
    {
        Idle,

        //Transformation states
        HeightMapGeneration,
        CaveGeneration,
        OreGeneration,
        StrataGeneration,

        //Testing
        DensityMapGeneration,

        //Working states
        Updating,

        //Request states
        NeedFaceUpdate,
        NeedMeshUpdate
    }
}