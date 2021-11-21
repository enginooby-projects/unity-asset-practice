using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enginoobz.Operator;
using Enginoobz.Core;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  public class Fighter : Attacker, IAction {
    [Tooltip("Override agent speed when approaching target before attack.")]
    [SerializeField, Min(0.5f)]
    private float _chaseSpeed = 5f;

    [SerializeField]
    private Transform handRight;

    [SerializeField]
    private Transform handLeft;

    [InlineEditor, SerializeField]
    private WeaponData _weaponData;

    [AutoRef, SerializeField, HideInInspector]
    private NavMeshAgentOperator _agentOpr;

    [AutoRef, SerializeField, HideInInspector]
    private Animator _animator;

    private readonly int attackHash = Animator.StringToHash("attack");
    private readonly int stopAttackHash = Animator.StringToHash("stopAttack");
    private readonly int isTurning = Animator.StringToHash("isTurning");
    private GameObject _currentWeapon;
    private Attackable _currentTarget;
    private bool _isAttacking;
    private float timeSinceLastAttack;
    private bool _isLookingAtCurrentTarget;
    private bool _isTurningToCurrentTarget;
    private Spawner projectileSpawner;
    private ParticleSystem[] weaponVfxs;

    public float ChaseSpeed { get => _chaseSpeed; set { if (value > 0.5f) _chaseSpeed = value; } }

    private void Start() {
      EquipWeapon(_weaponData);
    }

    public void EquipWeapon(WeaponData weaponData) {
      if (!weaponData) return;

      Destroy(_currentWeapon);
      _weaponData = weaponData;
      Transform weaponSlot = (_weaponData.IsRightHand) ? handRight : handLeft;
      _currentWeapon = _weaponData.Init(weaponSlot, _animator);
      if (_weaponData.HasProjectile) projectileSpawner = _currentWeapon.GetComponentInChildren<Spawner>();
      weaponVfxs = _currentWeapon?.GetComponentsInChildren<ParticleSystem>();
    }

    private void Update() {
      if (_isAttacking && _currentTarget) ApproachAndAttackCurrentTarget();
    }

    // ? Rename to set target
    /// <summary>
    /// [Update-safe method]
    /// </summary>
    public override void Attack(Attackable target) {
      if (_isAttacking && target == _currentTarget || target.IsDead) return;

      // print("Start attack " + target.name);
      _isAttacking = true;
      _currentTarget = target;
      _isLookingAtCurrentTarget = false;
      _agentOpr.SetSpeed(_chaseSpeed);
      _animator.ResetTrigger(stopAttackHash);
    }

    private void ApproachAndAttackCurrentTarget() {
      if (!transform.IsInRange(_currentTarget.transform, _weaponData.Range)) {
        // FIX: agent does not guarantee to arrive at the range (e.g target is on the air)
        _agentOpr.MoveTo(_currentTarget.transform.position, _weaponData.Range);
        _isLookingAtCurrentTarget = true; // agent auto turn towards to the destination (target)
      } else {
        if (Time.time - timeSinceLastAttack > _weaponData.Cooldown) {
          LookAtAndAttackCurrentTarget();
          timeSinceLastAttack = Time.time;
        }
      }
    }

    private void LookAtAndAttackCurrentTarget() {
      if (!_isLookingAtCurrentTarget && !_isTurningToCurrentTarget) {
        TurnToCurrentTarget();
      } else
        AttackCurrentTarget();
    }

    private void AttackCurrentTarget() {
      if (_currentTarget.IsDead) return;
      _animator.SetTrigger(attackHash);
    }

    private void TurnToCurrentTarget() {
      _isTurningToCurrentTarget = true;
      _animator.SetBool(isTurning, true);
      transform.DOLookAt(_currentTarget.transform.position, 400)
              .SetSpeedBased()
              .OnComplete(() => {
                _isLookingAtCurrentTarget = true;
                _isTurningToCurrentTarget = false;
                _animator.SetBool(isTurning, false);
                AttackCurrentTarget();
              });
    }

    /// <summary>
    /// [Update-safe method]
    /// </summary>
    // TIP: use flag to make update-safe method: method is ok to call in Update() w/o high performance cost
    public void Cancel() {
      if (!_isAttacking) return;

      // print("Cancel attack");
      _isAttacking = false;
      _isLookingAtCurrentTarget = false;
      _animator.SetTrigger(stopAttackHash);
    }

    #region ANIMATION EVENTS
    void OnAttackTriggeredByAnim() {
      if (!_currentTarget || _currentTarget.IsDead) {
        _isAttacking = false;
        return;
      }

      if (_weaponData.HasProjectile) {
        // ? Move projectile spawning logic to Spawner
        List<GameObject> projectilesGo = projectileSpawner.Spawn();

        projectilesGo.ForEach(go => {
          if (go.TryGetComponent<Projectile>(out Projectile projectile)) {
            projectile.SetTarget(_currentTarget.transform);
            projectile.Owner = this;
            projectile.onHitAttackableTarget += OnProjectileHit;
          }
        });
      } else {
        _currentTarget?.GetAttacked(this, _weaponData.Damage);
      }

      weaponVfxs?.Play();
    }

    /// <summary>
    /// Enemy hit by projectile does not have to be the current target.
    /// </summary>
    // ! Overloading method of animation event should be place below the event method in script, other delegate to it.
    // ! E.g. if OnHit(CombatTarget) is above OnHit(), animation invoke this will cause error
    void OnProjectileHit(Attacker attacker, Attackable target) {
      target.GetAttacked(attacker, _weaponData.Damage);
    }

    void OnHit() {
      OnAttackTriggeredByAnim();
    }

    void OnShoot() {
      OnAttackTriggeredByAnim();
    }

    // TIP: guarding animation events (simply delegate if animation event has similiar name)
    void Hit() {
      OnHit();
    }

    void Shoot() {
      OnShoot();
    }
    #endregion
  }
}