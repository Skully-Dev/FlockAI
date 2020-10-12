using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class AvoidanceBehavior : FilteredFlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement that flockBehaviour CalculateMove
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //agent is its own agent, context is other agents
    {
        //if no neighbors, return no adjustment.
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //getting average of all the opposite directions to all the items that are too close.
        Vector2 avoidanceMove = Vector2.zero; //starting value of zero to be added upon. Never assume a default value.
        int numAvoid = 0; //number of things to avoid
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators
        foreach (Transform item in filteredContext)
        {
            if(Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius) //if the item within the avoidance radius (if square distance between item and agent IS LESS THAN SquareAvoidanceRadius)
            {
                //then
                numAvoid++; //add 1 to the number of items to avoid
                avoidanceMove += (Vector2)(agent.transform.position - item.position); //Add the opposite direction of nearby item. I.E. agent to go in opposite direction. //therefore value is already local too!
            }
        }
        if (numAvoid > 0) //if any neighbours are within avoidance radius
        {
            avoidanceMove /= numAvoid; //get the average magnitude of the sum of the avoidance directions
        }

        return avoidanceMove;
    }
}
//Avoidance: aims to keep agent at distance from neighboring agents all up in its personal space.
//Finds the opposite direction from all Agents neighbors that are TOO CLOSE (the sum of all opposite directions from agents within avoidance radius / neighbor count = average opposite direction)
//Unless no neighbors, then return no adjustment.