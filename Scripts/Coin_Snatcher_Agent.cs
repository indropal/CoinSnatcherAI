using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;


public class Coin_Snatcher_Agent : Agent
{
    public GameObject coin;

    private Vector3 startPosition; // starting position of the Agent
    private Agent_Character_Controller characterController; // reference to the Agent_Character_Controller script attached to the Agent Body
    private Rigidbody r_body; // reference to Rigid Body for Game Physics

    // Call when the Agent is Initialized - called once
    public override void Initialize()
    {

        startPosition = transform.position;
        characterController = GetComponent<Agent_Character_Controller>();
        r_body = GetComponent<Rigidbody>();
    }

    // Training happens in Episiodes - An episode ends when the Agent collects the Coin or Time Runs Out & after which Episode needs to be reset
    public override void OnEpisodeBegin()
    {
        // Agent is reset after the end of each Episode by changing agent's position, turning it in a random direction, reset its velocity

        // Resetting Agents Position & Rotation
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        r_body.velocity = Vector3.zero;

        // Reset the position of the Coin 
        // generate a number between -4f to -2f & 4f to 2f
        // coinSpawnMult = Random.Range(0, 1f) > 0.5 ? Random.Range(-4.5f, -2f) : Random.Range(2f, 4.5f);

        coin.transform.position = startPosition + Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) * Vector3.forward * (Random.Range(0, 1f) > 0.5 ? Random.Range(-4.5f, -2f) : Random.Range(2f, 4.5f));

    }

    // Pass Actions into the Agent / User Control - Control Agent with Keyboard Input
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Read input values and round them. GetAxisRaw works better in this case
        // because of the DecisionRequester, which only gets new decisions periodically.
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical")); // control with Up & Down Arrow Keys
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")); // control with Left & Right Arrow Keys
        bool jump = Input.GetKey(KeyCode.Space);

        // Convert the actions to Discrete choices (0, 1, 2)
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = vertical >= 0 ? vertical : 2;
        actions[1] = horizontal >= 0 ? horizontal : 2;
        actions[2] = jump ? 1 : 0;
    }

    // Method to Control Agent Actions based on Input Received - whether NN Or Keyboard Input
    public override void OnActionReceived(ActionBuffers actions)
    {
        /*
         * No Longer needed when running in Inference...
        // Give -ve Reward to discourage Agent from straying away from the coin
        if (Vector3.Distance(startPosition, transform.position) > 7f)
        {
            AddReward(-1f); // Give -ve reward
            EndEpisode(); // End the Episode & restart a new one
        }
        */

        // Only initiate the actions of the Agent once the GameController has concluded its countdown at the beginning of the game
        // Ommit this portion (if statement check) when training the agent - this is just for playing the game



        if (GameController.instance.gamePlaying)
        {
            // Convert Actions from Discrete(0, 1, 2) to expected input values (-1, 0, +1) of the Character Controller
            float vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
            float horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
            bool jump = actions.DiscreteActions[2] > 0;

            characterController.ForwardInput = vertical;
            characterController.TurnInput = horizontal;
            characterController.JumpInput = jump;
        }
        else
        {
            // The Countdoen is still going on.. hence halt all kinds of action of the Agent
            characterController.ForwardInput = 0;
            characterController.TurnInput = 0;
            characterController.JumpInput = false;
        }
        
    }

    // Method to Update Agent Reward Once Agent collects the Coin -> End Episode
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "collectible")
        {
            AddReward(1f);
            //EndEpisode();
        }
    }
}
