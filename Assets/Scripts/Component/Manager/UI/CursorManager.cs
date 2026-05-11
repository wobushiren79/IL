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
            if (_listKnockTex.IsNull())
            {
                Texture2D sp1 = IconHandler.Instance.GetTextureUIByName("cursor_11");
                Texture2D sp2 = IconHandler.Instance.GetTextureUIByName("cursor_12");
                Texture2D sp3 = IconHandler.Instance.GetTextureUIByName("cursor_13");
                _listKnockTex.Add(sp1);
                _listKnockTex.Add(sp2);
                _listKnockTex.Add(sp3);
            }
            return _listKnockTex;
        }
    }
}