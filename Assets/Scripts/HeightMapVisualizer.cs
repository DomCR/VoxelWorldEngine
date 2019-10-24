using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelWorldEngine.NoiseVariations;

//[ExecuteInEditMode]
public class HeightMapVisualizer : MonoBehaviour
{
    public enum NoiseMethodType
    {
        Value,
        Perlin
    }
    //******************************************************
    public int Seed;
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

    [Space()]
    [Range(2, 1024)]
    public int Resolution = 256;

    public Gradient coloring;

    private Texture2D m_texture;

    public bool Refresh = false;
    //******************************************************
    private void OnEnable()
    {
        m_texture = new Texture2D(Resolution, Resolution, TextureFormat.RGB24, true);
        m_texture.name = "Procedural Texture";
        m_texture.wrapMode = TextureWrapMode.Clamp;
        GetComponent<MeshRenderer>().material.mainTexture = m_texture;

        FillTexture();
    }
    private void Update()
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

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        Random.InitState(Seed);

        NoiseMethod method = NoiseMap.noiseMethods[(int)NoiseType][dimensions - 1];
        float stepSize = 1f / Resolution;
        for (int y = 0; y < Resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            for (int x = 0; x < Resolution; x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                float sample = NoiseMap.Sum(method, point, Frequency, octaves, lacunarity, persistence);
                if (NoiseType != NoiseMethodType.Value)
                {
                    sample = sample * 0.5f + 0.5f;
                }
                m_texture.SetPixel(x, y, coloring.Evaluate(sample));
            }
        }

        m_texture.Apply();
    }
    //******************************************************
}
