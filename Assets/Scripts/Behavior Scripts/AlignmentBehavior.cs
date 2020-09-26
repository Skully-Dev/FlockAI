using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")] //now rmb project folder will allow creation of a scriptable object in Flock > Behaviou
public class AlignmentBehavior : FilteredFlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement that flockBehaviour CalculateMove
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //its own agent, context is other agents
    {
        if (context.Count == 0)
        {
            //maintain same direction
            return agent.transform.up;
        }

        //add all directions together and average
        Vector2 alignmentMove = Vector2.zero;
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up;
        }

        return alignmentMove;
    }
}
