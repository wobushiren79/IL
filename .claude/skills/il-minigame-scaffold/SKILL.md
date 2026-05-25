---
name: il-minigame-scaffold
description: 客栈传说 · 小游戏脚手架。一键生成新 MiniGame 全套结构：Handler（继承 BaseMiniGameHandler）+ Builder + Bean + Character Bean + UI（Main/Settlement/Component）+ Cpt 组件骨架 + 枚举追加清单 + Builder Prefab 路径建议。新增烹饪/弹幕/算账/辩论/战斗/出生类小游戏时使用。
---

# il-minigame-scaffold

**客栈传说 · 小游戏脚手架 Skill**

项目已有 6 种小游戏（Cooking / Barrage / Account / Debate / Combat / Birth），全部遵循统一架构：`BaseMiniGameHandler<Builder, Bean>` + `BaseMiniGameBuilder` + `MiniGameBaseBean` + UI 三件套（主面板 / 倒计时 / 结算）+ 多个 Cpt 组件。本 skill 一次性生成所有骨架。

---

## 用法

```
/il-minigame-scaffold <游戏名> <类型> [Cpt组件名...]
```

**类型参数：**

| 类型 | 胜利条件预设 | 适合场景 |
|---|---|---|
| `score` | winScore + winRank | 计分对抗（如烹饪比赛） |
| `survival` | winSurvivalTime + winLife | 生存类（如弹幕闪避） |
| `money` | winSurvivalTime + winMoneyS/M/L | 金钱接收（如算账） |
| `life` | winLife | 血量对抗（如辩论） |
| `bringDown` | winSurvivalNumber + winBringDownNumber | 击败角色（如群体战斗） |
| `fire` | winFireNumber | 次数限制（如出生小游戏） |
| `custom` | 不预设，全部留 TODO | 自定义胜负规则 |

**Cpt 组件名：** 空格分隔，对应将要在 `Builder` 中管理的子元素。

**示例：**
```
/il-minigame-scaffold Brewery score Stove Recipe Auditer
/il-minigame-scaffold Catch survival Net Fish Splash
/il-minigame-scaffold Quiz life Question Answer Timer
/il-minigame-scaffold Race custom Track Player Obstacle
```

---

## 背景知识

### 现有 MiniGame 体系（必读基础）

```
[基类层 - 不要修改]
BaseMiniGameHandler<B, D>          (Handler/MiniGame/BaseMiniGameHandler.cs)
  └─ Awake/InitGame/StartGame/EndGame 状态机
  └─ Builder Prefab 自动加载 ($"Assets/Prefabs/Builder/{builderName}.prefab")
  └─ 角色经验加成 / 慢镜头 / UIMiniGameEnd 打开

MiniGameBaseBean                   (Bean/MVC/MiniGame/MiniGameBaseBean.cs)
  └─ winSurvivalTime/winLife/winFireNumber/winScore/winRank...
  └─ listUserGameData / listEnemyGameData
  └─ abstract InitForMiniGame()
  └─ virtual GetListWinConditions() / GetGameName()

BaseMiniGameBuilder                (Component/Builder/MiniGame/BaseMiniGameBuilder.cs)
  └─ DestroyAll()  虚方法

MiniGameCharacterBean              (Bean/MVC/MiniGame/MiniGameCharacterBean.cs)
  └─ characterType (1=玩家 / 0=敌人)
  └─ characterCurrentLife / characterMaxLife
  └─ characterData (CharacterBean 引用)
```

### 状态机生命周期（每个小游戏必须实现）

```
GamePre  ───CountDownStart──▶  GamePre(倒计时中)
                                    │ CountDownEnd
                                    ▼
                                Gameing
                                    │ EndGame()
                                    ▼
                                GameEnd
                                    │ OnClickClose
                                    ▼
                                GameClose（事件全部注销）
```

### 事件命名约定

- 全局：`EventsInfo.MiniGame_*`（基类已处理）
- 单游戏：`EventsInfo.MiniGame<游戏名>_*`（如 `MiniGameCooking_MenuSelect`）

### 必修枚举（生成时输出待操作清单，不直接改）

