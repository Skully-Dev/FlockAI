using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract is like an interface, has requirements for the derived class, but unlike interfaces, has functionality
//often used when its common functionality that is useless on its own and requires implementation.
//Abstract can't be instanced but are inheritable.
//Abstract as well never be needing just a plain ContectFilter, always derived from.
//FlockBehavior is abstract as we'll never instantiate a FlockBehavior, we instantiate the behaviours derived from FlockBehaviour.
public abstract class FlockBehavior : ScriptableObject //inheriting from something other than monobehaviour, ScriptableObject
{
    /// <summary>
    /// The behavior will take over at this point, scriptable object will run whatever calculations it needs to and return back a vector2 = the way the agent should move.
    /// Since its abstract, we don't put the body to it in FlockBehaviour but the derived classes.
    /// 
    /// COHESION: aims to bring nearby agents together
    /// 
    /// ALIGNMENT: aims to align all agents facing direction
    /// 
    /// AVOIDANCE: aims to keep agent at a distance from neighboring agents all up in its personal space.
    /// 
    /// STAYINRADIUS: aims to keep agents within a defined circle.
    /// 
    /// COMPOSITE: Basically combines multiple behaviors
    /// 
    /// CHASE: Aims to move towards agents of different flocks.
    /// 
    /// EVADE: Aims to flee CHASE predators, Toggles agent isEvade.
    /// 
    /// SAFETYSIR: When Chased, Prey move towards safety radius predators can't penetrate.
    /// 
    /// WANDER: Aims to move towards a specific random location, followed by another and so on.
    /// 
    /// CHECK DERIVED BEHAVIOR FOR BODY AND FULL DESCRIPTION
    /// </summary>
    /// <param name="agent">The Agent we are working with to calculate its move.</param>
    /// <param name="context">What neighbours are around me(Agent), i.e. Other agents, obstacles or boundries etc.</param>
    /// <param name="flock">For situations we need information about the flock itself.</param>
    /// <returns></returns>
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock); //Incomplete classabstract as derived behaviours will eventually implement it.
}
