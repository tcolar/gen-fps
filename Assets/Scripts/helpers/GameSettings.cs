using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static float seed = 39807;
    public static float playerWalkSpeed = 5f;
    public static float playerRunSpeed = 15;
    public static float playerCrouchedSpeed = 2f;
    public static float playerFallSpeed = 5f;
    public static float playerJumForce = 5f;
    public static float playerStandHeight = 1.6f;
    public static float playerCrouchedHeight = 1;
    public static float mouseSensitivity = 5;
    public static float touchSensitivity = .3f;
    public static bool mouseInvert = false;
    public static float mouseVertMin = -70;
    public static float mouseVertMax = 80;
    public static float gravity = 20f;
    public static int terrainMaxAltitude = 300;
    public static int terrainHeightmapRes = 256;
    public static int terrainAlphamapRes = 256;
    public static int terrainRes = 2048;

    public static float factor_a2t = terrainRes / terrainAlphamapRes;
    public static float factor_t2a = terrainAlphamapRes / terrainRes;
    public static float factor_h2a = terrainAlphamapRes / terrainHeightmapRes;
    public static float factor_a2h = terrainHeightmapRes / terrainAlphamapRes;
}
