using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorldEngine.Noise.RawNoise
{
    public static class PerlinNoise3D
    {
        public static float Generate_01(float x, float y, float z)
        {
            float xy = Mathf.PerlinNoise(x, y);
            float yz = Mathf.PerlinNoise(y, z);
            float xz = Mathf.PerlinNoise(x, z);
            float yx = Mathf.PerlinNoise(y, x);
            float zy = Mathf.PerlinNoise(z, y);
            float zx = Mathf.PerlinNoise(z, x);

            float xyz = xy + yz + xz + yx + zy + zx;
            return xyz / 6f;
        }

        public static int ScalatedNoise(float x, float y, float z, float scale, float height, float power)
        {
            float rValue;
            rValue = PerlinNoise3D.Generate_01(x / scale, y / scale, z / scale);
            rValue *= height;

            if (power != 0)
            {
                rValue = Mathf.Pow(rValue, power);
            }

            return (int)rValue;
        }

        internal static float Generate_01(object p1, object p2, object p3)
        {
            throw new NotImplementedException();
        }
    }
}
