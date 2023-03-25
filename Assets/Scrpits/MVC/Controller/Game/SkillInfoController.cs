using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SkillInfoController : BaseMVCController<SkillInfoModel, ISkillInfoView>
{
    public SkillInfoController(BaseMonoBehaviour content, ISkillInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    public void GetSkillInfoByIds(List<long> ids, Action<List<SkillInfoBean>> aciton)
    {
        List<SkillInfoBean> listData =  GetModel().GetSkillByIds(ids);
        GetView().GetSkillInfoSuccess(listData, aciton);
    }


    public void GetAllSkillInfo(Action<List<SkillInfoBean>> aciton)
    {
        List<SkillInfoBean> listData = GetModel().GetAllSkill();
        GetView().GetSkillInfoSuccess(listData, aciton);
    }

}