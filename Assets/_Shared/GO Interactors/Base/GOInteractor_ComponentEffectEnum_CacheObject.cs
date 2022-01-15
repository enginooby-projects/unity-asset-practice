// * Effect type is enum = effect is indicate by an enum. 
// * Interactor singleton don't need to be created in scene.

using UnityEngine;

public abstract partial class GOI_EffectIsEnum<TSelf, TComponent, TEffectEnum, TCache> {
  [SerializeField]
  protected new TEffectEnum _effect;

  protected virtual TEffectEnum InitEffectEnum() => default(TEffectEnum);

  public abstract void Interact(GameObject go, TEffectEnum effect);

  public override void IncrementInteractingEffect() => _effect = _effect.Next();
  public override void DecrementInteractingEffect() => _effect = _effect.Previous();

  public override void AwakeSingleton() {
    base.AwakeSingleton();
    _effect = InitEffectEnum();
  }

  public override void Interact(GameObject go, GOInteractEffect<TEffectEnum> effectEnum) {
    Interact(go, _effect);
  }
}