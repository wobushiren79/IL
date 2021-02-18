using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIMainCreateAttributesChange : BaseUIChildComponent<UIMainCreate>, NumberChangeView.ICallBack
{

    public Text tvTitle;

    public NumberChangeView ncCook;
    public NumberChangeView ncSpeed;
    public NumberChangeView ncAccount;
    public NumberChangeView ncCharm;
    public NumberChangeView ncForce;
    public NumberChangeView ncLucky;

    public int pointsForMax;
    public int pointsForResidue;

    private void Start()
    {
        pointsForResidue = 6;
        pointsForMax = pointsForResidue + 6;


        ncCook.SetNumber(1);
        ncSpeed.SetNumber(1);
        ncAccount.SetNumber(1);
        ncCharm.SetNumber(1);
        ncForce.SetNumber(1);
        ncLucky.SetNumber(1);

        ncCook.SetCallBack(this);
        ncSpeed.SetCallBack(this);
        ncAccount.SetCallBack(this);
        ncCharm.SetCallBack(this);
        ncForce.SetCallBack(this);
        ncLucky.SetCallBack(this);

        SetTitle();
    }

    /// <summary>
    /// 随机速度
    /// </summary>
    public void RandomData()
    {
        ncCook.SetNumber(1);
        ncSpeed.SetNumber(1);
        ncAccount.SetNumber(1);
        ncCharm.SetNumber(1);
        ncForce.SetNumber(1);
        ncLucky.SetNumber(1);

        int randomPointS = pointsForResidue;
        for (int i = 0; i < randomPointS; i++)
        {
            int randAttr=  Random.Range(0,6);
            switch (randAttr)
            {
                case 0:
                    ncCook.SetNumber(ncCook.GetNumber() + 1);
                    break;
                case 1:
                    ncSpeed.SetNumber(ncSpeed.GetNumber() + 1);
                    break;
                case 2:
                    ncAccount.SetNumber(ncAccount.GetNumber() + 1);
                    break;
                case 3:
                    ncCharm.SetNumber(ncCharm.GetNumber() + 1);
                    break;
                case 4:
                    ncForce.SetNumber(ncForce.GetNumber() + 1);
                    break;
                case 5:
                    ncLucky.SetNumber(ncLucky.GetNumber() + 1);
                    break;
            }
        }
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    public void SetTitle()
    {
        if (tvTitle != null)
        {
            tvTitle.text = string.Format(TextHandler.Instance.manager.GetTextById(77), " "+ pointsForResidue );
        }
    }

    /// <summary>
    /// 获取属性点数
    /// </summary>
    /// <param name="cook"></param>
    /// <param name="speed"></param>
    /// <param name="account"></param>
    /// <param name="charm"></param>
    /// <param name="force"></param>
    /// <param name="lucky"></param>
    public void GetAttributesPoints(out int cook, out int speed, out int account, out int charm, out int force, out int lucky)
    {
        cook = ncCook.GetNumber();
        speed = ncSpeed.GetNumber();
        account = ncAccount.GetNumber();
        charm = ncCharm.GetNumber();
        force = ncForce.GetNumber();
        lucky = ncLucky.GetNumber();
    }

    /// <summary>
    /// 获取所有分配的点数
    /// </summary>
    /// <returns></returns>
    public int GetAllotPoints()
    {
        int allotPoints = 0;
        allotPoints += ncCook.GetNumber();
        allotPoints += ncSpeed.GetNumber();
        allotPoints += ncAccount.GetNumber();
        allotPoints += ncCharm.GetNumber();
        allotPoints += ncForce.GetNumber();
        allotPoints += ncLucky.GetNumber();
        return allotPoints;
    }

    #region 数字改变回调
    public void NumberChange(NumberChangeView view, int number)
    {
        int allotPoints = GetAllotPoints();
        if (allotPoints > pointsForMax)
        {
            view.SetNumber(view.GetNumber() - (allotPoints - pointsForMax));
        }
        else
        {
            pointsForResidue = pointsForMax - allotPoints;
        }
        SetTitle();
    }
    #endregion
}
