using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Test : MonoBehaviour {
    
    private void Start()
    {
        Vector3 testV = new Vector3(1, 1,0);
        Vector3 testV2 = new Vector3(-1, -1, 0);
        LogUtil.Log(Vector3.Cross(testV, testV2) + "");

    }

    private void Update()
    {
    }
    /// <summary>
        /// 获取某向量的垂直向量
        /// </summary>
    public static Vector3 GetVerticalDir(Vector3 _dir)
    {
        //（_dir.x,_dir.z）与（？，1）垂直，则_dir.x * ？ + _dir.z * 1 = 0
        if (_dir.z == 0)
        {
            return new Vector3(0, 0, -1);
        }
        else
        {
            return new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
        }
    }

}
