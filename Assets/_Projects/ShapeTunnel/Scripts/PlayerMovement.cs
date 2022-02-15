using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ShapeTunnel {
  public class PlayerMovement : MonoBehaviour {
    public float computerSpeed, movementSpeed;

    private Touch initTouch = new Touch();
    private bool touching = false;

    private void Start() =>
      transform.GetChild(0).GetComponent<Animation>().Play(); //Rotates the player (plays player's animation)

    void Update() {
      foreach (Touch touch in Input.touches) {
        if (touch.phase == TouchPhase.Began) //If finger touches the screen
        {
          if (touching == false) {
            touching = true;
            initTouch = touch;
          }
        }
        else if (touch.phase == TouchPhase.Moved) //iIf finger moves while touching the screen
        {
          float deltaX = initTouch.position.x - touch.position.x;
          transform.RotateAround(Vector3.zero, transform.forward,
            deltaX * movementSpeed * Time.deltaTime); //Rotates the player around the x axis


          initTouch = touch;
        }
        else if (touch.phase == TouchPhase.Ended) //If finger releases the screen
        {
          initTouch = new Touch();
          touching = false;
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