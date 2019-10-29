using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldEngine.Enums
{
    public enum BlockType : byte
    {
        NULL,
        //ROW 1
        GRASS_TOP,
        STONE,
        DIRT,
        GRASS_SIDE,
        WOOD_TILES,
        STONE_PAVEMENT,
        IRON_BLOCK,
        BRICK,
        TNT_SIDE,
        TNT_TOP,
        TNT_BOTTOM,
        SPIDER_NET,
        FLOWER_RED,
        FLOWER_YELLOW,
        MUD,
        SAPLING_OAK,
        BRICK_HALF,
        SANDSTONE_HALF,
        COBBLESTONE_HALF,
        
        ICON_BED,
        ICON_DOOR,
        ICON_WOOD_STONE,
        ICON_STAIR_WOOD_STONE,
        ICON_MULTYCOLOR,

        //ROW 2
        COBBLESTONE,
        BEDROCK,
        SAND,
        GRABEL,
        OAKTREE_SIDE,
        OAKTREE_TOP
    }
}
