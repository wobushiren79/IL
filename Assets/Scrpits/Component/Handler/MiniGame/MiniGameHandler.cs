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
            _handlerForCooking = GetHandler(_handlerForCooking);
            return _handlerForCooking;
        }
    }

    //弹幕
    protected MiniGameBarrageHandler _handlerForBarrage;

    public MiniGameBarrageHandler handlerForBarrage
    {
        get
        {
            _handlerForBarrage = GetHandler(_handlerForBarrage);
            return _handlerForBarrage;
        }
    }

    //算账
    protected MiniGameAccountHandler _handlerForAccount;

    public MiniGameAccountHandler handlerForAccount
    {
        get
        {
            _handlerForAccount = GetHandler(_handlerForAccount);
            return _handlerForAccount;
        }
    }

    //辩论
    protected MiniGameDebateHandler _handlerForDebate;

    public MiniGameDebateHandler handlerForDebate
    {
        get
        {
            _handlerForDebate = GetHandler(_handlerForDebate);
            return _handlerForDebate;
        }
    }

    //战斗
    protected MiniGameCombatHandler _handlerForCombat;

    public MiniGameCombatHandler handlerForCombat
    {
        get
        {
            _handlerForCombat = GetHandler(_handlerForCombat);
            return _handlerForCombat;
        }
    }

    //出生
    protected MiniGameBirthHandler _handlerForBirth;

    public MiniGameBirthHandler handlerForBirth
    {
        get
        {
            _handlerForBirth = GetHandler(_handlerForBirth);
            return _handlerForBirth;
        }
    }

    protected GambleTrickyCupHandler _handlerForGambleCup;
    public GambleTrickyCupHandler handlerForGambleCup
    {
        get
        {
            _handlerForGambleCup = GetHandler(_handlerForGambleCup);
            return _handlerForGambleCup;
        }
    }

    protected GambleTrickySizeHandler _handlerForGambleSize;
    public GambleTrickySizeHandler handlerForGambleSize
    {
        get
        {
            _handlerForGambleSize = GetHandler(_handlerForGambleSize);
            return _handlerForGambleSize;
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