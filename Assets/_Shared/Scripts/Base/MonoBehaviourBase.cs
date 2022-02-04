using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

// TODO:
// + Timespan for component/gameobject
// + Alternative fast Update()

/// <summary>
/// * Common convenient public functions & extenstion methods (useful esp. in binding events w/o writing more code) for all custom Components (MonoBehaviours).
/// * vs. ComponentOperator base is for built-in Unity Components
/// </summary>
public abstract class MonoBehaviourBase : MonoBehaviour {
  protected virtual void Awake() { }
  protected virtual void Start() { }
  protected virtual void Update() { }
  protected virtual void FixedUpdate() { }
  protected virtual void LateUpdate() { }

  #region LAZY LOCAL COMPONENT CACHE
  // Alternative to CacheStaticUtils
  // ? Does it cost many memories if variables are not used
  // ? Use a dictionary (like in CacheStaticUtils) instead of separate variables
  // Prefer to component with only one of its type on the GO ([DisallowedMultipleComponent])
  private Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();

  // private Rigidbody _rigidbody = null;
  // public Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();

  // Cons: expose un-used/un-available components from a MonoBehaviour => use protected instead of public
  public Transform Transform => My<Transform>();
  public Rigidbody Rigidbody => My<Rigidbody>();
  public Collider Collider => My<Collider>();
  public BoxCollider BoxCollider => My<BoxCollider>();
  public MeshRenderer MeshRenderer => My<MeshRenderer>();
  public MeshFilter MeshFilter => My<MeshFilter>();
  public Animator Animator => My<Animator>();

  /// <summary>
  /// Get cached singleton (on a GO) component.
  /// </summary>
  public T My<T>() where T : Component {
    if (_cachedComponents.TryGetValue(typeof(T), out var cachedComponent)) {
      // print("Get cached component");
      return (T)cachedComponent;
    }

    if (TryGetComponent<T>(out var component)) {
      // print("Get uncached component");
      _cachedComponents.Add(typeof(T), component);
      return component;
    }

    // ? Shoud add component if not found
    return null;
  }
  #endregion

  // [SerializeField, HideInInspector]
  // private Vector3? _initalPosition = null;
  // public Vector3 InitialPosition => _initalPosition ??= transform.position;

  [FoldoutGroup("MonoBehaviour Common")]
  // [Button]
  public void GetAutoReferences() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.ExecuteMenuItem("Tools/AutoRefs/Set AutoRefs");
#endif
  }

  #region ACTIVITY ===================================================================================================================================
  [FoldoutGroup("MonoBehaviour Common")]
  [SerializeField, Min(0f)] float lifespan;

  public void DisableForSecs(float seconds) => this.Disable(seconds);


  public void ToggleActive() {

  }
  #endregion ===================================================================================================================================

  #region EVENT ===================================================================================================================================
  #endregion ===================================================================================================================================
}
