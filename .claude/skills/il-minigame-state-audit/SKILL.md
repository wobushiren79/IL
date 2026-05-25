---
name: il-minigame-state-audit
description: 客栈传说 · 小游戏状态机与事件配对审计。检查所有 MiniGame*Handler 的 InitGame/StartGame/EndGame 状态机完备性、RegisterEvent/UnRegisterEvent 配对、MiniGameEnum 各 switch 是否补齐 case、Builder Prefab 是否存在、UI 是否注册。新增小游戏后或调试小游戏诡异行为时使用。
---

# il-minigame-state-audit

**客栈传说 · 小游戏状态机审计 Skill**

小游戏体系约束多：状态机四状态 + 事件注册必须成对 + 多处 switch 必须穷举 6 种 MiniGameEnum + Builder Prefab 必须存在 + UI 必须注册。本 skill 静态审计所有这些点。

---

## 用法

```
/il-minigame-state-audit [操作] [游戏名]
```

**操作：**

| 操作 | 说明 |
|---|---|
| `all`（默认） | 全量审计所有小游戏 |
| `handler <X>` | 仅审计 MiniGame<X>Handler |
| `events` | 仅检查事件 Register/UnRegister 配对 |
| `enums` | 仅检查 MiniGameEnum 相关 switch 完备性 |
| `builders` | 仅检查 Builder Prefab 存在性 |
| `ui` | 仅检查 UIMiniGame* 注册状态 |
| `state` | 仅检查 Handler 状态机方法重写情况 |

**示例：**
```
/il-minigame-state-audit
/il-minigame-state-audit handler Brewery
/il-minigame-state-audit events
/il-minigame-state-audit enums
```

---

## 背景知识

### 项目已有 6 种 MiniGame

```
MiniGameEnum:
  Cooking = 1, Barrage = 2, Account = 3, Debate = 4, Combat = 5
  Birth = 101 (特殊段)
```

### 必须穷举的 Switch（关键审计点）

| 位置 | 用途 |
|---|---|
| `MiniGameEnumTools.GetMiniGameData()` | enum → Bean 实例 |
| `MiniGameEnumTools.GetWorkerTypeByMiniGameType()` | enum → 对应职业（经验加成用） |
| `MiniGameBaseBean.GetListWinConditions()` | enum → 胜利条件列表 |
| `MiniGameBaseBean.GetGameName()` | enum → 游戏名文本 ID |
| `MiniGameBaseBean.CreateMiniGameCharacterBeanByType()` | enum → CharacterFor*Bean 实例 |

任一缺 case → 该小游戏在数据/UI 上"看起来正常"但运行时报 null。

### 事件配对契约

`BaseMiniGameHandler` 已处理：
- `MiniGame_GamePreCountDownStart` / `MiniGame_GamePreCountDownEnd` / `MiniGame_EventForOnClickClose`
- 在 `InitGame` 中 Register、`EventForOnClickClose` 中 UnRegister

子类必须自行处理：
- 自己 Register 的所有事件，必须在 `EndGame` 或 `EventForOnClickClose` 中 UnRegister

### Builder Prefab 强契约

`<X>Handler.Awake()` 中：
```csharp
builderName = "MiniGame<X>Builder";
base.Awake();  // 内部加载 Assets/Prefabs/Builder/{builderName}.prefab
```

若 Prefab 不存在 → Awake 抛异常被 catch → `LogUtil.Log` 提示 → `miniGameBuilder` 为 null → 后续所有 Builder 调用 NullRef。

---

## 执行步骤

### Step 1 — 发现所有 MiniGame Handler

Glob：`Assets/Scripts/Component/Handler/MiniGame/MiniGame*Handler.cs`

排除 `BaseMiniGameHandler.cs` 与 `MiniGameHandler.cs`（聚合入口）。

