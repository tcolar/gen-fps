using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersHelper
{
    private int alphamapResolution;
    private Dictionary<Biome, int> layerIndex = new Dictionary<Biome, int>();
    private TerrainLayer[] layers;
    private float[,,] alphaMaps;

    public LayersHelper(int alphamapResolution, Dictionary<Biome, Texture2D> layerTextures)
    {
        this.alphamapResolution = alphamapResolution;
        this.alphaMaps = new float[alphamapResolution, alphamapResolution, layerTextures.Count];
        this.layers = new TerrainLayer[layerTextures.Count];
        int index = 0;
        // Create the layers for each biome texture
        foreach (KeyValuePair<Biome, Texture2D> keyValue in layerTextures)
        {
            TerrainLayer layer = createLayer(keyValue.Key, keyValue.Value);
            this.layers.SetValue(layer, index);
            this.layerIndex.Add(keyValue.Key, index);
            index++;
        }
    }

    // Set the biome at a given alphamap location
    public void SetAt(int x, int z, Biome name, float value = 1f)
    {
        // Not sure why those are stored "backward" but whatever ....
        this.alphaMaps[z, x, this.layerIndex[name]] = value;
    }

    private TerrainLayer createLayer(Biome name, Texture2D texture)
    {
        var layer = new TerrainLayer();
        layer.diffuseTexture = texture;
        return layer;
    }

    public float[,,] GetAlphaMaps()
    {
        return this.alphaMaps;
    }

    public TerrainLayer[] GetTerrainLayers()
    {
        return this.layers;
    }
}
