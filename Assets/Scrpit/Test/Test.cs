using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Test : MonoBehaviour {

    public FoodForCoverCpt coverCpt;

    private void Start()
    {

 
    }

    private void Update()
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            coverCpt.ShowFood();
        }
    }

    public void TestM(int a)
    {
        LogUtil.Log("TestM"+a);
    }
}
