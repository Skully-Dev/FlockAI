using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Check/Same Flock")]//now rmb project folder will allow creation of a scriptable object in Flock > Check
public class SameFlockCheck : ContextCheck //Derived from ContextCheck, therefore requires the filter original check method.
{
    public override bool Check(FlockAgent agent, List<Transform> context)
    {
        foreach (Transform item in context)//for each Transform(gameObjects with colliders) near flock agent
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>(); //if an agent, will refer to it, else = null.
            if (itemAgent != null) // && itemAgent.AgentFlock == agent.AgentFlock) //therefore if FlockAgent
            {
                if (itemAgent.AgentFlock == agent.AgentFlock) //if part of the same flock
                {
                    if (itemAgent.state != Life.State.Hidden)
                    {
                        return true;
                    }
                }
            }
        }
        return false; //returns false if none found
    }
}
//Check the context list of agent for agents of the same flock
//returns true if an agent of the same flock was found in the original list of Transforms of colliders around but exluding the Agent itself