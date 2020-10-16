using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using JetBrains.Annotations;
using Boo.Lang;

public class ItemMountainInfiniteTowersCpt : ItemGameBaseCpt
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
            CharacterBean characterData = uiGameManager.gameDataManager.gameData.GetCharacterDataById(memberId);
            characterUI.SetCharacterData(characterData.body, characterData.equips);
            tvName.text = characterData.baseInfo.name;
        }
    }

    public void OnClickForCancel()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        foreach(string memberId in infiniteTowersData.listMembers)
        {
            CharacterBean characterData= uiGameManager.gameDataManager.gameData.GetCharacterDataById(memberId);
            characterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
        }
        uiGameManager.gameDataManager.gameData.RemoveInfiniteTowersData(infiniteTowersData);
        uiComponent.RefreshUI();
    }

    public void OnClickForContinue()
    {


    }
}