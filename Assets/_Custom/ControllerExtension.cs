using System.Collections;
using UnityEngine;
using WindowsInput;
using System;
using DG.Tweening;

// ? Rename to scripting controller
/// <summary>
/// Scripting controller (# user input) for precise/planned movements.
/// </summary>
public class ControllerExtension : MonoBehaviour {
  [SerializeField] private Transform _modelContainer;
  [SerializeField] private InputModifier _spinModelKey = new InputModifier();
  [SerializeField] private float _spinModelDuration = .5f;
  [SerializeField] private int _spinModelRounds = 1;

  // [SerializeField] private Rigidbody _rigidbody;
  // [SerializeField] private Camera _camera;
  // [SerializeField] private List<MonoBehaviour> _mbs = new List<MonoBehaviour>();

  private InputSimulator IS;

  // Use this for initialization
  void Start() {
    IS = new InputSimulator(); // TODO: Singleton
  }

  // UTIL: excute function during a condition
  public IEnumerator PressButtonCoroutine(Func<bool> runCondition) {
    while (runCondition()) {
      IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
      IS.Mouse.MoveMouseBy(500, 0);
    }
    yield break;
  }

  // UTIL: excute function during a time period
  public IEnumerator PressButtonCoroutine(float duration) {
    float timePassed = 0;
    while (timePassed < duration) {
      print("pressing A");
      IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
      timePassed += Time.deltaTime;
      yield return null;
    }
  }

  void LateUpdate() {
    if (_spinModelKey.IsTriggering) {
      SpinModel();
      // _camera.transform.DORotate(new Vector3(0, -90, 0), _turnLeftDuration, RotateMode.WorldAxisAdd);
    }
  }

  // UTIL
  public void SpinModel() {
    // ! if this function is recalled when not finish current spinning, original rotation is changed 
    Quaternion originalRotation = _modelContainer.localRotation;
    _modelContainer.DORotate(new Vector3(0, 360 * _spinModelRounds, 0), _spinModelDuration, RotateMode.WorldAxisAdd)
         .OnComplete(() => _modelContainer.localRotation = Quaternion.identity);
  }
}
