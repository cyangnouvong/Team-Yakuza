using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MenuAIWalkingScript : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject[] waypoints = new GameObject[13];
    int currWaypoint = -1;
    public AIState aiState;

    public enum AIState
    {
        Forward,
        Backward
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= 0.1f && !agent.pathPending)
        {
            setNextWaypoint();
        }

    }

    private void setNextWaypoint()
    {
        switch (aiState)
        {
            case AIState.Forward:
                if (currWaypoint == waypoints.Length - 1)
                {
                    aiState = AIState.Backward;
                    currWaypoint--;
                    agent.SetDestination(waypoints[currWaypoint].transform.position);
                }
                else
                {
                    currWaypoint++;
                    agent.SetDestination(waypoints[currWaypoint].transform.position);
                }
                break;
            case AIState.Backward:
                if (currWaypoint == 0)
                {
                    aiState = AIState.Forward;
                    currWaypoint++;
                    agent.SetDestination(waypoints[currWaypoint].transform.position);
                }
                else
                {
                    currWaypoint--;
                    agent.SetDestination(waypoints[currWaypoint].transform.position);
                }
                break;
        }
    }
}
