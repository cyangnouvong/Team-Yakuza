using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class CharacterControlScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private CharacterInputController cinput;
    private CharacterStats characterStats;

    // new variables added below:
    private float rootMovementSpeed;
    private float rootTurnSpeed;

    // classic input system only polls in Update()
    // so must treat input events like discrete button presses as
    // "triggered" until consumed by FixedUpdate()...
    bool _inputActionFired = false;

    bool _inputDrawSword = false;

    bool _inputSheathSword = false;

    // ...however constant input measures like axes can just have most recent value
    // cached.
    float _inputForward = 0f;
    float _inputTurn = 0f;


    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    private bool closeToJumpableGround;


    private int groundContactCount = 0;
    private bool canJump = true;

    public GameObject loseScreen;

    public AudioSource crashSource;

    private bool swordOut = false;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");

        characterStats = GetComponent<CharacterStats>();
        if (characterStats == null)
            Debug.Log("CharacterStats could not be found");

        crashSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameManager.Instance.RigisterPlayer(characterStats);
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

            _inputDrawSword = _inputDrawSword || cinput.DrawSword;

            _inputSheathSword = _inputSheathSword || cinput.SheathSword;

        }
        //Debug.Log(canJump);
        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && canJump && anim.GetFloat("vely") < 0.05)
        {
            StartCoroutine(JumpIdle());
        }
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && canJump && anim.GetFloat("vely") > 0.05)
        {
            StartCoroutine(JumpRunning());
        }

        if (Input.GetKeyDown(KeyCode.R) && !swordOut && CollectableEquipment.swordCollected) {
            anim.SetBool("draw", true);
            anim.SetBool("sneath", false);
            swordOut = true;
        } else if (Input.GetKeyDown(KeyCode.R) && swordOut) {
            anim.SetBool("sneath", true);
            anim.SetBool("draw", false);
            swordOut = false;
        }
        
        if (Input.GetMouseButton(0)) {
            anim.SetTrigger("attack");
        }

    }

    IEnumerator JumpIdle()
    {
        canJump = false;
        anim.SetBool("isJumping", true);
        yield return new WaitForSeconds(0.5f);
        rbody.AddForce(new Vector3(0, 300, 0), ForceMode.Impulse);
        anim.SetBool("isJumping", false);
        yield return new WaitForSeconds(2.3f);
        canJump = true;
    }

    IEnumerator JumpRunning()
    {
        Debug.Log("Jump Run");
        canJump = false;
        anim.SetBool("isJumping", true);
        rbody.AddForce(new Vector3(0, 250, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isJumping", false);
        yield return new WaitForSeconds(0.7f);
        canJump = true;
    }


    void FixedUpdate()
    {
        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            ++groundContactCount;
        }

        if (collision.transform.gameObject.tag == "driving car")
        {
            loseScreen.SetActive(true);

            crashSource.Play();
        }

    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            --groundContactCount;
        }

    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        if (isGrounded)
        {
            //use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rootMovementSpeed = 0.9f;
        }
        else
        {
            rootMovementSpeed = 1.5f;
        }

        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);

        if (anim.GetFloat("vely") < 0.05f)
        {
            rootTurnSpeed = 0.5f;
        }
        else
        {
            rootTurnSpeed = 1.2f;
        }
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