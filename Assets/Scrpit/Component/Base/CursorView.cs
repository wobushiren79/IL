using UnityEngine;
using UnityEditor;

public class CursorView : BaseMonoBehaviour
{
    public Texture2D cursorDef;
    public Texture2D cursorDown;

    private void Awake()
    {
        Cursor.SetCursor(cursorDef,Vector2.zero,CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorDown, Vector2.zero, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorDef, Vector2.zero, CursorMode.Auto);
        }
    }
}