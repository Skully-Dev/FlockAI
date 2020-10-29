using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    #region Reference Variables

    [Tooltip("Reference the agent prefab.")]
    public FlockAgent agentPrefab;

    [Tooltip("Populated with agents that belong to the flock")]
    List<FlockAgent> agents = new List<FlockAgent>();
    [Tooltip("The scriptable object behavior this flock will be following, e.g. the composite behavior consisting of various weighted behaviors.")]
    public FlockBehavior behavior;
    #endregion

    #region Flock size/density Initializing Variables
    [Tooltip("How many birds, from 10 to 500.")]
    [Range(10,500)]
    public int startingCount = 250;
    [Tooltip("How dence the flock will be on spawn. A constant. It determines the size of the spawn circle based on the number of spawn and this value.")]
    const float AgentDensity = 0.08f;
    #endregion

    #region Agent Stat Variables
    [Tooltip("Basically the speed multiplier. Ranging from 1 to 100. As the movement calculations are relativly small values, the drive facor multiplies that value.")]
    [Range(1f, 100f)] 
    public float driveFactor = 10f; 
    [Tooltip("Speed cap, the Maximum speed. Ranging from 1 to 100.")]
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Tooltip("The distance to be considered as a neighbor/obstacle etc. Ranging from 1 to 10.")]
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Tooltip("The distance to avoid colliding with neighbor agents around you, a ratio of neighborRadius. A value between no radius and neighbor radius. CAN NOT BE CHANGED DURING PLAY MODE")]
    public float avoidanceRadiusMultiplier = 0.5f;
    [Tooltip("CAN NOT BE CHANGED DURING PLAY MODE")]
    public float evadeRadiusMultiplier = 1.0f;//*****
    public float smallRadiusMultiplier = 0.2f;// class
    #endregion

    #region Utility / Squared Values Variables and Property
    //Utility variables
    //Squared values will be required, without them we'd need to square root some equations which for computers is much less efficient, for optimisation, we'll precode these number values.
    //i.e. compairing velocity againt max speed requires a vecors magnitude, which requires square rooting, which is relatively taxing.
    //Instead, we'll be just compairing the squares against each other, saving ourselves a taxing step of math.
    private float squareMaxSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;
    /// <summary>
    /// Gets squareAvoidanceRadius, but NOT Set.
    /// </summary>
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    #endregion
    private float squareEvadeRadius;//*****
    public float SquareEvadeRadius { get { return squareEvadeRadius; } } //*****
    
    private float squareSmallRadius;//class
    public float SquareSmallRadius { get { return squareSmallRadius; } } //class

    private void Start()
    {
        #region Initialize Utility Variable Values
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        squareEvadeRadius = squareNeighborRadius * evadeRadiusMultiplier * evadeRadiusMultiplier; //*****
        squareSmallRadius = squareNeighborRadius * smallRadiusMultiplier * smallRadiusMultiplier;//class
        #endregion

        #region Initialize and Instantiate the Flock
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate( //spawn clones of gameObject or Prefabs
                agentPrefab, //The Spawned Object
                Random.insideUnitCircle * startingCount * AgentDensity, //a circle based on density of flock //Spawn location, a random position within a circle of 1 radious, multiplied by the number of Agents and their AgentDensity to make the location large enough to fit all agents relative to density.
                Quaternion.Euler(Vector3.forward * Random.Range(0,360f)),//A rotation value between 0-360, rotating on z axis like a clock //Requires a 4 value rotation to face, we use Euler Quaternion to do this, with the z rotation of between 0 and 360 degrees
                transform //the parent location, this flocks transform
                );
            newAgent.name = "Agent " + i; //each one named agent index
            newAgent.Initialize(this);//So the agent itself knows what flock it belongs to.
            agents.Add(newAgent);//add it to the list array of agents
        }
        #endregion
    }
    private void Update()
    {
        #region Apply Behaviors to Each Agent in Flock
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent); //what agents/objects exist within the neighbor radius

            //FOR TESTING, be better to cashe/ set up so this line doesn't have to happen every frame
            //More neighbors = more red //if == 0 white //if >=6 then RED!
            //agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f); //density hack
            //Mathf.Lerp(a,b) //point between a and b

            Vector2 move = behavior.CalculateMove(agent, context, this); //returns the way the agent should move based on its behavior
            move *= driveFactor; //increases the speed of the behaviors movement
            if(move.sqrMagnitude > squareMaxSpeed) //Magnitude is speed/length of vector calculated using Pythagoras Theorem (A^2 + B^2 = C^2) using the transforms values // sqrMagnitude is the A^2 + B^2 values prior to square rooting! //square vs square, all that matters is which is larger, so squareroot doesn't change the outcome.
            {
                //if above max speed
                move = move.normalized * maxSpeed; //normalized returns a magnatude of 1 w same direction, so basically clamps it to max speed.
            }
            agent.Move(move); //moves the agent.
        }
        #endregion
    }

    #region GetNearbyObjects Method
    /// <summary>
    /// Returns a list of the transforms of ALL colliders within neighbor radius of Agent. (EXCLUDING ITS OWN COLLIDER)
    /// </summary>
    /// <param name="agent">The agent to check neighbor radius of, for colliders within.</param>
    /// <returns></returns>
    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>(); //a list to populate
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius); //circle overlap always returns an array //Physics2D.OverlapCircleAll creates an imaginary circle in space at a point and radius we choose and checks what colliders are within it. //in 3D use collider3D and physics3D.OverlapSphere
        //foreach collider in the array, as long as it is NOT ourselves, take the transform of that collider add to context list.
        foreach (Collider2D c in contextColliders)
        {
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
    #endregion
}
