using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LibNoise;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Noise.RawNoise
{
    public delegate float LibNoiseDelegate();
    public class NoiseLibrary : INoise
    {
        // Create the module network
        static ModuleBase moduleBase;

        public static float Perlin(NoiseMethodType type)
        {
            switch (type)
            {
                case NoiseMethodType.Value:
                    break;
                case NoiseMethodType.Perlin:
                    break;
                case NoiseMethodType.Billow:
                    break;
                case NoiseMethodType.RidgedMultifractal:
                    break;
                case NoiseMethodType.Voronoi:
                    break;
                case NoiseMethodType.Mix:
                    break;
                case NoiseMethodType.Practice:
                    break;
                default:
                    break;
            }

            return 0.0f;
        }

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public static double GetValue(double x, double y, double z, double frequency,  int octaveCount, double lacunarity, double persistence)
        {
            int _seed = 0;
            QualityMode _quality = QualityMode.High;

            var value = 0.0;
            var cp = 1.0;
            x *= frequency;
            y *= frequency;
            z *= frequency;
            for (var i = 0; i < octaveCount; i++)
            {
                var nx = LibNoise.Utils.MakeInt32Range(x);
                var ny = LibNoise.Utils.MakeInt32Range(y);
                var nz = LibNoise.Utils.MakeInt32Range(z);
                var seed = (_seed + i) & 0xffffffff;
                var signal = LibNoise.Utils.GradientCoherentNoise3D(nx, ny, nz, seed, _quality);
                value += signal * cp;
                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
                cp *= persistence;
            }
            return value;
        }
        public static double GetValue(Vector3 pos, float frequency, int octaves, float lacunarity, float persistence)
        {
            return GetValue(pos.x, pos.y, pos.z, frequency, octaves, lacunarity, persistence);
        }
    }
}
