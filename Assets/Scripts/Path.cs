using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class
public class Path : MonoBehaviour
{
    public List<Transform> waypoints;

    public float radius = 5f;

    [SerializeField]
    private Vector3 gizmoSize = Vector3.one;

    private void OnDrawGizmos()
    {
        if(waypoints == null || waypoints.Count == 0)
        {
            return;
        }
        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform waypoint = waypoints[i];

            if (waypoint == null)
            {
                continue; // go to the next iteration of this loop
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(waypoint.position, gizmoSize);

            //check if next waypoint is valid
            if (i + 1 < waypoints.Count && waypoints[i+1] != null)
            {
                //draw a line to the next waypoint
                Gizmos.DrawLine(waypoint.position, waypoints[i + 1].position);
            }
        }
    }
}
