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
        GRASS,
        STONE = BlockTextureMap.STONE,
        DIRT = BlockTextureMap.DIRT,
        WOOD_TILES = BlockTextureMap.WOOD_TILES,
        STONE_PAVEMENT = BlockTextureMap.STONE_PAVEMENT,
        IRON_BLOCK,
        BRICK,
        TNT,
        SPIDER_NET,
        FLOWER_RED,
        FLOWER_YELLOW,
        MUD = BlockTextureMap.MUD,
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
        BEDROCK = BlockTextureMap.BEDROCK,
        SAND = BlockTextureMap.SAND,
        GRABEL = BlockTextureMap.GRABEL,
        OAKTREE_LOG,

        //ROW 3
        GRASS_SPAWN = BlockTextureMap.GRASS,
    }
}
