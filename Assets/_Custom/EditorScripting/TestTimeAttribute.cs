using UnityEngine;

public class TestTimeAttribute : MonoBehaviour {
  [Time]
  [SerializeField] private int _time1 = 70;

  [Time(true)]
  [SerializeField] private int _time2 = 70;

  [Time]
  [SerializeField] private float _time3 = 70f;
}
