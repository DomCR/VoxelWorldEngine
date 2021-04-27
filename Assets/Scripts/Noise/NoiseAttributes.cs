using UnityEngine;

namespace VoxelWorldEngine.Noise
{
	public abstract class NoiseAttributes : ScriptableObject, INoise
	{
		public abstract float Compute(float x, float y, float z);
	}
}