对每个文件读取，提取：
```
{
  className: MiniGameBreweryHandler,
  gameName: Brewery,
  builderType: 从泛型 <B, D> 提取的 B,
  beanType: 从泛型 <B, D> 提取的 D,
  builderName: Awake 中赋值的字符串,
  overrides: { InitGame: 是否重写, StartGame: ..., EndGame: ..., GamePreCountDownEnd: ... }
}
```

### Step 2 — MiniGameEnum 完备性检查（操作 enums）

读取：
- `Assets/Scripts/Enums/MiniGame/MiniGameEnum.cs` → 全部 enum 项
- `Assets/Scripts/Bean/MVC/MiniGame/MiniGameBaseBean.cs` → 找 `GetListWinConditions` / `GetGameName` / `CreateMiniGameCharacterBeanByType` 方法体
- 解析 `MiniGameEnumTools` 类的 switch 方法

对每个 MiniGameEnum 项，检查上述 5 个 switch 是否都有对应 case：

```
对每个 enum value:
  for switch in [GetMiniGameData, GetWorkerTypeByMiniGameType,
                 GetListWinConditions, GetGameName, CreateMiniGameCharacterBeanByType]:
    if value not handled in switch:
      -> 🔴 Switch 漏 case
```

**输出：**
```
🔴 MiniGameEnumTools.GetMiniGameData() 漏 case：
  - MiniGameEnum.Brewery   → 调用时返回 null，闪退

🟠 MiniGameBaseBean.GetGameName() 漏 case：
  - MiniGameEnum.Brewery   → 返回 "???" 占位符
```

### Step 3 — 状态机方法重写检查（操作 state）

对每个 Handler：

```
必须重写的方法：
  - InitGame(D miniGameData)  → 至少配置 builderName 注册事件、调用 base
  - StartGame()                → 至少打开主 UI
  - EndGame(MiniGameResultEnum, bool isSlow)  → 至少 UnRegister 在 InitGame 中 Register 的事件

可选重写：
  - GamePreCountDownEnd()      → 倒计时结束后的衔接（默认空）
```

检查项：
- ❓ 未重写 `InitGame` → 警告（极少不重写）
- ❌ 重写 `InitGame` 但未调用 `base.InitGame()` → `🔴 基类初始化遗漏`
- ❌ 重写 `EndGame` 但未调用 `base.EndGame()` → `🔴 基类清理遗漏`
- ⚠️ Handler 的 Awake 未设置 `builderName` → `🟠 Builder 加载会失败`

### Step 4 — 事件 Register/UnRegister 配对（操作 events）

对每个 Handler 文件 grep：

```regex
EventHandler\.Instance\.RegisterEvent(?:<[^>]+>)?\s*\(\s*EventsInfo\.(MiniGame\w+)\s*,\s*(\w+)
EventHandler\.Instance\.UnRegisterEvent(?:<[^>]+>)?\s*\(\s*EventsInfo\.(MiniGame\w+)\s*,\s*(\w+)
```

建立每个 Handler 的 `(event, callback)` 集合：
- `registers`
- `unregisters`

**审计：**
```
foreach (event, callback) in registers:
  if (event, callback) not in unregisters:
    -> 🔴 内存泄漏 / 多次触发风险
       "Register 但未 UnRegister"

foreach (event, callback) in unregisters:
  if (event, callback) not in registers:
    -> 🟡 死代码
       "UnRegister 了从未 Register 的事件"
```

**特殊处理：**
- `BaseMiniGameHandler.InitGame()` 中已处理 `MiniGame_GamePreCountDownStart` 等基类事件，子类无需手动 UnRegister（除非显式 Override）→ 排除这几个事件
- `UnRegisterEvent` 应该出现在 `EndGame` 或 `EventForOnClickClose` 方法内（grep 检查所在函数）

### Step 5 — Builder Prefab 存在性检查（操作 builders）

对每个 Handler：
1. 从 `Awake()` 提取 `builderName` 字符串字面量
2. Glob `Assets/Prefabs/Builder/<builderName>.prefab`
3. 不存在 → `🔴 Builder Prefab 缺失`

