using UnityEngine;
using UnityEngine.UI;



using Pathfinding;
public class Test : BaseMonoBehaviour
{

    public ScrollGridVertical scrollGridVertical;

    private void Awake()
    {
        for (int i=0;i<50;i++)
        {
            int number = Random.Range(5, 1);
            LogUtil.Log("number:" + number);
        }
  
    }

    private void Update()
    {

    }

    public void CallBack(ScrollGridCell scrollGrid)
    {

    }


}
