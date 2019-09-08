using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    private Vector2 lookAngles;
    private Vector2 currentMouseLook;

    private int lastLookFrame;


    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        LockAndUnlockCursor();
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
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

    void LookAround()
    {
        currentMouseLook = new Vector2(
            Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

        lookAngles.x += currentMouseLook.x * GameSettings.mouseSensitivity * (GameSettings.mouseInvert ? 1f : -1f);
        lookAngles.y += currentMouseLook.y * GameSettings.mouseSensitivity;

        lookAngles.x = Mathf.Clamp(lookAngles.x, GameSettings.mouseVertMin, GameSettings.mouseVertMax);

        player.localRotation = Quaternion.Euler(lookAngles.x, lookAngles.y, 0f);
    }
}













































