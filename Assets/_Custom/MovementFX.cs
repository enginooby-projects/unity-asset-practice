using SCPE;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

// TODO
// + Flying collision FX

// REFACTOR: use state pattern

/// <summary>
/// Base class to create extension classes adding FXs for 3rd-party controllers.
/// </summary>
public class MovementFX : MonoBehaviour {
  [SerializeField]
  protected Rigidbody _rigidBody;

  [SerializeField]
  protected InputModifier _flyKey = new InputModifier(inputType: InputModifier.InputType.Axis, inputAxis: InputAxis.Vertical);

  [SerializeField]
  protected InputModifier _sprintFlyKey = new InputModifier(inputType: InputModifier.InputType.Axis, inputAxis: InputAxis.Vertical, modifierKey: ModifierKey.Lshift);

  [SerializeField, InlineEditor]
  private Volume _scpeVolume;

  [SerializeField]
  private AudioSource _audioSource; // TODO: Fade in/out

  [SerializeField, InlineEditor]
  private AudioClip _flySfx;

  private SpeedLines _speedLines;
  private Tweener _speedLinesTweener;
  private RadialBlur _radialBlur;
  private Tweener _radialBlurTweener;


  // Trigger flags to execute function once in Update
  private bool _triggerStopFlying;
  private bool _triggerSprintFlying;
  private bool _triggerFlying;
  private Tweener _audioTweener;

  public virtual bool IsFlying => _flyKey.IsTriggering && IsRigidBodyMoving();
  public virtual bool IsSprintFlying => _sprintFlyKey.IsTriggering && IsRigidBodyMoving();
  public virtual float FlySpeed { get; }
  public virtual float MoveForwardSpeed { get; }

  protected virtual void Awake() {
    _rigidBody ??= GetComponent<Rigidbody>();
    _scpeVolume.profile.TryGet<SpeedLines>(out _speedLines);
    _scpeVolume.profile.TryGet<RadialBlur>(out _radialBlur);
  }

  // UTIL
  /// <summary>
  /// Whether current velocity is not zero.
  /// </summary>
  protected bool IsRigidBodyMoving() {
    // return _rigidBody.velocity != Vector3.zero; // ! not correct
    //  return !Mathf.Approximately(_rigidBody.velocity.magnitude, 0f); // ! not correct
    return _rigidBody.velocity.magnitude > 10f; // TODO: Tune
  }

  void Update() {
    if (IsSprintFlying) {
      OnStartSprintFlying();
    } else if (IsFlying) {
      OnStartFlying();
    } else if (!IsFlying) {
      OnStopFlying();
    }
  }

  // REFACTOR
  private void OnStartSprintFlying() {
    if (_triggerSprintFlying) return;

    TweenRadialBlurAmount(.5f);
    TweenSpeedLineIntensity(1f);
    TweenAudioVolume(1f, clip: _flySfx);
    ResetUnconcurrentTriggers();
    _triggerSprintFlying = true;
  }

  private void OnStartFlying() {
    if (_triggerFlying) return;

    TweenRadialBlurAmount(.2f);
    TweenSpeedLineIntensity(.4f);
    TweenAudioVolume(.4f, clip: _flySfx);
    ResetUnconcurrentTriggers();
    _triggerFlying = true;
  }

  private void OnStopFlying() {
    if (_triggerStopFlying) return;

    TweenRadialBlurAmount(0f);
    TweenSpeedLineIntensity(0f);
    TweenAudioVolume(0f);
    ResetUnconcurrentTriggers();
    _triggerStopFlying = true;
  }

  // This should be invoked before setting any trigger/flag
  private void ResetUnconcurrentTriggers() {
    _triggerSprintFlying = false;
    _triggerFlying = false;
    _triggerStopFlying = false;
  }

  // REFACTOR
  private Tweener TweenSpeedLineIntensity(float endValue, float duration = 1f) {
    _speedLinesTweener.Kill();
    _speedLines.active = true;
    _speedLinesTweener = DOTween.To(x => _speedLines.intensity.value = x, _speedLines.intensity.value, endValue, duration);
    if (endValue == 0) _speedLinesTweener.OnComplete(() => _speedLines.active = false);
    return _speedLinesTweener;
  }

  private Tweener TweenRadialBlurAmount(float endValue, float duration = 1f) {
    _radialBlurTweener.Kill();
    _radialBlur.active = true;
    _radialBlurTweener = DOTween.To(x => _radialBlur.amount.value = x, _radialBlur.amount.value, endValue, duration);
    if (endValue == 0) _radialBlurTweener.OnComplete(() => _radialBlur.active = false);
    return _radialBlurTweener;
  }

  private Tweener TweenAudioVolume(float endValue, AudioClip clip = null, float duration = 1f) {
    _audioTweener.Kill();
    if (clip) _audioSource.PlayOneShot(clip);
    _audioTweener = _audioSource.DOFade(endValue, duration);
    if (endValue == 0) _audioTweener.OnComplete(() => _audioSource.Stop());
    return _audioTweener;
  }
}
