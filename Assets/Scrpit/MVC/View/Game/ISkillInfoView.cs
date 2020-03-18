using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface ISkillInfoView 
{
     void GetSkillInfoSuccess(List<SkillInfoBean> listData);
     void GetSkillInfoFail();
}