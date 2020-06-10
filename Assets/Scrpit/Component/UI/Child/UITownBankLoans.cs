using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class UITownBankLoans : BaseUIChildComponent<UITownBank>
{
    public GameObject objLoansContainer;
    public GameObject objLoansModel;

    public override void Open()
    {
        base.Open();
        CreateLoansData();
    }

    public void CreateLoansData()
    {
        CptUtil.RemoveChildsByActive(objLoansContainer);
        GameDataManager gameDataManager = uiComponent.uiGameManager.gameDataManager;
        InnAttributesBean innAttributes = gameDataManager.gameData.GetInnAttributesData();
        //获取客栈等级
        innAttributes.GetInnLevel(out int levelTitle, out int levelStar);
        //根据等级生成不同数量的贷款项目
        int loansNumber = (levelTitle == 0 ? 1 : (levelTitle - 1) * 5 + levelStar + 1);
        for (int i = 0; i < loansNumber; i++)
        {
            GameObject objLoans = Instantiate(objLoansContainer, objLoansModel);
            ItemTownBankLoansCpt itemLoans = objLoans.GetComponent<ItemTownBankLoansCpt>();

            UserLoansBean userLoans = new UserLoansBean(1000 * (i + 1), 0.15f + i * 0.02f, 10);
            itemLoans.SetData(userLoans);

            objLoans.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetDelay(i * 0.1f).SetEase(Ease.OutBack).From();
        }
    }

}