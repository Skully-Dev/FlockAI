using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject //inheriting from something other than monobehaviour, ScriptableObject
{
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock); //Incomplete class
}