并检查 Prefab 上挂载的脚本类型：
- 读取 `<builderName>.prefab` YAML，找 `MonoBehaviour` 的 `m_Script` GUID
- 与 Handler 泛型参数 `B`（如 `MiniGameBreweryBuilder`）对应的 .cs.meta GUID 对比
- 不一致 → `🟠 Prefab 挂载的脚本与 Handler 泛型不匹配`

### Step 6 — UI 注册检查（操作 ui）

对每个 MiniGame，期望存在的 UI：
- `UIMiniGame<X>` (主 UI)
- `UIMiniGame<X>Settlement` (结算 UI，可选)

检查：
- 脚本是否存在
- UIEnum 是否注册（联动 `il-scene-init-check`）
- UIHandler 是否注册 Addressable 地址
- Prefab 是否存在

### Step 7 — 输出报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · 小游戏状态机审计报告
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

扫描：
  Handler 文件：6 个（Cooking/Barrage/Account/Debate/Combat/Birth）
  MiniGameEnum：6 个

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  Enum Switch 完备性
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  ✅ MiniGameEnum.Cooking : 5/5 switch 覆盖
  ✅ MiniGameEnum.Barrage : 5/5 switch 覆盖
  🟠 MiniGameEnum.Birth   : 4/5 switch 覆盖
       缺：MiniGameEnumTools.GetWorkerTypeByMiniGameType()
       → 经验加成时 worker 类型默认值，可能将经验加错职业

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  Handler 状态机
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  MiniGameCookingHandler.cs
    ✅ InitGame: 重写 + 调用 base
    ✅ StartGame: 重写 + 调用 base
    ✅ EndGame: 重写 + 调用 base
    ✅ GamePreCountDownEnd: 重写
    ✅ builderName = "MiniGameCookingBuilder"

  MiniGameDebateHandler.cs
    🔴 EndGame: 重写但未调用 base.EndGame()
       → BaseMiniGameHandler.EndGame 中的角色经验加成 / 慢镜头 / 打开结算 UI 全部跳过
       建议：在重写方法最后加 base.EndGame(gameResulte, isSlow);

  MiniGameBirthHandler.cs
    🟠 builderName 字段未在 Awake 中赋值
       → base.Awake() 加载 "Assets/Prefabs/Builder/.prefab" 必然失败

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  事件 Register/UnRegister 配对
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  MiniGameCookingHandler.cs
    ✅ MiniGameCooking_MenuSelect (Register/UnRegister 成对)
    ✅ MiniGameCooking_CookingSettle
    ✅ MiniGameCooking_CookingSettlementClose

  MiniGameBarrageHandler.cs
    🔴 MiniGameBarrage_BulletHit (Line 56)
       → Register 在 InitGame 中，但 EndGame 中未 UnRegister
       → 多次 InitGame 后回调会重复触发
       建议：EndGame 中添加：
         EventHandler.Instance.UnRegisterEvent<BulletBean>(
             EventsInfo.MiniGameBarrage_BulletHit, EventForBulletHit);

  MiniGameAccountHandler.cs
    🟡 MiniGameAccount_OldEvent (Line 88)
       → 只有 UnRegisterEvent，无对应 RegisterEvent（死代码）

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  Builder Prefab
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  ✅ MiniGameCookingBuilder.prefab  (脚本匹配)
  ✅ MiniGameBarrageBuilder.prefab
  🔴 MiniGameBirthBuilder.prefab  缺失
     → 运行时进入小游戏 Awake 加载失败
     建议：创建该 Prefab 并挂载 MiniGameBirthBuilder 脚本

  🟠 MiniGameDebateBuilder.prefab  挂载的脚本是 MiniGameAccountBuilder
     → 类型转换失败，Handler.miniGameBuilder 为 null

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  UI 注册
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  UIMiniGameBirth         ✅ 已注册
  UIMiniGameBirthSettlement  🟠 脚本存在但 UIEnum 未注册
     建议：/il-enum-gen append UIEnum MINI_GAME_BIRTH_SETTLEMENT

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  汇总：12 处问题
    🔴 严重（必修）：5
    🟠 重要：5
    🟡 建议：2

  优先级建议：
    1. 修补缺失 Builder Prefab（2 处，直接阻断小游戏运行）
    2. 修正 EndGame 中未调用 base.EndGame() 与事件未注销（4 处，影响多次进入）
    3. 补齐 MiniGameEnumTools 漏的 case（1 处）
    4. UIEnum 注册（1 处）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

