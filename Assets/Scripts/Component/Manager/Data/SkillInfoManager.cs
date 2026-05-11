using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SkillInfoManager : BaseManager
{
    protected SkillInfoService skillInfoService;

    private void Awake()
    {
        skillInfoService = new SkillInfoService();
    }

    public void GetSkillByIds(List<long> ids, Action<List<SkillInfoBean>> aciton)
    {
        List<SkillInfoBean> listData = skillInfoService.QueryDataByIds(TypeConversionUtil.ListToArray(ids));
        aciton?.Invoke(listData);
    }
    public void GetSkillById(long id, Action<List<SkillInfoBean>> aciton)
    {
        List<SkillInfoBean> listData = skillInfoService.QueryDataByIds(new long[] { id });
        aciton?.Invoke(listData);
    }

    public void GetAllSkills(Action<List<SkillInfoBean>> aciton)
    {
        List<SkillInfoBean> listData = skillInfoService.QueryAllData();
        aciton?.Invoke(listData);
    }
}