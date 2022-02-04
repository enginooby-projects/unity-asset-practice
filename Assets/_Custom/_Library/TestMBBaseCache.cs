using UnityEngine;

public class TestMBBaseCache : MonoBehaviourBase {
  void Start() {
    // My<Rigidbody>().mass = 69;
    // My<Rigidbody>().mass = 19;
    Rigidbody.mass = 69;
    Rigidbody.mass = 19;
  }
}
