using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")] //now rmb project folder will allow creation of a scriptable object in Flock > Behaviou
public class AvoidanceBehavior : FilteredFlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement that flockBehaviour CalculateMove
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //its own agent, context is other agents
    {
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //getting average
        Vector2 avoidanceMove = Vector2.zero;
        int numAvoid = 0; //count of number agent is actually avoiding, number of things to avoid
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators
        foreach (Transform item in filteredContext)
        {
            if(Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                numAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position); //agent to go in opposite direction of item
            }
        }
        if (numAvoid > 0)
        {
            avoidanceMove /= numAvoid;
        }

        return avoidanceMove;
    }
}
