using UnityEngine;

namespace Project.BasketballShooting {
  public class GoalArea : MonoBehaviour {
    [SerializeField] private ParticleSystem _scoreVfx;

    public int Score { get; private set; }

    private void OnTriggerEnter(Collider other) {
      if (other.TryGetComponent(out ShotBall ball)) {
        Score++;
        _scoreVfx.Play();
      }
    }
  }
}