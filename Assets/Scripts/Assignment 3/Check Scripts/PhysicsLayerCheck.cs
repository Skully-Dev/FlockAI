using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Check/Physics Layer")] //now rmb project folder will allow creation of a scriptable object in Flock > Check
public class PhysicsLayerCheck : ContextCheck //Derived from ContextCheck, therefore requires the filter original check method.
{
    //A convenient way to access physics layers from inspector
    public LayerMask mask; //001001000

    public override bool Check(FlockAgent agent, List<Transform> context)
    {
        foreach (Transform item in context)//for each Transform(gameObjects with colliders) near flock agent
        {
            //if the layer of the item exists in the mask, the mask wont change with an OR |
            if (mask == (mask | (1 << item.gameObject.layer))) //bitwise operation,  if layer of original exists in this mask
            {
                return true;
            }
        }
        return false; //returns false if none found
    }
}
//Checks for objects on a specific layer mask
//can be used to chase or avoid etc.
