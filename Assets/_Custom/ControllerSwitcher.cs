using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControllerSwitcher : MonoBehaviour {
  [SerializeField] private InputModifier _switchKey;
  [SerializeField] private List<GameObject> _controllers;
  [ValueDropdown(nameof(_controllers))]
  [SerializeField] private GameObject _currentController;
  [SerializeField] private Camera _camera;
  [SerializeField] private GameObject _model;

  void Start() {

  }

  void Update() {
    if (_switchKey.IsTriggering) {
      GameObject previousController = _currentController;
      _currentController = _controllers.GetNext(previousController);
      print(previousController.transform.position);
      print(_currentController.transform.position);
      _currentController.transform.CopyLocal(previousController.transform);
      print(previousController.transform.position);
      print(_currentController.transform.position);
      previousController.SetActive(false);
      _currentController.SetActive(true);
      _model.transform.SetParent(_currentController.transform);
      _camera.transform.SetParent(_currentController.transform);
      FindObjectOfType<ThirdPersonOrbitCamBasic>().player = _currentController.transform;
    }
  }
}
