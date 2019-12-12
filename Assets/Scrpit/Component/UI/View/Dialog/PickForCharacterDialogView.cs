using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickForCharacterDialogView : DialogView,ItemGameDialogPickCharacterCpt.ICallBack
{
    public GameObject objPickCharacterContainer;
    public GameObject objPickCharacterModel;

    private GameDataManager mGameDataManager;
    private ToastManager mToastManager;

    public int pickCharacterMax = 0;
    public List<CharacterBean> listPickCharacter = new List<CharacterBean>();

    private void Awake()
    {
        mGameDataManager = FindObjectOfType<GameDataManager>();
        mToastManager = FindObjectOfType<ToastManager>();
    }

    public new void OnEnable()
    {
        base.OnEnable();
    }

    public void SetData(int pickCharacterMax)
    {
        this.pickCharacterMax = pickCharacterMax;
        InitData();
    }

    public new void InitData()
    {
        base.InitData();
        listPickCharacter.Clear();
        CptUtil.RemoveChildsByActive(objPickCharacterContainer);
        List<CharacterBean> listCharacter = mGameDataManager.gameData.GetAllCharacterData();
        foreach (CharacterBean characterData in listCharacter)
        {
            CreatePickItem(characterData);
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        SetTitle();
    }

    private void CreatePickItem(CharacterBean characterData)
    {
        GameObject objPick = Instantiate(objPickCharacterContainer, objPickCharacterModel);
        ItemGameDialogPickCharacterCpt pickCpt = objPick.GetComponent<ItemGameDialogPickCharacterCpt>();
        pickCpt.SetData(characterData);
        pickCpt.SetCallBack(this);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    public void SetTitle()
    {
        if (tvTitle != null)
        {
            tvTitle.text =GameCommonInfo.GetUITextById(4015) +"(" + listPickCharacter.Count + "/" + pickCharacterMax + ")";
        }
    }

    #region 选择回调
    public void PickCharacter(ItemGameDialogPickCharacterCpt itemView,bool isPick, CharacterBean characterData)
    {
        if (isPick)
        {
            if (listPickCharacter.Count >= pickCharacterMax)
            {
                mToastManager.ToastHint(GameCommonInfo.GetUITextById(1052));
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