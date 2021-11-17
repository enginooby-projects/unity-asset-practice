using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.RPG.Combat {
  public class WeaponPickup : MonoBehaviour {
    [SerializeField] private WeaponData weaponData;

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
        other.GetComponent<Fighter>().EquipWeapon(weaponData);
        Destroy(gameObject);
      }
    }
  }
}
