using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelWorldEngine.Enums
{
    public enum NoiseType
    {
        HEIGHT_2D,
        DENSITY_POSITIVE_3D,
        DENSITY_NEGATIVE_3D
    }

    //Test enum for noise layers
    public enum NoiseLayerType
    {
        /// <summary>
        /// Return null if is superior
        /// </summary>
        LIMIT_UPPER,
        /// <summary>
        /// Return null if is inferior
        /// </summary>
        LIMIT_LOWER,
        /// <summary>
        /// Return null if is superior, example: caves
        /// </summary>
        DENSITY_POSITIVE,
        /// <summary>
        /// Return null if is inferior, example: ores
        /// </summary>
        DENSITY_NEGATIVE
    }
}
