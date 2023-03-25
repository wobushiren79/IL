using UnityEditor;
using UnityEngine;

public class GameScenesManager : BaseManager
{
    protected BaseSceneInit _sceneInit;

    protected SceneBaseManager _sceneManager;

    protected SceneBaseHandler _sceneHandler;

    public T GetSceneInit<T>() where T : BaseSceneInit
    {
        if (_sceneInit == null)
        {
            _sceneInit = FindWithTag<T>(TagInfo.Tag_SceneInit);
        }
        return _sceneInit as T;
    }

    public T GetSceneManager<T>() where T : SceneBaseManager
    {
        if (_sceneManager == null)
        {
            _sceneManager = FindWithTag<T>(TagInfo.Tag_SceneManager);
        }
        return _sceneManager as T;
    }

    public T GetSceneHandler<T>() where T : SceneBaseHandler
    {
        if (_sceneHandler == null)
        {
            _sceneHandler = FindWithTag<T>(TagInfo.Tag_SceneHandler);
        }
        return _sceneHandler as T;
    }
}