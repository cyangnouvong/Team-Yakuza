using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrivingCarScript : MonoBehaviour
{
    public static int numWaypoint;
    private NavMeshAgent agent;
    public GameObject[] waypoints = new GameObject[numWaypoint];
    int currWaypoint = -1;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= 1f && !agent.pathPending)
        {
            setNextWaypoint();
        }

    }

    private void setNextWaypoint()
    {
        if (currWaypoint == waypoints.Length - 1)
        {
            currWaypoint = 0;
            agent.SetDestination(waypoints[currWaypoint].transform.position);
        } else
        {
            currWaypoint++;
            agent.SetDestination(waypoints[currWaypoint].transform.position);
        }
    }
}
