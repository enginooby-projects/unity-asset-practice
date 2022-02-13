using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Gaps {
  public class AudioButton : MonoBehaviourUIBase {
    public Sprite onSprite, offSprite;

    public bool sfx;

    private AudioManager AudioManager => AudioManager.Instance;

    protected override void Start() {
      UpdateSprite();
      // TIP: Use ternary for delegate type by casting
      // _button.onClick.AddListener(sfx ? (UnityAction) OnSfxButtonClicked : OnMusicButtonClicked);
      // Alternative: Use extension method
      _button.onClick.AddListenerTernary(sfx, OnSfxButtonClicked, OnMusicButtonClicked);
    }

    private void UpdateSprite() {
      var isAudioMuted = sfx ? AudioManager.IsSfxMuted : AudioManager.IsMusicMuted;
      _image.sprite = isAudioMuted ? offSprite : onSprite;
    }

    public void OnMusicButtonClicked() {
      AudioManager.ToggleMusic();
      UpdateSprite();
    }

    public void OnSfxButtonClicked() {
      AudioManager.ToggleSfx();
      UpdateSprite();
    }
  }
}