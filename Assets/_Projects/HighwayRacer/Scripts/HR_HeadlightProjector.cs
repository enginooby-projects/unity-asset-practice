using UnityEngine;

namespace Project.HighwayRacer {
  public class HR_HeadlightProjector : MonoBehaviour {
    private HR_GamePlayHandler _gpHandler;
    private Projector _projector;
    private Light _headlight;

    private void Start() {
      _gpHandler = HR_GamePlayHandler.Instance;
      _headlight = GetComponentInParent<Light>();
      _projector = GetComponent<Projector>();
      _projector.material = new Material(_projector.material);
    }

    private void Update() {
      if (!_headlight.enabled) {
        _projector.enabled = false;
        return;
      }

      _projector.enabled = true;

      if (_gpHandler && _gpHandler.dayOrNight == HR_GamePlayHandler.DayOrNight.Day)
        _projector.material.color = _headlight.color * _headlight.intensity * .05f;
      else
        _projector.material.color = _headlight.color * _headlight.intensity * .25f;

      _projector.farClipPlane = Mathf.Lerp(10f, 40f, (_headlight.range - 50) / 150);
      _projector.fieldOfView = Mathf.Lerp(40f, 30f, (_headlight.range - 50) / 150);
    }
  }
}