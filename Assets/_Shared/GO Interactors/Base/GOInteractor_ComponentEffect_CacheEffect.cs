using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract partial class GOInteractor<TSelf, TComponent, TEffect> {
  protected new Dictionary<GameObject, CachedEffects<TComponent>> _interactedGos = new Dictionary<GameObject, CachedEffects<TComponent>>();
  protected override void ClearInteractedGos() => _interactedGos.Clear();
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();
}


public abstract partial class GOInteractorEffectComponentCacheLastEffect<TSelf, TComponent> {
  /// <summary>
  /// Default implementation: if cached, set active. Otherwise, copy (unlinked) effect to GO.
  /// </summary>
  public override void Interact(GameObject go, TComponent effect) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      SetComponentActive(cache.ComponentEffect, true);
      if (effect.Equals(cache.LastEffect)) return;
    }

    var componentEffect = effect.CopyTo(go);
    var newCache = new CachedEffects<TComponent>(componentEffect, effect);

    // UTIL: implement TryAdd
    if (!_interactedGos.ContainsKey(go)) _interactedGos.Add(go, newCache);
  }

  public override void InteractRestore(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      SetComponentActive(cache.ComponentEffect, true);
    }
  }

  public override void InteractRevert(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      SetComponentActive(cache.ComponentEffect, false);
    }
  }

  public override void InteractToggle(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      SetComponentActive(cache.ComponentEffect, !GetComponentActive(cache.ComponentEffect));
    }
  }
}


public struct CachedEffects<TComponent> {
  public TComponent ComponentEffect;
  public TComponent LastEffect;

  public CachedEffects(TComponent componentEffect, TComponent lastEffect) {
    ComponentEffect = componentEffect;
    LastEffect = lastEffect;
  }
}