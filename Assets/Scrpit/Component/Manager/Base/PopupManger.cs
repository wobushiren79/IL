using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class PopupManger : BaseManager
{
    public Dictionary<string, GameObject> dicPopupModel = new Dictionary<string, GameObject>();

    /// <summary>
    /// 获取弹窗模型
    /// </summary>
    /// <param name="dialogName"></param>
    /// <returns></returns>
    public GameObject GetPopupModel(string popupName)
    {
        return GetGameObjectModel(dicPopupModel, "ui/popup", popupName);
    }
}