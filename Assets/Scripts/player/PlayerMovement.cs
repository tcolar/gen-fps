using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Controls the players motion (8 ways), where is he at
 */
public class PlayerMovement
{
    private CharacterController cc;
    private Transform player, fpCamera;

    private float vertVelocity;
    public float speed;
    private bool isCrouched;

    // Prevent going to the edge tiles (end of world)
    private float minPositon = GameSettings.factor_a2t;
    private float maxPosition = GameSettings.terrainRes - 2 * GameSettings.factor_a2t;

    public PlayerMovement(CharacterController cc, Transform player, Transform fpCamera)
    {
        this.cc = cc;
        this.player = player;
        this.fpCamera = fpCamera;
        speed = GameSettings.playerFallSpeed;
    }

    public void Move(Vector3 moveDirection)
    {
        moveDirection = player.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;

        // Apply gravity
        vertVelocity -= GameSettings.gravity * Time.deltaTime;
        moveDirection.y = vertVelocity * Time.deltaTime;

        if (cc.transform.position.x + moveDirection.x <= minPositon)
        {
            moveDirection.x = minPositon - cc.transform.position.x;
        }
        else if (cc.transform.position.x + moveDirection.x >= maxPosition)
        {
            moveDirection.x = maxPosition - cc.transform.position.x;
        }

        if (cc.transform.position.z + moveDirection.z <= minPositon)
        {
            moveDirection.z = minPositon - cc.transform.position.z;
        }
        else if (cc.transform.position.z + moveDirection.z >= maxPosition)
        {
            moveDirection.z = maxPosition - cc.transform.position.z;
        }

        cc.Move(moveDirection);
    }

    public void Jump()
    {
        if (cc.isGrounded)
        {
            vertVelocity = GameSettings.playerJumForce;
        }
    }

    public void Crouch(bool on)
    {
        if (on)
        {
            fpCamera.localPosition = new Vector3(0f, GameSettings.playerCrouchedHeight, 0f);
            speed = GameSettings.playerCrouchedSpeed;
            isCrouched = true;
        }
        else
        {
            fpCamera.localPosition = new Vector3(0f, GameSettings.playerStandHeight, 0f);
            speed = GameSettings.playerWalkSpeed;
            isCrouched = false;
        }
    }

    public void Run(bool on)
    {
        if (isCrouched) return;

        if (on)
        {
            speed = GameSettings.playerRunSpeed;
        }
        else
        {
            speed = GameSettings.playerWalkSpeed;
        }
    }
}
