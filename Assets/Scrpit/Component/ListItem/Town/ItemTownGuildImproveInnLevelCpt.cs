using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownGuildImproveInnLevelCpt : BaseMonoBehaviour
{
    public Text tvTitle;
    public Image ivTitleIcon;

    public GameObject objPreContainer;
    public GameObject objPreModel;
    public GameObject objRewardContainer;
    public GameObject objRewardModel;

    public Button btSubmit;

    private void Start()
    {
        if (btSubmit!=null)
        {
            btSubmit.onClick.AddListener(OnClickSubmit);
        }
    }

    public void SetData(string innLevelStr,Sprite spInnLevel, StoreInfoBean storeInfo)
    {
        SetTitleName(innLevelStr);
        SetTitleIcon(spInnLevel);
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
    /// 处理前置数据
    /// </summary>
    /// <param name="data"></param>
    private void HandlePreData(string data)
    {

    }

    /// <summary>
    /// 处理奖励数据
    /// </summary>
    /// <param name="data"></param>
    private void HandleRewardData(string data)
    {

    }

    /// <summary>
    /// 提交晋升
    /// </summary>
    public void OnClickSubmit()
    {

    }
}