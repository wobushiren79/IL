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

    //厨师招聘列表
    public List<CharacterBean> listCandidateChef = new List<CharacterBean>();
    //账房招聘列表
    public List<CharacterBean> listCandidateAccounting = new List<CharacterBean>();
    //伙计招聘列表
    public List<CharacterBean> listCandidateWaiter = new List<CharacterBean>();
    //吆喝招聘列表
    public List<CharacterBean> listCandidateAccost = new List<CharacterBean>();
    //打手招聘列表
    public List<CharacterBean> listCandidateBeater = new List<CharacterBean>();
    //综合招聘列表
    public List<CharacterBean> listCandidateComplex = new List<CharacterBean>();

    private void Awake()
    {
        CreateCandidateData();
    }

    /// <summary>
    /// 创建应聘者数据
    /// </summary>
    public void CreateCandidateData()
    {
        Random.InitState(GameCommonInfo.RandomSeed);
        CharacterBodyManager characterBodyManager = GetUIMananger<UIGameManager>().characterBodyManager;
        for (int i = 0; i < 10; i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomData(WorkerEnum.Chef);
            CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            listCandidateChef.Add(characterData);
        }
        for (int i = 0; i < 10; i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomData(WorkerEnum.Accounting);
            CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            listCandidateAccounting.Add(characterData);
        }
        for (int i = 0; i < 10; i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomData(WorkerEnum.Waiter);
            CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            listCandidateWaiter.Add(characterData);
        }
        for (int i = 0; i < 10; i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomData(WorkerEnum.Accost);
            CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            listCandidateAccost.Add(characterData);
        }
        for (int i = 0; i < 10; i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomData(WorkerEnum.Beater);
            CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            listCandidateBeater.Add(characterData);
        }
        for (int i = 0; i < 10; i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomData(0);
            CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            listCandidateComplex.Add(characterData);
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

    public override void RefreshUI()
    {
        switch (remarkData)
        {
            case "chef":
                CreateRecruitmentList(listCandidateChef);
                break;
            case "accounting":
                CreateRecruitmentList(listCandidateAccounting);
                break;
            case "accost":
                CreateRecruitmentList(listCandidateAccost);
                break;
            case "waiter":
                CreateRecruitmentList(listCandidateWaiter);
                break;
            case "beater":
                CreateRecruitmentList(listCandidateBeater);
                break;
            case "complex":
                CreateRecruitmentList(listCandidateComplex);
                break;
        }
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
        switch (remarkData)
        {
            case "chef":
                listCandidateChef.Remove(characterData);
                break;
            case "accounting":
                listCandidateAccounting.Remove(characterData);
                break;
            case "accost":
                listCandidateAccost.Remove(characterData);
                break;
            case "waiter":
                listCandidateWaiter.Remove(characterData);
                break;
            case "beater":
                listCandidateBeater.Remove(characterData);
                break;
            case "complex":
                listCandidateComplex.Remove(characterData);
                break;
        }
        RefreshUI();
    }
}