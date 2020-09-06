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

    protected GameTimeHandler gameTimeHandler;
    protected GameDataHandler gameDataHandler;
    protected LightHandler lightHandler;
    protected BaseSceneInit baseSceneInit;

    public override void Awake()
    {
        base.Awake();
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameDataHandler = Find<GameDataHandler>(ImportantTypeEnum.GameDataHandler);
        lightHandler = Find<LightHandler>(ImportantTypeEnum.LightHandler);
        baseSceneInit = Find<BaseSceneInit>(ImportantTypeEnum.Init);

        if (btSubNumber)
            btSubNumber.onClick.AddListener(OnClickForSub);
        if (btAddNumber)
            btAddNumber.onClick.AddListener(OnClickForAdd);
    }

    public override void Start()
    {
        base.Start();
        gameTimeHandler.SetTimeStatus(true);
        gameTimeHandler.SetTimeStop();
    }

    private void OnDisable()
    {
        gameTimeHandler.SetTimeStatus(false);
    }

    public void SetData()
    {
        jumpNumber = 1;
        SetJumpNumber(jumpNumber);
    }

    public void SetJumpNumber(int number)
    {
        gameTimeHandler.GetTime(out float hour, out float min);
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
        gameTimeHandler.GetTime(out float hour, out float min);
        if (tvCurrentTime)
        {
            tvCurrentTime.text = GameCommonInfo.GetUITextById(721) + "\n" + hour + ":" + (int)min;
        }
        if (tvJumpTime)
        {
            tvJumpTime.text = GameCommonInfo.GetUITextById(722) + "\n" + (jumpNumber + (int)hour) + ":" + (int)min;
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
        gameTimeHandler.GetTime(out float hour, out float min);
        gameTimeHandler.SetTime((jumpNumber + (int)hour), (int)min);
        lightHandler.CheckTime();
        baseSceneInit.RefreshScene();
        gameDataHandler.AddResearch(jumpNumber*60);
    }
}