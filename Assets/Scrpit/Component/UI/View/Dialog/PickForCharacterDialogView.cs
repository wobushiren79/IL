using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickForCharacterDialogView : DialogView, ItemGameDialogPickCharacterCpt.ICallBack
{
    public Text tvNull;
    public GameObject objPickCharacterContainer;
    public GameObject objPickCharacterModel;

    protected GameDataManager gameDataManager;
    protected ToastManager toastManager;

    public int pickCharacterMax = 0;
    public List<CharacterBean> listPickCharacter = new List<CharacterBean>();
    public List<string> listExpelCharacterId = new List<string>();

    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find< GameDataManager >( ImportantTypeEnum.GameDataManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
    }

    /// <summary>
    /// 设置选择角色上限
    /// </summary>
    /// <param name="pickCharacterMax"></param>
    public void SetPickCharacterMax(int pickCharacterMax)
    {
        this.pickCharacterMax = pickCharacterMax;
    }

    /// <summary>
    /// 设置排出角色
    /// </summary>
    /// <param name="characterId"></param>
    public void SetExpelCharacter(List<string> listCharacterId)
    {
        this.listExpelCharacterId = listCharacterId;
    }

    public void SetExpelCharacter(string characterId)
    {
        SetExpelCharacter(new List<string>() { characterId });
    }

    public override void InitData()
    {
        base.InitData();
        listPickCharacter.Clear();
        CptUtil.RemoveChildsByActive(objPickCharacterContainer);
        List<CharacterBean> listCharacter = gameDataManager.gameData.GetAllCharacterData();
        bool isNull = true;
        foreach (CharacterBean characterData in listCharacter)
        {
            //筛选不能选择的角色
            bool isExpel = false;
            if (listExpelCharacterId != null)
                foreach (string itemId in listExpelCharacterId)
                {
                    if (itemId.Equals(characterData.baseInfo.characterId))
                    {
                        isExpel = true;
                        break;
                    }
                }
            //排出研究中的角色
            if (characterData.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Research)
            {
                isExpel = true;
            }
            if (!isExpel)
            {
                CreatePickItem(characterData);
                isNull = false;
            }
        }
        if (isNull)
        {
            tvNull.gameObject.SetActive(true);
        }
        else
        {
            tvNull.gameObject.SetActive(false);
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        SetTitle();
    }

    public override void SubmitOnClick()
    {
        if (CheckUtil.ListIsNull(listPickCharacter))
        {
            if (audioHandler != null)
                audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1032));
        }
        else
        {
            base.SubmitOnClick();
        }
    }

    private void CreatePickItem(CharacterBean characterData)
    {
        GameObject objPick = Instantiate(objPickCharacterContainer, objPickCharacterModel);
        ItemGameDialogPickCharacterCpt pickCpt = objPick.GetComponent<ItemGameDialogPickCharacterCpt>();
        pickCpt.SetData(characterData);
        pickCpt.SetCallBack(this);
    }

    /// <summary>
    /// 获取选取的角色
    /// </summary>
    /// <returns></returns>
    public List<CharacterBean> GetPickCharacter()
    {
        return listPickCharacter;
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    public void SetTitle()
    {
        if (tvTitle != null)
        {
            if(CheckUtil.StringIsNull(dialogData.title))
            {
                tvTitle.text = GameCommonInfo.GetUITextById(4015) + "(" + listPickCharacter.Count + "/" + pickCharacterMax + ")";
            }
            else
            {
                tvTitle.text = dialogData.title + "(" + listPickCharacter.Count + "/" + pickCharacterMax + ")";
            }
        }
    }

    #region 选择回调
    public void PickCharacter(ItemGameDialogPickCharacterCpt itemView, bool isPick, CharacterBean characterData)
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (isPick)
        {
            if (listPickCharacter.Count >= pickCharacterMax)
            {
                toastManager.ToastHint(GameCommonInfo.GetUITextById(1052));
                itemView.ChangeStatus();
            }
            else
            {
                listPickCharacter.Add(characterData);
            }
        }
        else
        {
            listPickCharacter.Remove(characterData);
        }
        RefreshUI();
    }
    #endregion
}