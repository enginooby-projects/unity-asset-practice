using System;

public static class EnumUtils {
  /// <summary>
  /// If non enum string match the given string, return the first enum.
  /// </summary>
  public static T ToEnumString<T>(this string value) where T : Enum {
    var enumValues = (T[])Enum.GetValues(typeof(T));
    foreach (var enumValue in enumValues) {
      if (value.EqualIgnoreCase(enumValue.ToString())) return enumValue;
    }

    return enumValues[0];
  }

  /// <summary>
  /// Increment (loop) enum value.
  /// </summary>
  public static T Next<T>(this T src) where T : struct {
    if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

    T[] Arr = (T[])Enum.GetValues(src.GetType());
    int j = Array.IndexOf<T>(Arr, src) + 1;
    return (Arr.Length == j) ? Arr[0] : Arr[j];
  }

  /// <summary>
  /// Decrement (loop) enum value.
  /// </summary>
  public static T Previous<T>(this T src) where T : struct {
    if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

    T[] Arr = (T[])Enum.GetValues(src.GetType());
    int j = Array.IndexOf<T>(Arr, src) - 1;
    return (j <= -1) ? Arr[Arr.Length - 1] : Arr[j];
  }
}

