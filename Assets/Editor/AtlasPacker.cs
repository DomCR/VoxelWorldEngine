using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityTools
{
    public class AtlasPacker : EditorWindow
    {
        /// <summary>
        /// Size in pizels
        /// </summary>
        public static int BlockSize = 16;
        public static int AtlasSizeInBlocks = 16;
        public static int AtlasSize;

        Object[] rawTextures = new Object[256];
        List<Texture2D> sortedTextures = new List<Texture2D>();
        Texture2D atlas;

        [MenuItem("Voxel World/Atlas Packer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(AtlasPacker));
        }

        void OnGUI()
        {
            AtlasSize = BlockSize * AtlasSizeInBlocks;

            GUILayout.Label("Atlas Texture Packer", EditorStyles.boldLabel);

            //Allow the user to set the variables
            BlockSize = EditorGUILayout.IntField("Block Size", BlockSize);
            AtlasSizeInBlocks = EditorGUILayout.IntField("Atlas Size", AtlasSizeInBlocks);

            GUILayout.Label(atlas);

            //Load textures menu
            if (GUILayout.Button("Load Textures"))
            {
                LoadTextures();
                PackAtlas();
            }

            //Clear current atlas
            if(GUILayout.Button("Clear Textures"))
            {
                atlas = new Texture2D(AtlasSize, AtlasSize);
                Debug.Log("Atlas Packer: atlas cleared");
            }

            //Save the atlas into a file
            if(GUILayout.Button("Save Atlas"))
            {
                byte[] bytes = atlas.EncodeToPNG();

                try
                {
                    //TODO: Option to setup the name
                    File.WriteAllBytes(Application.dataPath + "/Textures/Atlas.png", bytes);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Atlas Packer ERROR: " + ex);
                }
            }
        }

        void PackAtlas()
        {
            //Setup the atlas in a single texture
            atlas = new Texture2D(AtlasSize, AtlasSize);
            Color[] pixels = new Color[AtlasSize * AtlasSize];

            for (int x = 0; x < AtlasSize; x++)
            {
                for (int y = 0; y < AtlasSize; y++)
                {
                    //Get the current block thath we are looking at.
                    int currBlockX = x / BlockSize;
                    int currBlockY = y / BlockSize;

                    int index = currBlockY * AtlasSizeInBlocks + currBlockX;

                    //Get the pixel in the current block.
                    int currPixelX = x - (currBlockX * BlockSize);
                    int currPixelY = y - (currBlockY * BlockSize);

                    if (index < sortedTextures.Count)
                        pixels[(AtlasSize - y - 1) * AtlasSize + x] = sortedTextures[index].GetPixel(x, BlockSize - y - 1);
                    else
                        pixels[(AtlasSize - y - 1) * AtlasSize + x] = new Color();
                }
            }

            atlas.SetPixels(pixels);
            atlas.Apply();
        }

        void LoadTextures()
        {
            //Reset the list
            sortedTextures.Clear();

            rawTextures = Resources.LoadAll("Atlas", typeof(Texture2D));

            int index = 0;
            foreach (Object tex in rawTextures)
            {
                Texture2D t2 = tex as Texture2D;

                if (t2.width == BlockSize && t2.height == BlockSize)
                    sortedTextures.Add((Texture2D)tex);
                else
                    Debug.Log("Atlas Packer: " + t2.name + " not loaded. Incorrect size.");

                index++;
            }

            Debug.Log("Atlas Packer: " + sortedTextures.Count + " textures loaded.");
        }
    }

}