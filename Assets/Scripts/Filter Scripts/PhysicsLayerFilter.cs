using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")] //now rmb project folder will allow creation of a scriptable object in Flock > Filter
public class PhysicsLayerFilter : ContextFilter //Derived from ContextFilter, therefore requires the filter original context method.
{
    //A convenient way to access physics layers from inspector
    public LayerMask mask; //001001000

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>(); //to be populated with physics layer transforms, objects to avoid.
        foreach (Transform item in original)//for each Transform(gameObjects with colliders) near flock agent
        {
            //if the layer of the item exists in the mask, the mask wont change with an OR |
            if (mask == (mask | (1 << item.gameObject.layer))) //bitwise operation,  if layer of original exists in this mask
            {
                filtered.Add(item); //therefore on mask layer and is added to filtered context list.
            }
        }
        return filtered;
    }
}
//Filtered list of objects on a specific layer mask.
//can be used to chase or avoid etc.

//bitwise operations

//Left-shift, right-shift
// x << y
// = 00001001 << 4
// = 10010000

//16 = 0010000

//complement operator
//~ 0001000
// = 1110111 //reversed

//And "&"
//000110001 &
//000100011
//=000100001 //only the ones that are both true stay 1 otherwise 0

//OR "|"
//000110001 |
//000100011
//=000110011 //if either are true1 then result is 1

//XOR / Exclusive OR '^'
//000110001 ^
//000100011
//=000010010 //only 1 can be true for it to result in true

//mask =   0100
//layer1 = 0001
//layer2 = 0100
