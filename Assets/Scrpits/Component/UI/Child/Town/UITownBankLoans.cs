﻿using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class UITownBankLoans : BaseUIView
{
    public GameObject objLoansContainer;
    public GameObject objLoansModel;

    public override void OpenUI()
    {
        base.OpenUI();
        CreateLoansData();
    }

    public void CreateLoansData()
    {
        CptUtil.RemoveChildsByActive(objLoansContainer);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnAttributesBean innAttributes = gameData.GetInnAttributesData();
        //获取客栈等级
        innAttributes.GetInnLevel(out int levelTitle, out int levelStar);
        //根据等级生成不同数量的贷款项目
        int loansNumber = (levelTitle == 0 ? 1 : (levelTitle - 1) * 5 + levelStar + 1);
        for (int i = 0; i < loansNumber; i++)
        {
            GameObject objLoans = Instantiate(objLoansContainer, objLoansModel);
            ItemTownBankLoansCpt itemLoans = objLoans.GetComponent<ItemTownBankLoansCpt>();

            UserLoansBean userLoans = new UserLoansBean(2000 * (i + 1) * 2, 0.15f + i * 0.02f, 10);
            itemLoans.SetData(userLoans);

            objLoans.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetDelay(i * 0.1f).SetEase(Ease.OutBack).From();
        }
    }

}