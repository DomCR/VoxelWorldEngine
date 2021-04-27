using UnityEngine;

namespace VoxelWorldEngine.ScriptableObjects
{
	[CreateAssetMenu(fileName = "World Layer", menuName = "Voxel World/World layer")]
	public class WorldLayerAttributes : ScriptableObject
	{
		public string Name = "Default";

		[Tooltip("Block of this layer")]
		public BlockAttributes Block;
	}
}
