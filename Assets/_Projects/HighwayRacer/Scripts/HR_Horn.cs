using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.HighwayRacer {
  public class HR_Horn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private bool _isPressing;

    private void OnEnable() {
      if (RCC_Settings.Instance.controllerType != RCC_Settings.ControllerType.Mobile) {
        gameObject.SetActive(false);
      }
    }

    private void Update() => RCC_SceneManager.Instance.activePlayerVehicle.highBeamHeadLightsOn = _isPressing;

    public void OnPointerDown(PointerEventData eventData) => _isPressing = true;

    public void OnPointerUp(PointerEventData eventData) => _isPressing = false;
  }
}