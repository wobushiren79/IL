using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogManager : BaseManager
{
    public List<DialogView> listDialog = new List<DialogView>();
    public GameObject objContainer;
    public Dictionary<string, GameObject> listObjModel = new Dictionary<string, GameObject>();
    protected string resUrl = "UI/Dialog/";

    public T CreateDialog<T>(DialogEnum dialogType, DialogView.IDialogCallBack callBack, DialogBean dialogBean) where T : DialogView
    {
        return CreateDialog<T>(dialogType, callBack, dialogBean, 0);
    }

    public T CreateDialog<T>(DialogEnum dialogType, DialogView.IDialogCallBack callBack, DialogBean dialogBean, float delayDelete) where T : DialogView
    {
        string dialogName = EnumUtil.GetEnumName(dialogType);
        GameObject objDialog = CreateDialog(dialogName);
        if (objDialog)
        {
            DialogView dialogView = objDialog.GetComponent<DialogView>();
            if (dialogView == null)
                Destroy(objDialog);
            dialogView.SetCallBack(callBack);
            dialogView.SetData(dialogBean);
            if (delayDelete != 0)
                dialogView.SetDelayDelete(delayDelete);

            //改变焦点
            EventSystem.current.SetSelectedGameObject(objDialog);

            listDialog.Add(dialogView);
            return dialogView as T;
        }
        else
        {
            LogUtil.LogError("没有找到指定Toast：" + "Resources/UI/Toast/" + dialogName);
            return null;
        }
    }
    public GameObject CreateDialog(string name)
    {
        GameObject objModel = null;
        if (listObjModel.TryGetValue(name, out objModel))
        {

        }
        else
        {
            objModel = CreatDialogModel(name);
        }
        if (objModel == null)
            return null;
        GameObject obj = Instantiate(objContainer, objModel);
        return obj;
    }

    private GameObject CreatDialogModel(string name)
    {
        GameObject objModel = Resources.Load<GameObject>(resUrl + name);
        objModel.name = name;
        listObjModel.Add(name, objModel);
        return objModel;
    }

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

    public void RemoveDialog(DialogView dialogView)
    {
        if (dialogView != null && listDialog.Contains(dialogView))
            listDialog.Remove(dialogView);
    }

    public RectTransform GetContainer()
    {
        return (RectTransform)objContainer.transform;
    }
}