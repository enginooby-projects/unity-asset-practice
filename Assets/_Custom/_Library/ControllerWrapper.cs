using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class ControllerWrapper : SerializedMonoBehaviour {
  [OdinSerialize] public GameObject Controller { get; private set; }
  [InlineButton(nameof(GetCamera), "Get")]
  [OdinSerialize] public Camera Camera { get; private set; }

  private void GetCamera() {
    Camera = transform.FindComponent<Camera>();
  }

  void Start() {

  }

  void Update() {

  }
}
