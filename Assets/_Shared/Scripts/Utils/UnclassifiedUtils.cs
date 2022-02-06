using System;
using System.Linq;
using UnityEngine;

public static class UnclassifiedUtils {
  // TODO: Move to StringUtils or PrimiteTypeUtils
  public static string RemoveWhitespace(this string input) {
    return new string(input.ToCharArray()
        .Where(c => !Char.IsWhiteSpace(c))
        .ToArray());
  }

  public static string ToSentenceCase(this string str) {
    return System.Text.RegularExpressions.Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
  }

  public static bool EqualIgnoreCase(this string string1, string string2) {
    return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Return true if value just below min, false if value just above max, null if value in range.
  /// </summary>
  public static bool? ReachingMinOrMax(this float value, float min, float max) {
    if (value <= min) return true;
    if (value >= max) return false;
    return null;
  }

  /// <summary>
  /// Return true if y position just below min, false if y position just above max, null if y position in range.
  /// </summary>
  public static bool? ReachingYMinOrMax(this Transform transform, float min, float max) {
    if (transform.position.y <= min) return true;
    if (transform.position.y >= max) return false;
    return null;
  }

  public static Rect GetHalfTop(this Rect rect) {
    return new Rect(rect.x, rect.y, rect.width, rect.height / 2);
  }

  public static Rect GetHalfBottom(this Rect rect) {
    return new Rect(rect.x, rect.y + rect.height / 2, rect.width, rect.height / 2);
  }
}

