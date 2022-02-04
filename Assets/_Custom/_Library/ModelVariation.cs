using System;
using Sirenix.OdinInspector;
using UnityEngine;

// Preset instantiate/link to object in scene

public class ModelVariation : ScriptableObject {

}


// ? Animator override controller
[Serializable, InlineProperty]
public class ModelPreset : ScriptableObject {
  // [SceneObjectsOnly]
  public GameObject ModelInstance; // ? Move to MonoBehaviour
  [AssetsOnly]
  [OnValueChanged(nameof(OnModelPrefabChanged))]
  [SerializeField] private GameObject _modelPrefab;
  [SerializeField] private Avatar _avatar;
  [SerializeField] private Collider _collider;

  // public float ColliderHeight = 2f;
  // public float ColliderRadius = 1f;
  // public Vector3 ColliderCenter = new Vector3(0, 0.5f, 0);

  private void OnModelPrefabChanged() {
    ModelInstance = null;

    if (_modelPrefab.TryGetComponent<Animator>(out var animator)) {
      _avatar = animator.avatar;
    }

    if (_modelPrefab.TryGetComponent<Collider>(out var collider)) {
      _collider = collider;
    }
  }
}