---
name: il-datetime-check
description: 客栈传说 · 游戏日期/时间系统审计。检查新建游戏 gameTime 是否被正确初始化、日历配置(DateInfo)与时间流转常量(maxForDay/月数/季节)是否一致、季节映射与特殊日期(结婚日/建筑日/生日)比对逻辑、0/0/0 测试哨兵是否影响正式流程。新建游戏日历空白、日期显示异常、季节/天气错乱、跨天/跨月/跨年边界出错时使用。只读分析。
---

# il-datetime-check

**客栈传说 · 游戏日期 / 时间系统完整性审计 Skill**

游戏内"年/月/日/时"由 `GameDataBean.gameTime`（`TimeBean`）单一数据源驱动，经
`GameTimeHandler` 推进，最终在 `UIGameDate` + `CalendarView` 上呈现。这条链路上任何
一处常量不一致或初始化遗漏，都会表现为「日历空白 / 没有具体日期 / 季节错乱 /
跨天卡死」等**静默 UI 异常**，编译期无法发现。本 skill 静态校验整条时间链路。

> 典型案例：新建游戏时 `Submit()` 创建 `new GameDataBean()` 但未初始化 `gameTime`，
> 导致 `年=月=日=0`，`CalendarView.InitMonth(0)` 在 `DateInfo`(仅 1~4 月) 中匹配不到任何
> 日，日历整月空白。同时 `0/0/0` 又被 `NpcImportantBuilder` 当作"测试模式"哨兵 —
> 一个未初始化 bug 同时污染了日历与 NPC 生成。这正是本 skill 要拦截的问题类型。

---

## 用法

```
/il-datetime-check [范围]
```

**范围参数：**

| 范围 | 说明 |
|---|---|
| `all`（默认） | 全量审计：初始化 + 常量一致性 + 季节 + 特殊日期 + 哨兵 |
| `init` | 仅检查新建/读档时 `gameTime` 初始化是否完整、是否落入 `0/0/0` 哨兵 |
| `const` | 仅检查时间常量一致性（maxForDay / 月上限 / DateInfo 配置范围 / 进位逻辑） |
| `season` | 仅检查 `SeasonsEnum` ↔ month 映射与 `SetSeasons` switch 完整性 |
| `special` | 仅检查特殊日期（结婚日/建筑日/生日/节日）与 `gameTime` 的比对逻辑 |
| `calendar` | 仅检查日历显示链路（CalendarView / ItemGameCalendarCpt / DateInfo） |
| `sentinel` | 仅列出所有把 `0/0/0` 当测试/调试哨兵的代码，评估对正式流程的影响 |

**示例：**
```
/il-datetime-check
/il-datetime-check init
/il-datetime-check const
/il-datetime-check sentinel
```

---

## 背景知识（项目实测常量，审计时以代码为准，发现不符即报）

### 时间数据源
| 项 | 位置 |
|---|---|
| 数据 Bean | `Assets/FrameWork/Scripts/Bean/TimeBean.cs`（year/month/day/hour/minute/second） |
| 挂载点 | `GameDataBean.gameTime`（`Assets/Scripts/Bean/MVC/User/GameDataBean.cs`） |
| 推进器 | `Assets/Scripts/Component/Handler/Game/GameTimeHandler.cs` |
| 日历 UI | `Assets/Scripts/Component/UI/Game/UIGameDate.cs` |
| 日历视图 | `Assets/Scripts/Component/UI/View/CalendarView.cs` |
| 日格子 | `Assets/Scripts/Component/ListItem/ItemGameCalendarCpt.cs` |
| 日期内容配置 | `Assets/Resources/JsonText/DateInfo.txt`（`DateInfoCfg`，fileName=`DateInfo`） |

### 历法常量（**一致性的核心**）
| 常量 | 期望值 | 来源 |
|---|---|---|
| 一个月天数上限 `maxForDay` | `42` | `GameTimeHandler.maxForDay` |
| 一年月数（季数） | `4`（month 1~4） | `GoToNextDay` 中 `month > 4` 进位 |
| 起始年份 | `221` | `UIGameDate.CoroutineForNextDay` 第一/第二年判断 `year==221||222` |
| 新游戏起始时间 | `年221 / 月1 / 日0`（开局推进到日1） | `UIMainCreate.Submit` 初始化 |
| `DateInfo` 配置范围 | `month ∈ [1,4]`，`day ∈ [1,42]` | `Assets/Resources/JsonText/DateInfo.txt` |

> ⚠️ `maxForDay`、`DateInfo` 的最大 day、UI 网格行列数（`CalendarView.OnGUI` 用 7 列 × 6 行 = 42）
> **三者必须同时等于 42**。任何一处改了而另两处没改，都会出现"日历缺格 / 多格 / 跨月错位"。

