using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FilteredFlockBehavior : FlockBehavior
{
    [Tooltip("Reference the context filter used by this behavior.")]
    public ContextFilter filter; //all behaviors derived from filteredFlockBehavior now have a ContextFilter reference slot.
}
//FilteredFlockBehaviour is the middleman for behaviours requiring filtered context
//Not all behaviours require filtered lists, like stay in radius only cares about the agent itself relative to the circle radius
//Some things will inherit from FilteredFlockBehaviour other things will inherit directly from flock behavior.