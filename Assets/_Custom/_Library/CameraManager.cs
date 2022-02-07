using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class CameraManager : MonoBehaviour {
  [SerializeField] private KeyCode switchCameraKey = KeyCode.C;
  [SerializeField] private List<Camera> cameras;
  [SerializeField] [ValueDropdown(nameof(cameras))] private Camera currentCamera;
  private Camera previousCamera;

  private void Start() {
    cameras.ForEach(camera => camera.gameObject.SetActive(false));
    currentCamera.gameObject.SetActive(true);
  }

  void Update() {
    if (switchCameraKey.IsUp()) SwitchNextCamera();
  }

  public void SwitchNextCamera() {
    Vector3 currentPos = currentCamera.transform.position;
    Vector3 dest = cameras.NavNext(currentCamera).transform.position;

    currentCamera.transform.DOMove(dest, 1f);
    currentCamera.gameObject.SetActive(false);
    currentCamera.transform.DOMove(currentPos, 1f);
    currentCamera = cameras.NavNext(currentCamera);
    currentCamera.gameObject.SetActive(true);
  }

  public void SwitchNextCameraTemp() {
    previousCamera = currentCamera;
    currentCamera = cameras.NavNext(currentCamera);
    previousCamera.transform.DOMove(currentCamera.transform.position, 500f);
    previousCamera.gameObject.SetActive(false);
    currentCamera.gameObject.SetActive(true);
  }
}
