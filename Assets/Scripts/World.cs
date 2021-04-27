using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelWorldEngine.Noise;
using VoxelWorldEngine.ScriptableObjects;

namespace VoxelWorldEngine
{
	[Serializable]
	public class World
	{
		[Serializable]
		public class WorldConfiguration
		{
			public int ChunkSizeX = 16;
			public int ChunkSizeY = 128;
			public int ChunkSizeZ = 16;
			[Range(0, 20)]
			[Tooltip("Influence of the biome in each column of the world.\n" +
				"This value slows down the world building considerably, keep the value low for a faster generation.")]
			public int BiomeInfluence = 0;
		}

		public int WorldXChunks = 1;
		public int WorldYChunks = 1;
		public int WorldZChunks = 1;

		[Space]

		public WorldConfiguration Configuration;
		public Dictionary<Vector3, Chunk> Chunks { get; set; } = new Dictionary<Vector3, Chunk>();

		[Space]

		public bool RenderSides = true;
		public Vector2 TextureTiling;
		[Tooltip("World atlas to apply at the world.")]
		public Material TextureAtlas;
		[Tooltip("Center of the world, can be a moving center.")]
		public Transform Center;

		[Tooltip("GameObject that contains the world, " +
			"the chunks and all the element in it will be placed as children in this container.")]
		public GameObject Container;

		[Space()]

		[Tooltip("Noise to create biomes in the world, this biome must return a value between 0 and 1.")]
		public NoiseAttributes WorldBiomesNoise;
		[Tooltip("Noise to create the bottom of the world, this noise defines the bottom height.")]
		public NoiseAttributes WorldBottomNoise;
		[Tooltip("Biomes in the world.")]
		public BiomeAttributes[] Biomes;
		/// <summary>
		/// Call this method to setup the variables by default.
		/// </summary>
		/// <remarks>
		/// Not assigned variables such as <see cref="TextureAtlas"/> or <see cref="Container"/> will by created using the default values.
		/// </remarks>
		public void Initialize()
		{
			Container = Container ? Container : new GameObject("World");

			Center = Center ? Center : Container.transform;

			TextureAtlas = TextureAtlas ? TextureAtlas : new Material(Shader.Find("Diffuse"));
		}
		/// <summary>
		/// Build the world with the class dimensions.
		/// </summary>
		public void BuildWorld()
		{
			BuildWorld(WorldXChunks, WorldYChunks, WorldZChunks);
		}
		/// <summary>
		/// Build the world with the given dimensions.
		/// </summary>
		/// <param name="xdim"></param>
		/// <param name="ydim"></param>
		/// <param name="zdim"></param>
		public void BuildWorld(int xdim, int ydim, int zdim)
		{
			Debug.Log("World build start.");

			for (int x = 0; x < xdim; x++)
			{
				for (int y = 0; y < ydim; y++)
				{
					for (int z = 0; z < zdim; z++)
					{
						BuildChunkAt(x, y, z);

						//yield return null;
					}
				}
			}
			Debug.Log("World build end.");
		}
		public void BuildChunkAt(int x, int y, int z)
		{
			Vector3 chunkPosition = new Vector3(x * Configuration.ChunkSizeX,
												y * Configuration.ChunkSizeY,
												z * Configuration.ChunkSizeZ);

			if (!Chunks.TryGetValue(chunkPosition, out Chunk c))
			{
				c = new Chunk(chunkPosition, this);
				Chunks.Add(chunkPosition, c);
			}
		}
		/// <summary>
		/// Redraw the chunks that have status set to DRAW.
		/// </summary>
		/// <returns></returns>
		public IEnumerator DrawWorld()
		{
			foreach (Chunk c in Chunks.Values.Where(o => o.State == ChunkState.Draw))
			{
				c.DrawChunk();
				yield return null;
			}
		}
		/// <summary>
		/// Get a block in the world, the coordinates must be absolutes.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public Block GetBlock(int x, int y, int z)
		{
			Vector3 key = new Vector3(
				(int)(x / Configuration.ChunkSizeX) * Configuration.ChunkSizeX,
				(int)(y / Configuration.ChunkSizeY) * Configuration.ChunkSizeY,
				(int)(z / Configuration.ChunkSizeZ) * Configuration.ChunkSizeZ);

			//Get the chunk where the block is
			Chunks.TryGetValue(key, out Chunk chunk);

			try
			{
				if (chunk != null)
					return chunk.Blocks[
					  (int)(x - chunk.Position.x),
					  (int)(y - chunk.Position.y),
					  (int)(z - chunk.Position.z)];
			}
			catch (Exception) { return null; }

			return null;
		}
		/// <summary>
		/// Get the biome in the current position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public BiomeAttributes GetBiome(int x, int z)
		{
			//TODO: fix the biome value generator.
			float noise = 0;

			if (WorldBiomesNoise)
			{
				noise = WorldBiomesNoise.Compute(x, 0, z);
				noise = Mathf.Lerp(0, Biomes.Length - 1, noise * Biomes.Length);
			}

			//float noise = Mathf.PerlinNoise(x, z);

			//Debug.Log(Mathf.RoundToInt(noise * Biomes.Length));
			//return Biomes[(int)(Biomes.Length * noise)];

			return Biomes[Mathf.RoundToInt(noise)];
			//return Biomes[(int)(noise *Biomes.Length)];
		}
	}
}
