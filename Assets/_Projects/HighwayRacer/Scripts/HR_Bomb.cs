using UnityEngine;

namespace Project.HighwayRacer {
  public class HR_Bomb : MonoBehaviour {
    private HR_PlayerHandler _handler;
    private Light _bombLight;
    private float _signalTimer;
    private AudioSource _bombTimerAudioSource;

    private AudioClip bombTimerAudioClip => HR_HighwayRacerProperties.Instance.bombTimerAudioClip;

    private void Awake() => gameObject.SetActive(HR_GamePlayHandler.Instance.IsBombMode);

    private void Start() {
      _handler = GetComponentInParent<HR_PlayerHandler>();
      _bombTimerAudioSource = HR_CreateAudioSource.NewAudioSource(gameObject, "Bomb Timer AudioSource", 0f, 0f, .25f,
        bombTimerAudioClip, false, false, false);
      _bombLight = GetComponentInChildren<Light>();
      _bombLight.enabled = true;
      _bombLight.intensity = 0f;
    }

    private void FixedUpdate() {
      if (!_handler || !_handler.bombTriggered) return;

      _signalTimer += Time.fixedDeltaTime * Mathf.Lerp(5f, 1f, _handler.bombHealth / 100f);

      // UTIL
      _bombLight.intensity = Mathf.Lerp(_bombLight.intensity, _signalTimer >= .5f ? 0f : 1f, Time.fixedDeltaTime * 50f);

      if (_signalTimer >= 1f) {
        _signalTimer = 0f;
        _bombTimerAudioSource.Play();
      }
    }
  }
}