using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SelectForNpcDialogView : DialogView
{
    public GameObject objNpcName;
    public Text tvNpcName;
    public GameObject objNpcType;
    public Text tvNpcType;


    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="baseNpc"></param>
    public void SetData(BaseNpcAI baseNpc)
    {
        CharacterBean characterData = baseNpc.characterData;
        if (characterData == null)
            return;
        SetName(characterData.baseInfo.name);

        if (baseNpc as NpcAICustomerCpt)
        {
            SetDataForCustomer((NpcAICustomerCpt)baseNpc);
        }
        else if (baseNpc as NpcAIWorkerCpt)
        {
            SetDataForWork((NpcAIWorkerCpt)baseNpc);
        }
        else if (baseNpc as NpcAIRascalCpt)
        {
            SetDataForRascal((NpcAIRascalCpt)baseNpc);
        }

    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (CheckUtil.StringIsNull(name))
        {
            objNpcName.SetActive(false);
        }
        else
        {
            objNpcName.SetActive(true);
            tvNpcName.text = GameCommonInfo.GetUITextById(61) + ":" + name;
        }
    }

    /// <summary>
    /// 设置类型
    /// </summary>
    /// <param name="type"></param>
    public void SetType(string type)
    {
        tvNpcType.text = type;
    }

    /// <summary>
    /// 设置顾客数据
    /// </summary>
    /// <param name="npcAICustomer"></param>
    public void SetDataForCustomer(NpcAICustomerCpt npcAICustomer)
    {
        SetType(GameCommonInfo.GetUITextById(60));
    }

    /// <summary>
    /// 设置工作者数据
    /// </summary>
    /// <param name="npcAIWorker"></param>
    public void SetDataForWork(NpcAIWorkerCpt npcAIWorker)
    {
        SetType(GameCommonInfo.GetUITextById(63));
    }

    /// <summary>
    /// 设置捣乱者数据
    /// </summary>
    /// <param name="npcAIRascal"></param>
    public void SetDataForRascal(NpcAIRascalCpt npcAIRascal)
    {
        SetType(GameCommonInfo.GetUITextById(59));
    }
}