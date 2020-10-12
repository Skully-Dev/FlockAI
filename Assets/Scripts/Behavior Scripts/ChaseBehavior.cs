using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/Chase")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class ChaseBehavior : FilteredFlockBehavior //Get the methods and vars from FilteredFlockBehaviour, which is derived from FlockBehavior so it has to implement that FlockBehaviour CalculateMove and had FilteredFlockBehavior ContextFilter reference slot.
{
    /// <summary>
    /// A value it seems we dont need to use, but a variable required as the SmoothDamp wants to store a value somewhere, it sounded like this is to change velocity which we dont need.
    /// </summary>
    Vector2 currentVelocity;
    [Tooltip("How long for Agent to get from current state to calculated state. Default 0.5 seconds. NOTE: value changes every frame, so this basically scales whatever the value is.")]
    public float agentSmoothTime = 0.5f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //agent is its own agent, context is other agents
    {
        //if no neibours return no adjustment
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //Add all the points together and get the average
        Vector2 chaseMove = Vector2.zero; //starting value of zero to be added upon. Never assume a default value.
        int numChase = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //filters for specific collider types
        foreach (Transform item in filteredContext)
        {
            numChase++;// add 1 to the number of items to chase
            chaseMove += (Vector2)item.position; //add the positions of all transforms of all agent neighbors of the different flocks
        }
        if (numChase > 0)
        {
            chaseMove /= numChase; //the average mid-point, a global position.
            //create offset from agent position //direction from a to b = b - a
            chaseMove -= (Vector2)agent.transform.position; //The mid-point relative to the agents position.
            chaseMove = Vector2.SmoothDamp(agent.transform.up, chaseMove, ref currentVelocity, agentSmoothTime); //smooth smooths out the transform adjustments, 
        }
        
        return chaseMove;
    }
}
//Chase: Aims to move towards the average location of agents of different flocks within neighbor radius. Smoothed out in same manner as Steered Cohesion
//Finds the mid point between all Agents neighbors of other flocks (aka prey) (the sum of all other flock prey positions / prey count = Average Global Position)
//AND offsets that position relative to Predator position, returning a desired direction that is then smoothed out to reduce jitters. Unless no prey, then return no adjustment.