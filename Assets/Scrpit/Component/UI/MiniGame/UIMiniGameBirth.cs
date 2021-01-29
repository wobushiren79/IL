using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMiniGameBirth : BaseUIComponent
{
    public RectTransform ui_Start;
    public RectTransform ui_End;
    public RectTransform ui_SpermContainer;
    public UIMiniGameBirthEnemyArea ui_EnemyArea;
    public UIMiniGameBirthEgg ui_Egg;

    public Button ui_Fire;

    public UIMiniGameBirthSperm modelForSperm;

    public override void Awake()
    {
        base.Awake();
        if (ui_Fire)
            ui_Fire.onClick.AddListener(OnClickForFire);
    }

    /// <summary>
    /// 点击发射
    /// </summary>
    public void OnClickForFire()
    {
        //获取发射数据
        MiniGameHandler.Instance.handlerForBirth.FireSperm(out MiniGameBirthSpermBean spermData);
        if (spermData == null)
            return;
        RectTransform rtfSperm = (RectTransform)Instantiate(modelForSperm.transform, ui_SpermContainer);
        rtfSperm.gameObject.SetActive(true);
        UIMiniGameBirthSperm sperm = rtfSperm.GetComponent<UIMiniGameBirthSperm>();
        spermData.positionStart = ui_Start.position;
        spermData.positionEnd = ui_End.position;
        sperm.InitData(spermData);
    }
}