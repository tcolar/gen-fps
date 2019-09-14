using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public void Awake()
    {
        UnityEngine.Random.InitState((int)GameSettings.seed);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        ResourceManager rm = ResourceManager.GetInstance();
        TerrainGenerator terrain = new TerrainGenerator(rm);
        terrain.Generate();
    }
}
