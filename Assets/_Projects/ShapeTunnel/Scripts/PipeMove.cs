using UnityEngine;

namespace Project.ShapeTunnel {
  public class PipeMove : MonoBehaviour {
    public float movementSpeed;

    private Vector3 _startPos;

    private void Start() {
      _startPos = transform.position; //Gets the pipe's position when game starts
      GetComponent<Rigidbody>().AddForce(transform.forward * -movementSpeed); //Moves pipe towards player
      InvokeRepeating(nameof(ResetPosition), 1.32f, 1.32f);
    }

    public void ResetPosition() => transform.position = _startPos;

    public void StopPipes() {
      CancelInvoke(nameof(ResetPosition)); //Pipes won't respawn
      GetComponent<Rigidbody>().Sleep();
    }
  }
}