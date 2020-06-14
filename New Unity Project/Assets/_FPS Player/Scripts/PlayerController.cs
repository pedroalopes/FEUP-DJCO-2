using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ManageUserSettings;

public enum Status { idle, moving, crouching, climbingLadder }
public enum Element { Earth, Fire, Water, Wind }
public class PlayerController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string ChangeElementEvent = "";
    FMOD.Studio.EventInstance changeElement;

    [FMODUnity.EventRef]
    public string PlayerRunEvent = "";
    FMOD.Studio.EventInstance playerRun;

    [FMODUnity.EventRef]
    public string PlayerJumpEvent = "";
    FMOD.Studio.EventInstance playerJump;

    [FMODUnity.EventRef]
    public string PlayerLandEvent = "";
    FMOD.Studio.EventInstance playerLand;

    public Status status;
    public Element element;
    public bool[] AllowedElements = new bool[4];
    private Dictionary<Element, int> elementDictionary;

    [SerializeField]
    private LayerMask ladderLayer;
    public GameObject playerCollider;
    private DoorTrigger lastButton;
    public Animator anim;

    Vector3 wallNormal = Vector3.zero;
    Vector3 ladderNormal = Vector3.zero;
    Vector3 pushFrom;

    PlayerMovement movement;
    PlayerInput playerInput;
    PlayerAction playerAction;
    AnimateLean animateLean;

    bool canInteract;
    bool canGrabLedge;
    bool controlledSlide;
    bool onButton = false;
    bool jumping = false;

    float rayDistance;
    float slideLimit;
    float slideTime;
    float radius;
    float height;
    float halfradius;
    float halfheight;

    private Element oldEle;

    int wallDir = 1;

    public delegate void LaunchFireball();
    public event LaunchFireball Launched;

    public bool soundEnabled;
    private UserSettings userSettings;

    private void Start()
    {
        playerRun = FMODUnity.RuntimeManager.CreateInstance(PlayerRunEvent);
        playerJump = FMODUnity.RuntimeManager.CreateInstance(PlayerJumpEvent);
        playerLand = FMODUnity.RuntimeManager.CreateInstance(PlayerLandEvent);
        changeElement = FMODUnity.RuntimeManager.CreateInstance(ChangeElementEvent);
        playerInput = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
        playerAction = GetComponent<PlayerAction>();

        slideLimit = movement.controller.slopeLimit - .1f;
        radius = movement.controller.radius;
        height = movement.controller.height;
        halfradius = radius / 2f;
        halfheight = height / 2f;
        rayDistance = halfheight + radius + .1f;

        elementDictionary = new Dictionary<Element, int>();
        elementDictionary[Element.Earth] = 0;
        elementDictionary[Element.Water] = 1;
        elementDictionary[Element.Wind] = 2;
        elementDictionary[Element.Fire] = 3;

        userSettings = ManageUserSettings.LoadUserSettings();
        soundEnabled = userSettings.sound.getSound("playerSounds");
    }

    /******************************* UPDATE ******************************/
    void Update()
    {
        //Updates
        UpdateInteraction();
        UpdateMovingStatus();
        UpdateAnimation();
        CheckJumping();
        playerRun.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        playerJump.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        playerLand.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        changeElement.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));

        //Check for movement updates
        CheckCrouching();
        CheckLadderClimbing();
        CheckElementChange();
        CheckGround();

        UseElement();
        PickUpObject();

        //update sound status
        userSettings = ManageUserSettings.LoadUserSettings();
        soundEnabled = userSettings.sound.getSound("playerSounds");
    }

    private void UpdateAnimation()
    {
        if (playerInput.run)
            anim.SetBool("isSprinting", true);
        else
            anim.SetBool("isSprinting", false);

        if (status == Status.moving)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);


    }

    private void CheckJumping()
    {
        if (playerInput.Jump() && onButton)
        {
            lastButton.removeCollision();
            onButton = false;
        }
    }

    private void CheckGround()
    {
        if (movement.grounded)
        {
            stopJump();
            if (jumping)
            {
                playLanding();
                jumping = false;
            }
            else if (playerInput.Jump())
            {
                stopFootsteps();
                playJump();
                jumping = true;
                anim.SetTrigger("Jumping");

            }
        }
        else
        {
            jumping = true;
            stopFootsteps();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.gameObject.tag == "Button" && !onButton)
        {
            lastButton = hit.transform.gameObject.GetComponent<DoorTrigger>();
            lastButton.addCollision();
            onButton = true;
        }
        else if (hit.transform.gameObject.tag == "EndLevel")
        {
            EndLevel end = hit.transform.gameObject.GetComponent<EndLevel>();
            end.ChangeLevel();
        }
        else if (onButton && hit.transform.gameObject.tag != "Button")
        {
            lastButton.removeCollision();
            lastButton = null;
            onButton = false;
        }

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
            if (playerInput.input.magnitude > 0.02f)
            {
                status = Status.moving;
                playFootsteps();
            }
            else
            {
                status = Status.idle;
                stopFootsteps();
            }
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

        if (playerInput.crouch)
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
        stopFootsteps();
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

    /*********************************************************************/



    /*********************************************************************/
    /* ACTIONS */

    void PickUpObject()
    {
        if (playerInput.pickUpKeyDown)
            playerAction.interactWithObject();
    }

    void UseElement()
    {
        if (playerInput.elementKeyDown)
        {
            playerAction.startElement(element);
        }
        else if (playerInput.elementKeyPressed)
        {
            playerAction.chargeElement(element);
        }
        else if (playerInput.elementKeyUp)
        {
            playerAction.releaseElement(element);
        }
    }

    void CheckElementChange()
    {
        if (AllowedElements[elementDictionary[playerInput.currentElement]])
        {
            if (oldEle != element)
                playChangeElement();
            oldEle = element;
            element = playerInput.currentElement;
            //playChangeElement();

            if (oldEle == Element.Fire && playerInput.currentElement != Element.Fire)
                Launched?.Invoke();

        }
    }

    /*********************************************************************/



    /*********************************************************************/
    /* SOUND */

    void playFootsteps()
    {
        if (!jumping && !IsPlaying(playerRun) && soundEnabled)
        {
            playerRun.start();
        }
    }
    void stopFootsteps()
    {
        if (IsPlaying(playerRun))
            playerRun.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void playJump()
    {
        if (soundEnabled)
        {
            playerJump.start();
        }
    }

    void playChangeElement()
    {
        if (soundEnabled)
        {
            changeElement.start();
        }
    }

    void stopJump()
    {
        if (IsPlaying(playerJump))
            playerJump.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void playLanding()
    {
        if (soundEnabled)
        {
            playerLand.start();
        }
    }

    void stopLand()
    {
        if (IsPlaying(playerLand))
            playerLand.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
    }
}

