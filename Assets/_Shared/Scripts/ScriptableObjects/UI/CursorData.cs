using UnityEngine;

namespace Enginoobz.UI {
  [CreateAssetMenu(fileName = "CS_", menuName = "UI/Cusor Data", order = 0)]
  public class CursorData : ScriptableObject {
    [SerializeField] private CursorName _name;
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Vector2 _hotSpot;

    public bool CompareName(CursorName nameTarget) {
      return _name == nameTarget;
    }

    public void SetCursor() {
      if (!_texture) return;

      Cursor.SetCursor(_texture, _hotSpot, CursorMode.Auto);
      Debug.Log("Set Cursor");
    }
  }

  // TIP: Explicitly assign int to enum used in SO, to prevent swapping value when change enum order
  public enum CursorName {
    None = 0,
    Move = 1,
    Attack = 2,
  }
}