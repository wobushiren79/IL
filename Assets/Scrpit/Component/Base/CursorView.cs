using UnityEngine;
using UnityEditor;

public class CursorView : BaseMonoBehaviour
{
    public Texture2D cursorDef;
    public Texture2D cursorDown;

    protected Vector2 offsetCursor;

    private void Awake()
    {
        offsetCursor = new Vector2(40,0);
        Cursor.SetCursor(cursorDef,Vector2.zero,CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorDown, offsetCursor, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorDef, offsetCursor, CursorMode.Auto);
        }
    }
}