using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because it is a ScriptableObject and not MonoBehavior, we need a way to create it.
[CreateAssetMenu(menuName = "Flock/Behavior/Composite")] //now rmb project folder will allow creation of a scriptable object in Flock > Behaviou
public class CompositeBehavior : FlockBehavior //Get the methods and vars from FlockBehaviour, so it has to implement that flockBehaviour CalculateMove
{
    [SerializeField]
    [Tooltip("Gives the option to not compress the nuances of behavior")]
    private bool capMagnitudesToWeightValues;

    [System.Serializable]
    public struct BehaviorGroup
    {
        [Tooltip("Reference behaviors to composit together")]
        public FlockBehavior behavior;
        [Tooltip("The influence of each behavior")]
        public float weight;
    }

    /// <summary>
    /// Each of the influencing behaviors and their weights
    /// </summary>
    public BehaviorGroup[] behaviors;


    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Life flock) //agent is its own agent, context is other agents
    {
        Vector2 move = Vector2.zero; //starting value of zero to be added upon. Never assume a default value.

        //int countMags = 0; // *****

        //loop through each behavior attached
        for (int i = 0; i < behaviors.Length; i++)
        {
            //Gets the calculateMove for each of the behaviors and scales the their result according to their associated weight
            Vector2 partialMove = behaviors[i].behavior.CalculateMove(agent, context, flock) * behaviors[i].weight;

            if (partialMove != Vector2.zero) //if some movement has been returned
            {
                if (capMagnitudesToWeightValues)
                {
                    #region Allows lower values BUT Caps Each Behaviours Movement Influence to their given weight value
                    //make sure the number we get for moving the agent isnt larger than the weight we gave it
                    if (partialMove.sqrMagnitude > behaviors[i].weight * behaviors[i].weight) //if larger
                    {
                        partialMove.Normalize(); //normalize
                        partialMove *= behaviors[i].weight; //and scale exactly to weight
                    }
                    #endregion
                }
                //countMags++; // *****
                
                //bring all behaviors together as one
                move += partialMove; //
            }
        }
        //move /= countMags; // *****
        return move;
    }
}
//Basically combines multiple behaviors
//returns the sum of the referenced behaviors CalculateMove results, each scaled by their weight and optionally capped values to their weight.
//Often is a little jittery. Use SteeredCohesion for smoother results
//***** = lines to scale combined magnatude based on number of behaviors in effect.