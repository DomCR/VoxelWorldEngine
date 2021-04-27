using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Noise;

[ExecuteInEditMode]
public class NoiseMapping : MonoBehaviour
{
	[Range(2, 1024)]
	public int Resolution = 256;
	public int ChunX = 16;
	public int ChunZ = 16;
	[Tooltip("The maximum height of the noise should be 255, due the limitations of the color gradient.")]
	public NoiseAttributes Noise;
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
		float stepSize = 1f / Resolution;
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

				m_texture.SetPixel(x, z, coloring.Evaluate(sample /255));
			}
		}

		m_texture.Apply();
	}
}
