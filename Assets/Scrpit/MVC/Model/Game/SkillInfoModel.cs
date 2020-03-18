using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillInfoModel : BaseMVCModel
{
    protected SkillInfoService skillInfoService;

    public override void InitData()
    {
        skillInfoService = new SkillInfoService();
    }

    public List< SkillInfoBean> GetAllSkill()
    {
       return skillInfoService.QueryAllData();
    }

    public List<SkillInfoBean> GetSkillByIds(long[] ids)
    {
        return skillInfoService.QueryDataByIds(ids);
    }

    public List<SkillInfoBean> GetSkillByIds(List<long> ids)
    {
        return skillInfoService.QueryDataByIds(ids.ToArray());

    }
}