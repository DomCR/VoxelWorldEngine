using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoxelWorldEngine
{
    public class Block
    {

        //******************************************************
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
        public static void BotFace(int x, int y, int z, List<Vector3> vertices, List<int> triangles, int faceCount)
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
        //******************************************************
    }
}
