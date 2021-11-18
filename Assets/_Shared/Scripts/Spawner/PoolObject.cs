using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Pool;

// ? Group PoolObject w/ SpawnWaveObject
public class PoolObject : MonoBehaviour {
  public IObjectPool<GameObject> pool;

  // [BoxGroup("Release Mode")]
  [LabelText("On Became Invisible")] public bool releaseOnBecameInvisible;

  // [BoxGroup("Release Mode")]
  [Min(0f)] public float lifespan;

  /// <summary>
  /// Event to add additional logic for cleaning specific pooled object (e.g. re-enable projectile to flying when reuse).
  /// </summary>
  public event System.Action onEnable;

  // ? Add Area

  private void Start() {
    ProcessLifespan();
  }

  private void OnEnable() {
    ProcessLifespan();
    onEnable?.Invoke();
  }

  private void ProcessLifespan() {
    if (lifespan != 0) StartCoroutine(ReleaseToPoolCoroutine(lifespan));
  }

  private void OnBecameInvisible() {
    if (releaseOnBecameInvisible) ReleaseToPool();
  }

  private IEnumerator ReleaseToPoolCoroutine(float delay) {
    yield return new WaitForSeconds(delay);
    ReleaseToPool();
  }

  public void ReleaseToPool() {
    // print("Release to pool");
    if (!CanReleaseToPool) return;
    pool?.Release(gameObject);
  }

  public bool CanReleaseToPool => gameObject.activeInHierarchy && gameObject && gameObject.activeSelf;

  private void OnDestroy() {
    // IMPL: remove from pool
  }
}
