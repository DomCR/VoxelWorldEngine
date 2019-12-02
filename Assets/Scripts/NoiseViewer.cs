using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.Noise.RawNoise;

[ExecuteInEditMode]
public class NoiseViewer : MonoBehaviour
{
    [Space()]
    [Range(2, 1024)]
    public int Resolution = 256;
    public int Seed;

    [Range(1, 999)]
    public float WidthAmplitude;

    public float Frequency;
    [Range(1, 8)]
    public int octaves = 1;
    [Range(1f, 4f)]
    public float lacunarity = 2f;
    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(1, 3)]
    public int dimensions = 3;
    public NoiseMethodType NoiseType;

    public Gradient coloring;

    public bool Refresh;
    //******************************************************
    private Texture2D m_texture;
    //******************************************************
    private void OnEnable()
    {
        m_texture = new Texture2D(Resolution, Resolution, TextureFormat.RGB24, true);
        m_texture.name = "Procedural Texture";
        m_texture.wrapMode = TextureWrapMode.Clamp;
        GetComponent<MeshRenderer>().material.mainTexture = m_texture;

        FillTexture();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            FillTexture();
            Refresh = false;
        }
    }
    //******************************************************
    public void FillTexture()
    {
        if (m_texture.width != Resolution)
        {
            m_texture.Resize(Resolution, Resolution);
        }

        NoiseMethod_delegate method = NoiseMap.NoiseMethods[(int)NoiseType][dimensions - 1];
        float stepSize = 1f / Resolution;
        for (int y = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++)
            {
                Vector3 point = new Vector3(x + Seed, 0, y + Seed);
                float sample = NoiseMap.Sum(method, point / WidthAmplitude, Frequency, octaves, lacunarity, persistence);

                //sample *= 0.5f;
                //sample += 0.50f;

                sample = Mathf.Lerp(0, 1.5f, sample);
                sample = Mathf.Clamp(sample, 0, 1);

                if (NoiseType == NoiseMethodType.Value)
                    sample *= 0.5f;

                //sample = Mathf.PerlinNoise(x / WidthAmplitude, y / WidthAmplitude);

                m_texture.SetPixel(x, y, coloring.Evaluate(sample));
            }
        }

        m_texture.Apply();
    }
}
