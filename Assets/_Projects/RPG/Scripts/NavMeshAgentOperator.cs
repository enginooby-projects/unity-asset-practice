using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO: Move into Library
[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentOperator : MonoBehaviour {
  [AutoRef, SerializeField, HideInInspector]
  private NavMeshAgent _agent;

  public void MoveTo(Vector3 dest) {
    _agent.destination = dest;
  }

  public Vector3 LocalVelocity => transform.InverseTransformVector(_agent.velocity);
}
