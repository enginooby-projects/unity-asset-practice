using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using Enginooby.Utils;

public class CameraManager : MonoBehaviour {
  [SerializeField] private KeyCode switchCameraKey = KeyCode.C;
  [SerializeField] private List<Camera> cameras;

  [SerializeField] [ValueDropdown(nameof(cameras))]
  private Camera currentCamera;

  private Camera previousCamera;

  private void Start() {
    cameras.ForEach(cam => cam.gameObject.SetActive(false));
    currentCamera.gameObject.SetActive(true);
  }

  private void Update() {
    if (switchCameraKey.IsUp()) SwitchNextCamera();
  }

  private void SwitchNextCamera() {
    var currentPos = currentCamera.transform.position;
    var dest = cameras.GetNext(currentCamera).transform.position;

    currentCamera.transform.DOMove(dest, 1f);
    currentCamera.gameObject.SetActive(false);
    currentCamera.transform.DOMove(currentPos, 1f);
    currentCamera = cameras.GetNext(currentCamera);
    currentCamera.gameObject.SetActive(true);
  }

  public void SwitchNextCameraTemp() {
    previousCamera = currentCamera;
    currentCamera = cameras.GetNext(currentCamera);
    previousCamera.transform.DOMove(currentCamera.transform.position, 500f);
    previousCamera.gameObject.SetActive(false);
    currentCamera.gameObject.SetActive(true);
  }
}