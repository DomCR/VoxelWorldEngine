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
        HolesGeneration,
        Updating,

        //Request States
        NeedFaceUpdate,
        NeedMeshUpdate
    }
}