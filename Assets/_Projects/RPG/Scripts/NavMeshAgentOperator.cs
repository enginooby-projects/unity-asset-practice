using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO: Move into Library
namespace Enginoobz.Operator {
  [RequireComponent(typeof(NavMeshAgent))]
  public class NavMeshAgentOperator : MonoBehaviour {
    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgent _agent;

    /// <summary>
    /// Not in Update().
    /// </summary>
    public void MoveTo(Vector3 dest, float stoppingDistance = 0) {
      _agent.stoppingDistance = stoppingDistance;
      _agent.destination = dest;
    }

    public Vector3 LocalVelocity => transform.InverseTransformVector(_agent.velocity);
  }
}