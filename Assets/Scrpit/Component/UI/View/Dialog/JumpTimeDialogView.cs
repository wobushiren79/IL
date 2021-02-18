using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class JumpTimeDialogView : DialogView
{
    public Text tvCurrentTime;
    public Text tvJumpTime;
    public Text tvJumpNumber;

    public Button btSubNumber;
    public Button btAddNumber;

    public int jumpNumber = 1;


    public override void Awake()
    {
        base.Awake();

        if (btSubNumber)
            btSubNumber.onClick.AddListener(OnClickForSub);
        if (btAddNumber)
            btAddNumber.onClick.AddListener(OnClickForAdd);
    }

    public override void Start()
    {
        base.Start();

        GameControlHandler.Instance.StopControl();

        GameTimeHandler.Instance.SetTimeStatus(true);
        GameTimeHandler.Instance.SetTimeStop();
    }

    private void OnDisable()
    {
        GameControlHandler.Instance.RestoreControl();

        GameTimeHandler.Instance.SetTimeStatus(false);
    }

    public void SetData()
    {
        jumpNumber = 1;
        SetJumpNumber(jumpNumber);
    }

    public void SetJumpNumber(int number)
    {
        GameTimeHandler.Instance.GetTime(out float hour, out float min);
        jumpNumber = number;
        if (jumpNumber < 1)
        {
            jumpNumber = 1;
        }
        else if (jumpNumber + (int)hour > 23)
        {
            jumpNumber = 23 - (int)hour;
        }
        tvJumpNumber.text = jumpNumber + "";
        SetTime();
    }

    public void SetTime()
    {
        GameTimeHandler.Instance.GetTime(out float hour, out float min);
        if (tvCurrentTime)
        {
            tvCurrentTime.text = TextHandler.Instance.manager.GetTextById(721) + "\n" + hour + ":" + (int)min;
        }
        if (tvJumpTime)
        {
            tvJumpTime.text = TextHandler.Instance.manager.GetTextById(722) + "\n" + (jumpNumber + (int)hour) + ":" + (int)min;
        }
    }

    public void OnClickForSub()
    {
        SetJumpNumber(jumpNumber - 1);
    }

    public void OnClickForAdd()
    {
        SetJumpNumber(jumpNumber + 1);
    }

    public override void SubmitOnClick()
    {
        base.SubmitOnClick();
        GameTimeHandler.Instance.GetTime(out float hour, out float min);
        GameTimeHandler.Instance.SetTime((jumpNumber + (int)hour), (int)min);
        GameScenesHandler.Instance.manager.GetSceneInit<BaseSceneInit>().RefreshScene();
        GameDataHandler.Instance.AddTimeProcess(jumpNumber * 60);
    }
}