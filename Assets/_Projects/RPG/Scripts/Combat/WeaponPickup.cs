using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Combat {
  [TypeInfoBox("To add new pickup, create new prefab variant from root prefab then add model as child of it.")]
  public class WeaponPickup : MonoBehaviour {

    [SerializeField] private WeaponData weaponData;

    private void Start() {
      GameObject prefab = GetComponentInChildren<MeshFilter>().gameObject;
      prefab.AddAndSetupColliderIfNotExist(isTrigger: true);
    }

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
        other.GetComponent<Fighter>().EquipWeapon(weaponData);
        Destroy(gameObject);
      }
    }
  }
}
