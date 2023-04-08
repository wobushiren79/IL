using System.Collections;
using UnityEngine;
using DG.Tweening;
public class AnimForMainStartTitle : BaseMonoBehaviour
{
    protected RectTransform rtfContent;
    protected float timeForRoate = 10;
    private void Awake()
    {
        rtfContent = GetComponent<RectTransform>();
        timeForRoate = Random.Range(5, 20);
    }

    public void Start()
    {
        StartAnim();
    }

    private void Update()
    {
        timeForRoate -= Time.deltaTime;
        if (timeForRoate <= 0)
        {
            int tempRandomAngle = Random.Range(0, 2) == 0 ? 360 : -360;
            rtfContent.DORotate(new Vector3(0, 0, tempRandomAngle), Random.Range(1f, 2f), RotateMode.FastBeyond360);
            timeForRoate = Random.Range(10f, 20f);
        }
    }

    public void StartAnim()
    {
        if (rtfContent == null)
            return;
        rtfContent.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 5).SetDelay(Random.Range(1f, 5f)).SetLoops(-1, LoopType.Yoyo);
    }

}