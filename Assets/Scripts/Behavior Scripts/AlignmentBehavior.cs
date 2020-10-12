using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class AlignmentBehavior : FilteredFlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement that flockBehaviour CalculateMove
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //agent is its own agent, context is other agents
    {
        //if no neighbors, maintain current direction.
        if (context.Count == 0)
        {
            return agent.transform.up; //will have forward motion, even without neighbors.
        }

        //add all directions together and get the average
        Vector2 alignmentMove = Vector2.zero; //starting value of zero to be added upon. Never assume a default value.
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up; //add the facing direction of all agent neighbors of the same flock
        }
        alignmentMove /= context.Count; //the average facing direction with magnitude average. Needs to be averaged or will reach crazy speeds when lots of neighbors present (this is how the vector2 will be applied to agent)

        return alignmentMove;
    }
}
//Alignment: aims to align all agents facing direction
//Alignment aims to get each agent to move in a common direction, each agent will set its heading to the average of all its neighbours.
//Finds the average facing direction between all Agents neighbours (the sum of all neighbors transform.up / neighbor count = a normalised average facing direction)
//returning a desired facing direction. Unless no neighbours, then return current facing direction.