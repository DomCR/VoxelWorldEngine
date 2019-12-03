using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Noise
{
    /// <summary>
    /// Delegate the noise methods
    /// </summary>
    /// <param name="point"></param>
    /// <param name="frequency"></param>
    /// <returns></returns>
    public delegate float NoiseDataDelegate(Vector3 point, float frequency);

    public class NoiseData
    {
        public static NoiseDataDelegate GetMethod(NoiseMethodType methodType)
        {
            switch (methodType)
            {
                case NoiseMethodType.Perlin:
                    break;
                case NoiseMethodType.Value:
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

            return null;
        }
    }
}
