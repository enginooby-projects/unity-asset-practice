using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.BasketballShooting {
  public class GUI : MonoBehaviour {
    public Shooter shooter;
    public GoalArea goalArea;

    public Text textScore;
    public Text textShotPower;
    public Text textDirection;

    private int _tempScore;
    private float _tempShotPower;
    private Vector3 _tempShotDir = Vector3.zero;

    private void Update() {
      // REFACTOR: Use observer pattern
      if (_tempScore != goalArea.Score) {
        _tempScore = goalArea.Score;
        textScore.text = _tempScore.ToString();
      }

      if (_tempShotPower != shooter.ShotPower) {
        _tempShotPower = shooter.ShotPower;
        textShotPower.text = _tempShotPower.ToString("0.000");
      }

      if (_tempShotDir != shooter.Direction) {
        _tempShotDir = shooter.Direction;
        var q = Quaternion.Euler(-shooter.transform.eulerAngles);
        var org = new Vector4(_tempShotDir.x, _tempShotDir.y, _tempShotDir.z, 1);
        var v = q * org;
        v.Normalize();
        // UTIL
        textDirection.text = v.x.ToString("0.00") + ", " + v.y.ToString("0.00") + ", " + v.z.ToString("0.00");
      }
    }

    // restart
    public void PushedResetButton() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
  }
}