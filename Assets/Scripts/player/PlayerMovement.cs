using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;

    private Vector3 moveDirection;
    private float vertVelocity;
    public float speed;

    // Prevent going to the edge tiles (end of world)
    private float minPositon = GameSettings.factor_a2t;
    private float maxPosition = GameSettings.terrainRes - 2 * GameSettings.factor_a2t;

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
