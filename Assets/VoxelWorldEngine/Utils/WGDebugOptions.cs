using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Utils
{
    [Serializable]
    public class WGDebugOptions
    {
        public bool IsActive;
        public ChunkState ChunkState;
        private ChunkState m_currState;

        public bool StateHasChanged()
        {
            if(ChunkState != m_currState)
            {
                m_currState = ChunkState;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
