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
        public static void CreateFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount, FaceType face)
        {
            GetFace(x, y, z, vertices, triangles, faceCount, face);
        }
        public static void GetFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount, FaceType face)
        {
            switch (face)
            {
                case FaceType.Top:
                    TopFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.North:
                    TopFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.East:
                    TopFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.South:
                    TopFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.West:
                    TopFace(x, y, z, vertices, triangles, faceCount);
                    break;
                case FaceType.Bottom:
                    TopFace(x, y, z, vertices, triangles, faceCount);
                    break;
                default:
                    break;
            }
        }
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
        #endregion
        //*********************************************************************************
        public static List<Vector2> GetTexture(BlockType blockType)
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
        public static Vector2 GetTextureMapping(BlockType block, FaceType face)
        {
            switch (face)
            {
                case FaceType.Top:
                    break;
                case FaceType.North:
                    break;
                case FaceType.East:
                    break;
                case FaceType.South:
                    break;
                case FaceType.West:
                    break;
                case FaceType.Bottom:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }
        public static Color GetColor(BlockType block)
        {
            switch (block)
            {
                //case BlockType.NULL:
                //    break;
                case BlockType.GRASS_TOP:
                    return new Color(255, 125, 255);
                default:
                    return new Color(255, 255, 255);
            }
        }
        public static Vector2 GetTexturePosition(BlockType block)
        {
            int value = ((int)block) - 1;
            //24, 36
            Vector2 map = new Vector2(((int)value % 24), ((int)(35 - (value / 24))));

            return map;

            //switch (block)
            //{
            //    case BlockType.NULL:
            //        return new Vector2();
            //    case BlockType.GRASS_TOP:
            //        return new Vector2(0, 35);
            //    default:
            //        return new Vector2();
            //}
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
    }
}
