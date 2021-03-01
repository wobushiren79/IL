using System.Collections;
using UnityEditor;
using UnityEngine;

public class NpcAIFamilyCpt : BaseNpcAI
{
    public enum FamilyIntentEnum
    {
        Idle,
    }
    public FamilyIntentEnum familyIntent;

    public override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }

    public void SetIntent(FamilyIntentEnum familyIntent)
    {
        this.familyIntent = familyIntent;
        switch (familyIntent)
        {
            case FamilyIntentEnum.Idle:
                SetIntentIdle();
                break;
        }
    }


    protected void SetIntentIdle()
    {
        //闲逛 有问题
        Vector3 movePosition = InnHandler.Instance.GetRandomInnPositon();
        bool canGo = CheckUtil.CheckPath(transform.position, movePosition);
        if (canGo)
            SetCharacterMove(movePosition);
    }

    /// <summary>
    /// 协程 闲置
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForIdle()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        SetIntent(FamilyIntentEnum.Idle);
    }

}