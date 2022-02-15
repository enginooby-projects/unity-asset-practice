using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.HighwayRacer {
  [RequireComponent(typeof(Button))]
  public class HR_ButtonSound : MonoBehaviour, IPointerClickHandler {
    private AudioSource _clickSound;

    public void OnPointerClick(PointerEventData data) {
      if (Camera.main == null) return;

      _clickSound = HR_CreateAudioSource.NewAudioSource(Camera.main.gameObject,
        HR_HighwayRacerProperties.Instance.buttonClickAudioClip.name, 0f, 0f, 1f,
        HR_HighwayRacerProperties.Instance.buttonClickAudioClip, false, true, true);
      _clickSound.ignoreListenerPause = true;
      _clickSound.ignoreListenerVolume = true;
    }
  }
}