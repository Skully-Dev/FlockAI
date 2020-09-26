using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original)//for each gameObject near flock agent
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>();
            if (itemAgent != null) // && itemAgent.AgentFlock == agent.AgentFlock) //if FlockAgent
            {
                if (itemAgent.AgentFlock == agent.AgentFlock) //if part of the same flock
                {
                    filtered.Add(item); //add it to filtered list
                }
            }
        }
        return filtered; //The surrounding agents of the same flock
    }
}
