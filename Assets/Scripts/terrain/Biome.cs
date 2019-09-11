using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome
{
    FOREST, // Hot Wet
    DESERT, // Hot Dry
    WOODLAND, // Normal Wet & Hot normal
    GRASSLAND, // Normal dry & Normal normal
    SNOWY, // cold wet, cold medium, high altitude
    TUNDRA, // cold & dry, high altitude
    LAVA, // low altitude
    ROCKY // Barren mountain tops
}

public class BiomeHelper
{
    private TerrainGenerator terrainGenerator;
    private Terrain terrain;
    private TerrainData terrainData;
    private Vector2Int hotSpot, wetSpot;
    private float hotRange, wetRange;
    private float tilePosJitter = 1f / GameSettings.terrainAlphamapRes;
    private TerrainResources terrainResources;

    public BiomeHelper(TerrainGenerator myTerrain)
    {
        this.terrainGenerator = new TerrainGenerator();
        this.terrain = myTerrain.terrain;
        this.terrainData = myTerrain.terrainData;
        this.terrainResources = myTerrain.terrainResources;
        var res = GameSettings.terrainAlphamapRes;
        // Randomly put a wet spot (avoid the edges to have more chances of wet/hot interactions)
        wetSpot = new Vector2Int(res / 4 + Random.Range(0, res / 2), res / 4 + Random.Range(0, res / 2));
        // Randomly put a hot spot (avoid the edges to have more chances of wet/hot interactions)
        hotSpot = new Vector2Int(res / 4 + Random.Range(0, res / 2), res / 4 + Random.Range(0, res / 2));
        // Randomize the ranges of the wet/hot effects
        hotRange = (float)GameSettings.terrainAlphamapRes * Random.Range(0.3f, 1f);
        wetRange = (float)GameSettings.terrainAlphamapRes * Random.Range(0.3f, 1f);

    }

    // Pos is the postion in the alphaMap
    public Biome GetBiomeAt(Vector2Int pos, float heigthRatio)
    {
        if (heigthRatio < 0.2f)
        { // Lava lakes
            return Biome.LAVA;
        }
        // Note: Randomize a bit the nunbers to make biome trasnstion more relaistics (not straight lines)
        heigthRatio += Random.Range(-0.1f, +0.1f);
        float hotDist = Vector2Int.Distance(pos, hotSpot) + Random.Range(-4f, +4f);
        float wetDist = Vector2Int.Distance(pos, wetSpot) + Random.Range(-4f, +4f);
        float hotNear = hotRange * 0.75f;
        float wetNear = wetRange * 0.75f;
        if (heigthRatio > 0.75f)
        {
            // Mountain tops, rocky or snowy
            return wetDist > wetRange ? Biome.ROCKY : Biome.SNOWY;
        }
        if (hotDist < hotNear)
        { // hot
            if (wetDist < wetNear) return Biome.FOREST;  // & wet
            else if (wetDist > wetRange) return Biome.DESERT; // & dry
            else return Biome.WOODLAND;
        }
        else if (hotDist > hotRange)
        { //cold
            if (wetDist > wetRange) return Biome.TUNDRA; // & dry
            else return Biome.SNOWY;
        }
        else
        { // normal
            if (wetDist < wetNear) return Biome.WOODLAND; // & dry
            else return Biome.GRASSLAND;
        }
    }

    // Add some items (tress, rocks) to a tile according to it's biome
    internal void AddItemsToTile(TerrainTile tile)
    {
        if (tile.pos.x == 0 || tile.pos.x >= GameSettings.terrainAlphamapRes - 1
        || tile.pos.y == 0 || tile.pos.y >= GameSettings.terrainAlphamapRes - 1)
        { // don't put stuff n the map's edges
            return;
        }
        var trees = terrainResources.trees;
        int rand = Random.Range(0, 40);
        if (tile.biome != Biome.LAVA && rand > 38)
        {
            AddRockToTile(tile);
            return;
        }
        switch (tile.biome)
        {
            case Biome.DESERT:
                if (rand == 0)
                {
                    AddTreeToTile(tile, trees.palmTree, rand);
                }
                break;
            case Biome.WOODLAND:
                if (rand < 6)
                {
                    AddTreeToTile(tile, rand < 3 ? trees.oakTree : trees.firTree, rand);
                }
                break;
            case Biome.FOREST:
                if (rand < 10)
                {
                    AddTreeToTile(tile, rand < 5 ? trees.oakTree : trees.firTree, rand);
                }
                break;
            case Biome.GRASSLAND:
                if (rand < 2)
                {
                    AddTreeToTile(tile, trees.oakTree, rand);
                }
                break;
            case Biome.SNOWY:
                if (rand < 4)
                {
                    AddTreeToTile(tile, trees.firTree, rand);
                }
                break;
            case Biome.TUNDRA:
                if (rand == 0)
                {
                    AddTreeToTile(tile, trees.firTree, rand);
                }
                break;
        }
    }

    internal void AddTreeToTile(TerrainTile tile, TreeInstance tree, int rand)
    {
        tree.position = new Vector3(tile.floatPos.x + Random.Range(0, tilePosJitter), 0f, tile.floatPos.y + Random.Range(0, tilePosJitter));
        terrain.AddTreeInstance(tree);
        tile.TreeInstanceIndex = terrainData.treeInstanceCount - 1;
    }

    internal void AddRockToTile(TerrainTile tile)
    {
        Vector3 pos = new Vector3(tile.terrainPos.x, tile.floaAltitude * GameSettings.terrainMaxAltitude, tile.terrainPos.y);
        GameObject rock = terrainResources.RandomRock();
        terrainGenerator.addObject(pos, terrainResources.RandomRock(), false);
        tile.rock = rock;
    }
}
