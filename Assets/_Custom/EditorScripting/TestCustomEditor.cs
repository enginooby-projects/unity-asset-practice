using UnityEngine;

public class TestCustomEditor : MonoBehaviour {
  [SerializeField] private int _level;
  [Time]
  [SerializeField] private int _timeLeft; // custom attribute in custom editor 

  public int Level {
    get => _level;
    set => _level = Mathf.Max(1, value);
  }
}
