// * Effect is setup by the component itself.
// * Prefab workflow. Interactor singleton need to be setup in scene.

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract partial class GOI_ComponentIsEffect<TSelf, TComponent> {
  // TODO: Replace by Collection
  [ValueDropdown(nameof(_effectPrefabs))]
  [SerializeField]
  private TComponent _currentEffect;
  [SerializeField]
  private List<TComponent> _effectPrefabs;

  public TComponent CurrentEffect => _currentEffect;

  // Fallback in case the singleton component is not available or have not set up effects
  private void InitializeEffect() {
    if (_currentEffect) return;

    if (_effectPrefabs.IsSet()) {
      _currentEffect = _effectPrefabs[0];
    } else {
      // TODO: Find asset by type in project, if not found throw error
    }
  }

  public override void AwakeSingleton() {
    base.AwakeSingleton();
    InitializeEffect();
  }

  public override void IncrementInteractingEffect() => _currentEffect = _effectPrefabs.GetNext(_currentEffect);
  public override void DecrementInteractingEffect() => _currentEffect = _effectPrefabs.GetPrevious(_currentEffect);

  public abstract void Interact(GameObject go, TComponent effect);
  public override void Interact(GameObject go) => Interact(go, _currentEffect);

  public override void InteractRestore(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var component)) {
      SetComponentActive(component, true);
    }
  }

  public override void InteractRevert(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var component)) {
      SetComponentActive(component, false);
    }
  }

  public override void InteractToggle(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var component)) {
      SetComponentActive(component, !GetComponentActive(component));
    }
  }

  protected virtual TComponent AddOrGetCachedComponent(GameObject go) {
    TComponent component;

    if (_interactedGos.ContainsKey(go)) {
      component = _interactedGos[go];
    } else {
      if (!go.TryGetComponent<TComponent>(out component)) {
        component = go.AddComponent<TComponent>();
        OnComponentAdded(go, component);
      }

      _interactedGos.Add(go, component);
    }

    component.enabled = true;
    return component;
  }

  protected virtual void OnComponentAdded(GameObject go, TComponent component) { }
}