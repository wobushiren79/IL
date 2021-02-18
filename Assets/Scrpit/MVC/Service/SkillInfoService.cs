using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillInfoService : BaseMVCService
{
    public SkillInfoService() : base("skill_info", "skill_info_details_" + GameDataHandler.Instance.manager.GetGameConfig().language)
    {

    }

    public List<SkillInfoBean> QueryAllData()
    {
        return BaseQueryAllData<SkillInfoBean>("skill_id");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<SkillInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<SkillInfoBean>("skill_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="menuInfo"></param>
    /// <returns></returns>
    public bool InsertData(SkillInfoBean skillInfo)
    {
        List<string> listLeftKey = new List<string>
        {
            "skill_id",
            "name",
            "content"
        };
        return BaseInsertDataWithLeft(skillInfo, listLeftKey);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteData(long id)
    {
        return BaseDeleteDataWithLeft("id", "skill_id", id + "");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="menuInfo"></param>
    /// <returns></returns>
    public bool Update(SkillInfoBean skillInfo)
    {
        if (DeleteData(skillInfo.id))
        {
            return InsertData(skillInfo);
        }
        return false;
    }
}