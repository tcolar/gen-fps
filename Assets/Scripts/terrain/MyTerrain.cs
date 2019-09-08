using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTerrain : MonoBehaviour
{
    public Terrain terrain;
    public TerrainData terrainData;
    public TerrainResources terrainResources;

    void Awake()
    {
        UnityEngine.Random.InitState((int)GameSettings.seed);

        Vector3 terrainSize = new Vector3(GameSettings.terrainAlphamapRes, GameSettings.terrainMaxAltitude, GameSettings.terrainAlphamapRes);

        terrainData = new TerrainData();
        terrainData.size = terrainSize;
        terrainData.heightmapResolution = GameSettings.terrainHeightmapRes;
        terrainData.alphamapResolution = GameSettings.terrainAlphamapRes + 1;
        float terrainCoordMax = terrainData.bounds.max.x;

        GameObject terrainObject = Terrain.CreateTerrainGameObject(terrainData);
        terrain = terrainObject.GetComponent<Terrain>();

        terrainResources = new TerrainResources(terrain, terrainData);
        this.generateBiomes();

        //ApplyFogSettings();
        ApplyGrassAndTreesSettings();

        terrain.Flush();
        terrainObject.SetActive(true);
    }

    // A mapping of Biomes to textures
    private Dictionary<Biome, Texture2D> GetLayers()
    {
        Dictionary<Biome, Texture2D> layers = new Dictionary<Biome, Texture2D>();
        layers[Biome.GRASSLAND] = terrainResources.textureGrass;
        layers[Biome.SNOWY] = terrainResources.textureSnowy;
        layers[Biome.LAVA] = terrainResources.textureWater;
        layers[Biome.ROCKY] = terrainResources.textureRocky;
        layers[Biome.TUNDRA] = terrainResources.textureTundra;
        layers[Biome.DESERT] = terrainResources.textureSand;
        layers[Biome.WOODLAND] = terrainResources.textureGrass;
        layers[Biome.FOREST] = terrainResources.textureGrass;
        return layers;
    }

    // Generate "random" Biomes for the alphamap, based on Perlin noise
    private void generateBiomes()
    {
        // Create the layers with the right texture for each biome
        Dictionary<Biome, Texture2D> layers = GetLayers();

        LayersHelper layersHelper = new LayersHelper(GameSettings.terrainAlphamapRes, layers);
        terrainData.terrainLayers = layersHelper.GetTerrainLayers();

        // Gnenerate an terrainData.heigthMap using Perlin Noise

        generateHeightsMap();

        // Map the terrain to layers according to biomes (based on altitude, temperature & humidity)
        BiomeHelper biomes = new BiomeHelper(this);
        TerrainTiles tiles = new TerrainTiles(biomes);

        Biome biome;
        int hmx, hmz;
        float height;
        float jitter = 1f / GameSettings.terrainAlphamapRes;
        for (int z = 0; z < GameSettings.terrainAlphamapRes; z++)
        {
            for (int x = 0; x < GameSettings.terrainAlphamapRes; x++)
            {
                hmx = (int)(x * GameSettings.factor_h2a);
                hmz = (int)(z * GameSettings.factor_h2a);
                height = terrainData.GetHeight(hmx, hmz) / GameSettings.terrainMaxAltitude;

                var pos = new Vector2Int(x, z);
                biome = biomes.GetBiomeAt(pos, height);

                layersHelper.SetAt(x, z, biome);

                if (height < 0.5f)
                {
                    height = 0.5f;
                    // Flaten the low lands, to get to ~50% of flat areas (easier to build on) 
                    float[,] map = new float[1, 1];
                    map[0, 0] = height;
                    terrainData.SetHeights(hmx, hmz, map);
                }

                // Create a tile (at alphamap location) with items on it according to it's biome
                tiles.AddTile(pos, height, biome);
            }
        }
        terrainData.SetAlphamaps(0, 0, layersHelper.GetAlphaMaps());
        print("Tree count:" + terrainData.treeInstanceCount);
    }

    // Generate terrain height map using Perlin noise
    private void generateHeightsMap()
    {
        float waveFreq1 = (30 + GameSettings.seed % 60) / 100;
        NoiseMapGeneration noiseMapGeneration = GetComponent<NoiseMapGeneration>();
        Wave[] waves = new Wave[] {
            new Wave(GameSettings.seed, waveFreq1, 1f),
        };
        var heights = noiseMapGeneration.GenerateNoiseMap(
            GameSettings.terrainAlphamapRes,
            GameSettings.terrainAlphamapRes,
            20f, waves);
        terrainData.SetHeights(0, 0, heights);
    }

    // Add an object (grounded) at the given terrain coordinates
    public void addObject(Vector3 pos, GameObject prefab, bool applyGravity)
    {
        float objHeight = terrainData.GetHeight((int)(pos.x * GameSettings.factor_t2a), (int)(pos.z * GameSettings.factor_t2a));
        // Create the object
        var obj = Instantiate(prefab, pos, Quaternion.identity);

        // We want the obkect to obey gravity ...
        if (applyGravity)
        {
            Rigidbody body = obj.AddComponent<Rigidbody>();
            body.GetComponent<MeshCollider>().convex = true;
            obj.isStatic = false;
            body.useGravity = true;
        }
    }

    // Flatten land at the given location (typically so we can build on it)
    private void flatten(float x, float z, int xWidth, int zWidth)
    {
        // Flatten the terrain at a given location
        float[,] map = new float[xWidth, zWidth];
        int hmx = (int)(x * GameSettings.factor_a2t);
        int hmz = (int)(z * GameSettings.factor_a2t);
        float hmy = terrainData.GetHeight(hmx, hmz) / GameSettings.terrainMaxAltitude;
        for (int i = 0; i != xWidth; i++)
        {
            for (int j = 0; j != zWidth; j++)
            {
                map[i, j] = hmy;
            }
        }
        terrainData.SetHeights((int)hmx, (int)hmz, map);
    }

    // The terain bounds
    public Bounds GetBounds()
    {
        return terrainData.bounds;
    }

    public void ApplyFogSettings()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 250;
        RenderSettings.fogEndDistance = 2000;
    }

    public void ApplyGrassAndTreesSettings()
    {
        //terrainData.wavingGrassAmount = 10;
        //terrainData.wavingGrassSpeed = 5;
        //terrainData.wavingGrassStrength = 5;
        terrain.treeDistance = 2000;
        terrain.treeBillboardDistance = 500;

    }
}


