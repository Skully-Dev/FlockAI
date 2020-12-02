using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Check/Prey")]//now rmb project folder will allow creation of a scriptable object in Flock > Check
public class PreyCheck : ContextCheck //Derived from ContextCheck, therefore requires the filter original check method.
{
    public override bool Check(FlockAgent agent, List<Transform> context)
    {
        foreach (Transform item in context)//for each Transform(gameObjects with colliders) near flock agent
        {
            Prey itemAgent = item.GetComponent<Prey>(); //if prey, will refer to it, else = null.
            if (itemAgent != null) // && itemAgent.AgentFlock == agent.AgentFlock) //therefore if Prey
            {
                return true;
            }
        }
        return false; //returns false if none found
    }
}
//Check the context list of agent for PREY agents
//returns true if an prey was found in the original list of Transforms of colliders around Agent itself