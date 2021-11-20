using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

// ? Make generics for int/float
[Serializable, InlineProperty]
public class Stat {
  [HideInInspector] public string statName;

  public Stat(string statName, int initialValue = 0) {
    this.statName = statName;
    this.InitialValue = initialValue;
    this.CurrentValue = initialValue;
    // this.ui.statName = statName;
    this.uis.ForEach(ui => ui.prefix = statName + ": ");
  }

  private const float LABEL_WIDTH_1 = 80f;

  [FoldoutGroup("$statName"), EnableIf(nameof(enable))]
  [HorizontalGroup("$statName/Debug"), LabelWidth(LABEL_WIDTH_1)]
  // [ProgressBar(nameof(MinValue), nameof(MaxValue), r: 1, g: 1, b: 1, Height = 30)]
  [DisplayAsString]
  public int CurrentValue;

  [FoldoutGroup("$statName")]
  // [EnableIf(nameof(EnableStatAndMax))]
  // [HorizontalGroup("$statName/Debug"), LabelWidth(LABEL_WIDTH_1)]
  // [Button]
  public float CurrentPercentage => CurrentValue * 100 / MaxValue;


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
    InitStatUIs();
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
  public int MaxValue = 100;
  private bool EnableStatAndMax() { return enable && enableMax; }
  private bool EnableStatAndDisableMax() { return enable && !enableMax; }
  #endregion ===================================================================================================================================

  // TODO: Implement multiple UIs (? scriptable objects)
  // [ToggleGroup(nameof(enable))]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [OnValueChanged(nameof(InitStatUIs), true)]
  // [HideLabel]
  [LabelText("UIs")]
  public List<StatUI> uis = new List<StatUI>() { new StatUI() };

  private void InitStatUIs() {
    UpdateStatUIs(InitialValue);
    // this.uis.ForEach(ui => ui.prefix = statName + ": ");
  }

  private void UpdateStatUIs(int value) {
    uis.ForEach(ui => ui.Update(value));
  }

  #region EVENTS ===================================================================================================================================
  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatChange = new UnityEvent();

  // TIP: Create both C# event and UnityEvent -> Use C# event (bind in script) instead of UnityEvent (bind in Inspector) for better performance.
  public event Action OnStatChangeEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatIncrease = new UnityEvent();

  // TIP: Create both C# event and UnityEvent -> Use C# event (bind in script) instead of UnityEvent (bind in Inspector) for better performance.
  public event Action OnStatIncreaseEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatDecrease = new UnityEvent();
  public event Action OnStatDecreaseEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatMin = new UnityEvent();
  public event Action OnStatMinEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatMax = new UnityEvent();
  public event Action OnStatMaxEvent;

  // [ToggleGroup(nameof(enable))]
  // [FoldoutGroup("enable/Manual Events")]
  [FoldoutGroup("$statName"), ShowIf(nameof(enable))]
  [FoldoutGroup("$statName/Events")]
  public UnityEvent OnStatZero = new UnityEvent();
  public event Action OnStatZeroEvent;
  #endregion ===================================================================================================================================

  #region PUBLIC METHODS ===================================================================================================================================
  /// <summary>
  /// Constraint the given amount in range of min-max then add to the current stat value.
  /// </summary>
  // ? Rename to Add()
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

  public void Add(int amount) {
    Update(amount);
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
    UpdateStatUIs(CurrentValue);

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

public enum StatName : int {
  [StringValue("Health")]
  Health,

  [StringValue("Level")]
  Level,

  [StringValue("Mana")]
  Mana,

  [StringValue("Stamia")]
  Stamia,

  [StringValue("Experience")]
  Experience,

  [StringValue("ExperienceReward")]
  ExperienceReward,


  [StringValue("Scores")]
  Scores,

  [StringValue("Lives")]
  Lives,
}