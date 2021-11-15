using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enginoobz.Core;

// TODO: Move into Library
namespace Enginoobz.Operator {
  [RequireComponent(typeof(NavMeshAgent))]
  public class NavMeshAgentOperator : MonoBehaviour, IAction {
    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgent _agent;

    /// <summary>
    /// Not in Update().
    /// </summary>
    // ! distance 0 sometimes cause jiggering/shaking movement
    public void MoveTo(Vector3 dest, float stoppingDistance = 0.5f) {
      _agent.isStopped = false;
      _agent.stoppingDistance = stoppingDistance;
      _agent.destination = dest;
    }

    public void Cancel() {
      _agent.isStopped = true;
    }

    public Vector3 LocalVelocity => transform.InverseTransformVector(_agent.velocity);


  }
}