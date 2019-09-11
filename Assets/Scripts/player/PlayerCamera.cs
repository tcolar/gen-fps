using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Control the player camera (what they lookAt)
*/
public class PlayerCamera
{
    private Vector2 lookAngles;
    private Transform player;

    public PlayerCamera(Transform player)
    {
        this.player = player;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LockUnlock()
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

    public void TurnCamera(Vector3 currentMouseLook, bool isMouse)
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float sensitivity = isMouse ? GameSettings.mouseSensitivity : GameSettings.touchSensitivity;
        float invert = isMouse && GameSettings.mouseInvert ? -1f : 1f;
        lookAngles.x -= currentMouseLook.x * sensitivity * invert;
        lookAngles.y += currentMouseLook.y * sensitivity;
        lookAngles.x = Mathf.Clamp(lookAngles.x, GameSettings.mouseVertMin, GameSettings.mouseVertMax);
        player.transform.localRotation = Quaternion.Euler(lookAngles.x, lookAngles.y, 0f);
    }
}
