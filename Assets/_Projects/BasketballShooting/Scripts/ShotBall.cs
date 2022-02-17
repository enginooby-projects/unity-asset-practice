using UnityEngine;

namespace Project.BasketballShooting {
  public class ShotBall : MonoBehaviour {
    public bool IsActive { get; private set; }

    public void SetActive() => IsActive = true;
  }
}