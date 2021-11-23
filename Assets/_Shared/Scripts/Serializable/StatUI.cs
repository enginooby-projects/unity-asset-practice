// * Display game stat UI
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[Serializable, InlineProperty]
public class StatUI {
  [HideInInspector]
  public string statName; // REFACTOR: Use StringBuilder
  public StatUI(string statName = "UI", string prefix = null, string suffix = null) {
    this.statName = statName;
    this.prefix ??= this.statName + ": ";
    this.suffix ??= "";
  }

  public enum UIType { Text, Icon, Slider }
  [BoxGroup("$statName")]
  [HideLabel, EnumToggleButtons]
  public UIType uiType = UIType.Text;

  public const float LABEL_WIDTH = 45f;

  #region TEXT ===================================================================================================================================
  [BoxGroup("$statName")]
  [LabelWidth(LABEL_WIDTH)]
  [ShowIf(nameof(uiType), UIType.Text)]
  [InlineEditor]
  [HideLabel]
  public TextMeshProUGUI label;

  // [BoxGroup("$statName")]
  [HorizontalGroup("$statName/Pre&Suffix")]
  [LabelWidth(LABEL_WIDTH)]
  [ShowIf(nameof(uiType), UIType.Text)]
  public string prefix;

  // [BoxGroup("$statName")]
  // [HorizontalGroup("$statName/Prefix & Suffix")]
  [HorizontalGroup("$statName/Pre&Suffix")]
  [LabelWidth(LABEL_WIDTH)]
  [ShowIf(nameof(uiType), UIType.Text)]
  public string suffix;
  #endregion ===================================================================================================================================

  #region ICON ===================================================================================================================================
  // [ShowIf(nameof(statType), UIType.Icon)]
  // public Image?? statIcon;
  #endregion ===================================================================================================================================

  #region SLIDER ===================================================================================================================================
  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField]
  private Slider _slider;

  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField, LabelText("Update Speed")]
  [Range(.1f, 1f)]
  private float _sliderUpdateSpeed = .5f; // TODO: ease update

  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField, LabelText("Destroy On Zero")]
  private bool _destroySliderOnZero;

  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField, LabelText("Enable Fill Gradient")]
  private bool _enableSliderFillGradient;

  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField, EnableIf(nameof(_enableSliderFillGradient)), LabelText("Fill Gradient")]
  private Gradient _sliderFillGradient;

  [ShowIf(nameof(uiType), UIType.Slider)]
  [SerializeField, EnableIf(nameof(_enableSliderFillGradient)), LabelText("Fill Image")]
  private Image _sliderFillImage;

  // TODO: Fading time, hide when max
  #endregion ===================================================================================================================================


  public void Update(int currentValue, Nullable<int> maxValue = null, Nullable<int> minValue = null) {
    switch (uiType) {
      case UIType.Text:
        if (label) label.text = prefix + currentValue + suffix;
        break;
      case UIType.Slider:
        if (maxValue.HasValue && _slider) {
          float fraction = (float)currentValue / (float)maxValue.Value;
          _slider.DOValue(fraction, _sliderUpdateSpeed).SetSpeedBased(true);
          if (_enableSliderFillGradient && _sliderFillImage) {
            _sliderFillImage.color = _sliderFillGradient.Evaluate(fraction);
          }

          if (_destroySliderOnZero && Mathf.Approximately(fraction, 0f)) {
            UnityEngine.Object.Destroy(_slider.gameObject);
          }
        }
        break;
    }
  }
}