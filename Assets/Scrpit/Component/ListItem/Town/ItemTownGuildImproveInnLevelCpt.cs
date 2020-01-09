using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownGuildImproveInnLevelCpt : BaseMonoBehaviour
{
    public Text tvTitle;
    public Image ivTitleIcon;

    public GameObject objPreContainer;
    public GameObject objPreModel;
    public GameObject objRewardContainer;
    public GameObject objRewardModel;

    public Button btSubmit;

    protected IconDataManager iconDataManager;

    private void Awake()
    {
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }

    private void Start()
    {
        if (btSubmit != null)
        {
            btSubmit.onClick.AddListener(OnClickSubmit);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="innLevelStr"></param>
    /// <param name="spInnLevel"></param>
    /// <param name="storeInfo"></param>
    public void SetData(string innLevelStr, Sprite spInnLevel, StoreInfoBean storeInfo)
    {
        SetTitleName(innLevelStr);
        SetTitleIcon(spInnLevel);
        CreatePreDataItem(storeInfo.pre_data);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="name"></param>
    public void SetTitleName(string name)
    {
        if (tvTitle == null)
            return;
        tvTitle.text = "晋升：" + name;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spInnLevel"></param>
    public void SetTitleIcon(Sprite spInnLevel)
    {
        if (ivTitleIcon == null || spInnLevel == null)
            return;
        ivTitleIcon.sprite = spInnLevel;
    }

    /// <summary>
    /// 创建前置数据
    /// </summary>
    /// <param name="preData"></param>
    public void CreatePreDataItem(string preData)
    {
        Dictionary<PreTypeEnum, string> listPreData = PreTypeEnumTools.GetPreData(preData);
        foreach (var itemData in listPreData)
        {
            GameObject objPre = Instantiate(objPreContainer, objPreModel);
            //设置描述
            string preDes=  PreTypeEnumTools.GetPreDescribe(itemData.Key,itemData.Value);
            Text tvContent = CptUtil.GetCptInChildrenByName<Text>(objPre, "Text");
            tvContent.text = preDes;
            //设置图标
            Sprite spIcon = PreTypeEnumTools.GetPreSprite(itemData.Key, iconDataManager);
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objPre, "Icon");
            ivIcon.sprite = spIcon;
        }
        GameUtil.RefreshRectViewHight((RectTransform)objPreContainer.transform, true);
        GameUtil.RefreshRectViewHight((RectTransform)transform,true);
    }

    /// <summary>
    /// 提交晋升
    /// </summary>
    public void OnClickSubmit()
    {

    }
}