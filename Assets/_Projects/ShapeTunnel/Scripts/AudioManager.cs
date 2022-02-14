using UnityEngine;

namespace Project.ShapeTunnel {
  public class AudioManager : MonoBehaviour {
    public AudioSource backgroundMusic, tokenSound, scoreSound, deathSound;

    // GameManager script might modify this value
    [HideInInspector] public bool soundIsOn = true;

    public void StopBgm() => backgroundMusic.Stop();
    public void PlayBgm() => backgroundMusic.Play(@if: soundIsOn);
    public void PlayTokenSound() => tokenSound.Play(@if: soundIsOn);
    public void PlayScoreSound() => scoreSound.Play(@if: soundIsOn);
    public void PlayDeathSound() => deathSound.Play(@if: soundIsOn);
  }
} 