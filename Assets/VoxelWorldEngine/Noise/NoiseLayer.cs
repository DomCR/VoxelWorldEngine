using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Noise
{
    [Serializable]
    public class NoiseLayer
    {
        public string Name;

        public float GapX;
        public float GapZ;

        public float WidthMagnitudeX = 1;
        public float WidthMagnitudeZ = 1;
        
        public float HeightMagnitude = 1;

        public float NoiseScaleX = 1;
        public float NoiseScaleY = 1;
        public float NoiseScaleZ = 1;

        public float Density = 1;

        public NoiseType Type;
        public NoiseLayerType LayerType;
        public NoiseMethod Method;

        public float Compute(float x, float y, float z)
        {
            switch (Method)
            {
                case NoiseMethod.UNITY_2D:
                    return m_unityNoise(x, z);
                case NoiseMethod.NOISE_MAP_PERLIN_3D:
                    return m_noiseMapPerlin3D(x, y, z);
                default:
                    return 0;
                    break;
            }
        }
        //********************************************************
        private int m_unityNoise(float x, float z)
        {
            return (int)(Mathf.PerlinNoise(
                (int)(x + GapX) / WidthMagnitudeX, 
                (int)(z + GapZ) / WidthMagnitudeZ) * HeightMagnitude);
        }
        private float m_noiseMapPerlin3D(float x, float y, float z)
        {
            return RawNoise.PerlinNoise3D.Generate_01(
                (int)x / NoiseScaleX,
                (int)y / NoiseScaleY,
                (int)z / NoiseScaleZ);
        }
    }
}
