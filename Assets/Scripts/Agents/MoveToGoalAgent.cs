using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
    public MoveAgent move;
    public HealthSystem healthSystem;

    private float _lastHealth;

    public event EventHandler<EventArgs> onEndEpisode;

    public override void Initialize()
    {
        move = this.GetComponent<MoveAgent>();
        move.OnInitialize(this.GetComponent<Rigidbody2D>());

        healthSystem = this.GetComponent<HealthSystem>();
        healthSystem.OnHealthChanged += OnHealthChanged;

        //m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    private void OnHealthChanged(object sender, float e)
    {
        bool healed = (_lastHealth < e);

        bool isFullHealth = (_lastHealth == e);

        float reward = healed ? 1f : -1f * (isFullHealth ? 0.5f : 1f);

        Debug.Log($"{this.gameObject.name} : reward({(healed ? "Heal" : (string)(isFullHealth ? "Full Health" : "Damaged"))}) = {reward}");
        SetReward(reward);

        if (e == 0)
        {
            DeadReward();
        }
    }

    private void DeadReward()
    {
        float reward = -10f;
        Debug.Log($"{this.gameObject.name} : reward(Death) = {reward}");
        SetReward(reward);
        EndEpisode();
        onEndEpisode?.Invoke(this, new EventArgs());
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //if (useVecObs)
        //{
        //    sensor.AddObservation(gameObject.transform.rotation.z);
        //    sensor.AddObservation(gameObject.transform.rotation.x);
        //    sensor.AddObservation(ball.transform.position - gameObject.transform.position);
        //    sensor.AddObservation(m_BallRb.velocity);
        //}
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.ContinuousActions);
    }

    public void MoveAgent(ActionSegment<float> act)
    {
        //DebugActionBuffer(act);

        Vector2 direction = new Vector2(act[0], act[1]);

        move.SetMoveDirection(direction);
    }

    public override void OnEpisodeBegin()
    {
        move.SetVelocity(Vector2.zero);

        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    public void SetResetParameters()
    {
        healthSystem.OnInitialize(false);
        _lastHealth = healthSystem.GetHealth();
    }

    private void DebugActionBuffer(ActionSegment<float> act)
    {
        for (int i = 0; i < act.Length; i++)
        {
            Debug.Log($"{this.gameObject.name}: {i} = {(float)act[i]}");
        }
    }
}
