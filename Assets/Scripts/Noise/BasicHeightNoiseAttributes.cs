using UnityEngine;

namespace VoxelWorldEngine.Noise
{
	[CreateAssetMenu(fileName = "Basic Height Noise", menuName = "Voxel World/Noise/Basic Height Noise")]
	public class BasicHeightNoiseAttributes : NoiseAttributes, INoise
	{
		[Tooltip("Maximum height of the noise.")]
		public float MaxHeight = 150f;
		[Tooltip("Minimum height of the noise.")]
		public float MinHeight = 0.0f;
		[Range(0, 1)]
		[Tooltip("Coordinate will be multiplied by this value to smooth the result.")]
		public float Smooth = 0.01f;
		[Range(1, 8)]
		[Tooltip("Number of iterations.")]
		public int Octaves = 4;
		[Tooltip("Persistance of the noise in each iteration.")]
		public float Persistence = 0.5f;

		public override float Compute(float x, float y, float z)
		{
			float height = map(MinHeight, MaxHeight, 0, 1, noiseIteration(x * Smooth, z * Smooth, Octaves, Persistence));
			return (int)height;
		}
		private float noiseIteration(float x, float z, int octaves, float persistance)
		{
			float total = 0;
			float frequency = 1;
			float amplitude = 1;
			float maxValue = 0;

			for (int i = 0; i < octaves; i++)
			{
				total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;

				maxValue += amplitude;

				amplitude *= persistance;
				frequency *= 2;
			}

			return total / maxValue;
		}
		private float map(float newmin, float newmax, float origmin, float origmax, float value)
		{
			return Mathf.Lerp(newmin, newmax, Mathf.InverseLerp(origmin, origmax, value));
		}
	}
}
