// * Effect type is enum = effect is indicate by an enum. 
// * Interactor singleton don't need to be created in scene.

using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br/>
/// TEffectEnum: Effect variation (enum) when the GO is interacted. <br/>
/// TCacheObject: Cached Object of the interated GO for implementing revert method. <br/>
/// </summary>
public abstract class GOInteractorEffectEnum<TSelf, TComponent, TEffectEnum, TCacheObject>
: GOInteractor<TSelf, TComponent, GOInteractEffect<TEffectEnum>, TCacheObject>
where TSelf : GOInteractorEffectEnum<TSelf, TComponent, TEffectEnum, TCacheObject>
where TComponent : MonoBehaviour
where TEffectEnum : struct
where TCacheObject : Object {
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