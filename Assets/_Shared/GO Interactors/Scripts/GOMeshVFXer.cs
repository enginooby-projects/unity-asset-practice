// Dependencies: Mesh Effects

using System.Collections;
using UnityEngine;

public class GOMeshVFXer : GOInteractorEffectComponent<GOMeshVFXer, PSMeshRendererUpdater> {
  // Add a small delay when changing effect 
  // to prevent destroying the previous effect also destroy the material of the next effect
  const float TRANSITION_DELAY = 0.2f;

  public override void Interact(GameObject go, PSMeshRendererUpdater effect) {
    StartCoroutine(AddMeshVFXCoroutine(go, effect));
  }

  public override void InteractRevert(GameObject go) {
    if (TryGetCachedComponent(go, out var meshUpdater)) {
      meshUpdater.IsActive = false;
    }
  }

  public override void InteractRestore(GameObject go) {
    if (TryGetCachedComponent(go, out var meshUpdater)) {
      meshUpdater.IsActive = true;
    }
  }

  public override void InteractToggle(GameObject go) {
    if (TryGetCachedComponent(go, out var meshUpdater)) {
      meshUpdater.IsActive = !meshUpdater.IsActive;
    }
  }

  //! This completely replaces AddOrGetCachedComponent in base since it adds component to a child not this GO.
  // REFACTOR: Generalize and move to base class (AddChildComponentEffectCoroutine)
  private IEnumerator AddMeshVFXCoroutine(GameObject go, PSMeshRendererUpdater effect) {
    if (_interactedGos.ContainsKey(go)) _interactedGos.Remove(go);

    DestroyOldMeshVFXes(go);
    yield return new WaitForSeconds(TRANSITION_DELAY);

    var meshUpdater = Instantiate(effect, go.transform.position, Quaternion.identity);
    meshUpdater.gameObject.transform.SetParent(go.transform);
    meshUpdater.UpdateMeshEffect(go);

    if (!_interactedGos.ContainsKey(go)) _interactedGos.Add(go, meshUpdater);
  }

  // REFACTOR: Generalize and move to base class (BeforeComponentAdd)
  // If don't destroy, the old traits such as lights, particles of the old VFX will remain 
  private void DestroyOldMeshVFXes(GameObject go) {
    var meshUpdaters = go.GetComponentsInChildren<PSMeshRendererUpdater>();
    foreach (var meshUpdater in meshUpdaters) {
      meshUpdater.IsActive = false;
      Destroy(meshUpdater.gameObject, TRANSITION_DELAY);
    }
  }
}
