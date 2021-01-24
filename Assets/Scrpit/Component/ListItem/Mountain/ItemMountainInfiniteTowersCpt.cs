using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemMountainInfiniteTowersCpt : ItemGameBaseCpt,DialogView.IDialogCallBack
{
    public Text tvLayer;
    public Text tvIsSend;
    public ProgressView pvForSend;
    public Button btCancel;
    public Button btContinue;

    public GameObject objMembersContianer;
    public GameObject objMembersModel;

    protected UserInfiniteTowersBean infiniteTowersData;

    private void Awake()
    {
        if (btContinue)
            btContinue.onClick.AddListener(OnClickForContinue);

        if (btCancel)
            btCancel.onClick.AddListener(OnClickForCancel);
    }

    public void SetData(UserInfiniteTowersBean infiniteTowersData)
    {
        this.infiniteTowersData = infiniteTowersData;
        SetLayer(infiniteTowersData.layer);
        SetIsSend(infiniteTowersData.isSend, infiniteTowersData.proForSend);
        SetMembers(infiniteTowersData.listMembers);
    }

    public void SetLayer(long layer)
    {
        tvLayer.text = layer + "";
    }

    public void SetIsSend(bool isSend, float sendPro)
    {
        if (isSend)
        {
            tvIsSend.gameObject.SetActive(true);
            btContinue.gameObject.SetActive(false);
            pvForSend.gameObject.SetActive(true);
            pvForSend.SetData(sendPro);
        }
        else
        {
            tvIsSend.gameObject.SetActive(false);
            btContinue.gameObject.SetActive(true);
            pvForSend.gameObject.SetActive(false);
            pvForSend.SetData(0);
        }
    }

    public void SetMembers(List<string> listMembers)
    {
        CptUtil.RemoveChildsByActive(objMembersContianer);
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        for (int i = 0; i < listMembers.Count; i++)
        {
            string memberId = listMembers[i];
            GameObject objItem = Instantiate(objMembersContianer, objMembersModel);

            CharacterUICpt characterUI = objItem.GetComponentInChildren<CharacterUICpt>();
            Text tvName = objItem.GetComponentInChildren<Text>();

            //设置数据
            CharacterBean characterData = uiGameManager.gameData.GetCharacterDataById(memberId);
            characterUI.SetCharacterData(characterData.body, characterData.equips);
            tvName.text = characterData.baseInfo.name;
        }
    }

    /// <summary>
    /// 点击 取消
    /// </summary>
    public void OnClickForCancel()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        DialogBean dialogData = new DialogBean();
        dialogData.content = GameCommonInfo.GetUITextById(3111);
        dialogData.dialogPosition = 0;
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
    }

    /// <summary>
    /// 点击继续
    /// </summary>
    public void OnClickForContinue()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();

        foreach (string memberId in infiniteTowersData.listMembers)
        {
            CharacterBean characterData = uiGameManager.gameData.GetCharacterDataById(memberId);
            if (characterData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest
                && characterData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work)
            {
                ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1141));
                return;
            }
        }

        //跳转场景
        GameCommonInfo.SetInfiniteTowersPrepareData(infiniteTowersData);
        SceneUtil.SceneChange(ScenesEnum.GameInfiniteTowersScene);
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogBean.dialogPosition == 0)
        {
            //删除确认
            UIGameManager uiGameManager = GetUIManager<UIGameManager>();
            if (infiniteTowersData.isSend)
            {
                foreach (string memberId in infiniteTowersData.listMembers)
                {
                    CharacterBean characterData = uiGameManager.gameData.GetCharacterDataById(memberId);
                    characterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
                }
            }
            infiniteTowersData.proForSend = -1;
            uiGameManager.gameData.RemoveInfiniteTowersData(infiniteTowersData);
            uiComponent.RefreshUI();
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}