- `Assets/Scripts/Enums/MiniGame/MiniGameEnum.cs` —— 追加枚举项
- `MiniGameEnumTools.GetMiniGameData()` —— switch 补 case
- `MiniGameEnumTools.GetWorkerTypeByMiniGameType()` —— switch 补 case（如需经验加成）
- `MiniGameBaseBean.GetListWinConditions()` —— switch 补 case
- `MiniGameBaseBean.GetGameName()` —— switch 补 case + 文本 ID
- `MiniGameBaseBean.CreateMiniGameCharacterBeanByType()` —— switch 补 case

---

## 执行步骤

### Step 1 — 解析输入

- 游戏名 `<X>`：PascalCase（如 `Brewery`、`Catch`）
- 类型 → 决定 `<X>Bean` 中胜利条件字段使用模式
- Cpt 组件列表 → 决定生成几个 `MiniGame<X><Cpt>Cpt.cs` 文件

输出确认表给用户：

```
准备生成：
  游戏名：Brewery
  类型：score（胜利分数 + 排名）
  Cpt：MiniGameBreweryStoveCpt / MiniGameBreweryRecipeCpt / MiniGameBreweryAuditerCpt

  待生成文件：
    Handler/MiniGame/MiniGameBreweryHandler.cs
    Manager/MiniGame/ ......（如需要）
    Builder/MiniGame/MiniGameBreweryBuilder.cs
    Bean/MVC/MiniGame/MiniGameBreweryBean.cs
    Bean/MVC/MiniGame/MiniGameCharacterForBreweryBean.cs
    UI/MiniGame/UIMiniGameBrewery.cs
    UI/MiniGame/UIMiniGameBrewerySettlement.cs
    MiniGame/GameBrewery/MiniGameBreweryStoveCpt.cs
    MiniGame/GameBrewery/MiniGameBreweryRecipeCpt.cs
    MiniGame/GameBrewery/MiniGameBreweryAuditerCpt.cs
```

### Step 2 — 生成 Handler

**路径：** `Assets/Scripts/Component/Handler/MiniGame/MiniGame<X>Handler.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;
using System;

public class MiniGame<X>Handler : BaseMiniGameHandler<MiniGame<X>Builder, MiniGame<X>Bean>
{
    // ─── UI 引用 ───
    protected UIMiniGame<X> uiMiniGame<X>;
    protected UIMiniGame<X>Settlement uiSettlement;

    public override void Awake()
    {
        builderName = "MiniGame<X>Builder";
        base.Awake();
    }

    /// <summary>
    /// 初始化游戏（注册事件 + 创建场景对象 + 打开倒计时 UI）
    /// </summary>
    public override void InitGame(MiniGame<X>Bean miniGameData)
    {
        // ─── 1. 注册游戏专属事件 ───
        // EventHandler.Instance.RegisterEvent<参数类型>(EventsInfo.MiniGame<X>_XXX, EventForXXX);
        // ⚠️ 任何 RegisterEvent 必须在 EndGame 中对应 UnRegisterEvent

        base.InitGame(miniGameData);

        // ─── 2. 创建场景对象（通过 Builder）───
        // miniGameBuilder.CreateAllCharacter(...);

        // ─── 3. 摄像头 ───
        InitCameraPosition();

        // ─── 4. 打开倒计时 UI ───
        OpenCountDownUI(miniGameData);
    }

    /// <summary>
    /// 摄像机初始化
    /// </summary>
    public void InitCameraPosition()
    {
        // BaseControl baseControl = GameControlHandler.Instance.StartControl<ControlForMiniGame<X>Cpt>(
        //     GameControlHandler.ControlEnum.MiniGame<X>);
        // baseControl.SetCameraFollowObj(baseControl.gameObject);
        // SetCameraPosition(miniGameData.userStartPosition);
    }

    /// <summary>
    /// 游戏开始（倒计时结束后由 GamePreCountDownEnd 调用）
    /// </summary>
    public override void StartGame()
    {
        base.StartGame();

        // ─── 1. 打开主 UI ───
        uiMiniGame<X> = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGame<X>>();
        uiMiniGame<X>.SetData(miniGameData);

        // ─── 2. 启动 NPC AI / 触发首个游戏阶段 ───
        // TODO: 实现具体开局逻辑
    }

    /// <summary>
    /// 游戏结束（务必注销全部 Register 过的事件）
    /// </summary>
    public override void EndGame(MiniGameResultEnum gameResult, bool isSlow)
    {
        base.EndGame(gameResult, isSlow);

        // EventHandler.Instance.UnRegisterEvent<参数类型>(EventsInfo.MiniGame<X>_XXX, EventForXXX);
    }

    #region 倒计时回调
    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        // 通常这里调用 StartGame() 或先播放剧情
        StartGame();
    }
    #endregion

    #region 事件回调（业务实现）
    // public void EventForXXX(<ParamType> data)
    // {
    //     // TODO: 处理事件
    // }
    #endregion
}
```

