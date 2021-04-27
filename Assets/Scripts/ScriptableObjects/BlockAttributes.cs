using System.Collections.Generic;
using UnityEngine;

namespace VoxelWorldEngine.ScriptableObjects
{
	[CreateAssetMenu(fileName = "World Block", menuName = "Voxel World/World block")]
	public class BlockAttributes : ScriptableObject
	{
		[Tooltip("Texture position in the map.")]
		public Vector2 TexturePos;
		//[Tooltip("Color for this block.")]
		//public Color Color;

		[Tooltip("Check if the block texture is transparent.")]
		public bool IsSolid;
		[Tooltip("If the block is empty will not be rendered.")]
		public bool IsEmpty = false;

		//public Dictionary<BlockSideType, Vector2> TextureBySide = new Dictionary<BlockSideType, Vector2>();
	}
}
