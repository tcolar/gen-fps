using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Control the player camera (lookAt), analog via Mouse or Touch motion
 */
public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    private Vector2 lookAngles;
    private Vector2 currentMouseLook;

    private int lastLookFrame;

    public static int midScreen = Screen.height / 2;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LockAndUnlockCursor();
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            if (Input.touchSupported)
            {
                TouchLookAround();
            }
            else
            {
                MouseLookAround();
            }
        }
    }

    void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void MouseLookAround()
    {
        currentMouseLook = new Vector2(
            Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

        TurnCamera(true);
    }

    void TouchLookAround()
    {
        for (int i = 0; i != Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.position.x < midScreen) continue;

            currentMouseLook = new Vector2(touch.deltaPosition.y, touch.deltaPosition.x);

            TurnCamera(false);
            return;
        }
    }

    void TurnCamera(bool isMouse)
    {
        float sensitivity = isMouse ? GameSettings.mouseSensitivity : GameSettings.touchSensitivity;
        float invert = isMouse && GameSettings.mouseInvert ? -1f : 1f;
        lookAngles.x -= currentMouseLook.x * sensitivity * invert;
        lookAngles.y += currentMouseLook.y * sensitivity;
        lookAngles.x = Mathf.Clamp(lookAngles.x, GameSettings.mouseVertMin, GameSettings.mouseVertMax);
        player.localRotation = Quaternion.Euler(lookAngles.x, lookAngles.y, 0f);
    }

}













































