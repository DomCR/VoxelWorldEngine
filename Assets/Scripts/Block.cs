using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelWorldEngine.ScriptableObjects;

namespace VoxelWorldEngine
{
	//[CreateAssetMenu(fileName = "Block", menuName = "Voxel World/Block")]
	public class Block
	{
		public static Block Empty(Vector3 pos, World world, Chunk owner)
		{
			BlockAttributes scr = ScriptableObject.CreateInstance<BlockAttributes>();
			scr.name = "Empty";
			scr.IsEmpty = true;
			scr.IsSolid = false;

			return new Block(scr, pos, world, owner);
		}
		public string Name = string.Empty;
		/// <summary>
		/// Position in the chunk.
		/// </summary>
		public Vector3 Position { get; }
		public bool IsSolid { get; }
		public bool IsTransparent { get; }
		public bool IsEmpty { get; }
		public Vector2 TexturePos { get; }
		//public Color Color { get; }

		private readonly World m_world;
		private readonly Chunk m_owner;
		public Block(BlockAttributes atts)
		{
			Name = atts.name;
			IsSolid = atts.IsSolid;
			TexturePos = atts.TexturePos;
			IsEmpty = atts.IsEmpty;
			//Color = atts.Color;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="atts"></param>
		/// <param name="pos">Position in the chunk.</param>
		/// <param name="world"></param>
		/// <param name="owner"></param>
		public Block(BlockAttributes atts, Vector3 pos, World world, Chunk owner) : this(atts)
		{
			m_world = world;
			m_owner = owner;
			Position = pos;
		}
		public bool HasSolidNeighbour(int x, int y, int z)
		{
			Vector3 absolutePos = this.m_owner.Position + new Vector3(x, y, z);

			Block neighbour = m_world.GetBlock((int)absolutePos.x, (int)absolutePos.y, (int)absolutePos.z);

			if (neighbour == null )
				return !m_world.RenderSides;
			else
				return neighbour.IsSolid;
		}
		/// <summary>
		/// Return the a mesh for each face that has to be drawn.
		/// </summary>
		/// <returns></returns>
		public List<CombineInstance> DrawMesh()
		{
			List<CombineInstance> instances = new List<CombineInstance>();

			//Empty blocks don't have mesh
			if (IsEmpty)
				return instances;

			if (!HasSolidNeighbour((int)Position.x, (int)Position.y, (int)Position.z + 1))
				instances.Add(createQuadInstance(BlockSideType.Front, Position));
			if (!HasSolidNeighbour((int)Position.x, (int)Position.y, (int)Position.z - 1))
				instances.Add(createQuadInstance(BlockSideType.Back, Position));
			if (!HasSolidNeighbour((int)Position.x, (int)Position.y + 1, (int)Position.z))
				instances.Add(createQuadInstance(BlockSideType.Top, Position));
			if (!HasSolidNeighbour((int)Position.x, (int)Position.y - 1, (int)Position.z))
				instances.Add(createQuadInstance(BlockSideType.Bottom, Position));
			if (!HasSolidNeighbour((int)Position.x - 1, (int)Position.y, (int)Position.z))
				instances.Add(createQuadInstance(BlockSideType.Left, Position));
			if (!HasSolidNeighbour((int)Position.x + 1, (int)Position.y, (int)Position.z))
				instances.Add(createQuadInstance(BlockSideType.Right, Position));

			return instances;
		}
		private CombineInstance createQuadInstance(BlockSideType side, Vector3 pos)
		{
			Mesh mesh = new Mesh();

			Vector3[] vertices = new Vector3[4];
			Vector3[] normals = new Vector3[4];
			Vector2[] uvs = new Vector2[4];
			int[] triangles = new int[6];

			float hUnit = m_world.TextureTiling.x;
			float vUnit = m_world.TextureTiling.y;

			// All possible UVs
			Vector2 uv00 = new Vector2(hUnit * TexturePos.x, vUnit * TexturePos.y);
			Vector2 uv01 = new Vector2(hUnit * TexturePos.x, vUnit * TexturePos.y + vUnit);
			Vector2 uv10 = new Vector2(hUnit * TexturePos.x + hUnit, vUnit * TexturePos.y);
			Vector2 uv11 = new Vector2(hUnit * TexturePos.x + hUnit, vUnit * TexturePos.y + vUnit);

			//all possible vertices 
			Vector3 p0 = new Vector3(-0.5f + pos.x, -0.5f + pos.y, 0.5f + pos.z);
			Vector3 p1 = new Vector3(0.5f + pos.x, -0.5f + pos.y, 0.5f + pos.z);
			Vector3 p2 = new Vector3(0.5f + pos.x, -0.5f + pos.y, -0.5f + pos.z);
			Vector3 p3 = new Vector3(-0.5f + pos.x, -0.5f + pos.y, -0.5f + pos.z);
			Vector3 p4 = new Vector3(-0.5f + pos.x, 0.5f + pos.y, 0.5f + pos.z);
			Vector3 p5 = new Vector3(0.5f + pos.x, 0.5f + pos.y, 0.5f + pos.z);
			Vector3 p6 = new Vector3(0.5f + pos.x, 0.5f + pos.y, -0.5f + pos.z);
			Vector3 p7 = new Vector3(-0.5f + pos.x, 0.5f + pos.y, -0.5f + pos.z);

			switch (side)
			{
				case BlockSideType.Bottom:
					vertices = new Vector3[] { p0, p1, p2, p3 };
					normals = new Vector3[] {Vector3.down, Vector3.down,
											Vector3.down, Vector3.down};
					uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
					triangles = new int[] { 3, 1, 0, 3, 2, 1 };
					break;
				case BlockSideType.Top:
					vertices = new Vector3[] { p7, p6, p5, p4 };
					normals = new Vector3[] {Vector3.up, Vector3.up,
											Vector3.up, Vector3.up};
					uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
					triangles = new int[] { 3, 1, 0, 3, 2, 1 };
					break;
				case BlockSideType.Left:
					vertices = new Vector3[] { p7, p4, p0, p3 };
					normals = new Vector3[] {Vector3.left, Vector3.left,
											Vector3.left, Vector3.left};
					uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
					triangles = new int[] { 3, 1, 0, 3, 2, 1 };
					break;
				case BlockSideType.Right:
					vertices = new Vector3[] { p5, p6, p2, p1 };
					normals = new Vector3[] {Vector3.right, Vector3.right,
											Vector3.right, Vector3.right};
					uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
					triangles = new int[] { 3, 1, 0, 3, 2, 1 };
					break;
				case BlockSideType.Front:
					vertices = new Vector3[] { p4, p5, p1, p0 };
					normals = new Vector3[] {Vector3.forward, Vector3.forward,
											Vector3.forward, Vector3.forward};
					uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
					triangles = new int[] { 3, 1, 0, 3, 2, 1 };
					break;
				case BlockSideType.Back:
					vertices = new Vector3[] { p6, p7, p3, p2 };
					normals = new Vector3[] {Vector3.back, Vector3.back,
											Vector3.back, Vector3.back};
					uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
					triangles = new int[] { 3, 1, 0, 3, 2, 1 };
					break;
			}

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;
			//mesh.colors = Enumerable.Repeat(Color, vertices.Length).ToArray();

			mesh.RecalculateBounds();

			CombineInstance instance = new CombineInstance();
			instance.mesh = mesh;
			instance.transform = m_world.Container.transform.localToWorldMatrix;

			return instance;
		}
	}
}
