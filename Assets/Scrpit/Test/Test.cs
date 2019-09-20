using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Test : MonoBehaviour {

    public float timeSclae = 1;

    private void Start()
    {

        StartCoroutine(TestTime());

    }

    private void Update()
    {
        Time.timeScale = timeSclae;
    }

    public IEnumerator TestTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            LogUtil.Log("over");
        }
    }
}