### Step 3 — 生成 Builder

**路径：** `Assets/Scripts/Component/Builder/MiniGame/MiniGame<X>Builder.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;

public class MiniGame<X>Builder : BaseMiniGameBuilder
{
    // ─── 场景对象引用（在 Prefab Inspector 中绑定）───
    // public Transform userStartPoint;
    // public Transform enemyStartPoint;
    // public List<MiniGame<X><CptA>Cpt> listCptA;
    // public List<MiniGame<X><CptB>Cpt> listCptB;

    // ─── 运行时角色容器 ───
    private List<NpcAIMiniGame<X>Cpt> _listAllCharacter = new List<NpcAIMiniGame<X>Cpt>();

    /// <summary>
    /// 创建所有角色（玩家 + 敌方 + 旁观）
    /// </summary>
    public void CreateAllCharacter(
        List<MiniGameCharacterBean> listUser, Vector3 userStartPos,
        List<MiniGameCharacterBean> listEnemy, List<Vector3> listEnemyStartPos)
    {
        // TODO: 调用资源加载 + Instantiate NPC AI
    }

    /// <summary>
    /// 按类型筛选角色
    /// </summary>
    // public List<NpcAIMiniGame<X>Cpt> GetCharacterByType(NpcAIMiniGame<X>Cpt.MiniGame<X>NpcTypeEnum type)
    // {
    //     return _listAllCharacter.Where(x => x.npcType == type).ToList();
    // }

    /// <summary>
    /// 获取操作角色
    /// </summary>
    // public NpcAIMiniGame<X>Cpt GetUserCharacter()
    // {
    //     return _listAllCharacter.FirstOrDefault(x => x.characterMiniGameData.characterType == 1);
    // }

    public override void DestroyAll()
    {
        // 销毁所有生成的角色与场景对象
        // foreach (var npc in _listAllCharacter) if (npc) Destroy(npc.gameObject);
        // _listAllCharacter.Clear();
    }
}
```

### Step 4 — 生成 Bean

**路径：** `Assets/Scripts/Bean/MVC/MiniGame/MiniGame<X>Bean.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;

public class MiniGame<X>Bean : MiniGameBaseBean
{
    // ─── 场景起点 ───
    public Vector3 userStartPosition;
    public List<Vector3> listEnemyStartPosition = new List<Vector3>();

    // ─── 游戏专属配置（根据类型自动生成） ───
    // score 类型示例：（除继承的 winScore/winRank 外）
    public int judgeNumber = 3;             // 评委人数

    // survival 类型示例：（除继承的 winSurvivalTime/winLife 外）
    // public float spawnInterval = 0.5f;

    public override void InitForMiniGame()
    {
        // 游戏数据初始化
        // 例：根据 winRank 和参赛角色总数做合法性兜底
    }
}
```

### Step 5 — 生成 Character Bean

**路径：** `Assets/Scripts/Bean/MVC/MiniGame/MiniGameCharacterFor<X>Bean.cs`

```csharp
using UnityEngine;

public class MiniGameCharacterFor<X>Bean : MiniGameCharacterBean
{
    // ─── 该游戏角色专属字段 ───
    // 例：score 类型 → 得分相关
    public int scoreForA;
    public int scoreForB;
    public int scoreForTotal;

    public void InitScore()
    {
        scoreForTotal = scoreForA + scoreForB;
    }
}
```

