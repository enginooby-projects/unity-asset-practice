using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Project.RPG.Combat;

namespace Project.RPG.Controller {
  public class AIController : MonoBehaviour {
    [AutoRef, SerializeField, HideInInspector]
    private Fighter _fighter;

    [SerializeField] private Reference _playerRef;

    [SerializeField, HideLabel]
    private AreaCircular _vision = new AreaCircular(label: "Vision", radius: 5, angle: 90);


    void Update() {
      if (_vision.Contains(_playerRef)) {
        _fighter.Attack(_playerRef);
      } else {
        _fighter.Cancel();
      }
    }

    private void OnDrawGizmos() {
      _vision.DrawGizmos();
    }
  }
}
