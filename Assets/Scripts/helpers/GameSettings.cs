using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static float seed = 9876;
    public static float playerWalkSpeed = 5f;
    public static float playerRunSpeed = 15f;
    public static float playerCrouchedSpeed = 2f;
    public static float playerFallSpeed = 5f;
    public static float playerJumForce = 8f;
    public static float playerStandHeight = 1.6f;
    public static float playerCrouchedHeight = 1;
    public static float mouseSensitivity = 5;
    public static float touchSensitivity = .3f;
    public static bool mouseInvert = false;
    public static float mouseVertMin = -70;
    public static float mouseVertMax = 80;
    public static float gravity = 20f;
    public static int terrainRes = 512;

    public static int terrainHeightmapRes = terrainRes / 8;
    public static int terrainAlphamapRes = terrainRes / 8;
    public static int terrainMaxAltitude = terrainRes / 4;

    public static float factor_a2t = terrainRes / terrainAlphamapRes;
    public static float factor_t2a = terrainAlphamapRes / terrainRes;
    public static float factor_h2a = terrainAlphamapRes / terrainHeightmapRes;
    public static float factor_a2h = terrainHeightmapRes / terrainAlphamapRes;
}
