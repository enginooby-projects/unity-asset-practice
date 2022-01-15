using MegaFiers;

public class GOMeshDeformer : GOInteractorEffectComponentCacheLastEffect<GOMeshDeformer, MegaModifier> {
  // ? Use Adapter pattern to implement generic property
  protected override void SetComponentActive(MegaModifier component, bool isActive) => component.ModEnabled = isActive;
  protected override bool GetComponentActive(MegaModifier component) => component.ModEnabled;
}