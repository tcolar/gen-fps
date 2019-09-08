
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class TerrainTiles
{
    private Dictionary<string, TerrainTile> terrainItems = new Dictionary<string, TerrainTile>();
    private BiomeHelper biomes;

    public TerrainTiles(BiomeHelper biomes)
    {
        this.biomes = biomes;
    }

    public void AddTile(Vector2Int pos, float floaAltitude, Biome biome)
    {
        TerrainTile tile = new TerrainTile(pos, floaAltitude, biome);
        string key = ToKey(pos);
        biomes.AddItemsToTile(tile);
        terrainItems[key] = tile;
    }

    private string ToKey(Vector2Int pos)
    {
        string x = pos.x.ToString();
        string y = pos.y.ToString();
        return $"{x}_{y}";
    }
}

class TerrainTile
{
    public Biome biome;
    public Vector2Int pos; // alphamap pos
    public Vector2 floatPos; // pos as 0-1 range, as the terrain wants it
    public float floaAltitude; // 0 - 1 range
    public int TreeInstanceIndex; // index in terrainData.treeInstances (if any)
    public Vector2 terrainPos; // Postion in the the terrain (0-2048)
    public GameObject rock;

    public TerrainTile(Vector2Int pos, float floaAltitude, Biome biome)
    {
        this.biome = biome;
        this.floaAltitude = floaAltitude;
        this.pos = pos;
        this.floatPos = new Vector2((float)pos.x / GameSettings.terrainAlphamapRes, (float)pos.y / GameSettings.terrainAlphamapRes);
        this.terrainPos = new Vector2(floatPos.x * 2048, floatPos.y * 2048);
    }
}
