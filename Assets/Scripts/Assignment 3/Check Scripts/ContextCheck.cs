using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract as well never be needing just a plain ContectCheck, always derived from.

//abstract like interface, has requirements for the derived class, but unlike interfaces, has functionality.
//useful for common functionality that is useless on its own and requires implementation.
//Abstract can't be instanced but are inheritable.
//ContextCheck is abstract as we'll never instantiate a ContextCheck, we instantiate the scripts derived from ContextCheck.
public abstract class ContextCheck : ScriptableObject
{
    /// <summary>
    /// Checks the original context for a requirement, if found, returns true.
    /// </summary>
    /// <param name="agent">The Agent in question, so it can be compaired to other things in the list</param>
    /// <param name="context">The original context list of colliders around the agent </param>
    /// <returns></returns>
    public abstract bool Check(FlockAgent agent, List<Transform> context);
}