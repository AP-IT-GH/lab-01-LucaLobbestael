using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;

public class CubeAgentRaysGreenPlane : Agent
{
    public Transform Target;
    public Transform GreenPlane;
    public float speedMultiplier = 0.5f;
    public float rotationMultiplier = 5f;

    public string goal = "Enemy";
    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
        }

        Target.GameObject().SetActive(true);
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        goal = "Enemy";
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        SetReward(-0.01f);
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);
        
        transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[1], 0.0f);
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (goal == "Enemy" && collision.gameObject.CompareTag("Target"))
        {
            SetReward(0.5f);
            collision.gameObject.SetActive(false);
            goal = "GreenPlane";
        }
        if (goal == "GreenPlane" && collision.gameObject.CompareTag("GreenPlane"))
        {
            SetReward(0.5f);
            goal = "Enemy";
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}
