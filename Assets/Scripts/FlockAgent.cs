using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script/component has to be attached to a gameObject with a Collider2D component attached to it, if one doesn't exist, it will create one for it
[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;

    public Flock AgentFlock { get { return agentFlock; } }//now can Get but not Set Flock agentFlock

    private Collider2D agentCollider;

    public Collider2D AgentCollider { get { return agentCollider; } } //They can Get agentCollider but not Set agentCollider
    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
