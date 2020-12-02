using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//worked but SLOW, as it is a double get component, not the most efficient approach.

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Check/Predator")]//now rmb project folder will allow creation of a scriptable object in Flock > Check
public class PredatorCheck : ContextCheck //Derived from ContextCheck, therefore requires the filter original check method.
{
    public override bool Check(FlockAgent agent, List<Transform> context)
    {
        foreach (Transform item in context)//for each Transform(gameObjects with colliders) near flock agent
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>(); //if an Agent, will refer to it, else = null.
            if (itemAgent != null)
            {
                Predator itemPredator = itemAgent.GetComponent<Predator>(); //if a Predator, will refer to it, else = null.
                if (itemPredator != null) //therefore if Predator
                {
                    return true;
                }
            }
        }
        return false; //returns false if none found
    }
}
//Check the context list of agent for PREDATOR agents
//returns true if an predator was found in the original list of Transforms of colliders around Agent itself