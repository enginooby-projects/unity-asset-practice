using Enginooby.Core;
using UnityEngine;

namespace Project.ShapeTunnel {
  // REFACTOR: Create in Library - Rotator
  public class MainObstacles : MonoBehaviour {
    [SerializeField] private RandomInt _rotateSpeed = new RandomInt(14, 34);

    private bool _isClockwise;

    private void Start() => _isClockwise = 50.Percent();

    private void Update() {
      transform.RotateForward(_rotateSpeed * _isClockwise.To1OrMinus1());
      gameObject.Destroy(@if: transform.childCount <= 1);
    }
  }
}