### Step 6 — 生成 UI 三件套

**主 UI：** `Assets/Scripts/Component/UI/MiniGame/UIMiniGame<X>.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;

public class UIMiniGame<X> : UIBaseMiniGame
{
    // public Button btQuit;
    // public Text tvTimer;
    // public Slider sliderProgress;

    private MiniGame<X>Bean _miniGameData;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OpenUI()
    {
        base.OpenUI();
    }

    public void SetData(MiniGame<X>Bean miniGameData)
    {
        _miniGameData = miniGameData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_miniGameData == null) return;
        // TODO: 刷新 UI
    }
}
```

**结算 UI：** `Assets/Scripts/Component/UI/MiniGame/UIMiniGame<X>Settlement.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMiniGame<X>Settlement : BaseUIComponent
{
    // public Transform itemContainer;
    // public Button btConfirm;

    public override void Awake()
    {
        base.Awake();
        // if (btConfirm != null) btConfirm.onClick.AddListener(OnClickForConfirm);
    }

    public void SetData(List<NpcAIMiniGame<X>Cpt> listPlayer)
    {
        // TODO: 创建结算列表项
        RefreshUI();
    }

    private void RefreshUI() { }

    private void OnClickForConfirm()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        EventHandler.Instance.TriggerEvent(EventsInfo.MiniGame<X>_SettlementClose);
        CloseUI();
    }
}
```

### Step 7 — 生成 Cpt 组件

对每个用户指定的 Cpt 名，路径：`Assets/Scripts/Component/MiniGame/Game<X>/MiniGame<X><Cpt>Cpt.cs`

```csharp
using UnityEngine;

public class MiniGame<X><Cpt>Cpt : BaseMonoBehaviour
{
    // ─── Inspector 字段 ───
    // public Transform anchor;
    // public ParticleSystem effect;

    public override void Awake()
    {
        base.Awake();
    }

    // ─── 业务方法 ───
    // public void OnPlayerEnter(NpcAIMiniGame<X>Cpt npc) { }
}
```

### Step 8 — 生成 NPC AI Cpt（占位）

**路径：** `Assets/Scripts/Component/NPC/MiniGame/NpcAIMiniGame<X>Cpt.cs`

```csharp
using UnityEngine;
using System.Collections.Generic;

public class NpcAIMiniGame<X>Cpt : BaseMonoBehaviour
{
    public enum MiniGame<X>NpcTypeEnum
    {
        Player = 0,
        Enemy = 1,
        // Auditer = 2, // 按需扩展
    }

    public enum MiniGame<X>IntentEnum
    {
        None = 0,
        Idle = 1,
        // 根据具体游戏阶段补充
    }

    public MiniGame<X>NpcTypeEnum npcType;
    public MiniGameCharacterFor<X>Bean characterMiniGameData;

    private MiniGame<X>IntentEnum _intent = MiniGame<X>IntentEnum.None;

    public void OpenAI() { /* 开启 AI 思考 */ }

    public void SetIntent(MiniGame<X>IntentEnum intent, params object[] args)
    {
        _intent = intent;
        // TODO: 切换意图
    }
}
```

### Step 9 — 输出注册清单（必须手动完成）

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  MiniGame<X> 系统脚手架已生成
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

已创建文件：
  ✅ Handler/MiniGame/MiniGame<X>Handler.cs
  ✅ Builder/MiniGame/MiniGame<X>Builder.cs
  ✅ Bean/MVC/MiniGame/MiniGame<X>Bean.cs
  ✅ Bean/MVC/MiniGame/MiniGameCharacterFor<X>Bean.cs
  ✅ UI/MiniGame/UIMiniGame<X>.cs
  ✅ UI/MiniGame/UIMiniGame<X>Settlement.cs
  ✅ MiniGame/Game<X>/MiniGame<X><Cpt1>Cpt.cs
  ✅ MiniGame/Game<X>/MiniGame<X><Cpt2>Cpt.cs
  ✅ NPC/MiniGame/NpcAIMiniGame<X>Cpt.cs

