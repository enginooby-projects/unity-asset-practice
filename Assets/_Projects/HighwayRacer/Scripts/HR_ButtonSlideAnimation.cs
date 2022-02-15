using UnityEngine;

namespace Project.HighwayRacer {
  public class HR_ButtonSlideAnimation : MonoBehaviour {
    public SlideFrom slideFrom;

    public enum SlideFrom {
      Left,
      Right,
      Top,
      Buttom
    }

    public bool actWhenEnabled;
    public bool actNow;
    public bool endedAnimation;
    public bool playSound = true;
    public HR_ButtonSlideAnimation playWhenThisEnds;

    private RectTransform _getRect;
    private Vector2 _originalPosition;
    private AudioSource _slidingAudioSource;

    private void Awake() {
      _getRect = GetComponent<RectTransform>();
      _originalPosition = GetComponent<RectTransform>().anchoredPosition;

      SetOffset();
    }

    private void SetOffset() {
      GetComponent<RectTransform>().anchoredPosition = slideFrom switch {
        SlideFrom.Left => new Vector2(-2000f, _originalPosition.y),
        SlideFrom.Right => new Vector2(2000f, _originalPosition.y),
        SlideFrom.Top => new Vector2(_originalPosition.x, 1000f),
        SlideFrom.Buttom => new Vector2(_originalPosition.x, -1000f),
        _ => GetComponent<RectTransform>().anchoredPosition
      };
    }

    private void OnEnable() {
      if (!actWhenEnabled) return;

      SetOffset();
      endedAnimation = false;
      Animate();
    }

    public void Animate() {
      if (GameObject.Find(HR_HighwayRacerProperties.Instance.labelSlideAudioClip.name))
        _slidingAudioSource = GameObject.Find(HR_HighwayRacerProperties.Instance.labelSlideAudioClip.name)
          .GetComponent<AudioSource>();
      else
        _slidingAudioSource = HR_CreateAudioSource.NewAudioSource(Camera.main.gameObject,
          HR_HighwayRacerProperties.Instance.labelSlideAudioClip.name, 0f, 0f, 1f,
          HR_HighwayRacerProperties.Instance.labelSlideAudioClip, false, false, true);

      _slidingAudioSource.ignoreListenerPause = true;
      _slidingAudioSource.ignoreListenerVolume = true;

      actNow = true;
    }

    // OPTI: Cache components
    private void Update() {
      if (!actNow || endedAnimation) return;

      if (playWhenThisEnds != null && !playWhenThisEnds.endedAnimation) return;

      if (_slidingAudioSource && !_slidingAudioSource.isPlaying && playSound)
        _slidingAudioSource.Play();

      // UTIL
      _getRect.anchoredPosition =
        Vector2.MoveTowards(_getRect.anchoredPosition, _originalPosition, Time.unscaledDeltaTime * 4000f);

      if (Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, _originalPosition) < .05f) {
        if (_slidingAudioSource && _slidingAudioSource.isPlaying && playSound)
          _slidingAudioSource.Stop();

        GetComponent<RectTransform>().anchoredPosition = _originalPosition;
        var animCounter = GetComponentInChildren<HR_CountAnimation>();
        if (animCounter && !animCounter.actNow) {
          animCounter.Count();
        }
        else {
          endedAnimation = true;
        }
      }

      if (endedAnimation && !actWhenEnabled)
        enabled = false;
    }
  }
}