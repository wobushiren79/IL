using UnityEngine;
using UnityEditor;

public class CursorHandler : BaseHandler<CursorHandler, CursorManager>
{
    public enum CursorType
    {
        Def = 1,
        Knock = 2,//敲打
    }
    public CursorType cursorType = CursorType.Def;

    protected CursorView _cursorView;

    public CursorView cursorView
    {
        get
        {
            if (_cursorView == null)
            {
                _cursorView = FindWithTag<CursorView>(TagInfo.Tag_Cursor);
            }
            return _cursorView;
        }
    }

    public void SetCursor(CursorType cursorType)
    {
        if (this.cursorType == cursorType)
            return;
        this.cursorType = cursorType;

        switch (cursorType)
        {
            case CursorType.Def:
                cursorView.SetDef();
                break;
            case CursorType.Knock:
                cursorView.SetCursor(manager.listKnockTex);
                break;
        }

    }
}