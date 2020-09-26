using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")] //now rmb project folder will allow creation of a scriptable object in Flock > Behaviou
public class CohesionBehavior : FilteredFlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement that flockBehaviour CalculateMove
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) //its own agent, context is other agents
    {
        if(context.Count == 0)
        {
            return Vector2.zero;
        }

        //add all points together and get the average
        Vector2 cohesionMove = Vector2.zero; //because never assume a default
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        //create offset from agent position
        //direction from a to b = b - a
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }
}
//Cohesion brings everyone together