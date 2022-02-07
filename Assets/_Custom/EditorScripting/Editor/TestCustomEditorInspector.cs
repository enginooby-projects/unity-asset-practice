using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestCustomEditor))]
// [CanEditMultipleObjects]
public class TestCustomEditorInspector : Editor {
  private TestCustomEditor _target;
  private SerializedObject _serializedObject;
  private SerializedProperty _serializedTimeLeft;

  private void OnEnable() {
    Debug.Log("OnEnable");
    _target = target as TestCustomEditor;
    _serializedObject = new SerializedObject(_target);
    _serializedTimeLeft = _serializedObject.FindProperty("_timeLeft");  // ! string error prone
  }

  private void OnDisable() {
    Debug.Log("OnDisable");
  }

  public override void OnInspectorGUI() {
    // DrawDefaultInspector();
    DrawCustomInspector();
  }

  // this field does not exist on the inspected class TestCustomEditor
  private Color _bonusColor;

  private void DrawCustomInspector() {
    EditorGUILayout.LabelField("Custom inspector", EditorStyles.boldLabel);

    EditorGUILayout.BeginHorizontal("Box");
    ShowInInspector(ref _bonusColor);
    // _color = EditorGUILayout.ColorField("Bonus Color", _color);

    // ShowInInspector(ref _target.Level); // impossible
    // int level = _target.Level; // work around
    // ShowInInspector(ref level);
    // _target.Level = level;

    _target.Level = EditorGUILayout.IntField("Validated " + nameof(_target.Level), _target.Level);
    EditorGUILayout.EndHorizontal();

    EditorGUILayout.BeginHorizontal("Button");
    EditorGUILayout.LabelField("Button is enabled if level is even");
    bool oldEnabled = GUI.enabled;
    GUI.enabled = _target.Level % 2 == 0;
    if (GUILayout.Button("Randomize Color", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight)))
      _bonusColor = Random.ColorHSV();
    EditorGUILayout.EndHorizontal();
    GUI.enabled = oldEnabled;

    EditorGUILayout.PropertyField(_serializedTimeLeft);
  }

  private void ShowInInspector(ref Color field) {
    // TODO: Get original name of passed variable
    field = EditorGUILayout.ColorField(nameof(field), field);
  }

  private void ShowInInspector(ref int field) {
    field = EditorGUILayout.IntField(nameof(field), field);
  }
}
