using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Transform leftFoot;
    private Transform rightFoot;

    // new variables added below:
    private float rootMovementSpeed;
    private float rootTurnSpeed;

    public GameObject townGate;
    public GameObject interactTextBox;
    public GameObject interactText;

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
    private bool closeToJumpableGround;


    private int groundContactCount = 0;

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

        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            Debug.Log("space was pressed");
            rbody.AddForce(new Vector3(0, 300f, 0), ForceMode.Impulse);
        }
    }


    void FixedUpdate()
    {
        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        float townGateDistance = -1.0f;
        if (townGate != null)
        {
            townGateDistance = Vector3.Distance(transform.position, townGate.transform.position);
        }

        if (townGateDistance != -1 && townGateDistance < 3.0f)
        {
            interactText.SetActive(true);
            interactTextBox.SetActive(true);
        } else
        {
            interactText.SetActive(false);
            interactTextBox.SetActive(false);
        }

        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        Debug.Log(anim.GetFloat("vely"));
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
            //rootMovementSpeed = 1.5f;
            rootMovementSpeed = 5f;
        }
        else
        {
            //rootMovementSpeed = 1.9f;
            rootMovementSpeed = 6.5f;
        }
        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);

        if (anim.GetFloat("vely") < 0.05f)
        {
            //rootTurnSpeed = 1.0f;
            rootTurnSpeed = 1.3f;
        }
        else
        {
            //rootTurnSpeed = 2.5f;
            rootTurnSpeed = 3.5f;
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