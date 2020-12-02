using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Life
{
    [Header("Behaviors")]
    [SerializeField]
    private FlockBehavior wanderBehavior;
    [SerializeField]
    private FlockBehavior flockBehavior;
    [SerializeField]
    private FlockBehavior evadeBehavior;
    [SerializeField]
    private FlockBehavior hideBehavior;

    [Header("Checks")]
    [SerializeField]
    private ContextCheck sameFlockCheck;
    [SerializeField]
    private ContextCheck predatorCheck;
    [SerializeField]
    private ContextCheck safetyCheck;

    protected override Vector2 StateBehavior(FlockAgent agent, List<Transform> context)
    {
        if (agent.state == State.Wander)
        {
            //running two checks, time consuming, maybe make single check that returns a state instead
            if (predatorCheck.Check(agent, context)) //if any are predators
            {
                agent.state = State.Evade;
                agent.AgentAnimator.SetInteger("state", 2);
            }
            else if (sameFlockCheck.Check(agent, context)) //if any from the same flock
            {
                agent.state = State.Flock;
                agent.AgentAnimator.SetInteger("state", 1);
            }
            return wanderBehavior.CalculateMove(agent, context, this);
        }
        else if (agent.state == State.Flock)
        {
            if (predatorCheck.Check(agent, context))
            {
                agent.state = State.Evade;
                agent.AgentAnimator.SetInteger("state", 2);
            }
            else if (!sameFlockCheck.Check(agent, context)) //if none from the same flock
            {
                agent.state = State.Wander;
                agent.AgentAnimator.SetInteger("state", 0);
            }
            return flockBehavior.CalculateMove(agent, context, this);
        }
        else if (agent.state == State.Evade)
        {
            if (predatorCheck.Check(agent, context))
            {
                if (safetyCheck.Check(agent,context))//safety obstacles check
                {
                    agent.state = State.Hide;
                    agent.AgentAnimator.SetInteger("state", 3);
                }
            }
            else //no predators
            {
                agent.state = State.Wander;
                agent.AgentAnimator.SetInteger("state", 0);
            }

            return evadeBehavior.CalculateMove(agent, context, this);
        }
        else if (agent.state == State.Hide)
        {
            if (!predatorCheck.Check(agent, context)) //no predators
            {
                agent.state = State.Wander;
                agent.AgentAnimator.SetInteger("state", 0);
            }
            return hideBehavior.CalculateMove(agent, context, this);
        }
        else if (agent.state == State.Hidden)
        {
            if (!predatorCheck.Check(agent, context))
            {
                agent.state = State.Wander;
                agent.AgentAnimator.SetInteger("state", 0);
            }
            return Vector2.zero;
        }
        else if (agent.state == State.Attacked)
        {
            return Vector2.zero;
        }
        else //Technically should never happen. Default to 0
        {
            Debug.Log("State not recognised as a Prey State");
            return Vector2.zero;
        }
    }
}
