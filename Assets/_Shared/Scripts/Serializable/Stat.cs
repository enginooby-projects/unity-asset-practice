using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

// ? Make generics for int/float
[Serializable, InlineProperty]
public class Stat {
  [HideInInspector] public string statName;
  //  public StatName StatName(){
  // StatName.
  //  }

  public Stat(string statName, int initialValue = 0) {
    this.statName = statName;
    this.InitialValue = initialValue;
    this.CurrentValue = initialValue;
    // this.ui.statName = statName;
    this.ui.prefix = statName + ": ";
  }

  private const float LABEL_WIDTH_1 = 80f;

  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  // [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  [DisplayAsString] public int CurrentValue;

  // [ToggleGroup(nameof(enable), groupTitle: "$statName")]
  [FoldoutGroup("$statName")]
  [HorizontalGroup("$statName/Enable"), LabelWidth(LABEL_WIDTH_1)]
  [LabelText("Enable Stat")]
  public bool enable = true;

  #region VALUES ===================================================================================================================================
  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Value"), LabelWidth(LABEL_WIDTH_1)]
  [OnValueChanged(nameof(OnInitialValueChanged))]
  public int InitialValue;

  private void OnInitialValueChanged() {
    CurrentValue = InitialValue;
    UpdateStatUI();
  }

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
  public UnityEvent OnStatChange = new UnityEvent();

  // TIP: Create both C# event and UnityEvent -> Use C# event (bind in script) instead of UnityEvent (bind in Inspector) for better performance.
  public event Action OnStatChangeEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatIncrease = new UnityEvent();

  // TIP: Create both C# event and UnityEvent -> Use C# event (bind in script) instead of UnityEvent (bind in Inspector) for better performance.
  public event Action OnStatIncreaseEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatDecrease = new UnityEvent();
  public event Action OnStatDecreaseEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMin = new UnityEvent();
  public event Action OnStatMinEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatMax = new UnityEvent();
  public event Action OnStatMaxEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Manual Events")]
  public UnityEvent OnStatZero = new UnityEvent();
  public event Action OnStatZeroEvent;
  #endregion ===================================================================================================================================

  #region PUBLIC METHODS ===================================================================================================================================
  /// <summary>
  /// Constraint the given amount in range of min-max then add to the current stat value.
  /// </summary>
  public void Update(int amountToAdd) {
    int valueAfterUpdate = CurrentValue + amountToAdd;
    ConstraintMinMax(valueAfterUpdate);
    Set(valueAfterUpdate);

    if (amountToAdd > 0) {
      OnStatIncrease.Invoke();
      OnStatDecreaseEvent?.Invoke();
    }
    if (amountToAdd < 0) {
      OnStatDecrease.Invoke();
      OnStatDecreaseEvent?.Invoke();
    }
  }

  private void ConstraintMinMax(int rawValue) {
    if (enableMin && rawValue < MinValue) {
      rawValue = MinValue;
    }

    if (enableMax && rawValue > MaxValue) {
      rawValue = MaxValue;
    }
  }

  /// <summary>
  /// Increase current stat value by 1.
  /// </summary>
  public void Increase() {
    Update(1);
  }

  /// <summary>
  /// Descrease current stat value by 1.
  /// </summary>
  public void Descrease() {
    Update(-1);
  }

  public void Set(int value) {
    if (value != CurrentValue) {
      OnStatChange.Invoke();
      OnStatChangeEvent?.Invoke();
    }

    CurrentValue = value;
    ui?.Update(CurrentValue);

    if (value == 0) {
      OnStatZero.Invoke();
      OnStatZeroEvent?.Invoke();
    }
    if (enableMin && value <= MinValue) {
      OnStatMin.Invoke();
      OnStatMinEvent?.Invoke();
    }
    if (enableMax && value >= MaxValue) {
      OnStatMax.Invoke();
      OnStatMaxEvent?.Invoke();
    }
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