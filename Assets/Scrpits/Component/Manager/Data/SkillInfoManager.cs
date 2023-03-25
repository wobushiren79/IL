using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SkillInfoManager : BaseManager, ISkillInfoView
{
    protected SkillInfoController skillInfoController;

    private void Awake()
    {
        skillInfoController = new SkillInfoController(this, this);
    }

    //public SkillInfoBean GetSkillById(long id)
    //{
    //    for (int i = 0; i < listSkillData.Count; i++)
    //    {
    //        SkillInfoBean skillInfo = listSkillData[i];
    //        if (id == skillInfo.id)
    //        {
    //            return skillInfo;
    //        }
    //    }
    //    return null;
    //}

    //public List<SkillInfoBean> GetSkillByIds(List<long> ids)
    //{
    //    List<SkillInfoBean> listData = new List<SkillInfoBean>();
    //    for (int i = 0; i < listSkillData.Count; i++)
    //    {
    //        SkillInfoBean skillInfo = listSkillData[i];
    //        if (ids.Contains(skillInfo.id))
    //        {
    //            listData.Add(skillInfo);
    //            continue;
    //        }
    //    }
    //    return listData;
    //}


    public void GetSkillByIds(List<long> ids, Action<List<SkillInfoBean>> aciton)
    {
        skillInfoController.GetSkillInfoByIds(ids, aciton);
    }
    public void GetSkillById(long id, Action<List<SkillInfoBean>> aciton)
    {
        skillInfoController.GetSkillInfoByIds(new List<long>() { id }, aciton);
    }

    public void GetAllSkills(Action<List<SkillInfoBean>> aciton)
    {
        skillInfoController.GetAllSkillInfo(aciton);
    }

    #region 数据回调
    public void GetSkillInfoFail()
    {

    }

    public void GetSkillInfoSuccess(List<SkillInfoBean> listData, Action<List<SkillInfoBean>> aciton)
    {
        aciton?.Invoke(listData);
    }
    #endregion


}