### 操作 `handler <X>` 单 Handler 详细模式

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  MiniGame<X>Handler 详细审计
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

文件：Assets/Scripts/Component/Handler/MiniGame/MiniGameBreweryHandler.cs
继承：BaseMiniGameHandler<MiniGameBreweryBuilder, MiniGameBreweryBean>

🎯 状态机方法：
  ✅ Awake             : 设置 builderName = "MiniGameBreweryBuilder"
  ✅ InitGame          : 重写 + 调用 base
  ✅ StartGame         : 重写 + 调用 base
  ✅ EndGame           : 重写 + 调用 base
  ⚠️ GamePreCountDownEnd : 未重写（StartGame 不会自动调用，请确认是否需要）

📣 事件契约：
  Register / UnRegister 配对（4 对）：
    ✅ MiniGameBrewery_RecipeSelect   (InitGame:23 ↔ EndGame:88)
    ✅ MiniGameBrewery_StoveReady     (InitGame:25 ↔ EndGame:89)
    🔴 MiniGameBrewery_AuditerArrive (InitGame:27 ↔ 无 UnRegister)

🏗️ Builder：
  ✅ Prefab: Assets/Prefabs/Builder/MiniGameBreweryBuilder.prefab
  ✅ 挂载脚本: MiniGameBreweryBuilder

🎨 UI：
  ✅ UIMiniGameBrewery.cs / .prefab
  ✅ UIEnum.MINI_GAME_BREWERY 已注册
  🟠 UIMiniGameBrewerySettlement.cs 存在但 UIEnum 未注册

🎲 MiniGameEnum 完备性：
  ✅ Brewery = 6 已存在
  ✅ GetMiniGameData()                    包含 case
  🔴 GetWorkerTypeByMiniGameType()        漏 case
  ✅ GetListWinConditions()               包含 case
  🔴 GetGameName()                        漏 case → 显示 "???"
  ✅ CreateMiniGameCharacterBeanByType()  包含 case

📋 行动清单：
  [ ] 修复 1 处事件未 UnRegister
  [ ] 在 MiniGameEnumTools.GetWorkerTypeByMiniGameType 补 Brewery case
  [ ] 在 MiniGameBaseBean.GetGameName 补 Brewery case + 添加文本 ID
  [ ] 注册 UIEnum.MINI_GAME_BREWERY_SETTLEMENT
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 注意事项

- **只读 skill**：不修改任何文件，所有修复需手动或联动其他生成器。
- **泛型解析**：从 `BaseMiniGameHandler<X, Y>` 提取 `X`、`Y` 类型名，需识别带命名空间的情况。
- **事件签名**：`RegisterEvent` 可带 0、1、2 个泛型类型参数（带数据 / 无数据），匹配 callback 时要注意签名一致。
- **builderName 多种赋值**：`builderName = "..."`、`builderName += "..."`、构造时设置——优先识别 `Awake` 中的字面量赋值，其他情况标记「动态 builderName」。
- **MiniGameHandler 聚合**：项目中可能有 `MiniGameHandler.cs` 做总入口聚合（非 BaseMiniGameHandler 子类），不在本审计范围。
- **关联工具：**
  - 事件链整体审计：`/il-event-flow-trace`
  - 场景挂载与 InitData 调用：`/il-scene-init-check handler MiniGame<X>Handler`
  - UI Prefab 字段绑定：spawn `il-ui-prefab-binder` 单审 UIMiniGame<X>
  - 完整新游戏脚手架：`/il-minigame-scaffold`
