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