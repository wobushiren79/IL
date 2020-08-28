using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillInfoHandler : BaseHandler, SkillInfoManager.ICallBack
{
    protected SkillInfoManager skillInfoManager;
    public List<SkillInfoBean> listSkillData;

    private void Awake()
    {
        skillInfoManager = Find<SkillInfoManager>(ImportantTypeEnum.SkillManager);
    }

    public void InitData()
    {
        skillInfoManager.SetCallBack(this);
        skillInfoManager.GetAllSkills();
    }

    public SkillInfoBean GetSkillById(long id)
    {
        for (int i = 0; i < listSkillData.Count; i++)
        {
            SkillInfoBean skillInfo = listSkillData[i];
            if (id== skillInfo.id)
            {
                return skillInfo;
            }
        }
        return null;
    }

    public List<SkillInfoBean> GetSkillByIds(List<long> ids)
    {
        List<SkillInfoBean> listData = new List<SkillInfoBean>();
        for (int i= 0;i< listSkillData.Count;i++)
        {
            SkillInfoBean skillInfo= listSkillData[i];
            if (ids.Contains( skillInfo.id))
            {
                listData.Add(skillInfo);
                continue;
            }
        }
        return listData;
    }


    public void GetSkillInfoSuccess(List<SkillInfoBean> listData)
    {
        listSkillData = listData;
        if (listSkillData == null)
            listSkillData = new List<SkillInfoBean>();
    }
}