using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public void Awake()
    {
        UnityEngine.Random.InitState((int)GameSettings.seed);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        ResourceManagerMain rm = GetComponent<ResourceManagerMain>();
        TerrainGenerator terrain = new TerrainGenerator(rm);
        terrain.Generate();
    }
}
