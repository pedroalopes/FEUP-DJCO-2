using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status { idle, moving, crouching, climbingLadder }
public enum Element { Earth, Fire, Water, Wind }

public class PlayerController : MonoBehaviour
{
    public Status status;
    public Element element;
    [SerializeField]
    private LayerMask ladderLayer;
   

    Vector3 wallNormal = Vector3.zero;
    Vector3 ladderNormal = Vector3.zero;
    Vector3 pushFrom;

    PlayerMovement movement;
    PlayerInput playerInput;
    AnimateLean animateLean;

    bool canInteract;
    bool canGrabLedge;
    bool controlledSlide;

    float rayDistance;
    float slideLimit;
    float slideTime;
    float radius;
    float height;
    float halfradius;
    float halfheight;

    int wallDir = 1;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();

        if (GetComponentInChildren<AnimateLean>())
            animateLean = GetComponentInChildren<AnimateLean>();

        slideLimit = movement.controller.slopeLimit - .1f;
        radius = movement.controller.radius;
        height = movement.controller.height;
        halfradius = radius / 2f;
        halfheight = height / 2f;
        rayDistance = halfheight + radius + .1f;
    }

    /******************************* UPDATE ******************************/
    void Update()
    {
        //Updates
        UpdateInteraction();
        UpdateMovingStatus();
     


        //Check for movement updates
        CheckCrouching();
        CheckLadderClimbing();
        CheckElementChange();
    

    }

    void UpdateInteraction()
    {
        if (!canInteract)
        {
            if (movement.grounded || movement.moveDirection.y < 0)
                canInteract = true;
        }
        else if ((int)status >= 6)
            canInteract = false;
    }

    void UpdateMovingStatus()
    {
        if ((int)status <= 1)
        {
            status = Status.idle;
            if (playerInput.input.magnitude > 0.02f)
                status = Status.moving;
        }
    }


    /******************************** MOVE *******************************/
    void FixedUpdate()
    {
        switch (status)
        {
            case Status.climbingLadder:
                LadderMovement();
                break;
            default:
                DefaultMovement();
                break;
        }
    }

    void DefaultMovement()
    {
        if (playerInput.run && status == Status.crouching)
            Uncrouch();

        movement.Move(playerInput.input, playerInput.run, (status == Status.crouching));
        if (movement.grounded && playerInput.Jump())
        {
            if (status == Status.crouching)
                Uncrouch();

            movement.Jump(Vector3.up, 1f);
            playerInput.ResetJump();
        }
    }
    /*********************************************************************/

    /***************************** CROUCHING *****************************/
    void CheckCrouching()
    {
        if (!movement.grounded || (int)status > 2) return;

        if(playerInput.crouch)
        {
            if (status != Status.crouching)
                Crouch();
            else
                Uncrouch();
        }
    }

    void Crouch()
    {
        movement.controller.height = halfheight;
        status = Status.crouching;
    }

    void Uncrouch()
    {
        movement.controller.height = height;
        status = Status.moving;
    }
    /*********************************************************************/

    /************************** LADDER CLIMBING **************************/
    void LadderMovement()
    {
        Vector3 input = playerInput.input;
        Vector3 move = Vector3.Cross(Vector3.up, ladderNormal).normalized;
        move *= input.x;
        move.y = input.y * movement.walkSpeed;

        bool goToGround = false;
        goToGround = (move.y < -0.02f && movement.grounded);

        if (playerInput.Jump())
        {
            movement.Jump((-ladderNormal + Vector3.up * 2f).normalized, 1f);
            playerInput.ResetJump();
            status = Status.moving;
        }

        if (!hasObjectInfront(0.05f, ladderLayer) || goToGround)
        {
            status = Status.moving;
            Vector3 pushUp = ladderNormal;
            pushUp.y = 0.25f;

            movement.ForceMove(pushUp, movement.walkSpeed, 0.25f, true);
        }
        else
            movement.Move(move, 1f, 0f);
    }

    void CheckLadderClimbing()
    {
        if (!canInteract)
            return;
        //Check for ladder all across player (so they cannot use the side)
        bool right = Physics.Raycast(transform.position + (transform.right * halfradius), transform.forward, radius + 0.125f, ladderLayer);
        bool left = Physics.Raycast(transform.position - (transform.right * halfradius), transform.forward, radius + 0.125f, ladderLayer);

        if (Physics.Raycast(transform.position, transform.forward, out var hit, radius + 0.125f, ladderLayer) && right && left)
        {
            if (hit.normal != hit.transform.forward) return;

            ladderNormal = -hit.normal;
            if (hasObjectInfront(0.05f, ladderLayer) && playerInput.input.y > 0.02f)
            {
                canInteract = false;
                status = Status.climbingLadder;
            }
        }
    }
    /*********************************************************************/

 
   
    /*********************************************************************/

    bool hasObjectInfront(float dis, LayerMask layer)
    {
        Vector3 top = transform.position + (transform.forward * 0.25f);
        Vector3 bottom = top - (transform.up * halfheight);

        return (Physics.CapsuleCastAll(top, bottom, 0.25f, transform.forward, dis, layer).Length >= 1);
    }

    void CheckElementChange()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            element = Element.Earth;
        } else if (Input.GetKey(KeyCode.Alpha2))
        {
            element = Element.Wind;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            element = Element.Fire;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            element = Element.Water;
        }

    }
}
