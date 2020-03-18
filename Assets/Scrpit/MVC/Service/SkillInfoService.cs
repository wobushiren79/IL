using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillInfoService : BaseMVCService
{
    public SkillInfoService() : base("skill_info", "skill_info_details_" + GameCommonInfo.GameConfig.language)
    {
    }

    public List<SkillInfoBean> QueryAllData()
    {
        return BaseQueryAllData<SkillInfoBean>();
    }

    public List<SkillInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<SkillInfoBean>("skill_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

}