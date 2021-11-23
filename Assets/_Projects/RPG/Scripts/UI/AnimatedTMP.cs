using UnityEngine;
using DG.Tweening;
using TMPro;

// TODO: Move to Library
public class AnimatedTMP : MonoBehaviour, IPoolObject {
  [SerializeField]
  private float _duration = 1f;

  private TextMeshProUGUI _tmp;
  private Tweener _fadeTweener;
  private Tweener _scaleTweener;
  private Tweener _fontSizeTweener;
  private Tweener _moveTweener;

  public void OnPoolReuse() {
    ReplayAnimations();
  }

  public void ReplayAnimations() {
    _fadeTweener?.Restart();
    _fontSizeTweener?.Restart();
    _moveTweener?.Restart();
  }

  private void Awake() {
    _tmp = GetComponent<TextMeshProUGUI>();
  }

  void Start() {
    // TODO: parameterize
    _fadeTweener = _tmp.DOFade(0, _duration).SetAutoKill(false);
    _fontSizeTweener = _tmp.DOFontSize(1.2f, _duration).SetAutoKill(false);
    _moveTweener = transform.DOLocalMoveY(6f, _duration).SetAutoKill(false);
  }

}
