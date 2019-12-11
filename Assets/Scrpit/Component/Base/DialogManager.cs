using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DialogManager : BaseMonoBehaviour
{

    public GameObject objDialogContainer;
    public List<GameObject> listObjDialogModel=new List<GameObject>();

    public DialogView CreateDialog(int dialogPosition, DialogView.IDialogCallBack callBack, DialogBean dialogBean)
    {
       return CreateDialog(dialogPosition, callBack, dialogBean, 0);
    }

    public DialogView CreateDialog(int dialogPosition, DialogView.IDialogCallBack callBack, DialogBean dialogBean, float delayDelete)
    {
        if (CheckUtil.ListIsNull(listObjDialogModel) || dialogPosition >= listObjDialogModel.Count)
        {
            LogUtil.LogError("创建Dialog失败，缺少Dialog模型");
            return null;
        }
        GameObject objDialog = Instantiate(objDialogContainer, listObjDialogModel[dialogPosition]);
        DialogView dialogView = objDialog.GetComponent<DialogView>();
        if (dialogView == null)
            Destroy(objDialog);
        dialogView.SetCallBack(callBack);
        dialogView.SetData(dialogBean);
        if (delayDelete != 0)
            dialogView.SetDelayDelete(delayDelete);
        return dialogView;
    }
}