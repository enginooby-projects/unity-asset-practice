using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Project.Gaps {
  public class Player : MonoBehaviourBase {
    [Tag] [SerializeField] private string tagDeadTrigger = "SideTrigger";
    [Tag] [SerializeField] private string tagObstacle = "Obstacle";
    [Tag] [SerializeField] private string tagLittleBall = "LittleBall";

    public float MoveForwardSpeed = 2f;

    private GamePlayManager GamePlayManagerScript;
    private Vector3 _movingPos;
    private Vector2 _tempPos;
    private float _angle;
    private bool _isTouchingObstacle;

    public bool IsDead { get; private set; }

    protected override void Start() {
      Time.timeScale = 1f;
      GamePlayManagerScript = FindObjectOfType<GamePlayManager>();
      _movingPos = new Vector3(0f, 0f, MoveForwardSpeed);
    }

    protected override void Update() {
      if (!_isTouchingObstacle)
        MoveForward(_movingPos);
      else if (MouseButton.Left.IsDown()) {
        ShootFromObstacle();
      }
    }

    private void ShootFromObstacle() {
      _isTouchingObstacle = false;
      _tempPos = Transform.parent.position.GetXZ() - _position.GetXZ();
      Transform.parent = null;
      _angle = Mathf.Atan2(_tempPos.y, _tempPos.x);
      _movingPos = new Vector3(-Mathf.Cos(_angle), 0f, -Mathf.Sin(_angle)) * MoveForwardSpeed;
      AudioManager.Instance.PlaySfx(AudioManager.Instance.shoot);
    }

    private void MoveForward(Vector3 speed) {
      if (GamePlayManagerScript.isStarted && !IsDead) {
        _position += Time.deltaTime * speed;
      }
    }

    private void OnCollisionEnter(Collision other) {
      if (other.CompareTag(tagDeadTrigger)) {
        StartCoroutine(DeadCoroutine());
      }
      else if (other.CompareTag(tagObstacle)) {
        OnObstacleTouch(other.gameObject);
      }
    }

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag(tagLittleBall)) {
        CollectLittleBall(other);
      }
    }

    private void CollectLittleBall(Collider ball) {
      Destroy(ball.gameObject);
      GamePlayManagerScript.CollectLittleBall();
      AudioManager.Instance.PlaySfx(AudioManager.Instance.littleBallCollect);
    }

    private IEnumerator DeadCoroutine() {
      if (IsDead) yield break;

      AudioManager.Instance.PlaySfx(AudioManager.Instance.crash);
      IsDead = true;
      Camera.main!.gameObject.GetComponent<CameraManager>().ZoomIn();
      GamePlayManagerScript.GameOver();
      My<Explosion>().Explode();

      _collider.enabled = false;
      _meshRenderer.enabled = false;
      _rigidbody.isKinematic = true;
      yield break;
    }

    public void OutOfScreen() {
      if (IsDead) return;

      IsDead = true;
      _collider.enabled = false;
      GamePlayManagerScript.GameOver();
      AudioManager.Instance.PlaySfx(AudioManager.Instance.crash);
    }

    private void OnObstacleTouch(GameObject obstacle) {
      if (_isTouchingObstacle) return;

      _isTouchingObstacle = true;
      _rigidbody.velocity = Vector3.zero;
      Transform.SetParent(obstacle.transform);
    }
  }
}