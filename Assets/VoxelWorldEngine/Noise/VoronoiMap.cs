using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNoise;

namespace VoxelWorldEngine.Noise
{
    public class VoronoiMap
    {
        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public static double GetValue(float x, float y, float z, float frequency,double displacement, bool distance)
        {
            int _seed = 0;

            x *= frequency;
            y *= frequency;
            z *= frequency;
            var xi = (x > 0.0 ? (int)x : (int)x - 1);
            var iy = (y > 0.0 ? (int)y : (int)y - 1);
            var iz = (z > 0.0 ? (int)z : (int)z - 1);
            var md = 2147483647.0;
            double xc = 0;
            double yc = 0;
            double zc = 0;
            for (var zcu = iz - 2; zcu <= iz + 2; zcu++)
            {
                for (var ycu = iy - 2; ycu <= iy + 2; ycu++)
                {
                    for (var xcu = xi - 2; xcu <= xi + 2; xcu++)
                    {
                        var xp = xcu + LibNoise.Utils.ValueNoise3D(xcu, ycu, zcu, _seed);
                        var yp = ycu + LibNoise.Utils.ValueNoise3D(xcu, ycu, zcu, _seed + 1);
                        var zp = zcu + LibNoise.Utils.ValueNoise3D(xcu, ycu, zcu, _seed + 2);
                        var xd = xp - x;
                        var yd = yp - y;
                        var zd = zp - z;
                        var d = xd * xd + yd * yd + zd * zd;
                        if (d < md)
                        {
                            md = d;
                            xc = xp;
                            yc = yp;
                            zc = zp;
                        }
                    }
                }
            }
            double v;
            if (distance)
            {
                var xd = xc - x;
                var yd = yc - y;
                var zd = zc - z;
                v = (Math.Sqrt(xd * xd + yd * yd + zd * zd)) * LibNoise.Utils.Sqrt3 - 1.0;
            }
            else
            {
                v = 0.0;
            }
            return v + (displacement * LibNoise.Utils.ValueNoise3D((int)(Math.Floor(xc)), (int)(Math.Floor(yc)),
                (int)(Math.Floor(zc)), 0));
        }
    }
}
