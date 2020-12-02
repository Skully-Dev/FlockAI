using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/Steered Cohestion")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class SteeredCohesion : FilteredFlockBehavior //Get the methods and vars from FilteredFlockBehaviour, which is derived from FlockBehavior so it has to implement that FlockBehaviour CalculateMove and had FilteredFlockBehavior ContextFilter reference slot.
{
    /// <summary>
    /// A value it seems we dont need to use, but a variable required as the SmoothDamp wants to store a value somewhere, it sounded like this is to change velocity which we dont need.
    /// </summary>
    Vector2 currentVelocity;
    [Tooltip("How long for Agent to get from current state to calculated state. Default 0.5 seconds. NOTE: value changes every frame, so this basically scales whatever the value is.")]
    public float agentSmoothTime = 0.5f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Life flock)
    {
        //if no neibours return no adjustment
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //otherwise

        //Add all the points together and get the average
        Vector2 cohesionMove = Vector2.zero; //starting value of zero to be added upon. Never assume a default value.
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators, if no ContextFilter reference was provided will use unfiltered context.
        int count = 0;
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) <= flock.SquareSmallRadius) //class
            {
                cohesionMove += (Vector2)item.position; //add the positions of all transforms of all agent neighbors of the same flock
                count++; //class
            }
        }
        if (count != 0)
        {
            cohesionMove /= count; //the average mid-point, a global position.
        }

        //create offset from agent position //direction from a to b = b - a
        cohesionMove -= (Vector2)agent.transform.position; //The mid-point relative to the agents position.
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime); //smooth smooths out the transform adjustments, 
        return cohesionMove;
    }
    
}
//SteeredCohesion aka Smoothed Cohesion
//Like Cohesion, aims to bring nearby agents together. Just smoothed out.
//Finds the mid point between all Agents neighbors (the sum of all neighbors positions / neighbor count = Average Global Position)
//AND offsets that position relative to Agent position, returning a desired direction that is then smoothed out to reduce jitters!. Unless no neighbors, then return no adjustment.