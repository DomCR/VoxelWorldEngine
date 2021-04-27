using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldEngine.Utils
{
	public struct Vector3Int : IEquatable<Vector3Int>
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }
		public bool Equals(Vector3Int other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}
	}
}
