using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Transform lookRoot;

    private bool isCrouched;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookRoot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouched = true;
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            isCrouched = false;
            Walk();
        }
        else
        {
            WalkOrSprint();
        }
    }

    void WalkOrSprint()
    {
        if (!isCrouched)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Sprint();
            }
        }
    }

    void Walk()
    {
        lookRoot.localPosition = new Vector3(0f, GameSettings.playerStandHeight, 0f);
        playerMovement.speed = GameSettings.playerWalkSpeed;
    }

    void Sprint()
    {
        lookRoot.localPosition = new Vector3(0f, GameSettings.playerStandHeight, 0f);
        playerMovement.speed = GameSettings.playerRunSpeed;
    }

    void Crouch()
    {
        lookRoot.localPosition = new Vector3(0f, GameSettings.playerCrouchedHeight, 0f);
        playerMovement.speed = GameSettings.playerCrouchedSpeed;
    }
}
