﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/WanderWaypoints")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class WanderWaypoints : FlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement CalculateMove.
{
    [SerializeField]
    private GameObject[] waypoints;
    [SerializeField]
    [Tooltip("How close agent needs to be considered at waypoint")]
    private float sqrMinDistance = 0.5f;
    [SerializeField]
    [Tooltip("The current waypoint target")]
    private int index = 0;

    /// <summary>
    /// A value it seems we dont need to use, but a variable required as the SmoothDamp wants to store a value somewhere, it sounded like this is to change velocity which we dont need.
    /// </summary>
    Vector2 currentVelocity;
    [Tooltip("How long for Agent to get from current state to calculated state. Default 0.5 seconds. NOTE: value changes every frame, so this basically scales whatever the value is.")]
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //agent is its own agent, context is other agents
    {
        Vector2 wanderMove = waypoints[index].transform.position - agent.transform.position;
        //distance between agent current waypoint
        if (wanderMove.sqrMagnitude < sqrMinDistance) //if close to waypoint position
        {
            index = Random.Range(0, waypoints.Length); //Gets a new random target waypoint
            wanderMove = waypoints[index].transform.position - agent.transform.position; //get direction to waypoint
        }
        wanderMove = wanderMove.normalized; //scale back to magnitude of 1
        wanderMove = Vector2.SmoothDamp(agent.transform.up, wanderMove, ref currentVelocity, agentSmoothTime); //smooth smooths out the transform adjustments, 

        return wanderMove;
    }
}
//WanderWaypoint: Aims to move towards a random waypoint, followed by another and so on.
//Finds direction to current wander point, if at point, gets a new random waypoint index.
//Determines direction to new wander point. Scales magnitude to 1. Smooths out tragectory for more flowy movement. Applies to movement.