using UnityEngine;
using UnityEditor;
using Enginoobz.Utils;

[CustomPropertyDrawer(typeof(TimeAttribute))]
public class TimeDrawer : PropertyDrawer {
  public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
    return EditorGUI.GetPropertyHeight(property) * 2;
  }

  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    if (property.IsInteger()) {
      property.intValue = EditorGUI.IntField(position.GetHalfTop(), label, Mathf.Max(0, property.intValue));
      EditorGUI.LabelField(position.GetHalfBottom(), "", FormatTime(property.intValue));
    } else {
      EditorGUI.HelpBox(position, "Use Time attribute for an int", MessageType.Error);
    }
  }

  private string FormatTime(int totalSeconds) {
    TimeAttribute time = attribute as TimeAttribute;

    // UTIL
    if (time.DisplayHours) {
      int hours = totalSeconds / (60 * 60);
      int minutes = ((totalSeconds % (60 * 60)) / 60);
      int seconds = (totalSeconds % 60);
      return string.Format("{0}:{1}:{2} (h:m:s)", hours, minutes.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
    } else {
      int minutes = (totalSeconds / 60);
      int seconds = (totalSeconds % 60);
      return string.Format("{0}:{1} (m:s)", minutes.ToString(), seconds.ToString().PadLeft(2, '0'));
    }
  }
}
