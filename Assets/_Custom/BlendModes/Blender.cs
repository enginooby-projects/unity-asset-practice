using System.Collections.Generic;
using UnityEngine;
using BlendModes;
using Enginooby.Utils;

// TODO
//+ Create customized effects
//? Create generic GoInteractor<MonoBehaviour> for interactors adding component to the GO, e.g., Blender, Highlighter
//? Rename to BlenderController and separate Blender w/o input key

/// <summary>
/// * Use cases: colorize GOs, see through, un-dhighlight
/// </summary>
public class Blender : MonoBehaviour {
  // GO between camera and target turns transparent, then inverse if no longer.
  // Normally used for player to be always visible
  [SerializeField] private GameObject _trackingTarget; //? Implement multiple targets
  [SerializeField] private bool _enableBlendThrough = true; // if disable, don't blend GOs behind the 1st one.
  [SerializeField] private InputModifier _blendKey = new(); // detect by collider
  [SerializeField] private InputModifier _revertKey = new();
  [SerializeField] private InputModifier _blendSelectedKey = new(); // blend current selected GO from Selector
  [SerializeField] private InputModifier _blendGlobalKey = new(); // set all blended GOs to current blend mode
  [SerializeField] private InputModifier _revertGlobalKey = new(); // revert all blended GOs
  [SerializeField] private InputModifier _changeEffectKey = new();
  [SerializeField] private BlendMode _blendMode;

  private Selector _selector;

  // original material //? Multiple materials
  private Dictionary<GameObject, Material> _blendedGos = new();

  private void Awake() {
    _selector = FindObjectOfType<Selector>();
  }

  private void Update() {
    // ProcessTrackingTarget();
    ProcessBlendedGos();
    ProcessBlendingByRayCast();
    ProcessBlendingSeletedObject();
    ProcessRevertingByRayCast();

    if (_changeEffectKey.IsTriggering)
      _blendMode = _blendMode.Next();
  }

  // REFACTOR
  private void ProcessBlendingByRayCast() {
    if (!_blendKey.IsTriggering || !RayUtils.IsMouseRayHit) return;

    if (_enableBlendThrough)
      foreach (var hit in RayUtils.HitsFromMouseRay) //? How to detect w/o Collider
        SetBlendMode(hit.transform.gameObject, _blendMode);
    else
      SetBlendMode(RayUtils.HitsFromMouseRay[0].transform.gameObject, _blendMode);
  }

  private void ProcessRevertingByRayCast() {
    if (!_revertKey.IsTriggering || !RayUtils.IsMouseRayHit) return;

    if (_enableBlendThrough)
      foreach (var hit in RayUtils.HitsFromMouseRay)
        RevertBlending(hit.transform.gameObject);
    else
      RevertBlending(RayUtils.HitsFromMouseRay[0].transform.gameObject);
  }

  private void ProcessBlendingSeletedObject() {
    if (_blendSelectedKey.IsTriggering && _selector)
      SetBlendMode(_selector.CurrentSelectedObject, _blendMode);
  }

  private void ProcessBlendedGos() {
    if (_blendGlobalKey.IsTriggering)
      foreach (var go in _blendedGos.Keys)
        SetBlendMode(go, _blendMode);

    if (_revertGlobalKey.IsTriggering)
      foreach (var go in _blendedGos.Keys)
        RevertBlending(go);
  }

  private void ProcessTrackingTarget() {
    _trackingTarget.DrawRayToCamera();
    // FIX: Hit GOs not correct
    _trackingTarget.GetHitsFromCameraRay().ForEach(go => SetBlendMode(go, _blendMode));
  }

  public void SetBlendMode(GameObject go, BlendMode blendMode) {
    BlendModeEffect blendComponent;

    // REFACTOR
    if (_blendedGos.ContainsKey(go)) {
      blendComponent = go.GetComponent<BlendModeEffect>();
      blendComponent.SetBlendMode(blendMode);
      blendComponent.enabled = true;
      return;
    }

    _blendedGos.Add(go, go.GetComponent<Renderer>().material);
    if (!go.TryGetComponent<BlendModeEffect>(out blendComponent)) {
      blendComponent = go.AddComponent<BlendModeEffect>();
      blendComponent.ShaderFamily = "LwrpUnlitTransparent"; // ! String
    }

    blendComponent.enabled = true;
    blendComponent.SetBlendMode(blendMode);
  }

  // TODO: Handle blended GO in edit time
  public void RevertBlending(GameObject go) {
    if (_blendedGos.TryGetValue(go, out var originalMaterial)) {
      go.GetComponent<BlendModeEffect>().enabled = false;
      go.GetComponent<Renderer>().material = originalMaterial;
    }
  }
}