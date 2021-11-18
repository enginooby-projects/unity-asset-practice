using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  // ? Create Projectile<TargetType> generic (replace CombatTarget)
  public class Projectile : MonoBehaviour {
    [SerializeField] private Transform _target; // ? Constraint target transform by type
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _chasingTarget;

    [SerializeField, LabelText("Impact VFX")] private ParticleSystem impactVfx;
    private float impactDuration;

    [SerializeField] private bool _stayOnCollisionPoint;
    [SerializeField] private float _stayOnCollisionDuration = 3f;

    private new Collider collider;
    private MeshRenderer meshRenderer;
    public event Action<CombatTarget> onHitCombatTarget;

    /// <summary>
    /// If not chasing target, then only aim at inital position.
    /// </summary>
    private Vector3 initialTargetPos;
    private PoolObject poolComponent;
    private bool enableFlying = true;

    void Start() {
      GetInitialTargerPos();
      if (impactVfx) impactDuration = impactVfx.main.duration;
      poolComponent = GetComponent<PoolObject>();
      poolComponent.onEnable += ResetForPool;
      collider = GetComponent<Collider>();
      meshRenderer = GetComponent<MeshRenderer>();
    }

    private void GetInitialTargerPos() {
      initialTargetPos = _target.GetColliderCenter();
      transform.LookAtY(initialTargetPos);
    }

    void Update() {
      Fly();
    }

    public void SetTarget(Transform target) {
      _target = target;
      GetInitialTargerPos();
    }

    private void Fly() {
      if (!enableFlying) return;

      // TODO: Paramiterize axis
      if (_chasingTarget)
        transform.LookAtAndMoveY(_target.GetColliderCenter(), distance: _speed);
      else {
        transform.position += transform.up * Time.deltaTime * _speed; // UTIL
      }
    }

    private IEnumerator OnTriggerEnter(Collider other) {
      print(gameObject.name + " hit " + other.name);
      if (impactVfx) impactVfx.Play();

      // TODO: option ignore everything not target type (only realse when hit target)
      if (other.TryGetComponent<CombatTarget>(out CombatTarget combatTarget)) {
        onHitCombatTarget?.Invoke(combatTarget);
      }

      // TIP: reset event to prevent action invokes mutiple times because projectile is reused by pool
      onHitCombatTarget = null;
      enableFlying = false;
      collider.enabled = false;

      if (!_stayOnCollisionPoint) {
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(impactDuration);
      } else {
        Transform initialParent = transform.parent;
        transform.SetParent(other.transform);
        Vector3 initialScale = transform.localScale;
        transform.DOScale(Vector3.zero, _stayOnCollisionDuration);

        yield return new WaitForSeconds(_stayOnCollisionDuration);
        transform.SetParent(initialParent);
        transform.localScale = initialScale;
      }

      poolComponent?.ReleaseToPool();
    }

    /// <summary>
    /// Clean dirty pooled projectile for reuse.
    /// </summary>
    public void ResetForPool() {
      enableFlying = true;
      collider.enabled = true;

      if (!_stayOnCollisionPoint) {
        meshRenderer.enabled = true;
      }
    }
  }
}
