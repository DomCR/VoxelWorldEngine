using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorldEngine.Noise
{
    [Serializable]
    public class NoiseLayer : Noise
    {
        

        public override float Compute(float x, float y)
        {
            throw new NotImplementedException();
        }
        public override float Compute(float x, float y, float z)
        {
            throw new NotImplementedException();
        }
    }
}
