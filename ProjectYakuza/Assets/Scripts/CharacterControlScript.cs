using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlScript : MonoBehaviour
{
    private Animator anim;
    private Animation animation;
    private Rigidbody rbody;
    private CharacterInputController cinput;

    private float animationSpeed = 1.2f;
    private float rootMovementSpeed = 1.5f;
    private float rootTurnSpeed = 1.5f;
    private bool groundContact;

    bool _inputActionFired = false;

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
        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        anim.speed = animationSpeed;
        Debug.Log(anim.GetFloat("vely"));
    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;


        newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rootMovementSpeed = 2f;
        } else
        {
            rootMovementSpeed = 3.0f;
        }
        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);
        if (anim.GetFloat("vely") < 0.05f)
        {
            rootTurnSpeed = 1.5f;
        } else
        {
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
