using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Noise;
using VoxelWorldEngine.ScriptableObjects;

namespace VoxelWorldEngine
{
	public enum ChunkState
	{
		Idle,
		Draw,
	}

	public class Chunk
	{
		public Block[,,] Blocks;

		public int ChunkSizeX { get; }
		public int ChunkSizeY { get; }
		public int ChunkSizeZ { get; }
		public int Influence { get; }
		/// <summary>
		/// Position in the world.
		/// </summary>
		public Vector3 Position { get { return m_gameObject.transform.position; } }
		public Transform Transform { get { return m_gameObject.transform; } }
		public ChunkState State { get; private set; }

		private readonly GameObject m_gameObject;
		private readonly MeshCollider m_collider;
		private readonly MeshFilter m_meshFilter;
		private readonly MeshRenderer m_renderer;
		private readonly World m_owner;

		public Chunk(Vector3 position, World owner)
		{
			m_owner = owner;

			//Setup the chunk variables
			ChunkSizeX = owner.Configuration.ChunkSizeX;
			ChunkSizeY = owner.Configuration.ChunkSizeY;
			ChunkSizeZ = owner.Configuration.ChunkSizeZ;
			Influence = owner.Configuration.BiomeInfluence;

			Blocks = new Block[
				owner.Configuration.ChunkSizeX,
				owner.Configuration.ChunkSizeY,
				owner.Configuration.ChunkSizeZ
				];

			//Create the chunk gameobject and set the world as the parent
			m_gameObject = new GameObject(position.ToString());
			m_gameObject.transform.position = position;
			m_gameObject.transform.parent = owner.Container.transform;

			//Add the needed componenets
			m_collider = m_gameObject.AddComponent<MeshCollider>();
			m_meshFilter = m_gameObject.AddComponent<MeshFilter>();
			m_renderer = m_gameObject.AddComponent<MeshRenderer>();

			//BuildHeight(m_owner.HeightNoise);
			BuildHeight();
		}
		public Chunk(Vector3 position, World owner, INoise noise) : this(position, owner)
		{
			BuildHeight(noise);
		}
		/// <summary>
		/// Draw the chunk in the current state.
		/// </summary>
		public void DrawChunk()
		{
			List<CombineInstance> instances = new List<CombineInstance>();
			foreach (Block b in Blocks)
			{
				if (b == null)
					continue;

				instances.AddRange(b.DrawMesh());
			}

			combineInstances(instances.ToArray());

			//Add the collider mesh
			m_collider.sharedMesh = m_meshFilter.mesh;

			State = ChunkState.Idle;
		}
		/// <summary>
		/// Build a height map using a noise method.
		/// </summary>
		public void BuildHeight()
		{
			//Build the chunk blocks
			for (int x = 0; x < ChunkSizeX; x++)
				for (int z = 0; z < ChunkSizeZ; z++)
				{
					int worldX = (int)(x + m_gameObject.transform.position.x);
					int worldZ = (int)(z + m_gameObject.transform.position.z);
					BiomeMerger biome = createChunkBiome(worldX, worldZ, Influence);

					for (int y = 0; y < ChunkSizeY; y++)
					{
						Vector3 pos = new Vector3(x, y, z);
						int worldY = (int)(y + m_gameObject.transform.position.y);
						float value = biome.ComputeHeight(worldX, worldY, worldZ);

						//TODO: Apply the noise and the biome
						if (y < value)
						{
							Blocks[x, y, z] = new Block(
								biome.MainBiome.Block,
								pos,
								m_owner,
								this);
						}
						else
						{
							Blocks[x, y, z] = Block.Empty(
								pos,
								m_owner,
								this);
						}
					}
				}

			State = ChunkState.Draw;
		}
		public void BuildHeight(INoise noise)
		{
			//Build the chunk blocks
			for (int x = 0; x < ChunkSizeX; x++)
				for (int z = 0; z < ChunkSizeZ; z++)
				{
					BiomeAttributes biome = m_owner.GetBiome(x, z);

					for (int y = 0; y < ChunkSizeY; y++)
					{
						//Vector3 pos = ;
						int worldX = (int)(x + m_gameObject.transform.position.x);
						int worldY = (int)(y + m_gameObject.transform.position.y);
						int worldZ = (int)(z + m_gameObject.transform.position.z);

						float value = noise.Compute(worldX, worldY, worldZ);

						//TODO: Apply the noise and the biome
						if (y < value)
						{
							Blocks[x, y, z] = new Block(
								biome.Block,
								new Vector3(x, y, z),
								m_owner,
								this);
						}
						else
						{
							Blocks[x, y, z] = Block.Empty(
								new Vector3(x, y, z),
								m_owner,
								this);
						}
					}
				}

			State = ChunkState.Draw;
		}
		//*******************************************************************
		private void combineInstances(CombineInstance[] instances)
		{
			//Combine all the meshes
			m_meshFilter.mesh.CombineMeshes(instances);

			m_meshFilter.mesh.RecalculateNormals();

			//Perform optimizations
			m_meshFilter.mesh.Optimize();
			m_meshFilter.mesh.OptimizeIndexBuffers();
			m_meshFilter.mesh.OptimizeReorderVertexBuffer();

			//Update the material
			m_renderer.material = m_owner.TextureAtlas;
		}
		private BiomeMerger createChunkBiome(float x, float z, int influence)
		{
			if (influence < 0)
				throw new ArgumentException("Biome influence cannot be less than 0.",nameof(influence));

			BiomeMerger wrapper = new BiomeMerger();

			for (int i = -influence; i <= influence; i++)
			{
				for (int j = -influence; j <= influence; j++)
				{
					//Surroundings
					wrapper.AddBiome(m_owner.GetBiome((int)(x + i), (int)(z + j)));
				}
			}

			return wrapper;
		}
	}
}
