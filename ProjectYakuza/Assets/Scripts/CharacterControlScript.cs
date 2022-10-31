using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlScript : MonoBehaviour
{
    /*
    Animator anim;
    Rigidbody rbody;
    float velx = 0.0f;
    float velz = 0.0f;
    public float acceleration = 10.0f;
    public float deceleration = 2.0f;
    public float maxWalkVel = 0.5f;
    public float minWalkVel = -0.5f;
    public float maxSpeed = 6.0f;
    public float maxTurnSpeed = 6.0f;
    int velZHash;
    int velXHash;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        velZHash = Animator.StringToHash("velz");
        velZHash = Animator.StringToHash("velx");
    }
    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        if (!runPressed)
        {
            if (forwardPressed && velz < maxWalkVel)
            {
                velz += Time.deltaTime * acceleration;
                velz = velz > maxWalkVel ? maxWalkVel : velz;
            }
            if (leftPressed && velx > minWalkVel)
            {
                velx -= Time.deltaTime * acceleration;
                velx = velx < minWalkVel ? minWalkVel : velx;
            }
            if (rightPressed && velx < maxWalkVel)
            {
                velx += Time.deltaTime * acceleration;
                velx = velx > maxWalkVel ? maxWalkVel : velx;
            }
            if (!forwardPressed && velz > 0.0f)
            {
                velz -= Time.deltaTime * deceleration;
                velz = velz < 0.0f ? 0.0f : velz;
                Debug.Log(velz);
            }
            if (!leftPressed && velx < 0.0f)
            {
                velx += Time.deltaTime * deceleration;
                velx = velx > 0.0f ? 0.0f : velz;
            }
            if (!rightPressed && velx > 0.0f)
            {
                velx -= Time.deltaTime * deceleration;
                velx = velx < 0.0f ? 0.0f : velz;
            }
            if (!leftPressed && !rightPressed && velx != 0.0 && (velx > -0.05f && velx < 0.05f))
            {
                velz = 0.0f;
            }
        }
        Debug.Log("vel z: " + velz);
        Debug.Log("vel x: " + velx);
        anim.SetFloat("velx", velx);
        anim.SetFloat("velz", velz);
    }
    void OnAnimatorMove()
    {
        Vector3 newRootPosition;
        Quaternion newRootRotation;
        newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        //use rotational root motion as is
        newRootRotation = anim.rootRotation;
        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower
        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, maxSpeed);
        newRootRotation = Quaternion.LerpUnclamped(this.transform.rotation, newRootRotation, maxTurnSpeed);
        // old way
        //this.transform.position = newRootPosition;
        //this.transform.rotation = newRootRotation;
        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }
    void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo(0);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            anim.SetLookAtWeight(0);
        }
    }
    */
    private Animator anim;
    private Animation animation;
    private Rigidbody rbody;
    private CharacterInputController cinput;

    public float animationSpeed = 1.2f;
    public float rootMovementSpeed = 15.0f;
    public float rootTurnSpeed = 20.0f;
    private bool groundContact;


    // classic input system only polls in Update()
    // so must treat input events like discrete button presses as
    // "triggered" until consumed by FixedUpdate()...
    bool _inputActionFired = false;

    // ...however constant input measures like axes can just have most recent value
    // cached.
    float _inputForward = 0f;
    float _inputTurn = 0f;


    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    void Awake()
    {

        anim = GetComponent<Animator>();
        animation = GetComponent<Animation>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");
    }

    private void Update()
    {
        if (cinput.enabled)
        {
            _inputForward = cinput.Forward;
            _inputTurn = cinput.Turn;

            // Note that we don't overwrite a true value already stored
            // Is only cleared to false in FixedUpdate()
            // This makes certain that the action is handled!
            _inputActionFired = _inputActionFired || cinput.Action;
        }
        if (Input.GetKeyUp("space") && (IsGrounded() || groundContact))
        {
            Debug.Log("space was pressed");
            rbody.AddForce(new Vector3(0f, 350f, 0f), ForceMode.Impulse);
        }
    }

   private bool IsGrounded()
    {
        if (Physics.Raycast(rbody.transform.position, Vector3.down, 0.2f, LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("is ground");
            return true;
        } else
        {
            return false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Jumpable")
        {
            groundContact = true;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Jumpable")
        {
            groundContact = false;

        }
    }


    void FixedUpdate()
    {

        bool doButtonPress = false;
        bool doMatchToButtonPress = false;

        //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground.
        //This is good for allowing player to jump and not be frustrated that the jump button doesn't
        //work

        // TODO HANDLE BUTTON MATCH TARGET HERE
        // get info about current animation
        var animState = anim.GetCurrentAnimatorStateInfo(0);

        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("velz", _inputForward);
        anim.speed = animationSpeed;
    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;


        newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower

        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(this.transform.rotation, newRootRotation, rootTurnSpeed);

        // old way
        //this.transform.position = newRootPosition;
        //this.transform.rotation = newRootRotation;

        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo(0);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            anim.SetLookAtWeight(0);
        }
    }
}
