using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  [CreateAssetMenu(fileName = "Weapon", menuName = "Project/RPG/Weapon Data", order = 0)]
  public class WeaponData : ScriptableObject {
    [SerializeField] public GameObject Prefab;
    [SerializeField] public AnimatorOverrideController AnimController;

    [Tooltip("Move to target and stop at this distance to attack.")]
    [SerializeField] public float Range = 2;
    [SerializeField] public int Damage = 1;

    [Tooltip("Delay between attacks - Opposite of rate.")]
    [SerializeField, Min(0.5f)] public float Cooldown = 1f;
    [SerializeField] public bool IsRightHand = true;

    public void Init(Transform weaponSlot, Animator animator) {
      if (Prefab) Instantiate(Prefab, parent: weaponSlot);
      if (AnimController) animator.runtimeAnimatorController = AnimController;
    }
  }
}
