using UnityEngine;

namespace Project.Gaps {
  public class Map : MonoBehaviourBase {
    public GameObject player;

    private readonly float _mapLength = 20f;

    protected override void Update() {
      var zDiff = player.transform.position.z - _position.z;
      if (zDiff > _mapLength * 5f / 4f) {
        _position = _position.OffsetZ(_mapLength * 4f);
      }
    }
  }
}