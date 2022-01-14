using EPOOutline;
using UnityEngine;

public class GOHighlighter : GOInteractorEffectComponentCacheLastEffect<GOHighlighter, Outlinable> {
  public override void Interact(GameObject go, Outlinable effect) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      cache.ComponentEffect.enabled = true;
      if (effect.Equals(cache.LastEffect)) return;

      Destroy(cache.ComponentEffect);
      _interactedGos.Remove(go);
    }

    AddOutlinable(go, effect);
  }

  // instantiate a new Outlinable to use as template instead of directly using SerializeField prefab 
  // to prevent change linking (update in this GameObject will cause update in effect prefab)
  // REFACTOR: Generalize AddComponentEffectByCloning
  private void AddOutlinable(GameObject go, Outlinable effect) {
    var outlinableTemplate = Instantiate(effect);
    var outlinable = go.AddComponent<Outlinable>().GetLinkedCopyOf(outlinableTemplate);
    var outlineTarget = new OutlineTarget(go.GetComponent<Renderer>());
    var cache = new CachedEffects<Outlinable>(outlinable, effect);

    Destroy(outlinableTemplate.gameObject);
    outlinable.OutlineTargets.Add(outlineTarget);
    if (!_interactedGos.ContainsKey(go))
      _interactedGos.Add(go, cache);
  }

  public override void InteractRestore(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      cache.ComponentEffect.enabled = true;
    }
  }

  public override void InteractRevert(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      cache.ComponentEffect.enabled = false;
    }
  }

  public override void InteractToggle(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      cache.ComponentEffect.enabled = !cache.ComponentEffect.enabled;
    }
  }
}