using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SkillInfoManager : BaseManager
{
    private void Awake()
    {
    }

    public void GetSkillByIds(List<long> ids, Action<List<SkillInfoBean>> action)
    {
        List<SkillInfoBean> listData = new List<SkillInfoBean>();
        if (ids != null)
        {
            foreach (long id in ids)
            {
                SkillInfoBean data = SkillInfoCfg.GetItemData(id);
                if (data != null)
                    listData.Add(data);
            }
        }
        action?.Invoke(listData);
    }

    public void GetSkillById(long id, Action<List<SkillInfoBean>> action)
    {
        GetSkillByIds(new List<long> { id }, action);
    }

    public void GetAllSkills(Action<List<SkillInfoBean>> action)
    {
        var dicData = SkillInfoCfg.GetAllData();
        List<SkillInfoBean> listData = new List<SkillInfoBean>();
        if (dicData != null)
        {
            foreach (var item in dicData)
                listData.Add(item.Value);
        }
        action?.Invoke(listData);
    }
}
