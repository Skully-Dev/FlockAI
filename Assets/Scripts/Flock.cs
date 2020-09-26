using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab; //referencing the agent prefab
    List<FlockAgent> agents = new List<FlockAgent>(); //agents that belong to flock
    public FlockBehavior behavior; //the ones it will be following

    [Range(10,500)]//any value from 10 to 500 birds adjusted by slider
    public int startingCount = 250; //how many birds
    const float AgentDensity = 0.08f; //how dence the flock will be on spawn, const is constant, 

    [Range(1f, 100f)] //speed multiplier ranging from 1 to 100
    public float driveFactor = 10f; //multiplier for our speed
    [Range(1f, 100f)] //slider ranging from 1 to 100
    public float maxSpeed = 5f; //the base speed
    [Range(1f, 10f)]//slider ranging from 1 to 10
    public float neighborRadius = 1.5f; //how far between neighbours
    [Range(0f, 1f)]
    public float avoidanceRadiumMultiplier = 0.5f;//you don't want to collide with the agents around you, makes sure you dont collide with the neighbour radious, make this number around 1/2 the above.

    //Squared values will be required, without them we'd need to square root some equations, for optimisation, we'll precode these number values.
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiumMultiplier * avoidanceRadiumMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate( //spawn clones of gameObject or Prefabs
                agentPrefab, //The Spawned Object
                Random.insideUnitCircle * startingCount * AgentDensity, //Spawn location, a random distance inside a circle of 1 radious, multiplied by the number of Agents and their AgentDensity to make the location large enough
                Quaternion.Euler(Vector3.forward * Random.Range(0,360f)),//Requires a 4 value rotation to face, we use Euler Quaternion to do this, with the z rotation of between 0 and 360 degrees
                transform //the parent location
                );
            newAgent.name = "Agent " + i; //each one named agent index
            newAgent.Initialize(this);//clone
            agents.Add(newAgent);//add it to the list array of agents
        }
    }
    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //FOR TESTING
            //agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f); //density hack
                //Mathf.Lerp(a,b) //point between a and b

            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if(move.sqrMagnitude > squareMaxSpeed) //magnitude is speed of vector, legth of vector
            {
                move = move.normalized * maxSpeed; //normalized returns a magnatude of 1 w same diirection
            }
            agent.Move(move); //moves the agent at this move speed
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius); //circle overlap always returns an array
        foreach(Collider2D c in contextColliders)
        {
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
