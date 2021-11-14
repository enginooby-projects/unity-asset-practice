using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentOperator : MonoBehaviour {
  [SerializeField] Transform target;
  [AutoRef, SerializeField, HideInInspector] NavMeshAgent agent;

  void Update() {
    agent.SetDestination(target.position);
  }
}
