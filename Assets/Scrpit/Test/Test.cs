using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {

    private void Start()
    {
        Test1(200, 100);
    }

    public void Test1(int cook,int lucky)
    {
        float prefectFoodRate = cook * 0.0015f + lucky * 0.0005f;
        float goodFoodRate =  cook * 0.006f + lucky * 0.001f;
        float normalFoodRate = (1 - goodFoodRate - prefectFoodRate) * (0.6f + cook * 0.004f);
        float badFoodRate = 1- normalFoodRate- goodFoodRate- prefectFoodRate;
        LogUtil.Log("prefectFoodRate:"+ prefectFoodRate);
        LogUtil.Log("goodFoodRate:" + goodFoodRate);
        LogUtil.Log("normalFoodRate:" + normalFoodRate);
        LogUtil.Log("badFoodRate:" + badFoodRate);
    }


}
