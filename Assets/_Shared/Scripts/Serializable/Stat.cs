using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

// ? Make generics for int/float
[Serializable, InlineProperty]
public class Stat {
  [HideInInspector] public string statName;

  public Stat(string statName, int initialValue = 0) {
    this.statName = statName;
    this.InitialValue = initialValue;
    this.CurrentValue = initialValue;
    // this.ui.statName = statName;
    this.ui.prefix = statName + ": ";
  }

  private const float LABEL_WIDTH_1 = 80f;

  // [ToggleGroup(nameof(enable), groupTitle: "$statName")]
  [FoldoutGroup("$statName")]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]
  [LabelText("Enable Stat")]
  public bool enable = true;

  #region VALUES ===================================================================================================================================
  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  [OnValueChanged(nameof(UpdateStatUI))]
  public int InitialValue;
  [HideInInspector] public int CurrentValue;

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]
  public bool enableMin = true;

  [FoldoutGroup("$statName"), EnableIf(nameof(EnableStatAndMin))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  public int MinValue;
  private bool EnableStatAndMin() { return enable && enableMin; }

  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]

  public bool enableMax;

  [FoldoutGroup("$statName"), EnableIf(nameof(EnableStatAndMax))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  [PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
  public int MaxValue;
  private bool EnableStatAndMax() { return enable && enableMax; }
  #endregion ===================================================================================================================================

  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [OnValueChanged(nameof(UpdateStatUI), true)]
  [HideLabel] public StatUI ui = new StatUI();

  private void UpdateStatUI() {
    ui.Update(InitialValue);
  }

  #region EVENTS ===================================================================================================================================
  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatIncrease = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatDecrease = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMin = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMax = new UnityEvent();

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatZero = new UnityEvent();
  #endregion ===================================================================================================================================

  #region PUBLIC METHODS ===================================================================================================================================
  public void Update(int amountToAdd) {
    int valueAfterUpdate = CurrentValue + amountToAdd;

    if (enableMin && valueAfterUpdate < MinValue) {
      valueAfterUpdate = MinValue;
    }

    if (enableMax && valueAfterUpdate > MaxValue) {
      valueAfterUpdate = MaxValue;
    }

    Set(valueAfterUpdate);
    if (amountToAdd > 0) OnStatIncrease.Invoke();
    if (amountToAdd < 0) OnStatDecrease.Invoke();
  }

  public void Increase() {
    Update(1);
  }

  public void Descrease() {
    Update(-1);
  }

  public void Set(int value) {
    CurrentValue = value;
    ui?.Update(CurrentValue);
    if (value <= 0) OnStatZero.Invoke();
    if (enableMin && value == MinValue) OnStatMin.Invoke();
    if (enableMax && value == MaxValue) OnStatMax.Invoke();
  }

  public void SetZero() {
    Set(0);
  }

  public void SetMin() {
    Set(MinValue);
  }

  public void SetMax() {
    Set(MaxValue);
  }
  #endregion ===================================================================================================================================
}