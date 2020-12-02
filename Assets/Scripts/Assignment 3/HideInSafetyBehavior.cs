using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/HideInSafety")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class HideInSafetyBehavior : FilteredFlockBehavior //Get the methods and vars from FilteredFlockBehaviour, which is derived from FlockBehavior so it has to implement that FlockBehaviour CalculateMove and had FilteredFlockBehavior ContextFilter reference slot.
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Life flock) //agent is its own agent, context is other agents
    {

        Vector2 SafetyDistance = Vector2.zero; //starting value of zero to be added upon. Never assume a default value. // This is so C# knows there is always a value, its basically a default.

        //if(filter == null){ filteredContext = context } else { filteredContext = filter.Filter(agent, context)}
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context); //Ternary operators, if no ContextFilter reference was provided will use unfiltered context.

        if (filteredContext.Count == 0)
        {
            return Vector2.zero; //return no change
        }
        else if (filteredContext.Count == 1)
        {
            SafetyDistance = filteredContext[0].position - agent.transform.position;
            if (SafetyDistance.sqrMagnitude <= 0.2)
            {
                agent.state = Life.State.Hidden;
                agent.AgentAnimator.SetInteger("state", 4);
            }
            return SafetyDistance.normalized;
        }
        else
        {
            float bestDistance = 0f;
            Vector2 tempDistanceVector;
            float tempDistanceFloat;
            foreach (Transform item in filteredContext)
            {
                tempDistanceVector = item.position - agent.transform.position;
                tempDistanceFloat = tempDistanceVector.sqrMagnitude;
                if (bestDistance == 0f) //not set yet
                {
                    bestDistance = tempDistanceFloat;
                    SafetyDistance = tempDistanceVector;
                }
                else if (bestDistance > tempDistanceFloat) //if closer that cur best
                {
                    bestDistance = tempDistanceFloat;
                    SafetyDistance = tempDistanceVector;
                }
            }
            return SafetyDistance.normalized;
        }

    }
}
