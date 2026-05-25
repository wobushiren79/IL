---
name: il-event-flow-trace
description: 客栈传说 · MsgEnum 全局事件链路追踪。分析每个全局事件的发布点（TriggerEvent）和订阅点（AddListener），找出未被订阅的事件、未被发布的死代码订阅、跨系统耦合关系。重构与调试事件死循环时使用。
---

# il-event-flow-trace

**客栈传说 · 事件流追踪 Skill**

项目使用 `EventHandler`（全局单例）做跨系统通信，事件 ID 定义在 `Assets/Scripts/Enums/Base/MsgEnum.cs`。本 skill 静态分析所有事件的发布/订阅链路，生成依赖图与异常清单。

---

## 用法

```
/il-event-flow-trace [操作] [事件名]
```

**操作：**

| 操作 | 说明 |
|---|---|
| `all`（默认） | 全量审计：未订阅 + 死订阅 + 重复订阅 + 系统耦合矩阵 |
| `event <事件名>` | 追踪单个事件的完整链路（发布者 + 订阅者 + 触发条件） |
| `system <系统名>` | 追踪指定系统发布/订阅的所有事件 |
| `unsubscribed` | 仅找有发布但无订阅的事件 |
| `dead` | 仅找有订阅但无发布的事件 |
| `coupling` | 输出跨系统事件耦合矩阵（发布方 × 订阅方） |
| `cleanup-check` | 检查 `OnDestroy` / `CloseUI` 中是否对应取消监听 |

**示例：**
```
/il-event-flow-trace
/il-event-flow-trace event MSG_GAME_DATA_INIT
/il-event-flow-trace system Brewery
/il-event-flow-trace unsubscribed
/il-event-flow-trace cleanup-check
```

---

## 背景知识

### 事件系统调用模式（项目固定）

```csharp
// 发布事件
EventHandler.Instance.TriggerEvent(MsgEnum.MSG_BREWERY_UPDATE);
EventHandler.Instance.TriggerEvent(MsgEnum.MSG_BREWERY_COMPLETE, brewBean);  // 带参数

// 订阅事件（通常在 Awake/OnEnable 或 OpenUI 中）
EventHandler.Instance.AddListener(MsgEnum.MSG_BREWERY_UPDATE, OnBreweryUpdate);

// 取消订阅（通常在 OnDestroy/OnDisable 或 CloseUI 中）
EventHandler.Instance.RemoveListener(MsgEnum.MSG_BREWERY_UPDATE, OnBreweryUpdate);

// 回调签名
private void OnBreweryUpdate(IEventData data) { ... }
private void OnBreweryUpdate() { ... }
```

### 局部事件（不在本 skill 范围）
`BaseEvent` 实例（对象级）随宿主销毁自动清理，不需要 Remove。本 skill **只**审计 `EventHandler` 全局事件。

### MsgEnum 分段约定（按观察推测）
- 1000~1999：游戏主流程
- 2000~2999：UI 相关
- 3000~3999：NPC/角色
- 4000~4999：小游戏
- 5000~5999：客栈管理
- ...（具体以读取实际文件为准）

---

## 执行步骤

### Step 1 — 解析 MsgEnum 全集

读取 `Assets/Scripts/Enums/Base/MsgEnum.cs`，提取所有事件项：
```
allEvents : Map<string, { value: int, comment: string?, segment: string }>
```

按数值段推断分类。若有注释（`// 注释`）记录其语义。

### Step 2 — 扫描发布点（TriggerEvent）

在 `Assets/Scripts/` 与 `Assets/FrameWork/Scripts/` 中 grep：
```regex
EventHandler\.Instance\.TriggerEvent\s*\(\s*MsgEnum\.([A-Z_]+)
```

对每个匹配记录：
```
{
  event: MSG_XXX,
  file: 路径,
  line: 行号,
  context: 所在方法名（向上回溯 'private/public/protected void' 找最近函数声明）,
  system: 从路径推断（Manager/Handler/UI 子目录）,
  hasParam: 是否带参（看 TriggerEvent 第二个参数）
}
```

**特殊情况：**
- `TriggerEvent(eventId)` 用变量传入 → 标为「动态事件」，单独列出
- `string.Format` / 字符串拼接构造事件 → 极少见，标为「字符串事件」

### Step 3 — 扫描订阅点（AddListener / RemoveListener）

类似 Step 2，匹配 `AddListener` 和 `RemoveListener`，分别建立：
```
listeners : Map<event, List<{ file, line, context, callback }>>
removers  : Map<event, List<{ file, line, context, callback }>>
```

对每个 `AddListener(MsgEnum.XXX, OnYyy)` 记录回调方法名 `OnYyy`，便于配对检查。

### Step 4 — 审计

#### unsubscribed（有发布无订阅）
```
foreach event in publishers:
  if event not in listeners:
    -> 报告："死事件，发布但没人监听"
```

#### dead（有订阅无发布）
```
foreach event in listeners:
  if event not in publishers:
    -> 报告："死订阅，监听了永不会触发的事件"
```

#### duplicate（重复订阅）
同一 `(file, callback, event)` 组合在多处 `AddListener` 而无对应 `RemoveListener` → 可能重复绑定，事件会触发多次。

#### cleanup-check（订阅未取消）
对每个 `AddListener`：
1. 找到所在类
2. 查类中的 `OnDestroy()` / `OnDisable()` / `CloseUI()` / `OnClose()` 方法
3. 检查是否有对应的 `RemoveListener(同事件, 同回调)`
4. 缺失 → `🟠 内存泄漏隐患（订阅未取消）`

