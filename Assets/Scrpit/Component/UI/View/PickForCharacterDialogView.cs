using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickForCharacterDialogView : DialogView
{
    public GameObject objPickCharacterContainer;
    public GameObject objPickCharacterModel;

    public int pickCharacterMax = 0;
    public GameDataManager gameDataManager;
    public List<CharacterBean> listPickCharacter = new List<CharacterBean>();

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
        List<CharacterBean> listCharacter = gameDataManager.gameData.GetAllCharacterData();
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
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    public void SetTitle()
    {
        if (tvTitle != null)
        {
            tvTitle.text = "选择角色(" + listPickCharacter.Count + "/" + pickCharacterMax + ")";
        }
    }
}