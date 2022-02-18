using System;
using UnityEngine;

namespace Project.ShapeTunnel {
  public class PlayerMovement : MonoBehaviour {
    public float computerSpeed, movementSpeed;

    private Touch _initTouch;
    private bool _isTouching;

    private void Start() =>
      transform.GetChild(0).GetComponent<Animation>().Play(); //Rotates the player (plays player's animation)

    private void Update() {
      foreach (var touch in Input.touches) {
        switch (touch.phase) {
          //If finger touches the screen 
          case TouchPhase.Began: {
            if (!_isTouching) {
              _isTouching = true;
              _initTouch = touch;
            }

            break;
          }
          //iIf finger moves while touching the screen
          case TouchPhase.Moved: {
            var deltaX = _initTouch.position.x - touch.position.x;
            transform.RotateAround(Vector3.zero, transform.forward,
              deltaX * movementSpeed * Time.deltaTime); //Rotates the player around the x axis
            _initTouch = touch;
            break;
          }
          //If finger releases the screen
          case TouchPhase.Ended:
            _initTouch = new Touch();
            _isTouching = false;
            break;
        }
      }

      //If you play on computer---------------------------------

      if (Input.GetKey(KeyCode.A))
        transform.RotateAround(Vector3.zero, transform.forward,
          computerSpeed * movementSpeed * Time.deltaTime); //Rotates the player around the x axis
      else if (Input.GetKey(KeyCode.D))
        transform.RotateAround(Vector3.zero, transform.forward,
          -computerSpeed * movementSpeed * Time.deltaTime); //Rotates the player around the x axis

      //--------------------------------------------------------
    }
  }
}