using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillInfoController : BaseMVCController<SkillInfoModel, ISkillInfoView>
{
    public SkillInfoController(BaseMonoBehaviour content, ISkillInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    public void GetSkillInfoByIds(List<long> ids)
    {
        List<SkillInfoBean> listData=  GetModel().GetSkillByIds(ids);
        GetView().GetSkillInfoSuccess(listData);
    }


    public void GetAllSkillInfo()
    {
        List<SkillInfoBean> listData = GetModel().GetAllSkill();
        GetView().GetSkillInfoSuccess(listData);
    }

}