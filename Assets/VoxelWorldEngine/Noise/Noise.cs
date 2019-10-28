using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Noise
{
    [Serializable]
    public abstract class Noise
    {
        public string Name { get; set; }

        public float WidthMagnitudeX { get; set; }
        public float WidthMagnitudeZ { get; set; }
        public float HeightMagnitude { get; set; }
        public float NoiseScaleX { get; set; }
        public float NoiseScaleY { get; set; }
        public float NoiseScaleZ { get; set; }

        public bool IsNegative { get; set; }
        public float Density { get; set; }

        public NoiseType Type { get; set; }

        public Noise()
        {

        }

        public abstract float Compute(float x, float y);
        public abstract float Compute(float x, float y, float z);
        public abstract float Compute(float x, float y, float z, int octaves);
    }
}
