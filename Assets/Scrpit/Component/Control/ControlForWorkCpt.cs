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
    public BuildBedCpt selectBed;
    //选中射线
    protected Ray selectRay;
    //选中生成的弹出框
    protected DialogView dialogSelectView;

    public int layer = 0;

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
    }

    private void FixedUpdate()
    {
        HandleForZoom();
    }

    private void OnDisable()
    {
        ClearSelect();

        if (gameObject.activeSelf)
        {
            //如果只是enable=false 

        }
        else
        {
            //如果自身都不用了 则同时也还原镜头
            SetCameraOrthographicSize();
        }
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
            if (selectNpc == null && selectBed == null)
            {
                ClearSelect();
            }
            else
            {
                if (selectNpc != null)
                {
                    if (selectNpc.transform.position.y >= 90)
                    {
                        SetLayer(2);
                    }
                    else
                    {
                        SetLayer(1);
                    }
                    SetFollowPosition(selectNpc.transform.position);
                }
                if (selectBed != null)
                {
                    SetFollowPosition(selectBed.transform.position);
                }
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
                        cursorHandler.SetCursor(CursorHandler.CursorType.Knock);
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
                else if (itemHit.collider.transform.tag.Equals(TagInfo.Tag_Bed))
                {
                    GameObject objSelect = itemHit.collider.gameObject;
                    selectBed = objSelect.GetComponentInParent<BuildBedCpt>();
                    if (selectBed)
                    {
                        audioHandler.PlaySound(AudioSoundEnum.ButtonForShow);
                        DialogBean dialogData = new DialogBean();
                        dialogSelectView = dialogManager.CreateDialog(DialogEnum.SelectForBed, this, dialogData);
                        ((SelectForBedDialogView)dialogSelectView).SetData(selectBed);
                        return;
                    }
                }
            }
            selectNpc = null;
            selectBed = null;
        }
    }


    public override void StartControl()
    {
        base.StartControl();
        SetLayer(1);
    }

    /// <summary>
    /// 设置层数
    /// </summary>
    /// <param name="layer"></param>
    public void SetLayer(int layer)
    {
        if (this.layer == layer)
            return;
        this.layer = layer;
        InitCameraRange(layer);
    }

    protected void InitCameraRange(int layer)
    {
        InnBuildBean innBuild = gameDataManager.gameData.GetInnBuildData();
        if (layer == 1)
        {
            //定义镜头的移动范围
            cameraMove.minMoveX = -0.1f;
            cameraMove.maxMoveX = innBuild.innWidth + 1;
            cameraMove.minMoveY = -0.1f;
            cameraMove.maxMoveY = innBuild.innHeight + 1;
            //定义镜头的初始位置
            SetFollowPosition(new Vector3(innBuild.innWidth / 2f, innBuild.innHeight / 2f, 0));
        }
        else if (layer == 2)
        {
            //定义镜头的移动范围
            cameraMove.minMoveX = -0.1f;
            cameraMove.maxMoveX = innBuild.innSecondWidth + 1;
            cameraMove.minMoveY = -0.1f + 100;
            cameraMove.maxMoveY = innBuild.innSecondHeight + 1 + 100;
            //定义镜头的初始位置
            SetFollowPosition(new Vector3(innBuild.innSecondWidth / 2f, 100 + innBuild.innSecondHeight / 2f, 0));
        }
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
        base.HandleForMouseButtonMove(out float moveButtonX, out float moveButtonY);
        cameraMove.MoveForUnscaled(moveX + moveButtonX, moveY + moveButtonY);
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
        selectBed = null;
        if (dialogSelectView != null)
        {
            Destroy(dialogSelectView.gameObject);
            dialogSelectView = null;
        }
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