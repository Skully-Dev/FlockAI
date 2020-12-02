using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/Move Forward")] //now rmb project folder will allow creation of a scriptable object in Flock > Behavior
public class MoveForwardBehavior : FlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement that flockBehaviour CalculateMove
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Life flock)
    {
        return agent.transform.up; //will have forward motion
    }
}
