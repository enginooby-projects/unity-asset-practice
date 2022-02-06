using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestCustomEditor))]
// [CanEditMultipleObjects]
public class TestCustomEditorInspector : Editor {
  private TestCustomEditor _target;

  private void OnEnable() {
    Debug.Log("OnEnable");
    _target = target as TestCustomEditor;
  }

  private void OnDisable() {
    Debug.Log("OnDisable");
  }

  public override void OnInspectorGUI() {
    // DrawDefaultInspector();
    DrawCustomInspector();
  }

  private Color _color;

  private void DrawCustomInspector() {
    EditorGUILayout.LabelField("Custom inspector", EditorStyles.boldLabel);

    EditorGUILayout.BeginHorizontal("Box");
    _color = EditorGUILayout.ColorField("Bonus Color", _color);
    _target.Level = EditorGUILayout.IntField("Validated " + nameof(_target.Level), _target.Level);
    EditorGUILayout.EndHorizontal();

    EditorGUILayout.BeginHorizontal("Button");
    EditorGUILayout.LabelField("Button is enabled if level is even");
    bool oldEnabled = GUI.enabled;
    GUI.enabled = _target.Level % 2 == 0;
    if (GUILayout.Button("Randomize Color", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight)))
      _color = Random.ColorHSV();
    EditorGUILayout.EndHorizontal();
    GUI.enabled = oldEnabled;
  }
}
