using UnityEditor;
using UnityEngine;

public class MiniGameHandler : BaseHandler<MiniGameHandler, MiniGameManager>
{
    //下厨
    protected MiniGameCookingHandler _handlerForCooking;

    public MiniGameCookingHandler handlerForCooking
    {
        get
        {
            return GetHandler(_handlerForCooking);
        }
    }

    //弹幕
    protected MiniGameBarrageHandler _handlerForBarrage;

    public MiniGameBarrageHandler handlerForBarrage
    {
        get
        {
            return GetHandler(_handlerForBarrage);
        }
    }

    //算账
    protected MiniGameAccountHandler _handlerForAccount;

    public MiniGameAccountHandler handlerForAccount
    {
        get
        {
            return GetHandler(_handlerForAccount);
        }
    }

    //辩论
    protected MiniGameDebateHandler _handlerForDebate;

    public MiniGameDebateHandler handlerForDebate
    {
        get
        {
            return GetHandler(_handlerForDebate);
        }
    }

    //战斗
    protected MiniGameCombatHandler _handlerForCombat;

    public MiniGameCombatHandler handlerForCombat
    {
        get
        {
            return GetHandler(_handlerForCombat);
        }
    }

    //出生
    protected MiniGameBirthHandler _handlerForBirth;

    public MiniGameBirthHandler handlerForBirth
    {
        get
        {
            return GetHandler(_handlerForBirth);
        }
    }

    protected T GetHandler<T>(T handler) where T : BaseMonoBehaviour
    {
        if (handler == null)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(transform);
            handler = obj.AddComponent<T>();
            obj.name = handler.GetType().Name;
        }
        return handler;
    }
}