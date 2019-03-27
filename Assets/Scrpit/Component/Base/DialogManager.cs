using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DialogManager :BaseMonoBehaviour
{
    public List<DialogView> listDialogModel;
    public GameObject parentObj;

    public void CreateDialog(int dialogPosition, DialogView.IDialogCallBack callBack,DialogBean dialogBean)
    {
        if (CheckUtil.ListIsNull(listDialogModel) || dialogPosition >= listDialogModel.Count)
        {
            LogUtil.LogError("创建Dialog失败，缺少Dialog模型");
            return;
        }
        DialogView dialogViewModel= listDialogModel[dialogPosition];
        GameObject dialogObj=  Instantiate(dialogViewModel.gameObject, dialogViewModel.transform);
        dialogObj.SetActive(true);
        dialogObj.transform.SetParent(parentObj.transform);
        DialogView dialogView = dialogObj.GetComponent<DialogView>();
        if (dialogView == null)
            Destroy(dialogObj);
        dialogView.SetCallBack(callBack);
        dialogView.SetData(dialogBean);
    }
}