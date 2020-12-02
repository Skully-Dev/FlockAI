using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Life
{
    [Header("Behaviors")]
    [SerializeField]
    private FlockBehavior wanderBehavior;
    [SerializeField]
    private FlockBehavior pursuitBehavior;
    [SerializeField]
    private FlockBehavior attackBehavior;

    [Header("Checks")]
    [SerializeField]
    private ContextCheck preyCheck;
    [SerializeField]
    private ContextCheck sameFlockCheck;

    protected override Vector2 StateBehavior(FlockAgent agent, List<Transform> context)
    {
        if (agent.state == State.Wander)
        {
            if (preyCheck.Check(agent,context))
            {
                agent.state = State.Pursuit;
                agent.AgentAnimator.SetInteger("state", 6);
            }
            return wanderBehavior.CalculateMove(agent, context, this);
        }
        else if (agent.state == State.Pursuit)
        {
            if (!preyCheck.Check(agent, context))
            {
                agent.state = State.Wander;
                agent.AgentAnimator.SetInteger("state", 0);
            }
            return pursuitBehavior.CalculateMove(agent, context, this);
        }
        else if (agent.state == State.Attack)
        {
            agent.state = State.Wander;
            return attackBehavior.CalculateMove(agent, context, this);
        }
        else //Technically should never happen. Default to 0
        {
            Debug.LogError("State not recognised as a Predator State");
            return Vector2.zero;
        }
    }

}
