using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DialogManager : BaseMonoBehaviour
{

    public GameObject objDialogContainer;
    public List<GameObject> listObjDialogModel=new List<GameObject>();

    public DialogView CreateDialog(DialogEnum dialogType, DialogView.IDialogCallBack callBack, DialogBean dialogBean)
    {
       return CreateDialog(dialogType, callBack, dialogBean, 0);
    }

    public DialogView CreateDialog(DialogEnum dialogType, DialogView.IDialogCallBack callBack, DialogBean dialogBean, float delayDelete)
    {
        if (CheckUtil.ListIsNull(listObjDialogModel))
        {
            LogUtil.LogError("创建Dialog失败，缺少Dialog模型");
            return null;
        }
        GameObject objCreateDialogModel = null;
        foreach (GameObject itemDialog in listObjDialogModel)
        {
            if (itemDialog.name.Equals(EnumUtil.GetEnumName(dialogType)))
            {
                objCreateDialogModel = itemDialog;
                break;
            }
        }
        if (objCreateDialogModel == null)
            return null;
        GameObject objDialog = Instantiate(objDialogContainer, objCreateDialogModel);
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