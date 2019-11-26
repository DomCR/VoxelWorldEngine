using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorldEngine.Biomes
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// - Noise info
    /// - Vegetation blocks
    /// - Ores
    /// - Cave density
    /// 
    /// DebugUtils: Assign a block to display in the height map
    /// </remarks>
    public abstract class Biome
    {
        [Header("Generic noise properties")]
        [Range(0, 999f)]
        [Tooltip("Wave length of the noise")]
        public float WidthMagnitude = 125;
        [Range(0, 999f)]
        [Tooltip("Wave height of the noise")]
        public float HeightMagnitude = 200;

        public int MaxHeight = 0;
        public int MinHeight = 0;

        public static void GetBiomePersistance(int x, int z)
        {
            //Opt 1:
            //Compute a noise
            //biome presence = noise * nBiomes
            //...

            //Opt 2:
            //Compute a generic biome noise

            //foreach biome (in range of generic noise)
            //  compute biome nosie

            //multiply the variables of each biome by it's presence
            //the biome with more presence is the active

            float generic = Mathf.PerlinNoise(x, z);

            //biomesInRange = biomes.Where(o => o.MinHeight < generic && o.MaxHeight > generic) //Biomes at the height range
            //foreach biome in biomesInRange
            //  calculate presence
            //  compute biome nosie * presence
        }

        public float ComputePresence(float noiseValue)
        {
            //noiseValue = world temperature?
            //min world tmp < noiseValue < max world tmp
            //noiseValue/maxtmp -> normalized value
            //dif = noiseValue/maxtmp - biomeTMP/maxtmp --> dif equals presence?

            throw new NotImplementedException();
        }
    }
}
