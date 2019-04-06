using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcInfoManager : BaseManager,INpcInfoView
{
    public NpcInfoController npcInfoController;

    public List<NpcInfoBean> listNpcInfo;
    private void Awake()
    {
        npcInfoController = new NpcInfoController(this,this);
    }

    /// <summary>
    /// 获取一个随机NPC的数据
    /// </summary>
    public CharacterBean GetRandomCharacterData()
    {
        NpcInfoBean itemData= RandomUtil.GetRandomDataByList(listNpcInfo);
        CharacterBean characterData = new CharacterBean();
        return characterData;
    }

    #region 数据回调
    public void GetNpcInfoFail(int type)
    {
       
    }

    public void GetNpcInfoSuccess(int type, List<NpcInfoBean> listNpcInfo)
    {
        this.listNpcInfo = listNpcInfo;
    }
    #endregion
}