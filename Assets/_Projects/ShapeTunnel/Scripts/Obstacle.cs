using UnityEngine;

namespace Project.ShapeTunnel {
  public class Obstacle : MonoBehaviourBase {
    public float movementSpeed, rotationSpeed;
    public Mesh[] obstMeshes;

    private Collision _playerCollision;
    private bool _isStopped;

    protected override void Start() {
      //Makes the obstacle move towards the player
      _rigidbody.AddForce(transform.forward * -movementSpeed);
      _playerCollision = FindObjectOfType<Collision>();
      if (CompareTag("Prism Instance")) ChooseRandomShape();
    }

    protected override void Update() {
      //Rotates the obstacle around the Y axis
      transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
      //If the game is over and the obstacle still moves towards the player
      if (_playerCollision.gameIsOver && !_isStopped) {
        _isStopped = true;
        _rigidbody.Sleep();
      }
    }

    public void ChooseRandomShape() {
      var randomMeshIndex = Random.Range(0, obstMeshes.Length);
      _meshFilter.mesh = obstMeshes[randomMeshIndex];

      //Prism and cube needs to be rotated
      float angle;
      //Sets the temporary tag
      (tag, angle) = randomMeshIndex switch {
        0 => ("Cube Instance", 90f),
        1 => ("Prism Instance", -90f),
        2 => ("Sphere Instance", 0f),
        _ => (tag, 0f)
      };

      transform.Rotate(Vector3.right, angle);
    }
  }
}