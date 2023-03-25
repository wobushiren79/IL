using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;
public class UIMiniGameBirthEnemyArea : BaseUIView
{
    public List<GameObject> listObjEnemyModel = new List<GameObject>();

    public List<GameObject> listEnemy = new List<GameObject>();

    public bool isCreateEnemy = false;

    public void StartCreateEnemy()
    {
        StartCoroutine(CoroutineForCreateEnemy());
    }

    public void StopCreateEnemy()
    {
        isCreateEnemy = false;
        StopAllCoroutines();
        listEnemy.Clear();
    }

    public IEnumerator CoroutineForCreateEnemy()
    {
        isCreateEnemy = true;
        while (isCreateEnemy)
        {       
            //获取敌人速度
            float enmeyBuildInterval = MiniGameHandler.Instance.handlerForBirth.miniGameData.enmeyBuildInterval;
            yield return new WaitForSeconds(enmeyBuildInterval);
            CreateEnemyItem();
        }
    }

    public void CreateEnemyItem()
    {
        GameObject objModel = RandomUtil.GetRandomDataByList(listObjEnemyModel);
        GameObject objEnemy = Instantiate(gameObject, objModel);
        UIMiniGameBirthEnemy enemy = objEnemy.GetComponent<UIMiniGameBirthEnemy>();
        GetRandomEnemyStartAndEndPosition(out Vector3 startPosition, out Vector3 endPosition);

        //获取敌人速度
        float speedEnemy = MiniGameHandler.Instance.handlerForBirth.miniGameData.enmeySpeed;
        //设置敌人输入
        enemy.SetData(startPosition, endPosition, speedEnemy);
    }

    protected void GetRandomEnemyStartAndEndPosition(out Vector3 startPosition, out Vector3 endPosition)
    {
        RectTransform rtf = (RectTransform)transform;
        float width = rtf.rect.width;
        float height = rtf.rect.height;
        Vector3 randomPostion1 = new Vector3
            (rtf.anchoredPosition.x - width / 2f, Random.Range(rtf.anchoredPosition.y - height / 2f, rtf.anchoredPosition.y + height / 2f), 0);
        Vector3 randomPostion2 = new Vector3
            (rtf.anchoredPosition.x + width / 2f, Random.Range(rtf.anchoredPosition.y - height / 2f, rtf.anchoredPosition.y + height / 2f), 0);
        int randomDirection = Random.Range(0, 2);
        if (randomDirection == 0)
        {
            startPosition = randomPostion1;
            endPosition = randomPostion2;
        }
        else
        {
            startPosition = randomPostion2;
            endPosition = randomPostion1;
        }
    }

}