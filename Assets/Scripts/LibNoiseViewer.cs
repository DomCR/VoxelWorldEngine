using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

[ExecuteInEditMode]
public class LibNoiseViewer : MonoBehaviour
{
    public enum NoiseType
    {
        Perlin,
        Billow,
        RidgedMultifractal,
        Voronoi,
        Mix,
        Practice
    }

    public int Resolution = 256;
    public NoiseType noise = NoiseType.Perlin;
    public float Zoom = 1f;
    public float offset = 0f;
    public float turbulence = 0f;
    public int perlinOctaves = 6;
    public double displacement = 4;
    public double frequency = 2;
    public double lacunarity = 2;
    public double persistence = 2;
    public int seed = 0;

    private Noise2D m_noiseMap = null;
    private Texture2D[] m_textures = new Texture2D[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Generate();
    }

    public void OnGUI()
    {
        int y = 0;
        foreach (string i in System.Enum.GetNames(typeof(NoiseType)))
        {
            if (GUI.Button(new Rect(0, y, 100, 20), i))
            {
                noise = (NoiseType)Enum.Parse(typeof(NoiseType), i);
                Generate();
            }
            y += 20;
        }


        frequency = double.Parse(GUI.TextField(new Rect(0, 120, 100, 20), frequency.ToString()));
        displacement = double.Parse(GUI.TextField(new Rect(0, 140, 100, 20), displacement.ToString()));
        Resolution = int.Parse(GUI.TextField(new Rect(0, 160, 100, 20), Resolution.ToString()));

        perlinOctaves = int.Parse(GUI.TextField(new Rect(0, 180, 100, 20), perlinOctaves.ToString()));
        turbulence = float.Parse(GUI.TextField(new Rect(0, 200, 100, 20), turbulence.ToString()));
        Zoom = float.Parse(GUI.TextField(new Rect(0, 220, 100, 20), Zoom.ToString()));
    }
    //**************************************************************************
    public void Generate()
    {
        // Create the module network
        ModuleBase moduleBase;
        switch (noise)
        {
            case NoiseType.Billow:
                moduleBase = new Billow();
                break;

            case NoiseType.RidgedMultifractal:
                moduleBase = new RidgedMultifractal();
                break;

            case NoiseType.Voronoi:
                // moduleBase = new Voronoi();
                //seed = UnityEngine.Random.Range (0, 100);
                moduleBase = new Voronoi(frequency, displacement, seed, false);
                break;

            case NoiseType.Mix:
                Perlin perlin = new Perlin();
                var rigged = new RidgedMultifractal();
                moduleBase = new Add(perlin, rigged);
                break;

            case NoiseType.Practice:
                var bill = new Billow();
                bill.Frequency = frequency;
                moduleBase = new Turbulence(turbulence / 10, bill);
                break;
            default:
                //var defPerlin = new Perlin(frequency, lacunarity, persistence, perlinOctaves, 0, QualityMode.High);
                var defPerlin = new Perlin();
                defPerlin.OctaveCount = perlinOctaves;
                moduleBase = defPerlin;
                break;
        }

        // Initialize the noise map
        this.m_noiseMap = new Noise2D(Resolution, Resolution, moduleBase);
        this.m_noiseMap.GeneratePlanar(
            offset + -1 * 1 / Zoom,
            offset + offset + 1 * 1 / Zoom,
            offset + -1 * 1 / Zoom,
            offset + 1 * 1 / Zoom, true);

        // Generate the textures
        this.m_textures[0] = this.m_noiseMap.GetTexture(GradientPresets.Grayscale);
        this.m_textures[0].Apply();

        this.m_textures[1] = this.m_noiseMap.GetTexture(GradientPresets.Terrain);
        this.m_textures[1].Apply();

        this.m_textures[2] = this.m_noiseMap.GetNormalMap(3.0f);
        this.m_textures[2].Apply();

        //display on plane
        GetComponent<Renderer>().material.mainTexture = this.m_textures[0];


        //write images to disk
        //File.WriteAllBytes (Application.dataPath + "/../Gray.png", m_textures [0].EncodeToPNG ());
        //File.WriteAllBytes (Application.dataPath + "/../Terrain.png", m_textures [1].EncodeToPNG ());
        //File.WriteAllBytes (Application.dataPath + "/../Normal.png", m_textures [2].EncodeToPNG ());

        //Debug.Log ("Wrote Textures out to " + Application.dataPath + "/../");	
    }
}
