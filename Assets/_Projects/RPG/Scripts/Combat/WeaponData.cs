using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  [CreateAssetMenu(fileName = "Weapon", menuName = "Project/RPG/Weapon Data", order = 0)]
  /// <summary>
  /// Invoke Init() to setup weapon data.
  /// </summary>
  public class WeaponData : ScriptableObject {
    [OnValueChanged(nameof(GetProjectileSpawner))]
    public GameObject Prefab;
    public AnimatorOverrideController AnimController;

    [Tooltip("Move to target and stop at this distance to attack.")]
    public float Range = 2;
    public int Damage = 1;

    [Tooltip("Delay between attacks - Opposite of rate.")]
    [Min(0.5f)] public float Cooldown = 1f;
    public bool IsRightHand = true;

    [OnValueChanged(nameof(GetProjectileSpawner))]
    public bool HasProjectile;

    [ShowIf(nameof(HasProjectile))]
    [InlineEditor(InlineEditorModes.FullEditor)]
    // TODO: Validate if projectile spawner exist in prefab
    // this links to Spawner in prefab, hence changes auto update
    public Spawner ProjectileSpawner;

    private void GetProjectileSpawner() {
      if (HasProjectile) ProjectileSpawner = Prefab.GetComponentInChildren<Spawner>();
    }

    public GameObject Init(Transform weaponSlot, Animator animator) {
      GameObject weapon = null;
      if (Prefab) weapon = Instantiate(Prefab, parent: weaponSlot);

      if (AnimController) {
        animator.runtimeAnimatorController = AnimController;
      }

      return weapon;
    }
  }
}
