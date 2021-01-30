using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CursorManager : BaseManager
{
    protected List<Texture2D> _listKnockTex = new List<Texture2D>();

    public List<Texture2D> listKnockTex
    {
        get
        {
            if (CheckUtil.ListIsNull(_listKnockTex))
            {
                Texture2D sp1 = IconDataHandler.Instance.manager.GetTextureUIByName("cursor_11");
                Texture2D sp2 = IconDataHandler.Instance.manager.GetTextureUIByName("cursor_12");
                Texture2D sp3 = IconDataHandler.Instance.manager.GetTextureUIByName("cursor_13");
                _listKnockTex.Add(sp1);
                _listKnockTex.Add(sp2);
                _listKnockTex.Add(sp3);
            }
            return _listKnockTex;
        }
    }
}