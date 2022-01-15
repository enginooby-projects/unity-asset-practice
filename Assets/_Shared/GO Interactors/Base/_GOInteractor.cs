// TIP: "Menu" file to review relationships in complex generic class hierarchy 

using UnityEngine;

public abstract partial class GOInteractor : MonoBehaviourSingleton<GOInteractor> { }


public abstract partial class GOInteractor<TSelf> : GOInteractor
where TSelf : GOInteractor<TSelf> { }


/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component type to the GO. <br/>
/// </summary>
public abstract partial class GOInteractor<TSelf, TComponent>
: GOInteractor<TSelf>
where TSelf : GOInteractor<TSelf, TComponent>
where TComponent : MonoBehaviour { }


/// <summary>
/// Cache last effect (component) to compare with current when re-interacting.
/// </summary>
public abstract partial class GOInteractor<TSelf, TComponent, TEffect>
: GOI_ComponentIsEffect<TSelf, TComponent> // ! Should be GOInteractor<TSelf>
                                           // : GOInteractor<TSelf>
where TSelf : GOInteractor<TSelf, TComponent, TEffect>
where TComponent : MonoBehaviour { }


/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component type to the GO. <br/>
/// TEffect: Effect variation when the GO is interacted. <br/>
/// TCache: Cached Object/effect of the interated GO for implementing revert method.<br/>
/// </summary>
public abstract partial class GOInteractor<TSelf, TComponent, TEffect, TCache>
: GOInteractor<TSelf>
where TSelf : GOInteractor<TSelf, TComponent, TEffect, TCache>
where TComponent : MonoBehaviour
where TEffect : GOInteractEffect // TODO: Generalize interacting effect config (enum/prefab/SO/struct)
where TCache : Object { }// TODO: Generalize - GOInteractCache


/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br/>
/// TComponent is also the interacting effect which can be setup with prefabs.
/// </summary>
public abstract partial class GOI_ComponentIsEffect<TSelf, TComponent>
: GOInteractor<TSelf, TComponent>
where TSelf : GOI_ComponentIsEffect<TSelf, TComponent>
where TComponent : MonoBehaviour { }


public abstract partial class GOInteractorEffectComponentCacheLastEffect<TSelf, TComponent>
: GOInteractor<TSelf, TComponent, TComponent>
where TSelf : GOInteractorEffectComponentCacheLastEffect<TSelf, TComponent>
where TComponent : MonoBehaviour { }



/// <summary>
/// TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br/>
/// TEffectEnum: Effect variation (enum) when the GO is interacted. <br/>
/// TCacheObject: Cached Object of the interated GO for implementing revert method. <br/>
/// </summary>
public abstract partial class GOI_EffectIsEnum<TSelf, TComponent, TEffectEnum, TCache>
: GOInteractor<TSelf, TComponent, GOInteractEffect<TEffectEnum>, TCache>
where TSelf : GOI_EffectIsEnum<TSelf, TComponent, TEffectEnum, TCache>
where TComponent : MonoBehaviour
where TEffectEnum : struct
where TCache : Object { }