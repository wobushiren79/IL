using UnityEngine;
using UnityEditor;

public class NpcPasserHandler : BaseHandler
{
    [Header("自动初始化数据")]
    public NpcPasserBuilder npcPasserBuilder;

    private void Awake()
    {
        npcPasserBuilder = Find<NpcPasserBuilder>(ImportantTypeEnum.NpcBuilder);
    }

}