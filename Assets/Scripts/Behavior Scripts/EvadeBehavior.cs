using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/Evade")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class EvadeBehavior : FilteredFlockBehavior //Get the methods and vars from FilteredFlockBehaviour, which is derived from FlockBehavior so it has to implement that FlockBehaviour CalculateMove and had FilteredFlockBehavior ContextFilter reference slot.
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //agent is its own agent, context is other agents
    {
        //if no neighbors, return no adjustment.
        if (context.Count == 0)
        {
            agent.isEvade = false;
            return Vector2.zero;
        }

        //getting average of all the opposite directions to all the items that are within evade radius.
        Vector2 evadeMove = Vector2.zero; //starting value of zero to be added upon. Never assume a default value.
        int numEvade = 0; //number of things to avoid
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators, if no ContextFilter reference was provided will use unfiltered context.
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareEvadeRadius) //if the item within the evade radius (if square distance between item and agent IS LESS THAN SquareEvadeRadius)
            {
                //then
                numEvade++; //add 1 to the number of items to evade
                evadeMove += (Vector2)(agent.transform.position - item.position); //Add the opposite direction of nearby item. I.E. agent to go in opposite direction. //therefore value is already local too!
            }
        }
        if (numEvade > 0) //if any dangerous agents are within evade radius
        {
            agent.isEvade = true;
            evadeMove /= numEvade; //get the average magnitude of the sum of the evade directions
        }
        else
        {
            agent.isEvade = false;
        }

        return evadeMove;
    }
}
//Evade: aims to move away from filtered objects within evadeRadius (similar to avoidance but larger, its radius is based on neighbor radius)
//Finds the opposite direction from all Agents that are dangerous and within EvadeRadius(the sum of all opposite directions from dangerous colliders within evade radius / count = average opposite direction)
//Unless no neighbors, then return no adjustment.