待手动操作（重要！）：

  [ ] 1. MiniGameEnum.cs 追加：
         /il-enum-gen append MiniGameEnum <X>
         （建议值：现有最大 +1，避开 101 之后的特殊段）

  [ ] 2. MiniGameEnumTools.GetMiniGameData() 添加 case：
            case MiniGameEnum.<X>:
                miniGameData = new MiniGame<X>Bean();
                break;

  [ ] 3. MiniGameEnumTools.GetWorkerTypeByMiniGameType() 添加 case：
            case MiniGameEnum.<X>: return WorkerEnum.<对应职业>;

  [ ] 4. MiniGameBaseBean.GetListWinConditions() switch 添加：
            case MiniGameEnum.<X>:
                GetListWinConditionsForWin<胜利条件>(listWinConditions);
                break;

  [ ] 5. MiniGameBaseBean.GetGameName() switch 添加：
            case MiniGameEnum.<X>:
                gameName = TextHandler.Instance.GetTextById(<游戏名文本ID>);
                break;
         需在 excel_ui_text 中追加文本 ID

  [ ] 6. MiniGameBaseBean.CreateMiniGameCharacterBeanByType() switch 添加：
            case MiniGameEnum.<X>:
                itemUserGameData = new MiniGameCharacterFor<X>Bean();
                break;

  [ ] 7. EventsInfo.cs 中追加事件 ID：
         public const string MiniGame<X>_<动作1> = "MiniGame<X>_<动作1>";
         public const string MiniGame<X>_SettlementClose = "MiniGame<X>_SettlementClose";

  [ ] 8. UIEnum.cs 追加：
         /il-enum-gen append UIEnum MINI_GAME_<X>
         /il-enum-gen append UIEnum MINI_GAME_<X>_SETTLEMENT

  [ ] 9. 创建 Builder Prefab：
         路径：Assets/Prefabs/Builder/MiniGame<X>Builder.prefab
         挂载脚本：MiniGame<X>Builder
         ⚠️ 文件名必须与 Handler.Awake() 中 builderName 字符串完全一致

  [ ] 10. 创建 UI Prefab 并在 Addressables 中注册：
          - UIMiniGame<X>.prefab     → Address: UI/UIMiniGame<X>
          - UIMiniGame<X>Settlement.prefab → Address: UI/UIMiniGame<X>Settlement

  [ ] 11. 在合适场景（通常 GameInnScene/GameArenaScene）的 Handlers 节点下挂载
          MiniGame<X>Handler GameObject

  [ ] 12. 在游戏触发入口（如 ArenaHandler / InnInteractive）添加：
            MiniGame<X>Handler.Instance.InitGame(new MiniGame<X>Bean { ... });

  [ ] 13. （若按类型 = score/life 等含奖励）补 excel_minigame_<x>.xlsx 配置表

后续审计：
  /il-minigame-state-audit handler MiniGame<X>Handler
  /il-scene-init-check handler MiniGame<X>Handler
  /il-addressable-audit unregistered
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 注意事项

- **不自动改基类**：`MiniGameEnum`、`MiniGameEnumTools`、`MiniGameBaseBean` 的 switch 补 case 必须手动完成（值由用户/`/il-enum-gen` 决定，避免冲突）。
- **不自动创建 Prefab**：Unity Prefab 必须在 Editor 中创建，本 skill 只生成脚本与路径清单。
- **builderName 强契约**：`Awake()` 中赋值的 `builderName` 必须与 `Assets/Prefabs/Builder/<name>.prefab` 文件名完全一致，错一个字符就静默失败（Handler.Awake 中只 Log 不抛错）。
- **事件注销配对**：Step 2 生成的 Handler 模板中 `RegisterEvent` 与 `UnRegisterEvent` 必须 1:1，遗漏会造成多次 InitGame 后回调多次触发。生成完成后建议跑 `/il-minigame-state-audit`。
- **类型 vs 字段映射**：`type` 参数仅决定 Bean 中预留的胜利条件字段以及在哪里调用 `GetListWinConditionsForXxx()`，**不**改变继承结构。
- **关联工具：**
  - 状态机/事件审计：`/il-minigame-state-audit`
  - 数值平衡：spawn `il-minigame-balance-analyst`
  - 全流程追踪：spawn `il-minigame-flow-tracer`
