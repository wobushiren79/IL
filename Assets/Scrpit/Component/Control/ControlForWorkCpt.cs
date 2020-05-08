using UnityEngine;
using UnityEditor;

public class ControlForWorkCpt : BaseControl,DialogView.IDialogCallBack
{
    //角色移动组建
    public CharacterMoveCpt cameraMove;

    protected GameDataManager gameDataManager;
    protected DialogManager dialogManager;
    protected AudioHandler audioHandler;

    //选中的NPC
    public BaseNpcAI selectNpc;
    //选中射线
    protected Ray selectRay;
    //选中生成的弹出框
    protected DialogView dialogSelectView;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        dialogManager= Find<DialogManager>(ImportantTypeEnum.DialogManager);
        audioHandler= Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

    private void Update()
    {
        HandleForSelect();
    }

    private void FixedUpdate()
    {
        HandleForControlMove();
        HandleForFollowMove();
    }

    private void OnDisable()
    {
        ClearSelect();
    }

    /// <summary>
    /// 移动处理
    /// </summary>
    public void HandleForControlMove()
    {
        if (cameraMove == null)
            return;
        float hMove = Input.GetAxis(InputInfo.Horizontal);
        float vMove = Input.GetAxis(InputInfo.Vertical);
        if (hMove == 0 && vMove == 0)
        {
            // cameraMove.StopAnim();
        }
        else
        {
            cameraMove.Move(hMove, vMove);
        }
    }

    /// <summary>
    /// 跟随移动处理
    /// </summary>
    public void HandleForFollowMove()
    {
        if (dialogSelectView)
        {
            SetFollowPosition(selectNpc.transform.position);
            //如果超出边界则取消选择
            if (!CheckIsInBorder(selectNpc.transform.position))
            {
                ClearSelect();
            }      
        }
    }

    /// <summary>
    /// 选中物体处理
    /// </summary>
    public void HandleForSelect()
    {
        if (Input.GetButtonDown(InputInfo.Confirm))
        {
            if (dialogSelectView != null)
                return;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //如果超出边界则不选择
            if (!CheckIsInBorder(mousePos))
            {
                return;
            }

            RaycastHit2D[] hitAll = Physics2D.RaycastAll(mousePos, Vector2.zero);
            foreach (RaycastHit2D itemHit in hitAll)
            {
                if (itemHit.collider.transform.tag.Equals(TagInfo.Tag_NpcBody))
                {
                    GameObject objSelect = itemHit.collider.gameObject;
                    selectNpc = objSelect.GetComponentInParent<BaseNpcAI>();
                    if (selectNpc)
                    {
                        audioHandler.PlaySound(AudioSoundEnum.ButtonForShow);
                        DialogBean dialogData = new DialogBean();
                        dialogSelectView = dialogManager.CreateDialog(DialogEnum.SelectForNpc, this,dialogData);
                        ((SelectForNpcDialogView)dialogSelectView).SetData(selectNpc);
                        //如果是员工
                        if(selectNpc as NpcAIWorkerCpt)
                        {

                        }
                        return;
                    }
                }
            }
            selectNpc = null;
        }
    }

    public override void StartControl()
    {
        base.StartControl();
        cameraFollowObj.transform.position = new Vector3(5, 5);
        cameraMove.minMoveX = -5;
        cameraMove.maxMoveX = gameDataManager.gameData.GetInnBuildData().innWidth + 5;
        cameraMove.minMoveY = -5;
        cameraMove.maxMoveY = gameDataManager.gameData.GetInnBuildData().innHeight + 5;
    }

    /// <summary>
    ///  清楚选择
    /// </summary>
    protected void ClearSelect()
    {
        selectNpc = null;
        if (dialogSelectView != null)
        {
            Destroy(dialogSelectView.gameObject);
            dialogSelectView = null;
        }   
    }

    /// <summary>
    /// 检测是否在边界内
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool CheckIsInBorder(Vector3 position)
    {
        if(position.x >= cameraMove.minMoveX
            && position.x <= cameraMove.maxMoveX
            && position.y >= cameraMove.minMoveY
            && position.y <= cameraMove.maxMoveY)
        {
            return true;
        }
        return false;
    }

    #region 选中弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {

    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}