### 季节映射
| month | SeasonsEnum | 文本 ID | 颜色 |
|---|---|---|---|
| 1 | `Spring`(1) 春 | 33 | 绿 |
| 2 | `Summer`(2) 夏 | 34 | 红 |
| 3 | `Autumn`(3) 秋 | 35 | 橙 |
| 4 | `Winter`(4) 冬 | 36 | 蓝 |
| 0 | `Other`(0) | —（空字符串） | 黑 |

- 枚举定义：`Assets/FrameWork/Scripts/Enums/BaseGameEnum.cs` 中 `SeasonsEnum`
- `month` 直接强转 `(SeasonsEnum)month`，所以 **month 必须落在 1~4**；`month==0` 会得到 `Other` → 季节文字为空。

### `0/0/0` 测试哨兵
- `年==0 && 月==0 && 日==0` 在 `NpcImportantBuilder.BuildNpc` 中被当作"测试模式：默认生成所有 NPC"。
- 含义：**正式游戏永远不应处于 `0/0/0`**。任何能让正式存档停留在 `0/0/0` 的路径都是 bug。

### 时间流转关键方法（`GameTimeHandler`）
| 方法 | 作用 / 边界 |
|---|---|
| `GoToNextDay(n)` | `day+1`；`day>maxForDay→day=1,month+1`；`month>4→month=1,year+1` |
| `SetNewDay()` | 重置 `hour=6,min=0`、刷新随机种子/每日限制/建筑日/家族成员天数 |
| `TimeLapse()` | `min`累加；`min>=60→hour+1`；`hour>=24→结束当天(EndDay)` |
| `GetTime(out y,out m,out d)` | 读 `gameData.gameTime` |

---

## 执行步骤

### Step 1 — 初始化审计（操作 `init`）

**1a. 新建游戏路径**
- 定位创建入口：grep `new GameDataBean\(\)`，重点 `UIMainCreate.cs` 的 `Submit()`。
- 检查紧随其后是否对 `gameTime` 赋值（`SetTimeForYMD(...)` 或逐字段赋值）。
- 判定：
  ```
  若创建后 gameTime 未被赋值 → 🔴 新建游戏 gameTime 未初始化，开局将停留在 0/0/0
     后果：日历空白(InitMonth(0) 无匹配)、季节为空、被 NpcImportantBuilder 误判为测试模式
     建议：gameData.gameTime.SetTimeForYMD(221, 1, 0);
  ```

**1b. 读档路径**
- grep `QueryDataByUserId` / `GetGameDataByUserId`，确认读出的存档 `gameTime` 来自持久化、未被覆盖为 0。
- 检查 `gameData == null` 时的兜底 `new GameDataBean()`（`GameDataManagerPartial.cs`）是否也会落入 `0/0/0`。

**1c. TimeBean 默认值**
- `TimeBean` 无参构造不赋初值 → 所有字段默认 `0`。确认这一前提，凡是"创建后未显式设日期"的路径都标红。

### Step 2 — 常量一致性审计（操作 `const`）

读取并交叉比对：
1. `GameTimeHandler.maxForDay` 的字面值
2. `GameTimeHandler.GoToNextDay` 中 `month > X` 的 `X`（期望 4）
3. `DateInfo.txt` 中实际出现的 `month` 集合与每月最大 `day`
4. `CalendarView.OnGUI` 网格列数/行数（`width/7`、`height/6` → 7×6=42）

```
maxForDay(42) == DateInfo最大day(42) == 网格容量(7×6) ?
  不等 → 🔴 历法天数三处不一致：日历会缺格/多格/跨月错位
月上限(4) == DateInfo最大month ?
  不等 → 🔴 月份进位与配置不一致：某季无日期数据或越界
```

> 额外检查：`DateInfo` 是否每个 `month`(1~4) 都覆盖了 `day` 1~42 连续无缺漏（缺某天 → 该天格子不显示）。

### Step 3 — 季节映射审计（操作 `season`）

1. 读 `SeasonsEnum`（`BaseGameEnum.cs`）确认 `Spring=1..Winter=4, Other=0`。
2. 检查 `CalendarView.SetSeasons` 的 switch 是否 4 个季节 case 齐全、`default` 是否安全（置空而非崩溃）。
3. grep 全项目 `(SeasonsEnum)` 强转点（如 `ItemTownGoodsMarketCpt`、`GameSeasonsHandler`），确认强转前 `month` 一定 ∈ [1,4]：
   ```
   若存在 month 可能为 0 的强转 → 🟠 month=0 → SeasonsEnum.Other → 季节/天气/物价逻辑异常
   ```
4. `GameSeasonsHandler.ChangeSeasons` 与 `month` 的联动是否一致。

### Step 4 — 特殊日期审计（操作 `special`）

游戏内多处把"某 TimeBean 与当前 gameTime 比对"决定行为，三字段必须**全比**（year+month+day），漏比会误触发：

