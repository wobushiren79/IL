using UnityEngine;
using UnityEditor;

public class CursorHandler : BaseHandler
{
    public enum CursorType
    {
        Def = 1,
        Knock = 2,//敲打
    }
    public CursorView cursorView;
    public CursorManager cursorManager;
    public CursorType cursorType= CursorType.Def;

    public void SetCursor(CursorType cursorType)
    {
        if(this.cursorType == cursorType)
            return;
        this.cursorType = cursorType;

        switch (cursorType)
        {
            case CursorType.Def:
                cursorView.SetDef();
                break;
            case CursorType.Knock:    
                cursorView.SetCursor(cursorManager.listKnockTex);
                break;
        }

    }
}