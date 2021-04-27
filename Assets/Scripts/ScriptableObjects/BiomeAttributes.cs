using UnityEngine;
using VoxelWorldEngine.Noise;

namespace VoxelWorldEngine.ScriptableObjects
{
	[CreateAssetMenu(fileName = "World Biome", menuName = "Voxel World/Biome")]
	public class BiomeAttributes : ScriptableObject
	{
		[Tooltip("Noise to be applied in this biome.")]
		public NoiseAttributes Noise;
		public BlockAttributes Block;
	}
}
