// * Effect is setup by the component itself.
// * Prefab workflow. Interactor singleton need to be setup in scene.

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br/>
/// TComponent is also the interacting effect which can be setup with prefabs.
/// </summary>
public abstract class GOInteractorEffectComponent<TSelf, TComponent> : GOInteractor<TSelf, TComponent>
where TSelf : GOInteractorEffectComponent<TSelf, TComponent>
where TComponent : MonoBehaviour {
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

  protected bool TryGetCachedComponent(GameObject go, out TComponent component) {
    if (_interactedGos.ContainsKey(go)) {
      component = _interactedGos[go];
      return true;
    }

    component = null;
    return false;
  }

  protected virtual void OnComponentAdded(GameObject go, TComponent component) { }
}


/// <summary>
/// Cache last effect (component) to compare with current when re-interacting.
/// </summary>
public abstract class GOInteractorEffectComponent<TSelf, TComponent, TLastEffect> : GOInteractorEffectComponent<TSelf, TComponent>
where TSelf : GOInteractorEffectComponent<TSelf, TComponent, TLastEffect>
where TComponent : MonoBehaviour {
  protected new Dictionary<GameObject, CachedEffects<TComponent>> _interactedGos = new Dictionary<GameObject, CachedEffects<TComponent>>();
  protected override void ClearInteractedGos() => _interactedGos.Clear();
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();
}


public abstract class GOInteractorEffectComponentCacheLastEffect<TSelf, TComponent> : GOInteractorEffectComponent<TSelf, TComponent, TComponent>
where TSelf : GOInteractorEffectComponentCacheLastEffect<TSelf, TComponent>
where TComponent : MonoBehaviour {
}

public struct CachedEffects<TComponent> {
  public TComponent ComponentEffect;
  public TComponent LastEffect;

  public CachedEffects(TComponent componentEffect, TComponent lastEffect) {
    ComponentEffect = componentEffect;
    LastEffect = lastEffect;
  }
}