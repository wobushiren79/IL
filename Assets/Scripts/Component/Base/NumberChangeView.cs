using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class NumberChangeView : BaseMonoBehaviour
{
    public Button btAdd;
    public Button btSub;

    public Text tvNumber;

    //当前数字
    public int currentNumber = 0;
    public int minNumber = 1;
    protected ICallBack callBack;

    private void Start()
    {
        if (btAdd)
        {
            btAdd.onClick.AddListener(OnClickAdd);
        }
        if (btSub)
        {
            btSub.onClick.AddListener(OnClickSub);
        }
    }

    /// <summary>
    /// 设置最小数字
    /// </summary>
    public void SetMinNumber(int minNumber)
    {
        this.minNumber = minNumber;
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 点击增加
    /// </summary>
    public void OnClickAdd()
    {
        SetNumber(currentNumber + 1);
    }

    /// <summary>
    /// 点击减少
    /// </summary>
    public void OnClickSub()
    {
        SetNumber(currentNumber - 1);
    }

    /// <summary>
    /// 设置数字
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(int number)
    {
        if (number < minNumber)
        {
            return;
        }
        currentNumber = number;
        if (tvNumber != null)
            tvNumber.text = currentNumber + "";
        if (callBack != null)
            callBack.NumberChange(this, number);
    }

    /// <summary>
    /// 获取数字
    /// </summary>
    /// <returns></returns>
    public int GetNumber()
    {
        return currentNumber;
    }

    public interface ICallBack
    {
        void NumberChange(NumberChangeView view, int number);
    }
}