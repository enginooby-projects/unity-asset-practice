// * Custom effect type

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br/>
/// TEffect: Effect variation when the GO is interacted. <br/>
/// TCacheObject: Cached Object of the interated GO for implementing revert method. <br/>
/// </summary>
public abstract class GOInteractor<TSelf, TComponent, TEffect, TCacheObject> : GOInteractor<TSelf>
where TSelf : GOInteractor<TSelf, TComponent, TEffect, TCacheObject>
where TComponent : MonoBehaviour
where TEffect : GOInteractEffect // TODO: Generalize interacting effect (enum/prefab). Wrap config in custom struct
where TCacheObject : Object {
  [SerializeField]
  protected TEffect _effect; // ? Implement list

  public override void IncrementInteractingEffect() => _effect.Increment();
  public override void DecrementInteractingEffect() => _effect.Decrement();
  protected virtual TEffect InitInteractingEffect() => default(TEffect);

  protected new Dictionary<GameObject, Tuple<TComponent, TCacheObject>> _interactedGos = new Dictionary<GameObject, Tuple<TComponent, TCacheObject>>();
  protected override void ClearInteractedGos() => _interactedGos.Clear();
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();

  /// <summary>
  /// Cache necessary Object of the GO to implement reverting method.
  /// </summary>
  protected abstract TCacheObject CacheObject(GameObject go);

  public abstract void Interact(GameObject go, TEffect effect);

  public override void Interact(GameObject go) => Interact(go, _effect);

  public TComponent AddOrGetCachedComponent(GameObject go) {
    TComponent component;

    if (_interactedGos.ContainsKey(go)) {
      component = _interactedGos[go].Item1;
    } else {
      var cachedObject = CacheObject(go) as TCacheObject;

      if (!go.TryGetComponent<TComponent>(out component)) {
        component = go.AddComponent<TComponent>();
        OnComponentAdded(go, component);
      }

      var cache = new Tuple<TComponent, TCacheObject>(component, cachedObject);
      _interactedGos.Add(go, cache);
    }

    component.enabled = true;

    return component;
  }

  protected virtual void OnComponentAdded(GameObject go, TComponent component) { }

  public override void AwakeSingleton() {
    base.AwakeSingleton();
    _effect = InitInteractingEffect();
  }
}
