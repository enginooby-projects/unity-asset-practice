using UnityEngine;

namespace Project.Gaps {
  /// <summary>
  /// Follow player, support zoom in.
  /// </summary>
  public class CameraManager : MonoBehaviourBase {
    public GameObject player;

    public float smoothTime = 0.3f;

    public float yOffset, zOffset;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _playerPosition => player.transform.position;
    
    private float _lastZoomInX;


    protected override void Update() {
      FollowPlayer();

      if (_playerPosition.z < _position.z - 1f) {
        player.GetComponent<Player>().OutOfScreen();
      }
    }

    private void FollowPlayer() {
      var target = new Vector3(_lastZoomInX, yOffset, _playerPosition.z - zOffset);

      if (target.z > _position.z) {
        _position = Vector3.SmoothDamp(_position, target, ref _velocity, smoothTime);
      }
    }

    public void ZoomIn() {
      smoothTime = 0.1f;
      _lastZoomInX = _playerPosition.x;
      zOffset = 6f;
      yOffset = 8f;
    }
  }
}