using UnityEngine;

namespace Enginoobz.UI {
  [CreateAssetMenu(fileName = "CS_", menuName = "UI/Cusor Data", order = 0)]
  public class CursorData : ScriptableObject {
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Vector2 _hotSpot;

    // TODO: Make method Update-safe
    public void SetCursor() {
      if (!_texture) return;

      Cursor.SetCursor(_texture, _hotSpot, CursorMode.Auto);
      Debug.Log("Set Cursor");
    }

    public void ChangeAndSetCursor(CursorData lastCursor) {
      if (lastCursor == this) return;

      lastCursor = this;
      lastCursor.SetCursor();
    }
  }
}