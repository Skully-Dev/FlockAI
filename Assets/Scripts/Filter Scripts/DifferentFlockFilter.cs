using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu (menuName = "Flock/Filter/Different Flock")]//now rmb project folder will allow creation of a scriptable object in Flock > Filter
public class DifferentFlockFilter : ContextFilter //Derived from ContextFilter, therefore requires the filter original context method.
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>(); //to be populated with agents of other flocks
        foreach (Transform item in original)//for each Transform(gameObjects with colliders) near flock agent
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>(); //if an agent, will refer to it, else = null.
            if (itemAgent != null) // && itemAgent.AgentFlock == agent.AgentFlock) //therefore if FlockAgent
            {
                if (itemAgent.AgentFlock != agent.AgentFlock) //if NOT part of the same flock
                {
                    filtered.Add(item); //add it to filtered list
                }
            }
        }
        return filtered; //The surrounding agents of different flock
    }
}
//A Filtered list of all neighboring FlockAgents of DIFFERENT flocks.
//Makes a new filtered list that adds all FlockAgents of different flocks from the original list of Transforms of colliders around the Agent.