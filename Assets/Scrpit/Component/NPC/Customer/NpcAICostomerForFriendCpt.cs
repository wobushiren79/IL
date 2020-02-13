using UnityEngine;
using UnityEditor;

public class NpcAICostomerForFriendCpt : NpcAICustomerCpt
{
    public override void IntentForLeave()
    {
        //添加好感
        if (orderForCustomer != null)
        {
            PraiseTypeEnum praiseType = orderForCustomer.innEvaluation.GetPraise();
            //小于0则不增加好感，大于0则增加数值好感
            int AddFavorability = (int)praiseType < 0 ? 0 : (int)praiseType;
            characterFavorabilityData.AddFavorability(AddFavorability);
            //弹出喜爱图标
            if (AddFavorability > 0)
                SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love);
        }
        base.IntentForLeave();
    }
}