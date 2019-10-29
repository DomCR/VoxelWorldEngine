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

        public int Compute(float x, float z)
        {
            //Get the height map
            int height = 0;

            switch (Method)
            {
                case NoiseMethod.UNITY_2D:
                    height += m_unityNoise(x, z);
                    break;
                default:
                    break;
            }

            return height;
        }
        public int Compute(float x, float y, float z)
        {
            throw new NotImplementedException();
        }
        //********************************************************
        private int m_unityNoise(float x, float z)
        {
            return (int)(Mathf.PerlinNoise(
                (int)(x + GapX) / WidthMagnitudeX, 
                (int)(z + GapZ) / WidthMagnitudeZ) * HeightMagnitude);
        }
    }
}
