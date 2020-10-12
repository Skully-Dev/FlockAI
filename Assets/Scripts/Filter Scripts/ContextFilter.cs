using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract as well never be needing just a plain ContectFilter, always derived from.

//abstract like interface, has requirements for the derived class, but unlike interfaces, has functionality.
//useful for common functionality that is useless on its own and requires implementation.
//Abstract can't be instanced but are inheritable.
//ContextFilter is abstract as we'll never instantiate a ContextFilter, we instantiate the behaviours derived from ContextFilter.
public abstract class ContextFilter : ScriptableObject
{
    /// <summary>
    /// list will be all the neibours, once filtered will only obtain the ones filtered for like agents of the same flock
    /// </summary>
    /// <param name="agent">The Agent in question, so it can be compaired to other things in the list</param>
    /// <param name="original">The original context list of colliders around the agent </param>
    /// <returns></returns>
    public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original);
}
