using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/WanderRandom")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class WanderRandom : FlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement CalculateMove.
{
    [SerializeField]
    [Tooltip("The current desired random position inside the circle")]
    private Vector2 currentPoint = Vector2.zero;
    [SerializeField]
    [Tooltip("Size of the unit circle for random position")] 
    private float circleSize = 10f;
    [SerializeField]
    [Tooltip("How close agent needs to be considered at point")]
    private float sqrMinDistance = 0.5f;

    /// <summary>
    /// A value it seems we dont need to use, but a variable required as the SmoothDamp wants to store a value somewhere, it sounded like this is to change velocity which we dont need.
    /// </summary>
    Vector2 currentVelocity;
    [Tooltip("How long for Agent to get from current state to calculated state. Default 0.5 seconds. NOTE: value changes every frame, so this basically scales whatever the value is.")]
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //agent is its own agent, context is other agents
    {
        Vector2 wanderMove = currentPoint - (Vector2)agent.transform.position; //direction to current wander target
        //distance between agent and current target
        if (wanderMove.sqrMagnitude < sqrMinDistance) //if close to wander position
        {
            currentPoint = Random.insideUnitCircle * circleSize; //get new wander position
            wanderMove = currentPoint - (Vector2)agent.transform.position; //get direction to position
        }
        wanderMove = wanderMove.normalized; //scale back to magnitude of 1
        wanderMove = Vector2.SmoothDamp(agent.transform.up, wanderMove, ref currentVelocity, agentSmoothTime); //smooth smooths out the transform adjustments, 

        return wanderMove;
    }
}
//WanderRandom: Aims to move towards a specific random location, followed by another and so on.
//Finds direction to current wander point, if at point, gets a random new target wander point from within a circle of a specified size.
//Determines direction to new wander point. Scales magnitude to 1. Smooths out tragectory for more flowy movement. Applies to movement.