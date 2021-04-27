using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Noise;
using VoxelWorldEngine.ScriptableObjects;

namespace VoxelWorldEngine
{
	public class BiomeMerger 
	{
		private class BiomeEqualityComparer : IEqualityComparer<BiomeAttributes>
		{
			public bool Equals(BiomeAttributes b1, BiomeAttributes b2)
			{
				if (b2 == null && b1 == null)
					return true;
				else if (b1 == null || b2 == null)
					return false;
				else if (b1.name == b2.name)
					return true;
				else
					return false;
			}

			public int GetHashCode(BiomeAttributes b)
			{
				return b.name.GetHashCode();
			}
		}
		private class BiomePresence
		{
			public int Count = 1;
			public float Presence = 1;
		}
		private Dictionary<BiomeAttributes, BiomePresence> m_biomes { get; set; } = new Dictionary<BiomeAttributes, BiomePresence>(new BiomeEqualityComparer());
		public BiomeAttributes MainBiome
		{
			get
			{
				return m_biomes.FirstOrDefault(o => o.Value.Count == m_biomes.Values.Max(a => a.Count)).Key;
			}
		}
		public float ComputeHeight(float x, float y, float z)
		{
			float sample = 0;

			foreach (var item in m_biomes)
			{
				float offset = item.Key.Noise.Compute(x + sample, y, z);

				sample += (offset * item.Value.Presence);
			}

			return sample;
		}
		public void AddBiome(BiomeAttributes biome)
		{
			if (m_biomes.TryGetValue(biome, out BiomePresence presence))
			{
				presence.Count++;
			}
			else
			{
				m_biomes.Add(biome, new BiomePresence());
			}

			recalculatePresence();
		}
		private void recalculatePresence()
		{
			if (m_biomes.Count > 1)
			{

			}

			foreach (var item in m_biomes.Values)
			{
				item.Presence = (float)((float)item.Count / (float)(m_biomes.Values.Sum(i => i.Count)));
			}
		}
	}
}
