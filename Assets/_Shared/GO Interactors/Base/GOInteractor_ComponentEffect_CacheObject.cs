// * Custom effect type

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract partial class GOInteractor<TSelf, TComponent, TEffect, TCache> {
  [SerializeField]
  protected TEffect _effect; // ? Implement list

  public override void IncrementInteractingEffect() => _effect.Increment();
  public override void DecrementInteractingEffect() => _effect.Decrement();
  protected virtual TEffect InitInteractingEffect() => default(TEffect);

  protected new Dictionary<GameObject, Tuple<TComponent, TCache>> _interactedGos = new Dictionary<GameObject, Tuple<TComponent, TCache>>();
  protected override void ClearInteractedGos() => _interactedGos.Clear();
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();

  /// <summary>
  /// Cache necessary Object of the GO to implement reverting method.
  /// </summary>
  protected abstract TCache CacheObject(GameObject go);

  public abstract void Interact(GameObject go, TEffect effect);

  public override void Interact(GameObject go) => Interact(go, _effect);

  public TComponent AddOrGetCachedComponent(GameObject go) {
    TComponent component;

    if (_interactedGos.ContainsKey(go)) {
      component = _interactedGos[go].Item1;
    } else {
      var cachedObject = CacheObject(go) as TCache;

      if (!go.TryGetComponent<TComponent>(out component)) {
        component = go.AddComponent<TComponent>();
        OnComponentAdded(go, component);
      }

      var cache = new Tuple<TComponent, TCache>(component, cachedObject);
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
