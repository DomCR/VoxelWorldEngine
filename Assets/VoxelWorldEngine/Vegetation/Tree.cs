using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Enums;

namespace VoxelWorldEngine.Vegetation
{
    public class VoxelTree
    {
        public static void Plant(int x, int y, int z, ref BlockType[,,] arr)
        {
            int logHeight = 5;
            //Set the log
            for (int i = 0; i < logHeight; i++)
            {
                arr[x, y + i, z] = BlockType.OAKTREE_LOG;
            }
            //Set the leaves

        }
    }
}