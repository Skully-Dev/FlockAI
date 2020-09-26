using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextFilter : ScriptableObject
{
    /// <summary>
    /// list will be all the neibours, once filtered will only obtain the ones filtered for like agents of the same flock
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="original"> Everything around each agent</param>
    /// <returns></returns>
    public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original);
}
