using System;
using System.Collections.Generic;
using System.Linq;
using Enginooby.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

// TODO
// + VFX & SFX on changed

// REFACTOR: Make ModelVariation SO
/// <summary>
/// For changing character (visual) while keeping behaviours, camera.
/// </summary>
public class ModelSwitcher : MonoBehaviour {
  // * Clone data from preset to 
  [SerializeField] private List<Animator> _animators = new();

  [SerializeField] private List<Collider> _colliders = new();

  [SerializeField] private List<Rigidbody> _rigidbodies = new();

  // * Switch
  // ? FX per model preset
  [SerializeField] [LabelText("Switch VFX")]
  private ParticleSystem _switchVfx;

  [SerializeField] [LabelText("Switch SFX")]
  private AudioClip _switchSfx;

  [SerializeField] private InputModifier _switchKey;

  [SerializeField] private InputModifier _toggleKey;

  // * Model
  [SerializeField] [ValueDropdown(nameof(Models))] [OnValueChanged(nameof(OnModelSelected))]
  private GameObject _currentModel;

  // TODO: Deactivate GO of newly-added preset
  [SerializeField] private List<ModelAvatarPreset> _presets = new();


  [SerializeField] [HideInInspector] private ModelAvatarPreset _currentPreset;
  private IEnumerable<GameObject> Models => _presets.Select(preset => preset.Model);
  private ModelAvatarPreset _lastPreset;

  private void OnModelSelected() {
    var preset = _presets.Find(preset => preset.Model.Equals(_currentModel));
    ChangeCurrentPreset(preset);
  }

  private void ChangeCurrentPreset(ModelAvatarPreset preset) {
    _lastPreset = _currentPreset ?? preset;
    _currentPreset = preset;
    _lastPreset.Model.SetActive(false);
    _currentPreset.Model.SetActive(true);
    _animators.ForEach(animator => animator.avatar = _currentPreset.Avatar);
    _switchVfx?.Play();
  }

  private void Update() {
    if (_switchKey.IsTriggering) ChangeCurrentPreset(_presets.GetNext(_currentPreset));

    if (_toggleKey.IsTriggering) _currentPreset.Model.ToggleActive(); // TODO: Add FX
  }
}

// ? Generics
// ? Change to Dictionary to avoid duplication
// REFACTOR: Make ModelPreset SO with collider data, prefab model, scene model
[Serializable]
[InlineProperty]
public class ModelAvatarPreset {
  [OnValueChanged(nameof(TryGetAvatar))] public GameObject Model;
  public Avatar Avatar;

  private void TryGetAvatar() {
    if (Model.TryGetComponent<Animator>(out var animator)) Avatar = animator.avatar;
  }
}