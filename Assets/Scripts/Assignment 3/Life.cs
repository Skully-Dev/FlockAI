using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Previously Flock
// Specialised Class of Flock for Base Class State Machienes Generalised Code. Specialised FSM code will be in Predator and Prey. 

public abstract class Life : MonoBehaviour
{
    public enum State //the various states, see those methods for further details
    {
        Wander, //BOTH //Flock(Alignment, Steered Cohesion, Avoidance,  Stay In Radius)
        Flock, //PREY
        Evade, //PREY //Evade: aims to move away from Predators within radius (similar to avoidance but larger, its radius is based on neighbor radius) //ALSO looks for somewhere to hide i.e. obstacles
        Hide, //PREY // wander within radius of hidden object, until # seconds from an enemy sighting.
        Hidden, //PREY
        Attacked, //PREY
        Pursuit, //PREDATOR //Offset Pursuit (Pursuit, Alignment, Steered Cohesion, Avoidance, Obstacle Avoidance, Stay In Radius)
        Attack, //PREDATOR //When on pursuit and prey hides, search around hide location for # seconds
    }

    #region Reference Variables
    [SerializeField, Tooltip("Reference the agent prefab.")]
    protected FlockAgent agentPrefab;

    [Tooltip("Populated with agents that belong to the flock")]
    protected List<FlockAgent> agents = new List<FlockAgent>();
    #endregion

    #region Flock size/density Initializing Variables
    [Tooltip("How many birds, from 10 to 500.")]
    [SerializeField, Range(10,500)]
    protected int startingCount = 250;
    [Tooltip("How dence the flock will be on spawn. A constant. It determines the size of the spawn circle based on the number of spawn and this value.")]
    const float AgentDensity = 0.08f;
    #endregion

    #region Agent Stat Variables
    [Tooltip("Basically the speed multiplier. Ranging from 1 to 100. As the movement calculations are relativly small values, the drive facor multiplies that value.")]
    [SerializeField, Range(1f, 100f)] 
    protected float driveFactor = 10f; 
    [Tooltip("Speed cap, the Maximum speed. Ranging from 1 to 100.")]
    [SerializeField, Range(1f, 100f)]
    protected float maxSpeed = 5f;
    [Tooltip("The distance to be considered as a neighbor/obstacle etc. Ranging from 1 to 10.")]
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f; //used by Pursuit behavior class
    [SerializeField, Tooltip("CAN NOT BE CHANGED DURING PLAY MODE")]
    protected float smallRadiusMultiplier = 0.2f;
    [SerializeField, Tooltip("The distance to avoid colliding with neighbor agents around you, a ratio of neighborRadius. A value between no radius and neighbor radius. CAN NOT BE CHANGED DURING PLAY MODE")]
    protected float avoidanceRadiusMultiplier = 0.1f;
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
    
    private float squareSmallRadius;
    public float SquareSmallRadius { get { return squareSmallRadius; } }
    #endregion

    private void Start()
    {
        #region Initialize Utility Variable Values
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        //squareMediumRadius = squareNeighborRadius * mediumRadiusMultiplier * mediumRadiusMultiplier; //*****
        squareSmallRadius = squareNeighborRadius * smallRadiusMultiplier * smallRadiusMultiplier;//class
        #endregion

        SpawnLife();
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


            //Vector2 move = behavior.CalculateMove(agent, context, this); //returns the way the agent should move based on its behavior
            Vector2 move = StateBehavior(agent, context); //returns the way the agent should move based on its behavior


            move *= driveFactor; //increases the speed of the behaviors movement
            if (move.sqrMagnitude > squareMaxSpeed) //Magnitude is speed/length of vector calculated using Pythagoras Theorem (A^2 + B^2 = C^2) using the transforms values // sqrMagnitude is the A^2 + B^2 values prior to square rooting! //square vs square, all that matters is which is larger, so squareroot doesn't change the outcome.
            {
                //if above max speed
                move = move.normalized * maxSpeed; //normalized returns a magnatude of 1 w same direction, so basically clamps it to max speed.
            }
            agent.Move(move); //moves the agent.
        }
        #endregion
    }

    /// <summary>
    /// Initialize and Instantiate the Flock
    /// </summary>
    private void SpawnLife()
    {
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate( //spawn clones of gameObject or Prefabs
                agentPrefab, //The Spawned Object
                Random.insideUnitCircle * startingCount * AgentDensity, //a circle based on density of flock //Spawn location, a random position within a circle of 1 radious, multiplied by the number of Agents and their AgentDensity to make the location large enough to fit all agents relative to density.
                Quaternion.Euler(Vector3.forward * Random.Range(0, 360f)),//A rotation value between 0-360, rotating on z axis like a clock //Requires a 4 value rotation to face, we use Euler Quaternion to do this, with the z rotation of between 0 and 360 degrees
                transform //the parent location, this flocks transform
                );
            newAgent.name = "Agent " + i; //each one named agent index
            newAgent.Initialize(this);//So the agent itself knows what flock it belongs to.
            agents.Add(newAgent);//add it to the list array of agents

            newAgent.state = State.Wander; //initialize each agents state as wonder
        }
    }

    #region GetNearbyObjects Method
    /// <summary>
    /// Returns a list of the transforms of ALL colliders within neighbor radius of Agent. (EXCLUDING ITS OWN COLLIDER)
    /// </summary>
    /// <param name="agent">The agent to check neighbor radius of, for colliders within.</param>
    /// <returns></returns>
    protected List<Transform> GetNearbyObjects(FlockAgent agent)
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

    protected abstract Vector2 StateBehavior(FlockAgent agent, List<Transform> context);
}
