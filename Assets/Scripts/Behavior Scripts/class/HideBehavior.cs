using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Hide")]
public class HideBehavior : FilteredFlockBehavior
{
    public ContextFilter obstaclesFilter;

    public float hideBehindObstacleDistance = 2f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Life flock)
    {
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //hide from
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        //hide behind
        List<Transform> obstacleContext = (filter == null) ? context : obstaclesFilter.Filter(agent, context);

        //find nearest obstacle to hide behind
        float nearestDistance = float.MaxValue;
        Transform nearestObstacle = null;
        foreach (Transform item in obstacleContext)
        {
            float distance = Vector2.Distance(item.position, agent.transform.position);
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestObstacle = item;
            }
        }

        //if no obstacle
        if (nearestObstacle == null)
        {
            return Vector2.zero;
        }

        //find best hiding spot
        Vector2 hidePosition = Vector2.zero; //location agent should try moving to
        foreach (Transform item in filteredContext)
        {
            Vector2 obstacleDirection = nearestObstacle.position - item.position;

            obstacleDirection = obstacleDirection.normalized * hideBehindObstacleDistance; //maybe try +=

            hidePosition += (Vector2)nearestObstacle.position + obstacleDirection;
        }
        hidePosition /= filteredContext.Count;

        //FOR DEBUG ONLY
        Debug.DrawRay(hidePosition, Vector2.up * 1f); //draws a line to where its gonna try and hide

        return hidePosition - (Vector2)agent.transform.position; //offsets to agent local position
    }
}
