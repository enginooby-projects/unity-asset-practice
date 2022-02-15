using UnityEngine;

namespace Project.Gaps {
  public class AudioManager : MonoBehaviourSingleton<AudioManager> {
    [Header("Audio Sources")] public AudioSource efxSource;

    public AudioSource musicSource;

    [Header("Background Music")] public AudioClip bgMusic;

    public AudioClip gameMusic;

    [Header("Sound Effects")] public AudioClip buttonClick;

    public AudioClip crash;

    public AudioClip highscore;

    public AudioClip littleBallCollect;

    public AudioClip shoot;

    public bool IsMusicMuted { get; private set; }
    public bool IsSfxMuted { get; private set; }

    private const string MuteMusicPrefs = "MuteMusic";
    private const string MuteSfxPrefs = "MuteEfx";

    private void Start() {
      IsMusicMuted = PlayerPrefs.GetInt(MuteMusicPrefs).ToBool();
      IsSfxMuted = PlayerPrefs.GetInt(MuteSfxPrefs).ToBool();
      PlayMusic(bgMusic);
    }

    public void PlayMusic(AudioClip clip) => musicSource.PlayIfStopped(clip, !IsMusicMuted);

    public void PlaySfx(AudioClip clip) => efxSource.PlayOneShot(clip, !IsSfxMuted);

    public void ToggleMusic() {
      PlayerPrefs.SetInt(MuteMusicPrefs, IsMusicMuted.ToInt());
      IsMusicMuted = !IsMusicMuted;
      musicSource.PlayOrStop(bgMusic, !IsMusicMuted);
    }

    public void ToggleSfx() {
      PlayerPrefs.SetInt(MuteSfxPrefs, IsSfxMuted.ToInt());
      IsSfxMuted = !IsSfxMuted;
    }
  }
}