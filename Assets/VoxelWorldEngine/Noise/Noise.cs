using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;
using VoxelWorldEngine.Noise.RawNoise;

namespace VoxelWorldEngine.Noise
{
    [Serializable]
    public abstract class Noise
    {
        public string Name;

        public float WidthMagnitudeX;
        public float WidthMagnitudeZ;
        public float HeightMagnitude;
        public float NoiseScaleX;
        public float NoiseScaleY;
        public float NoiseScaleZ;

        public bool IsNegative;
        public float Density;

        public NoiseType Type;

        public NoiseLayer[] Layers;

        public Noise()
        {

        }

        public void LoadNoiseFromFile()
        {
            throw new NotImplementedException();
        }

        //Method scheme
        public void ComputeHeight(float x, float y, float z)
        {
            //Get the noise layers: (foreach)
            //Compute in order (stablish layer priority)
            float limit_upper = 0;
            float limit_lower = 0;
            float density_positive = 0;
            float density_negative = 0;

            foreach (NoiseLayer layer in Layers)
            {
                switch (layer.LayerType)
                {
                    case NoiseLayerType.LIMIT_UPPER:
                        limit_upper += layer.Compute(x, z);
                        break;
                    case NoiseLayerType.LIMIT_LOWER:
                        limit_lower -= layer.Compute(x, z);
                        break;
                    case NoiseLayerType.DENSITY_POSITIVE:
                        density_positive += layer.Compute(x, y, z);
                        break;
                    case NoiseLayerType.DENSITY_NEGATIVE:
                        density_negative += layer.Compute(x, y, z);
                        break;
                    default:
                        break;
                }
            }
        }

        public abstract float Compute(float x, float z);
        public abstract float Compute(float x, float y, float z);
    }

    public class Generator
    {
        Noise[] noises;

        int ChunksX;
        int ChunksY;
        int ChunksZ;

        public float WidthMagnitude { get; private set; }
        public float HeightMagnitude { get; private set; }
        public float Threshold { get; private set; }
        public int NoiseScale { get; private set; }

        public BlockType GetWorldBlock(Vector3 pos)
        {
            //Check edges
            if (pos.z < 0 || pos.y < 0 || pos.x < 0 ||
                (int)pos.x >= ChunksX * Chunk.XSize ||
                (int)pos.y >= ChunksY * Chunk.YSize ||
                (int)pos.z >= ChunksZ * Chunk.ZSize)
                return BlockType.NULL;

            //Set the bottom edge blocks 
            int height_endBlock = (int)(Mathf.PerlinNoise(
                pos.x / WidthMagnitude * 50f,
                pos.z / WidthMagnitude * 50f) * 10);
            if (pos.y < height_endBlock)
                return BlockType.STONE;

            #region layer (example)
            //Compute the upper height
            int upper_limit = (int)(Mathf.PerlinNoise(
                (int)pos.x / WidthMagnitude,
                (int)pos.z / WidthMagnitude) * HeightMagnitude);
            if ((int)pos.y > upper_limit)
                return BlockType.NULL;

            //Compute the lower height
            int lower_limit = 0;
            if ((int)pos.y < lower_limit)
                return BlockType.NULL;

            //Apply the layer density
            if (PerlinNoise3D.Generate_01(
                (int)pos.x / NoiseScale,
                (int)pos.y / NoiseScale,
                (int)pos.z / NoiseScale) >= Threshold)
                return BlockType.GRASS_TOP;
            else
                return BlockType.NULL;
            #endregion
        }
    }
}
