using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Steered Cohestion")]
public class SteeredCohesion : FilteredFlockBehavior
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neibours return no adjustment
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //Add all the points together and average
        Vector2 cohesionMove = Vector2.zero;
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;//add them all togehter
        }
        cohesionMove /= context.Count;//dvide by the count, avrg

        //create offset
        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime); //smooth smooths out the transform adjustments, 
        return cohesionMove;
    }
    
}