**特例：**
- 类继承 `BaseUIComponent` 且重写 `CloseUI()` → 也检查父类是否在 `CloseUI` 中自动清理（通常需手动）
- 类继承 `BaseSingletonMonoBehaviour<T>` → 永不销毁，监听可不取消

#### coupling（跨系统耦合矩阵）
按 system 维度聚合：
```
              订阅方
            ┌─────────────────────────────────────┐
            │ Brewery  Inn   NPC   UI    ...       │
   ┌────────┼─────────────────────────────────────┤
发 │ Game   │   3      5     2     12              │
布 │ Inn    │   1      -     4     8               │
方 │ NPC    │   0      2     -     6               │
   └────────┴─────────────────────────────────────┘
```
数字 = 跨系统事件数。诊断耦合热点（某个系统被多个其他系统订阅 = 中枢系统）。

### Step 5 — 输出报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · 事件流追踪报告
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

MsgEnum 全集：134 个事件
发布点：287 处（动态事件 5 处）
订阅点：156 处

❌ 未被订阅（12 个事件，可能为遗留代码）：
  MSG_OBSOLETE_FEATURE  (line 88)
    发布于：Assets/Scripts/Component/Manager/.../FooManager.cs:42
  MSG_OLD_BREWERY_TICK
    发布于：3 处，订阅 0 处

🗑️ 死订阅（5 个事件，监听永不触发）：
  MSG_NEW_PLAYER_FIRST_LOGIN
    订阅于：UIMainView.cs:67
    全项目无对应 TriggerEvent

⚠️ 重复订阅风险（3 处）：
  MSG_GAME_DATA_UPDATE 在 GameDataManager.Awake() 与
                        GameDataHandler.OnReload() 中都调用 AddListener
                        但只有 OnReload 有对应 RemoveListener
                        → 多次 OnReload 后会重复触发

🟠 订阅未取消（8 处，内存泄漏）：
  UIBrewery.cs:Awake() AddListener(MSG_BREWERY_UPDATE, RefreshUI)
    但 CloseUI() / OnDestroy() 中无对应 RemoveListener

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  跨系统耦合矩阵
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
                  订阅方
              Game  Inn  NPC  Brewery  UI  ...
  ┌─────────┬───────────────────────────────
  Game      │  -    8    5    3        24
  发  Inn   │  2    -    6    1        15
  布  NPC   │  1    4    -    0        9
  方  Brewery│ 0    1    0    -        4
      UI    │  3    2    1    2        -

  耦合热点（被订阅次数 Top 3）：
    1. Game     → 41 次被订阅
    2. Inn      → 28 次被订阅
    3. NPC      → 22 次被订阅

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  动态事件（无法静态分析的调用，请人工核查）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  - Assets/Scripts/.../EventForwarder.cs:23
    EventHandler.Instance.TriggerEvent(eventId)  ← eventId 为参数
  - ...
```

### 操作 `event <事件名>` 单事件追踪

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  事件追踪：MSG_BREWERY_COMPLETE  (value=5021)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📤 发布点（2 处）：
  Assets/Scripts/Component/Manager/Brewery/BreweryManager.cs:67
    方法：FinishBrew(BrewBean)
    上下文：酿造完成时通知，带 BrewBean 参数
  Assets/Scripts/Component/Manager/Brewery/BreweryManager.cs:89
    方法：CancelBrew()
    上下文：玩家手动取消时同样触发

📥 订阅点（3 处）：
  Assets/Scripts/Component/UI/Game/UIBrewery.cs:88
    回调：OnBreweryComplete(IEventData)
    → CloseUI 中有对应 RemoveListener  ✅
  Assets/Scripts/Component/Handler/Achievement/AchievementHandler.cs:34
    回调：CheckBreweryAchievement
    → OnDestroy 中有对应 RemoveListener  ✅
  Assets/Scripts/Component/Manager/Inn/InnRevenueManager.cs:55
    回调：AddBrewIncome
    → ⚠️ 无对应 RemoveListener

链路图：
  BreweryManager.FinishBrew/CancelBrew
        ↓ TriggerEvent
  EventHandler
        ↓ 广播
        ├─ UIBrewery.OnBreweryComplete         ✅
        ├─ AchievementHandler.CheckXxx          ✅
        └─ InnRevenueManager.AddBrewIncome      ⚠️ 泄漏
```

---

## 注意事项

- **只读 skill**：不修改任何代码，只生成报告。
- **回调签名匹配**：项目中回调可能有 0 或 1 个参数（`IEventData`），grep 时不要按完整签名匹配，否则会漏。
- **动态事件不报错**：变量传入的事件名属于设计自由度，不当作问题，单独列出供人工审视。
- **避免误判 Lambda**：`AddListener(MsgEnum.X, () => { ... })` 这种匿名 Lambda 无法直接 RemoveListener，但项目中如有此用法，标为「⚠️ 不可移除的订阅」。
- **大项目优化**：800+ 文件全量 grep 后建议先按事件名分组再生成报告，避免单事件重复输出。
- **MsgEnum 解析**：枚举可能含 `=` 显式赋值或省略（自增），都需正确解析数值。
- **关联工具**：
  - 若发现订阅未取消问题集中在某文件，可用 `/il-code-reviewer` 跑该文件做深度审查
  - 若要新增事件枚举值，用 `/il-enum-gen append MsgEnum MSG_XXX`
