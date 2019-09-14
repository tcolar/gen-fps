using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
Handles Mouse / keyboard / Touch events
 */
public class InputMain : MonoBehaviour
{
    private CharacterController cc;
    private ResourceManager rm;
    private PlayerCamera playerCamera;
    private PlayerMovement playerMovement;
    private Vector2 curTouchBase;
    private Vector3 curDirection = new Vector3(0, 0, 0);
    private Vector2 curLookDir;

    private int midScreen = Screen.width / 2;
    private bool isMobile;

    private bool enableTouchUi = true; //isMobile

    // touch state
    private bool touchCrouched;
    private bool touchFiring;
    private Image crouchedImage;

    public void Awake()
    {
        rm = ResourceManager.GetInstance();
        isMobile = SystemInfo.deviceType == DeviceType.Handheld;
        cc = GameObject.FindObjectOfType<CharacterController>();
        crouchedImage = GameObject.Find("Crouch").GetComponent<Image>();
        var player = GameObject.Find("Player");
        var fpCamera = GameObject.Find("Player Camera");
        playerCamera = new PlayerCamera(player.transform);
        playerMovement = new PlayerMovement(cc, player.transform, fpCamera.transform);

        if (!enableTouchUi)
        {
            GameObject.Find("UiCanvas").SetActive(false);
        }
    }

    public void Update()
    {
        // Player Movement (D pad) via keyboard or touch
        this.UpdatePlayerDirection();

        // keyboard modes (crouch / jump / run)
        this.handleKeyboardEvents();

        playerMovement.Move(curDirection);

        // Camera aiming direction (Mouse or touch screen)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerCamera.LockUnlock();
        }
        this.UpdatePlayerLookDirection();

        if (curLookDir.magnitude != 0)
        {
            playerCamera.TurnCamera(curLookDir, !isMobile);
        }
    }

    // Keyboard events, other than AWSD
    public void handleKeyboardEvents()
    {
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
    }

    // Handle Player movement 
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

                        if (touch.position.y - curTouchBase.y > 60)
                        {
                            playerMovement.Run(touch.position.y - curTouchBase.y > 200);
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

    // Handle Player Camera aiming
    public void UpdatePlayerLookDirection()
    {
        if (isMobile)
        {
            for (int i = 0; i != Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.position.x < midScreen) continue;

                curLookDir = new Vector2(touch.deltaPosition.y, touch.deltaPosition.x);
                return;
            }
            curLookDir = new Vector2(0, 0);
        }
        else
        {
            curLookDir = new Vector2(Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));
        }
    }

    // Touch button event handlers

    public void EventJumpClicked()
    {
        playerMovement.Jump();
    }

    public void EventCrouchClicked()
    {
        touchCrouched = !touchCrouched;
        crouchedImage.sprite = touchCrouched ? rm.spriteCrouchOn : rm.spriteCrouchOff;
        playerMovement.Crouch(touchCrouched);
    }

    public void EventFire(bool on)
    {
        touchFiring = on;
    }
}
