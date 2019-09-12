using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Control the player camera (lookAt), analog via Mouse or Touch motion
 */
public class PlayerMain : MonoBehaviour
{
    private InputHandler inputHandler;

    void Awake()
    {
        CharacterController cc = GetComponent<CharacterController>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Transform fpCamera = transform.GetChild(0);
        inputHandler = new InputHandler(cc, transform, fpCamera);
    }

    void Update()
    {
        inputHandler.Update();
    }
}













































