using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ControlForWorkCpt : BaseControl, DialogView.IDialogCallBack
{
    //角色移动组建
    public CharacterMoveCpt cameraMove;

    protected GameDataManager gameDataManager;
    protected DialogManager dialogManager;
    protected AudioHandler audioHandler;

    //指针控制
    public CursorHandler cursorHandler;
    public List<Texture2D> listTexCursorDaze;

    //选中的NPC
    public BaseNpcAI selectNpc;
    //选中射线
    protected Ray selectRay;
    //选中生成的弹出框
    protected DialogView dialogSelectView;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        cursorHandler = Find<CursorHandler>(ImportantTypeEnum.CursorHandler);
    }

    private void Update()
    {
        HandleForSelect();

        HandleForMouseMove();
        HandleForControlMove();
        HandleForFollowMove();
        HandleForZoom();
    }

    private void FixedUpdate()
    {

    }

    private void OnDisable()
    {
        ClearSelect();
        SetCameraOrthographicSize();
    }

    /// <summary>
    /// 缩放
    /// </summary>
    public void HandleForZoom()
    {
        if (Input.GetButton(InputInfo.Zoom_In))
        {
            ZoomCamera( - 0.1f);
        }
        if (Input.GetButton(InputInfo.Zoom_Out))
        {
            ZoomCamera( + 0.1f);
        }
        if (Input.GetAxis(InputInfo.Zoom_Mouse) > 0)
        {
            ZoomCamera(- 0.2f);
        }
        if (Input.GetAxis(InputInfo.Zoom_Mouse) < 0)
        {
            ZoomCamera(+ 0.2f);
        }
    }

    protected void ZoomCamera(float addZoom)
    {
        float size = GetCameraOrthographicSize();
        size += addZoom;
        if (size < 7)
        {
            size = 7;
        }
        else if (size > 12)
        {
            size = 12;
        }
        SetCameraOrthographicSize(size);
    }

    /// <summary>
    /// 移动处理
    /// </summary>
    public void HandleForControlMove()
    {
        if (cameraMove == null)
            return;
        //float hMove = Input.GetAxis(InputInfo.Horizontal);
        //float vMove = Input.GetAxis(InputInfo.Vertical);
        float hMove = 0;
        float vMove = 0;
        if (Input.GetButton(InputInfo.Direction_Left))
        {
            hMove += -1;
        }
        if (Input.GetButton(InputInfo.Direction_Right))
        {
            hMove += 1;
        }
        if (Input.GetButton(InputInfo.Direction_Up))
        {
            vMove += 1;
        }
        if (Input.GetButton(InputInfo.Direction_Down))
        {
            vMove += -1;
        }
 

        if (hMove == 0 && vMove == 0)
        {
            //cameraMove.StopAnim();
  
        }
        else
        {
            cameraMove.MoveForUnscaled(hMove, vMove);
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
    /// 偷懒员工处理
    /// </summary>
    public NpcAIWorkerCpt HandleForDazeWorker()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hitAll = Physics2D.RaycastAll(mousePos, Vector2.zero);
        foreach (RaycastHit2D itemHit in hitAll)
        {
            if (itemHit.collider.transform.tag.Equals(TagInfo.Tag_NpcBody))
            {
                GameObject objSelect = itemHit.collider.gameObject;
                NpcAIWorkerCpt npcAIWorker = objSelect.GetComponentInParent<NpcAIWorkerCpt>();
                if (npcAIWorker)
                {
                    //如果在偷懒 则惊醒
                    if (npcAIWorker.GetWorkerStatus() == NpcAIWorkerCpt.WorkerIntentEnum.Daze)
                    {
                        cursorHandler.SetCursor( CursorHandler.CursorType.Knock);
                        return npcAIWorker;
                    }
                }
            }
        }
        cursorHandler.SetCursor(CursorHandler.CursorType.Def);
        return null;
    }

    /// <summary>
    /// 选中物体处理
    /// </summary>
    public void HandleForSelect()
    {
        //偷懒员工选中处理
        NpcAIWorkerCpt workerDaze = HandleForDazeWorker();

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
            //如有已经提前选中了偷懒的员工
            if (workerDaze != null)
            {
                audioHandler.PlaySound(AudioSoundEnum.Fight);
                workerDaze.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Surprise, 3);
                workerDaze.SetDazeBufferTime(10);
                workerDaze.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
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
                        dialogSelectView = dialogManager.CreateDialog(DialogEnum.SelectForNpc, this, dialogData);
                        ((SelectForNpcDialogView)dialogSelectView).SetData(selectNpc);
                        //如果是员工
                        if (selectNpc as NpcAIWorkerCpt)
                        {
                            NpcAIWorkerCpt npcAIWorker = selectNpc as NpcAIWorkerCpt;
                            npcAIWorker.SetDazeEnabled(false);
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
    /// 鼠标控制镜头移动
    /// </summary>
    public void HandleForMouseMove()
    {
        if (cameraMove == null)
            return;
        //如果有选中NPC查看则无法控制镜头移动
        if (dialogSelectView)
            return;
        base.HandleForMouseMove(out float moveX, out float moveY);
        cameraMove.MoveForUnscaled(moveX, moveY);
    }

    /// <summary>
    ///  清楚选择
    /// </summary>
    protected void ClearSelect()
    {
        //如果是员工
        if (selectNpc != null && selectNpc as NpcAIWorkerCpt)
        {
            NpcAIWorkerCpt npcAIWorker = selectNpc as NpcAIWorkerCpt;
            //恢复偷懒
            npcAIWorker.SetDazeEnabled(true);
        }
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
        if (position.x >= cameraMove.minMoveX
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
        ClearSelect();
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
        ClearSelect();
    }
    #endregion
}