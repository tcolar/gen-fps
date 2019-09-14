using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles loading the resources
public class ResourceManager
{
    public Texture2D textureGrass;
    public Texture2D textureWater;
    public Texture2D textureSnowy;
    public Texture2D textureRocky;
    public Texture2D textureTundra;
    public Texture2D textureSand;

    public GameObject prefabTreePalm;
    public GameObject prefabTreeOak;
    public GameObject prefabTreeFir;
    public GameObject prefabTreePoplar;

    public Sprite spriteCrouchOn;
    public Sprite spriteCrouchOff;

    public MyTrees trees;
    public GameObject[] rocks;

    Shader treeShader;

    private static ResourceManager instance;

    public static ResourceManager GetInstance()
    {
        if (instance == null)
        {
            instance = new ResourceManager();
        }
        return instance;
    }

    private ResourceManager()
    {
        treeShader = Shader.Find("Nature/SpeedTree");
        textureGrass = LoadTexture("layers/grass");
        textureWater = LoadTexture("layers/water");
        textureSnowy = LoadTexture("layers/snow");
        textureRocky = LoadTexture("layers/rocky");
        textureTundra = LoadTexture("layers/tundra");
        textureSand = LoadTexture("layers/sand");

        prefabTreePalm = LoadPrefab("Prefabs/trees/Palm_tree");
        prefabTreeOak = LoadPrefab("Prefabs/trees/Oak_tree");
        prefabTreeFir = LoadPrefab("Prefabs/trees/Fir_tree");

        spriteCrouchOff = LoadSprite("ui/crouch");
        spriteCrouchOn = LoadSprite("ui/crouch_on");

        rocks = new GameObject[4] {
            LoadPrefab("Prefabs/rocks/rock1"),
            LoadPrefab("Prefabs/rocks/rock2"),
            LoadPrefab("Prefabs/rocks/rock3"),
            LoadPrefab("Prefabs/rocks/rock4"),
        };
        foreach (GameObject rock in rocks)
        {
            rock.transform.localScale = new Vector3(3, 3, 3);
        }
    }

    private Texture2D LoadTexture(string texturePath)
    {
        Texture2D texture = Resources.Load<Texture2D>(texturePath);
        if (texture == null)
        {
            throw new System.Exception("Missing texture: " + texturePath);
        }
        return texture;
    }

    private Sprite LoadSprite(string spritePath)
    {
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            throw new System.Exception("Missing sprite: " + spritePath);
        }
        return sprite;
    }

    private GameObject LoadPrefab(string prefabPath)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            throw new System.Exception("Missing prefab: " + prefabPath);
        }
        return prefab;
    }

    public void instantiateTrees(Terrain terrain, TerrainData terrainData)
    {
        MyTrees trees = new MyTrees();
        TreePrototype[] protos = new TreePrototype[] {
            createTreePrototype(prefabTreePalm),
            createTreePrototype(prefabTreeFir),
            createTreePrototype(prefabTreeOak),
        };
        terrainData.treePrototypes = protos;
        trees.palmTree = instantiateTree(0);
        trees.firTree = instantiateTree(1);
        trees.oakTree = instantiateTree(2);
        this.trees = trees;
    }

    private TreePrototype createTreePrototype(GameObject treePrefab)
    {
        TreePrototype tp = new TreePrototype();
        tp.prefab = treePrefab;
        tp.bendFactor = .2f;
        return tp;
    }

    private TreeInstance instantiateTree(int protoIndex)
    {
        TreeInstance tree = new TreeInstance();
        tree.prototypeIndex = protoIndex;
        tree.heightScale = 1f;
        tree.widthScale = 1f;
        return tree;
    }

    public GameObject RandomRock()
    {
        return rocks[UnityEngine.Random.Range(0, rocks.Length)];
    }
}

public class MyTrees
{
    public TreeInstance palmTree;
    public TreeInstance firTree;
    public TreeInstance oakTree;
}
