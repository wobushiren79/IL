using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
public class NpcAIWorkerForChefCpt : BaseMonoBehaviour
{
    private NpcAIWorkerCpt mNpcAIWorker;

    private void Start()
    {
        mNpcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }


    public enum ChefStatue
    {
        Idle,//空闲
        GoToCook,//做菜之前的路上
        Cooking,//做菜中
    }

    //灶台
    public BuildStoveCpt stoveCpt;
    //做的菜
    public MenuInfoBean menuInfo;
    //烹饪点
    public List<Vector3> cookPosition;
    //厨师状态
    public ChefStatue chefStatue;

    private void FixedUpdate()
    {
        if (CheckUtil.ListIsNull(cookPosition))
            return;
        switch (chefStatue)
        {
            case ChefStatue.GoToCook:
                if (Vector2.Distance(transform.position, cookPosition[0]) < 0.1f)
                {
                    chefStatue = ChefStatue.Cooking;
                    StartCoroutine(StartCook());
                }
                break;
            case ChefStatue.Cooking:
                break;
        }
    
    }

    public void SetCookData(BuildStoveCpt stoveCpt, MenuInfoBean menuInfo)
    {
        this.stoveCpt = stoveCpt;
        this.menuInfo = menuInfo;
        cookPosition = stoveCpt.GetCookPosition();
        if (CheckUtil.ListIsNull(cookPosition))
        {
            LogUtil.Log("厨师寻路失败-没有灶台烹饪点");
            return;
        }
        mNpcAIWorker.characterMoveCpt.SetDestination(cookPosition[0]);
        chefStatue = ChefStatue.GoToCook;
    }

    public IEnumerator StartCook()
    {
        yield return new WaitForSeconds(menuInfo.cook_time);
        stoveCpt.SetFood(menuInfo);
    }
}