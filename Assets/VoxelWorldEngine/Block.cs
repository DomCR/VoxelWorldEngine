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
        public static List<Vector2> GetTexture(EBlock blockType)
        {
            List<Vector2> newUV = new List<Vector2>();
            Vector2 TexturePos = GetTexturePosition(blockType);
            float tUnit = 0.0625f;
            //float tUnit = 1 / 16;
            //float vUnit = 16 / 256;

            TexturePos = new Vector2(0, 15);

            newUV.Add(new Vector2(tUnit * TexturePos.x + tUnit, tUnit * TexturePos.y));
            newUV.Add(new Vector2(tUnit * TexturePos.x + tUnit, tUnit * TexturePos.y + tUnit));
            newUV.Add(new Vector2(tUnit * TexturePos.x, tUnit * TexturePos.y + tUnit));
            newUV.Add(new Vector2(tUnit * TexturePos.x, tUnit * TexturePos.y));

            //newUV.Add(new Vector2(tUnit, tUnit * 16));
            //newUV.Add(new Vector2(tUnit, tUnit * 16 + tUnit));
            //newUV.Add(new Vector2(0, tUnit * 16 + tUnit));
            //newUV.Add(new Vector2(0, tUnit * 16));

            return newUV;
        }

        public static Vector2 GetTextureMapping(EBlock block, EFace face)
        {
            switch (face)
            {
                case EFace.Top:
                    break;
                case EFace.North:
                    break;
                case EFace.East:
                    break;
                case EFace.South:
                    break;
                case EFace.West:
                    break;
                case EFace.Bottom:
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }

        public static Vector2 GetTexturePosition(EBlock block)
        {
            switch (block)
            {
                case EBlock.NULL:
                case EBlock.SOLID:
                default:
                    return new Vector2();
            }
        }

        public static bool IsTransparent(EBlock id)
        {
            switch (id)
            {
                //AIR, TRANSPARENT, FLUID ...
                case EBlock.NULL:
                    return true;
                //All the solids
                default:
                    return false;
            }
        }
    }
}
