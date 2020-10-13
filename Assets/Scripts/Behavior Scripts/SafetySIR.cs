using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/SafetySIR")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class SafetySIR : FlockBehavior
{
    [SerializeField]
    [Tooltip("The mid-point position of the circle to stay within.")]
    private Vector2 center;
    [SerializeField]
    [Tooltip("How large the circle to stay within is")]
    private float radius = 5;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (agent.isEvade)//if agent is trying to evade
        {
            Vector2 centerOffset = center - (Vector2)agent.transform.position; //the direction to center, mag would be the distance to center.
            float t = centerOffset.magnitude / radius; //if t=0, at center. if t=1, at circumference. if t>1, outside circle
            //Could use sqrMagnitude if you precalculate radius squared, would still be accurate that t<0 = inside and t>0 = outside circle

            if (t < 0.9f) //if within 90% of radius // this would NOT be an accurate 90% if we were to use sqrMag above.
            {
                return Vector2.zero; //don't bother changing anything
            }

            //otherwise if close to or beyond radius

            return centerOffset * t * t; //return to center at a quadratically(looks more swoopish) more intense rate the further away agent is.
        }
        return Vector2.zero; //don't bother changing anything
    }
}
//When Agent is prey and currently is evading a predator, move towards safety obstacle
//its surrounded by forces predators can't penetrate, stay within safety circle until predator gone.