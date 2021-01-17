using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogManager : BaseManager
{
    //打开的dialog列表
    public List<DialogView> listDialog = new List<DialogView>();

    //所有的dialog模型
    public Dictionary<string, GameObject> listObjModel = new Dictionary<string, GameObject>();

    /// <summary>
    /// 获取弹窗模型
    /// </summary>
    /// <param name="dialogName"></param>
    /// <returns></returns>
    public GameObject GetDialogModel(string dialogName)
    {
        return GetGameObjectModel(listObjModel, "ui/dialog", dialogName);
    }

    /// <summary>
    /// 增加弹窗
    /// </summary>
    /// <param name="dialogView"></param>
    public void AddDialog(DialogView dialogView)
    {
        listDialog.Add(dialogView);
    }

    /// <summary>
    /// 移除弹窗
    /// </summary>
    /// <param name="dialogView"></param>
    public void RemoveDialog(DialogView dialogView)
    {
        if (dialogView != null && listDialog.Contains(dialogView))
            listDialog.Remove(dialogView);
    }

    /// <summary>
    /// 关闭所有弹窗
    /// </summary>
    public void CloseAllDialog()
    {
        for (int i = 0; i < listDialog.Count; i++)
        {
            DialogView dialogView = listDialog[i];
            if (dialogView != null)
                dialogView.DestroyDialog();
        }
        listDialog.Clear();
    }

    public RectTransform GetContainer()
    {
        return (RectTransform)gameObject.transform;
    }


}