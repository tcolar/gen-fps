using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Controls the players motion (8 ways), via keyboard or touch joystick
 */
public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;

    private Vector3 moveDirection;
    private float vertVelocity;
    public float speed;

    // Prevent going to the edge tiles (end of world)
    private float minPositon = GameSettings.factor_a2t;
    private float maxPosition = GameSettings.terrainRes - 2 * GameSettings.factor_a2t;

    private Vector2 curTouchBase;
    private Vector3 curDirection = new Vector3(0, 0, 0);

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        speed = GameSettings.playerFallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        moveDirection = GetMoveDirection();

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;

        ApplyGravity();

        if (cc.transform.position.x + moveDirection.x <= minPositon
             || cc.transform.position.x + moveDirection.x >= maxPosition)
        {
            moveDirection.x = 0;
        }

        if (cc.transform.position.z + moveDirection.z <= minPositon
            || cc.transform.position.z + moveDirection.z >= maxPosition)
        {
            moveDirection.z = 0;
        }

        cc.Move(moveDirection);
    }

    private Vector3 GetMoveDirection()
    {
        if (Input.touchSupported)
        {
            for (int i = 0; i != Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.position.x >= MouseLook.midScreen) continue;
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
            // no new touch event -> keep going
            return curDirection;
        }
        else
        {
            return new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));
        }
    }

    void ApplyGravity()
    {
        vertVelocity -= GameSettings.gravity * Time.deltaTime;
        PlayerJump();
        moveDirection.y = vertVelocity * Time.deltaTime;
    }

    void PlayerJump()
    {
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertVelocity = GameSettings.playerJumForce;
        }
    }
}
