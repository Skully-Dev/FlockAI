using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class
[CreateAssetMenu (menuName = "Flock/Behavior/Wander")]
public class WanderBehavior : FilteredFlockBehavior
{
    Path path = null;
    int currentWaypoint = 0;

    Vector2 waypointDirection = Vector2.zero;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(path == null)
        {
            FindPath(agent, context);
        }

        return FollowPath(agent); //FollowPath()
    }

    public bool InRadius(FlockAgent agent)
    {
        //direction to waypoint
        waypointDirection = (Vector2)path.waypoints[currentWaypoint].position - (Vector2)agent.transform.position;

        //waypointDirection.magnitude would give us the distance of the vector
        if (waypointDirection.magnitude < path.radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private Vector2 FollowPath(FlockAgent agent)
    {
        if (path == null)
        {
            return Vector2.zero;
        }
        if (InRadius(agent))
        {
            currentWaypoint++;
            if (currentWaypoint >= path.waypoints.Count)
            {
                currentWaypoint = 0;
            }
            return Vector2.zero;
        }
        return waypointDirection;
    }

    private void FindPath(FlockAgent agent, List<Transform> context)
    {
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        if (filteredContext.Count == 0)
        {
            return;
        }

        int randomPath = Random.Range(0, filteredContext.Count);
        path = filteredContext[randomPath].GetComponentInParent<Path>();
    }


}
