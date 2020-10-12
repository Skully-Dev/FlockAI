using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script requires the GameObject it is attached to, to have a Collider2D component, if one doesn't exist, it will create one for it.
//HOWEVER, since collider2D isn't specific, it'll require us to add the required component before script can be attached.
[RequireComponent(typeof(Collider2D))] //Needed for finding agent neighbors (using physics to see if anything in radius around Agent)
public class FlockAgent : MonoBehaviour
{
    /// <summary>
    /// The flock the instance of the agent is a part of
    /// </summary>
    Flock agentFlock;

    /// <summary>
    /// The flock this agent instance is a part of.
    /// Get but not Set as the flock only needs to initally set this value.
    /// </summary>
    public Flock AgentFlock { get { return agentFlock; } }

    /// <summary>
    /// Reference/Cashe collider on this instance of agent.
    /// </summary>
    private Collider2D agentCollider;
    /// <summary>
    /// Reference to this instances collider.
    /// public accessor to agentCollider. So can Get agentCollider but NOT Set.
    /// as agentCollider only needs to be assigned to at start.
    /// </summary>
    public Collider2D AgentCollider { get { return agentCollider; } } 
    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Allows flock when it creates the agent to initialize/set Agents flock reference to itself.
    /// </summary>
    /// <param name="flock">The Flock that called the method when instancing THIS agent. I.E. Passes itself in.</param>
    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    /// <summary>
    /// To tell agent to move to a new position, after position has been calculated in FlockBehaviors.
    /// Turns agent to face direction of position it will be moving towards.
    /// Moves agent to that position.
    /// </summary>
    /// <param name="velocity">the offset position agent will be moving to</param>
    public void Move(Vector2 velocity)
    {
        transform.up = velocity; //up is the front of the agent, this will make its direction face the velocity position. //In 3D use forward, not up
        transform.position += (Vector3)velocity * Time.deltaTime; //Cast as Vector3 as position is a Vector3 struct. deltaTime for consistant movement, regardless of framerate.

    }
}
