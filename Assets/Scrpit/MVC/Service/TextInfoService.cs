using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class TextInfoService
{
    private string mTableName;
    private string mLeftDetailsTableName;

    public List<TextInfoBean> QueryDataByMarkId(TextEnum textEnum, long markId)
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
            default:
                return null;
        }
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "text_id" };
        string[] colName = new string[] { mTableName + ".mark_id" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { markId + "" };
        return SQliteHandle.LoadTableData<TextInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
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


    public void UpdateDataByMarkIdFor(TextEnum textEnum, long markId, List<TextInfoBean> listData)
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
            default:
                return;
        }
        //先删除旧的数据
        SQliteHandle.DeleteTableDataAndLeft(
            ProjectConfigInfo.DATA_BASE_INFO_NAME,
            mTableName,
            new string[] { "mark_id"},
            new string[] { "=" },
            new string[] { markId + "" }
            );
        //再存储新的
        //插入数据
        if (CheckUtil.ListIsNull(listData))
            return;
        foreach (TextInfoBean itemData in listData)
        {
            Dictionary<string, object> mapData = ReflexUtil.GetAllNameAndValue(itemData);
            List<string> listMainKeys = new List<string>();
            List<string> listMainValues = new List<string>();
            List<string> listLeftKeys = new List<string>();
            List<string> listLeftValues = new List<string>();
            foreach (var item in mapData)
            {
                string itemKey = item.Key;
                if (itemKey.Equals("name")|| itemKey.Equals("content") || itemKey.Equals("id"))
                {
                    if (itemKey.Equals("id"))
                    {
                        listLeftKeys.Add("text_id");
                    }
                    else
                    {
                        listLeftKeys.Add(item.Key);
                    }
                    listLeftValues.Add(Convert.ToString(item.Value));
                }
                if (itemKey.Equals("id") 
                    || itemKey.Equals("type") 
                    || itemKey.Equals("mark_id")
                    || itemKey.Equals("text_order")
                    || itemKey.Equals("next_order")
                    || itemKey.Equals("user_id")
                    || itemKey.Equals("wait_time")
                    || itemKey.Equals("select_type")
                    || itemKey.Equals("select_result")
                    || itemKey.Equals("add_favorability"))
                {
                    listMainKeys.Add(item.Key);
                    listMainValues.Add(Convert.ToString(item.Value));
                }
            }
            SQliteHandle.InsertValues(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,TypeConversionUtil.ListToArray(listMainKeys), TypeConversionUtil.ListToArray(listMainValues));
            SQliteHandle.InsertValues(ProjectConfigInfo.DATA_BASE_INFO_NAME, mLeftDetailsTableName, TypeConversionUtil.ListToArray(listLeftKeys), TypeConversionUtil.ListToArray(listLeftValues));
        }
    }
}