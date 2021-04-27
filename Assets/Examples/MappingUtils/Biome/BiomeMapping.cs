using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Noise;

[ExecuteInEditMode]
public class BiomeMapping : MonoBehaviour
{
	[Range(2, 1024)]
	public int Resolution = 256;
	[Tooltip("Dimension of the chunk in the x axis.")]
	public int ChunX = 16;
	[Tooltip("Dimension of the chunk in the z axis.")]
	public int ChunZ = 16;
	[Tooltip("The biome noise must return a value between 1 and 0.")]
	public NoiseAttributes Noise;
	//[Tooltip("Number of the biomes to simulate in the grid.")]
	//public int nBiomes = 1;
	public Gradient coloring;
	public bool Refresh;
	//******************************************************
	private Texture2D m_texture;
	//******************************************************
	void Update()
	{
		if (Refresh)
		{
			m_texture = new Texture2D(Resolution, Resolution, TextureFormat.RGB24, true);
			m_texture.name = "Procedural Texture";
			m_texture.wrapMode = TextureWrapMode.Clamp;
			GetComponent<MeshRenderer>().sharedMaterial.mainTexture = m_texture;

			FillTexture();
			Refresh = false;
		}
	}
	// Update is called once per frame
	void FillTexture()
	{
		for (int x = 0; x < Resolution; x++)
		{
			for (int z = 0; z < Resolution; z++)
			{
				if (x % ChunX == 0 || z % ChunZ == 0)
				{
					m_texture.SetPixel(x, z, Color.black);
					continue;
				}

				float sample = Noise.Compute(x, 0, z);

				m_texture.SetPixel(x, z, coloring.Evaluate(sample));
			}
		}

		m_texture.Apply();
	}
}
