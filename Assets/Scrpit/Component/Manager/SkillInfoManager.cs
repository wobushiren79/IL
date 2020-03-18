using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillInfoManager : BaseManager,ISkillInfoView
{
    protected SkillInfoController skillInfoController;
    protected ICallBack callBack;

    private void Awake()
    {
        skillInfoController = new SkillInfoController(this,this);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    public void GetSkillByIds(List<long> ids)
    {
        skillInfoController.GetSkillInfoByIds(ids);
    }
    public void GetSkillById(long id)
    {
        skillInfoController.GetSkillInfoByIds(new List<long>() { id });
    }
    #region 数据回调
    public void GetSkillInfoFail()
    {

    }

    public void GetSkillInfoSuccess(List<SkillInfoBean> listData)
    {
        if (callBack != null)
            callBack.GetSkillInfoSuccess(listData);
    }
    #endregion

    public interface ICallBack
    {
        void GetSkillInfoSuccess(List<SkillInfoBean> listData);
    }


}