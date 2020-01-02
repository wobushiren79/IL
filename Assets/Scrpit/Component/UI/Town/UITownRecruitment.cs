using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITownRecruitment : UIBaseOne
{
    //人员数量
    public Text tvNumber;

    public GameObject objCandidateContent;
    public GameObject objCandidateModel;

    private void Awake()
    {
        if (GameCommonInfo.DailyLimitData.listRecruitmentCharacter==null)
        {
            CreateCandidateData();
        }
        else
        {
            CreateRecruitmentList(GameCommonInfo.DailyLimitData.listRecruitmentCharacter);
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        CreateRecruitmentList(GameCommonInfo.DailyLimitData.listRecruitmentCharacter);
    }

    /// <summary>
    /// 创建应聘者数据
    /// </summary>
    public void CreateCandidateData()
    {
        GameCommonInfo.InitRandomSeed();
        CharacterBodyManager characterBodyManager = GetUIMananger<UIGameManager>().characterBodyManager;
        for (int i = 0; i < 10; i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomWorkerData(characterBodyManager);
            GameCommonInfo.DailyLimitData.AddRecruitmentCharacter(characterData);
        }
    }

    private new void Update()
    {
        base.Update();
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        if (gameDataManager != null && tvNumber != null)
        {
            tvNumber.text = gameDataManager.gameData.listWorkerCharacter.Count + "/" + gameDataManager.gameData.workerNumberLimit;
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    /// <summary>
    /// 创建列表数据
    /// </summary>
    /// <param name="listData"></param>
    public void CreateRecruitmentList(List<CharacterBean> listData)
    {
        CptUtil.RemoveChildsByActive(objCandidateContent.transform);
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean itemData = listData[i];
            GameObject objCandidate = Instantiate(objCandidateContent, objCandidateModel);
            ItemTownCandidateCpt itemCpt = objCandidate.GetComponent<ItemTownCandidateCpt>();
            itemCpt.SetData(itemData);
        }
    }


    /// <summary>
    /// 移除数据
    /// </summary>
    /// <param name="characterData"></param>
    public void RemoveCandidate(CharacterBean characterData)
    {
        GameCommonInfo.DailyLimitData.RemoveRecruitmentCharacter(characterData);
        RefreshUI();
    }
}