using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enginoobz.Core;

// TODO: Move into Library
namespace Enginoobz.Operator {
  [RequireComponent(typeof(NavMeshAgent))]
  public class NavMeshAgentOperator : MonoBehaviourBase, IAction {
    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgent _agent;

    /// <summary>
    /// Not in Update().
    /// </summary>
    // ! distance 0 sometimes cause jiggering/shaking movement
    public void MoveTo(Vector3 dest, float stoppingDistance = 0.5f) {
      if (!_agent && !_agent.enabled) return;

      _agent.isStopped = false;
      _agent.stoppingDistance = stoppingDistance;
      _agent.destination = dest;
    }

    public void Cancel() {
      _agent.isStopped = true;
    }

    public Vector3 LocalVelocity => transform.InverseTransformVector(_agent.velocity);

    private void Update() {
      if (enableAnimation) HandleAnimation();
    }

    #region PUBLIC METHODS ===================================================================================================================================
    // TODO: Declare method for IOperator
    /// <summary>
    /// Destroy the operator along with its component
    /// </summary>
    public void DestroyWithComponent() {
      Destroy(_agent);
      Destroy(this);
    }
    #endregion

    #region ANIMATION ===================================================================================================================================
    [SerializeField] private bool enableAnimation = true;
    // ! SPECIFIC
    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;
    private readonly int _forwardSpeedHash = Animator.StringToHash("forwardSpeed");

    private void HandleAnimation() {
      _animator.SetFloat(_forwardSpeedHash, LocalVelocity.z);
    }
    #endregion ===================================================================================================================================
  }
}