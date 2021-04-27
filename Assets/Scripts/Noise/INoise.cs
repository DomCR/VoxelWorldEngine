using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldEngine.Noise
{
	public interface INoise
	{
		/// <summary>
		/// Return the value of the noise.
		/// </summary>
		/// <param name="x">X world coordinate.</param>
		/// <param name="y">Y, height if the noise is in 2d it can be ignored.</param>
		/// <param name="z">Z world coordinate.</param>
		/// <returns></returns>
		float Compute(float x, float y, float z);
	}
}
