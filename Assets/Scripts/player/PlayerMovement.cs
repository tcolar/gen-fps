using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;

    private Vector3 moveDirection;
    private float vertVelocity;
    public float speed;

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
        moveDirection = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f,
            Input.GetAxis(Axis.VERTICAL));
        // print("hz:" + Input.GetAxis("Horizontal"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;

        ApplyGravity();

        if (cc.transform.position.x + moveDirection.x < 0)
        {
            moveDirection.x = -cc.transform.position.x;
        }

        if (cc.transform.position.z + moveDirection.z < 0)
        {
            moveDirection.z = -cc.transform.position.z;
        }

        cc.Move(moveDirection);
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
