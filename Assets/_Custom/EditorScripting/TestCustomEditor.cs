using UnityEngine;

public class TestCustomEditor : MonoBehaviour {
  [SerializeField] private int _level;

  public int Level {
    get => _level;
    set => _level = Mathf.Max(1, value);
  }

  void Start() {

  }

  void Update() {

  }
}
