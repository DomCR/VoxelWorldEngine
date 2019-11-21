using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine
{
    public static class Block
    {

        //*********************************************************************************
        #region Face generation methods
        public static void GetFace(int x, int y, int z, out List<Vector3> vertices, out List<int> triangles, int faceCount, FaceType face)
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            switch (face)
            {
                case FaceType.Top:
                    TopFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.North:
                    NorthFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.East:
                    EastFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.South:
                    SouthFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.West:
                    WestFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.Bottom:
                    BottomFace(x, y, z, vertices, triangles, faceCount);
                    break;
                default:
                    break;
            }
        }
        //TODO: Optimize methods in a single one (Get Face)
        public static void TopFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount)
        {
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x, y, z));

            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 1); //2
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4 + 3); //4
        }
        public static void NorthFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount)
        {
            vertices.Add(new Vector3(x + 1, y - 1, z + 1));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x, y - 1, z + 1));

            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 1); //2
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4 + 3); //4
        }
        public static void EastFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount)
        {
            vertices.Add(new Vector3(x + 1, y - 1, z));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y - 1, z + 1));

            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 1); //2
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4 + 3); //4
        }
        public static void SouthFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount)
        {
            vertices.Add(new Vector3(x, y - 1, z));
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y - 1, z));

            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 1); //2
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4 + 3); //4
        }
        public static void WestFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount)
        {
            vertices.Add(new Vector3(x, y - 1, z + 1));
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x, y - 1, z));

            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 1); //2
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4 + 3); //4
        }
        public static void BottomFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount)
        {
            vertices.Add(new Vector3(x, y - 1, z));
            vertices.Add(new Vector3(x + 1, y - 1, z));
            vertices.Add(new Vector3(x + 1, y - 1, z + 1));
            vertices.Add(new Vector3(x, y - 1, z + 1));

            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 1); //2
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4 + 3); //4
        }
        public static void AddTriangles(List<int> triangles, int faceCount)
        {
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 1); //2
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4); //1
            triangles.Add(faceCount * 4 + 2); //3
            triangles.Add(faceCount * 4 + 3); //4
        }
        #endregion
        //*********************************************************************************
        public static void PlaceNotBlock(int x, int y, int z, List<Vector3> vertices, List<int> triangles)
        {
            vertices.Add(new Vector3(x + 1f, y - 1, z + 0.5f));
            vertices.Add(new Vector3(x + 1f, y, z + 0.5f));
            vertices.Add(new Vector3(x, y, z + 0.5f));
            vertices.Add(new Vector3(x, y - 1, z + 0.5f));

            vertices.Add(new Vector3(x + 0.5f, y - 1, z));
            vertices.Add(new Vector3(x + 0.5f, y, z));
            vertices.Add(new Vector3(x + 0.5f, y, z + 1));
            vertices.Add(new Vector3(x + 0.5f, y - 1, z + 1));
        }
        //*********************************************************************************
        public static List<Vector2> GetTexture(BlockTextureMap blockType)
        {
            List<Vector2> newUV = new List<Vector2>();
            Vector2 texturePos = GetTexturePosition(blockType);
            //float tUnit = 0.0625f;
            float hUnit = 16f / 384f;
            float vUnit = 16f / 576f;

            //texturePos = new Vector2(0, 15);

            //hUnit = 32f / 128f;
            //vUnit = 32f / 128f;

            //hUnit = 0.25f;
            //vUnit = 0.25f;

            newUV.Add(new Vector2(hUnit * texturePos.x + hUnit, vUnit * texturePos.y));
            newUV.Add(new Vector2(hUnit * texturePos.x + hUnit, vUnit * texturePos.y + vUnit));
            newUV.Add(new Vector2(hUnit * texturePos.x, vUnit * texturePos.y + vUnit));
            newUV.Add(new Vector2(hUnit * texturePos.x, vUnit * texturePos.y));

            return newUV;
        }
        public static Vector2 GetTexturePosition(BlockTextureMap block)
        {
            int value = ((int)block) - 1;
            //24, 36
            Vector2 map = new Vector2(((int)value % 24), ((int)(35 - (value / 24))));

            return map;
        }
        //*********************************************************************************
        public static bool IsSolid(BlockType id)
        {
            switch (id)
            {
                case BlockType.NULL:
                case BlockType.FLOWER_RED:
                case BlockType.FLOWER_YELLOW:
                case BlockType.SPIDER_NET:
                case BlockType.GRASS_SPAWN:
                    return false;
            }

            return true;
        }
        public static bool NotBlock(BlockType id)
        {
            switch (id)
            {
                case BlockType.GRASS_SPAWN:
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsTransparent(BlockType id)
        {
            switch (id)
            {
                //AIR, TRANSPARENT, FLUID ...
                case BlockType.NULL:
                    return true;
                //All the solids
                default:
                    return false;
            }
        }
        public static bool IsFertile(BlockType id)
        {
            switch (id)
            {
                case BlockType.GRASS:
                case BlockType.DIRT:
                    return true;
                default:
                    return false;
            }
        }
    }
}
