using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface ISkillInfoView
{
    void GetSkillInfoSuccess(List<SkillInfoBean> listData, Action<List<SkillInfoBean>> aciton);
    void GetSkillInfoFail();
}