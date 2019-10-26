using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Test : MonoBehaviour {
    
    private void Start()
    {

        ReflexUtil.GetInvokeMethod(this,"TestM");

    }


    public void TestM(int a)
    {
        LogUtil.Log("TestM"+a);
    }
}
