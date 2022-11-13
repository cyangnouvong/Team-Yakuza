using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public float positionSmoothTime = 1f;		// a public variable to adjust smoothing of camera motion
    public float rotationSmoothTime = 1f;
    public float positionMaxSpeed = 50f;        //max speed camera can move
    public float rotationMaxSpeed = 50f;
    public Transform desiredPose;			// the desired pose for the camera, specified by a transform in the game
    public Transform target;
    public float speed = 0.5f;              // speed of camera rotation with right mouse button

    public static bool followPlayer = false;

    protected Vector3 currentPositionCorrectionVelocity;
    //protected Vector3 currentFacingCorrectionVelocity;
    //protected float currentFacingAngleCorrVel;
    protected Quaternion quaternionDeriv;

    protected float angle;
    
    void LateUpdate()
    {
        if (desiredPose != null && followPlayer)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPose.position, ref currentPositionCorrectionVelocity, positionSmoothTime, positionMaxSpeed, Time.deltaTime);
            var targForward = desiredPose.forward;
            if (Input.GetMouseButton(0))
            {
                // https://www.youtube.com/watch?v=FIiKuP-9KuY
                // camera view rotation by holding down right mouse button
                transform.eulerAngles += speed * new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            } else
            {
                transform.rotation = QuaternionUtil.SmoothDamp(transform.rotation,
                    Quaternion.LookRotation(targForward, Vector3.up), ref quaternionDeriv, rotationSmoothTime);
            }
        }
    }
}
