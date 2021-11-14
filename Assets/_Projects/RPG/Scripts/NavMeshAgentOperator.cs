using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentOperator : MonoBehaviour {
  [SerializeField] private Transform _target;
  [AutoRef, SerializeField, HideInInspector] private NavMeshAgent _agent;

  void Update() {
    ProcessMoveToCursor();
  }

  // ? Separate to player
  #region CONTROL 
  private Ray _lastRay;

  private void ProcessMoveToCursor() {
    Debug.DrawRay(_lastRay.origin, _lastRay.direction * 100, Color.red);

    if (!MouseButton.Left.IsDown()) return;
    _lastRay = Camera.main.ScreenPointToRay(Input.mousePosition); //
    if (Physics.Raycast(_lastRay, out RaycastHit hit)) {
      _agent.destination = hit.point;
    }
  }
  #endregion
}
