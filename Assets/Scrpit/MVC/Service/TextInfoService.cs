using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class TextInfoService : BaseMVCService<TextInfoBean>
{
    public TextInfoService() : base("", "")
    {

    }

    /// <summary>
    /// 初始化表名
    /// </summary>
    /// <param name="textEnum"></param>
    private void InitTableByTextType(TextEnum textEnum)
    {
        switch (textEnum)
        {
            case TextEnum.Look:
                mTableName = "text_look";
                mLeftDetailsTableName = "text_look_details_" + GameCommonInfo.GameConfig.language;
                break;
            case TextEnum.Talk:
                mTableName = "text_talk";
                mLeftDetailsTableName = "text_talk_details_" + GameCommonInfo.GameConfig.language;
                break;
            case TextEnum.Story:
                mTableName = "text_story";
                mLeftDetailsTableName = "text_story_details_" + GameCommonInfo.GameConfig.language;
                break;
        }
    }

    /// <summary>
    /// 通过标记ID查询对话
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="markId"></param>
    /// <returns></returns>
    public List<TextInfoBean> QueryDataByMarkId(TextEnum textEnum, long markId)
    {
        InitTableByTextType(textEnum);
        return BaseQueryData("text_id", mTableName + ".mark_id", markId + "");
    }

    /// <summary>
    /// 通过用户ID查询对话
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<TextInfoBean> QueryDataByUserId(TextEnum textEnum, long userId)
    {
        InitTableByTextType(textEnum);
        return BaseQueryData("text_id", mTableName + ".user_id", userId + "");
    }

    /// <summary>
    /// 通过markid更新数据
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="markId"></param>
    /// <param name="listData"></param>
    public void UpdateDataByMarkId(TextEnum textEnum, long markId, List<TextInfoBean> listData)
    {
        InitTableByTextType(textEnum);

        //先删除旧的数据
        BaseDeleteData("mark_id", markId + "");
        //再存储新的
        //插入数据
        if (CheckUtil.ListIsNull(listData))
            return;
        List<string> leftName = new List<string>();
        leftName.Add("name");
        leftName.Add("content");
        leftName.Add("text_id");
        foreach (TextInfoBean itemData in listData)
        {
            BaseInsertDataWithLeft(itemData, leftName);
        }
    }

    /// <summary>
    /// 通过ID更新数据
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="id"></param>
    /// <param name="textData"></param>
    public void UpdateDataById(TextEnum textEnum, long id, TextInfoBean textData)
    {
        InitTableByTextType(textEnum);
        //先删除旧的数据
        BaseDeleteDataById(id);
        //再存储新的
        //插入数据
        if (textData==null)
            return;
        List<string> leftName = new List<string>();
        leftName.Add("name");
        leftName.Add("content");
        leftName.Add("text_id");
        BaseInsertDataWithLeft(textData, leftName);
    }


    public List<TextInfoBean> QueryDataForFirstOrderByFavorability(long characterId, int favorability)
    {
        mTableName = "text_talk";
        mLeftDetailsTableName = "text_talk_details_" + GameCommonInfo.GameConfig.language;
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "text_id" };
        string[] colName = new string[] { mTableName + ".user_id", mTableName + ".text_order", mTableName + ".condition_min_favorability", mTableName + ".condition_max_favorability" };
        string[] operations = new string[] { "=", "=", "<=", ">" };
        string[] colValue = new string[] { characterId + "", "1", favorability + "", favorability + "" };
        return SQliteHandle.LoadTableData<TextInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }


    public List<TextInfoBean> QueryDataForFirstOrderByFirstMeet(long characterId)
    {
        mTableName = "text_talk";
        mLeftDetailsTableName = "text_talk_details_" + GameCommonInfo.GameConfig.language;
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "text_id" };
        //string[] colName = new string[] { mTableName + ".user_id", mTableName + ".\"order\"", mTableName + ".condition_first_meet" };
        string[] colName = new string[] { mTableName + ".user_id", mTableName + ".text_order", mTableName + ".condition_first_meet" };
        string[] operations = new string[] { "=", "=", "=" };
        string[] colValue = new string[] { characterId + "", "1", "1" };
        return SQliteHandle.LoadTableData<TextInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}