using UnityEditor;
using UnityEngine;

public class StoryInfoHandler : BaseHandler<StoryInfoHandler, StoryInfoManager>
{
    protected StoryBuilder _builderForStory;

    public StoryBuilder builderForStory
    {
        get
        {
            if (_builderForStory == null)
            {
                GameObject objBuild = new GameObject("BuilderForStory");
                objBuild.transform.SetParent(transform);
                _builderForStory = objBuild.AddComponent<StoryBuilder>();
            }
            return _builderForStory;
        }
    }
}