| 特殊日 | 代码位置（参考） | 应比对字段 |
|---|---|---|
| 结婚日 | `FamilyDataBean.timeForMarry`、`UIGameDate.CoroutineForNextDay` | year+month+day |
| 建筑日 | `InnBuildBean.listBuildDay`、`CalendarView.SetBuildDay` | year+month+day |
| 生日/成长 | `CharacterForFamilyBean.CheckIsGrowUp/AddBirthDay` | 天数累加 |
| 节日/事件 | `EventTriggerEnum`、`ShowConditionEnum` 的日期条件 | 视条件类型 |
| 第一/二年免营业 | `UIGameDate`：`year==221||222 && month==1 && day==1` | year+month+day |

审计要点：
```
foreach 日期比对点:
  if 只比了 day 或 month（漏 year）→ 🟠 跨年/跨月会误判（如每年同月同日都触发结婚日）
  if 比对的 TimeBean 可能为 null → 🔴 空引用风险
```

### Step 5 — 日历显示链路审计（操作 `calendar`）

链路：`UIGameDate.OpenUI → CalendarView.InitData(y,m,d) → InitMonth(m) → 实例化 ItemGameCalendarCpt → SetCurrentDay(d)`

检查：
1. `InitMonth(m)` 用 `DateInfoCfg.GetAllArrayData()` 过滤 `item.month==m`；若 `m∉[1,4]` → 空列表 → 空日历。回链到 Step 1/2 找根因。
2. `ChangeData` 中 `month` 相等走 `SetCurrentDay`，不相等才重建月份 —— 确认跨月动画/重建分支正确。
3. `ItemGameCalendarCpt` 的 `tvDay/tvDetails/tvWeather/ivBackground` 是否可能为空（联动 `/il-ui-prefab-binder`）。
4. `objDayModel` 预制是否挂了 `ItemGameCalendarCpt`（`GetComponent` 失败则该格静默不显示）。

### Step 6 — 哨兵审计（操作 `sentinel`）

grep 全项目同时命中 `year == 0`、`month == 0`、`day == 0` 的判断块（典型：`NpcImportantBuilder`）：
```
列出每一处把 0/0/0 当"测试/调试/默认全开"的代码：
  - 文件:行号 — 该分支行为（如"生成所有 NPC"）
评估：
  正式游戏 gameTime 是否绝无可能为 0/0/0？
    若 Step 1 发现存在未初始化路径 → 🔴 哨兵会被正式流程误触发
    若已确保初始化 → ✅ 哨兵仅调试期生效，标记为"刻意为之"
```

### Step 7 — 输出报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · 日期/时间系统审计报告
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

历法常量：
  maxForDay = 42 ✅   月上限 = 4 ✅   起始年 = 221 ✅
  DateInfo：month 1~4 ✅ / 每月 day 1~42 连续 ✅
  网格容量 7×6=42 ✅（三处一致）

🔴 初始化（1 处）：
  UIMainCreate.Submit() 创建 new GameDataBean() 后未初始化 gameTime
    → 开局停留在 0/0/0：日历空白 + 被 NpcImportantBuilder 误判为测试模式
    → 建议：gameData.gameTime.SetTimeForYMD(221, 1, 0);

🟠 特殊日期（1 处）：
  CalendarView.SetBuildDay 比对建筑日时仅比 day+month，year 用了外层 year，
    跨年同月同日仍会被标"建"。请确认是否预期。

季节映射：✅ 4 季 case 齐全，month 强转点均保证 ∈[1,4]

哨兵 0/0/0（1 处）：
  NpcImportantBuilder.cs:83 — "测试模式默认生成所有 NPC"
    依赖"正式游戏永不为 0/0/0"。因上面初始化 bug，当前会被误触发 → 修复初始化后即安全。

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  总计问题：2 处（🔴 1 / 🟠 1）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 注意事项

- **只读 skill**：仅分析与给修复建议，不改任何代码、配置、场景。
- **以代码为准**：背景知识里的常量值是实测快照；审计时必须重新读取文件确认，若代码已变更，以代码为准并报告"常量已变化"。
- **Excel ↔ JSON**：`DateInfo.txt` 由 Excel 导出。若怀疑配置缺漏，应回到 Excel 源核对（联动 `/il-excel-sync` / `/il-localization-audit`）。
- **文本 ID**：季节/年份文本走 `TextHandler.GetTextById`（如 32/29 年份、33~36 季节）。文本缺失走 `/il-localization-audit`。
- **关联工具**：
  - 日历 UI 字段绑定 → `/il-ui-prefab-binder`
  - 场景初始化(进入游戏触发 EndDay→打开 UIGameDate) → `/il-scene-init-check`
  - 时间相关事件链 → `/il-event-flow-trace`
  - 诡异跨天/结算流程卡死 → `il-bug-hunter` agent
