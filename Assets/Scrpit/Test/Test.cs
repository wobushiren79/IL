using UnityEngine;
using UnityEngine.UI;



using Pathfinding;
using System;
using System.Threading.Tasks;

public class Test : BaseMonoBehaviour
{



    private void Awake()
    {
        TestAsync();
        LogUtil.Log("Awake");
    }

    public async void TestAsync()
    {
        LogUtil.Log("TestAsync");
        string logReurn = await TestAsync2();
        LogUtil.Log("logReurn");
        await Task.Delay(TimeSpan.FromSeconds(5));
        LogUtil.Log("Complete");
    }

    public async Task<string> TestAsync2()
    {
        LogUtil.Log("TestAsync2");
        await Task.Delay(TimeSpan.FromSeconds(2));
        LogUtil.Log("Complete2");
        return "return";
    }
}
