using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public void Awake()
    {
        UnityEngine.Random.InitState((int)GameSettings.seed);
        TerrainGenerator terrain = new TerrainGenerator();
        terrain.Generate();
    }
}
