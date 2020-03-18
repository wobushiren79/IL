using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class PickForSkillDialogView : DialogView, ItemDialogPickForSkillCpt.ICallBack
{
    public Text tvNull;
    public GameObject objSkillContainer;
    public GameObject objSkillModel;

    public List<SkillInfoBean> listSkillInfo;
    public Dictionary<long, int> listUsedData;

    public SkillInfoBean selectedSkill;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="listSkillInfo">拥有的技能</param>
    /// <param name="listUsedData">技能使用情况</param>
    public void SetData(List<SkillInfoBean> listSkillInfo, Dictionary<long, int> listUsedData)
    {
        this.listSkillInfo = listSkillInfo;
        this.listUsedData = listUsedData;
        CreateListSkill();
    }
    
    /// <summary>
    /// 获取选中的技能
    /// </summary>
    /// <param name="skillInfo"></param>
    public void GetSelectedSkill(out SkillInfoBean skillInfo)
    {
        skillInfo = selectedSkill;
    }

    /// <summary>
    /// 创建技能列表
    /// </summary>
    public void CreateListSkill()
    {
        tvNull.gameObject.SetActive(false);
        if (listSkillInfo == null || listSkillInfo.Count == 0)
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        for (int i = 0; i < listSkillInfo.Count; i++)
        {
            SkillInfoBean itemSkillInfo = listSkillInfo[i];
            CreateSkillItem(itemSkillInfo);
        }
    }

    /// <summary>
    /// 创建技能
    /// </summary>
    /// <param name="skillInfo"></param>
    public void CreateSkillItem(SkillInfoBean skillInfo)
    {
        int usedNumber = 0;
        if (listUsedData != null)
        {
            listUsedData.TryGetValue(skillInfo.id, out usedNumber);
        }
       
        GameObject objItem = Instantiate(objSkillContainer, objSkillModel);
        ItemDialogPickForSkillCpt itemSkill= objItem.GetComponent<ItemDialogPickForSkillCpt>();
        itemSkill.SetData(skillInfo, usedNumber);
        itemSkill.SetCallBack(this);
    }

    #region 选择回调
    public void SelectedSkill(SkillInfoBean skillInfo)
    {
        selectedSkill = skillInfo;
        SubmitOnClick();
    }
    #endregion
}