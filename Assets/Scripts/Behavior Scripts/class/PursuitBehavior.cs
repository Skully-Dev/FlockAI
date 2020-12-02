using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Pursuit")]
public class PursuitBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Life flock)
    {
        //Ternary operators, if no ContextFilter reference was provided will use unfiltered context.
        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //filters for specific collider types 

        //if no neibours return no adjustment
        if (filteredContext.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero; //starting value of zero to be added upon. Never assume a default value.

        //Get a direction to pursue with closest targets and large flocks having more influence
        foreach (Transform item in filteredContext)
        {
            float distance = Vector2.Distance(item.position, agent.transform.position); 
            float distancePercent = distance / flock.neighborRadius; // 0 = on target, 1 = at radius
            float inverseDistancePercent = 1 - distancePercent; // 1 = on target, 0 = at radius
            float weight = inverseDistancePercent / filteredContext.Count; 

            Vector2 direction = (item.position - agent.transform.position) * weight;

            move += direction;

            if (distance < 0.5f) //if close to prey
            {
                agent.state = Life.State.Attack;
                agent.AgentAnimator.SetInteger("state", 7);
                FlockAgent itemAgent = item.GetComponent<FlockAgent>();
                itemAgent.state = Life.State.Attacked;
                itemAgent.AgentAnimator.SetInteger("state", 5);
            }
        }

        return move;
    }
}
