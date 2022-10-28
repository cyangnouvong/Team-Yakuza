using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    Animator anim;
    Rigidbody rbody;
    float velx = 0.0f;
    float velz = 0.0f;
    public float acceleration = 5.0f;
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
        /*
        rbody.MovePosition(rbody.position + this.transform.forward * velz * Time.deltaTime * maxSpeed);
        rbody.MoveRotation(rbody.rotation * Quaternion.AngleAxis(velx * Time.deltaTime * 100, Vector3.up));
        */

        Debug.Log("vel z: " + velz);
        Debug.Log("vel x: " + velx);

        anim.SetFloat("velx", velx);
        anim.SetFloat("velz", velz);
    }

    void OnAnimatorMove()
    {
        Vector3 newRootPosition = anim.rootPosition;
        Quaternion newRootRotation = anim.rootRotation;

        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, maxSpeed);
        newRootRotation = Quaternion.LerpUnclamped(this.transform.rotation, newRootRotation, maxTurnSpeed);

        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }
}
