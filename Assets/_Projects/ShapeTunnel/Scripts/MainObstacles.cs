﻿using Enginooby.Core;
using UnityEngine;

namespace Project.ShapeTunnel {
  // REFACTOR: Create in Library - Rotator
  public class MainObstacles : MonoBehaviour {
    [SerializeField] private RandomInt rotateSpeed = new RandomInt(14, 34);
    private bool _isClockwise;

    private void Start() => _isClockwise = RandomUtils.Percent(50f) ? true : false;

    private void Update() {
      transform.RotateForward(rotateSpeed * _isClockwise.To1OrMinus1());
      gameObject.Destroy(@if: transform.childCount <= 1);
    }
  }
}