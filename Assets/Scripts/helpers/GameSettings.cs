using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static float seed = 39807;
    public static float playerWalkSpeed = 5f;
    public static float playerRunSpeed = 50f;
    public static float playerCrouchedSpeed = 2f;
    public static float playerFallSpeed = 5f;
    public static float playerJumForce = 5f;
    public static float playerStandHeight = 1.6f;
    public static float playerCrouchedHeight = 1;
    public static float mouseSensitivity = 5;
    public static bool mouseInvert = false;
    public static float mouseVertMin = -70;
    public static float mouseVertMax = 80;
    public static float gravity = 20f;
    public static int terrainWidth = 256;
    public static int terrainMaxAltitude = 300;
    public static int terrainHeightmapRes = 256;
    public static int terrainAlphamapRes = 256;
}
