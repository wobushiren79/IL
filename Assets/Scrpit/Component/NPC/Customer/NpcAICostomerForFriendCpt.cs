using UnityEngine;
using UnityEditor;

public class NpcAICostomerForFriendCpt : NpcAICustomerForGuestTeamCpt
{
    public override void Awake()
    {
        base.Awake();
        customerType = CustomerTypeEnum.Friend;
    }

    public override void IntentForLeave()
    {
        //添加好感
        if (orderForCustomer != null)
        {
            PraiseTypeEnum praiseType = orderForCustomer.innEvaluation.GetPraise();
            if (praiseType == PraiseTypeEnum.Excited || praiseType == PraiseTypeEnum.Happy)
            {
                int AddFavorability = 0;
                if (praiseType == PraiseTypeEnum.Excited)
                {
                    AddFavorability = 1;
                }
                else if (praiseType == PraiseTypeEnum.Happy)
                {
                  
                }
                characterFavorabilityData.AddFavorability(AddFavorability);
                //弹出喜爱图标
                if (AddFavorability > 0)
                    SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love , 10);
            }
        }
        base.IntentForLeave();

        orderForCustomer = new OrderForCustomer(CustomerTypeEnum.Friend, this);
    }
}