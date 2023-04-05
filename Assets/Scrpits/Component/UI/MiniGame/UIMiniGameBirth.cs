using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public partial class UIMiniGameBirth : BaseUIComponent
{
    public UIMiniGameBirthSperm modelForSperm;

    public override void Awake()
    {
        base.Awake();
        if (ui_BTFire)
            ui_BTFire.onClick.AddListener(OnClickForFire);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        ui_EnemyArea.StartCreateEnemy();
        RefreshUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        //设置开火数量
        MiniGameBirthBean miniGameBirthData = MiniGameHandler.Instance.handlerForBirth.miniGameData;
        SetFireNumber(miniGameBirthData.fireNumber, miniGameBirthData.winFireNumber);
        //设置怀孕进度
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData=  gameData.GetFamilyData();
        SetBirthPro(familyData.birthPro);
    }

    /// <summary>
    /// 设置开火数量
    /// </summary>
    /// <param name="currentNumber"></param>
    /// <param name="totalNumber"></param>
    public void SetFireNumber(int currentNumber,int totalNumber)
    {
        if (ui_TVFireNumber)
        {
            ui_TVFireNumber.text = currentNumber + "/" + totalNumber;
        }
    }

    /// <summary>
    /// 设置怀孕进度
    /// </summary>
    /// <param name="pro"></param>
    public void SetBirthPro(float pro)
    {
        if (ui_BirthPro)
        {
           ui_BirthPro.SetData(pro);
        }
    }

    /// <summary>
    /// 点击发射
    /// </summary>
    public void OnClickForFire()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
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
        //设置开火数量
        RefreshUI();
    }
}