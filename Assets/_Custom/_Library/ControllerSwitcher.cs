using System.Collections.Generic;
using Enginooby.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControllerSwitcher : MonoBehaviour {
  [SerializeField] private InputModifier _switchKey;
  [SerializeField] private List<ControllerWrapper> _wrappers;

  [ValueDropdown(nameof(_wrappers))] [SerializeField]
  private ControllerWrapper _currentWrapper;

  [SerializeField] private GameObject _model;
  [SerializeField] private Camera _transitionCamera;

  private void Update() {
    if (_switchKey.IsTriggering) {
      var previousWrapper = _currentWrapper;
      _currentWrapper = _wrappers.GetNext(previousWrapper);

      _currentWrapper.gameObject.SetActive(true);

      if (_currentWrapper.Controller.TryGetComponent<ControllerMover>(out var mover))
        mover.CopyTransform(previousWrapper.Controller.transform);
      else
        _currentWrapper.Controller.transform.CopyLocal(previousWrapper.Controller.transform);
      _model.transform.SetParent(_currentWrapper.Controller.transform);
      _model.transform.ResetLocal();

      previousWrapper.gameObject.SetActive(false);
      _currentWrapper.Camera.transform.Copy(previousWrapper.Camera.transform);
      // TODO: Transitioning camera from previous to current controller.
    }
  }
}