using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Project.RPG.Controller {
  public class AIController : MonoBehaviour {
    [SerializeField] private Reference _playerRef;

    [SerializeField, HideLabel]
    private AreaCircular _vision = new AreaCircular(label: "Vision", radius: 5, angle: 90);

    void Start() {

    }

    void Update() {
      if (_vision.Contains(_playerRef)) {
        print("See player");
      }
    }

    private void OnDrawGizmos() {
      _vision.DrawGizmos();
    }
  }
}
