using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentController : MonoBehaviour {
  [AutoRef, SerializeField, HideInInspector]
  private NavMeshAgent _agent;

  [AutoRef, SerializeField, HideInInspector]
  private Animator _animator;

  private Ray _lastRay;


  void Update() {
    ProcessMoveToCursor();
    ProcessAnimator();
  }

  private void ProcessAnimator() {
    Vector3 localVel = transform.InverseTransformVector(_agent.velocity);
    _animator.SetFloat("forwardSpeed", localVel.z); // TODO: Hash string
  }

  private void ProcessMoveToCursor() {
    Debug.DrawRay(_lastRay.origin, _lastRay.direction * 100, Color.red);

    if (!MouseButton.Left.IsDown()) return;
    _lastRay = Camera.main.ScreenPointToRay(Input.mousePosition); //
    if (Physics.Raycast(_lastRay, out RaycastHit hit)) {
      _agent.destination = hit.point;
    }
  }
}
