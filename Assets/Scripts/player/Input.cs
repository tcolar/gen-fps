using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Control the player camera (lookAt), analog via Mouse or Touch motion
 */
public class InputHandler
{
    private CharacterController cc;
    private PlayerCamera playerCamera;
    private PlayerMovement playerMovement;
    private Vector2 curTouchBase;
    private Vector3 curDirection = new Vector3(0, 0, 0);
    private Vector2 curLookAt = new Vector2(0, 0);

    public static int midScreen = Screen.height / 2;
    public bool isMobile;

    public InputHandler(CharacterController cc, Transform player, Transform fpCamera)
    {
        this.cc = cc;
        playerCamera = new PlayerCamera(player);
        playerMovement = new PlayerMovement(cc, player, fpCamera);
        isMobile = SystemInfo.deviceType == DeviceType.Handheld;
    }

    public void Update()
    {
        // Player Movement
        this.UpdatePlayerDirection();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerMovement.Run(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerMovement.Run(false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerMovement.Crouch(true);
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            playerMovement.Crouch(false);
        }
        playerMovement.Move(curDirection);

        // Camera direction
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerCamera.LockUnlock();
        }
        this.UpdatePlayerLookDirection();
        playerCamera.TurnCamera(curLookAt, !isMobile);
    }

    public void UpdatePlayerDirection()
    {
        if (isMobile)
        {
            for (int i = 0; i != Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.position.x >= midScreen) continue;
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // If new finger drop, reset that as the movement base
                        curTouchBase = new Vector2(touch.position.x, touch.position.y);
                        curDirection = new Vector3(0, 0, 0);
                        break;
                    case TouchPhase.Stationary:
                    case TouchPhase.Moved:
                        // Convert motion to D pad motion (8 directions)
                        curDirection.x = 0;
                        curDirection.z = 0;
                        // TODO: make 30/60 % of sreen size
                        // TODO: match running speed with keyboard one
                        // TODO: joystick overlays
                        if (touch.position.x - curTouchBase.x > 60)
                        {
                            curDirection.x = 1;
                        }
                        else if (touch.position.x - curTouchBase.x < -60)
                        {
                            curDirection.x = -1;
                        }
                        if (touch.position.y - curTouchBase.y > 200)
                        {
                            curDirection.z = 3; // running fwd
                        }
                        else if (touch.position.y - curTouchBase.y > 60)
                        {
                            curDirection.z = 1;
                        }
                        else if (touch.position.y - curTouchBase.y < -60)
                        {
                            curDirection.z = -1;
                        }
                        break;
                    case TouchPhase.Ended:
                        // Finger lifted = stop moving
                        curDirection = new Vector3(0, 0, 0);
                        break;
                }
            }
        }
        else
        {
            curDirection = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));
        }
    }


    public void UpdatePlayerLookDirection()
    {
        if (isMobile)
        {
            for (int i = 0; i != Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.position.x < midScreen) continue;

                curLookAt = new Vector2(touch.deltaPosition.y, touch.deltaPosition.x);
                return;
            }
        }
        else
        {
            curLookAt = new Vector2(Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));
        }
    }
}
