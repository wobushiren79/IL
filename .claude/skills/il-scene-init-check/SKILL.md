---
name: il-scene-init-check
description: 客栈传说 · 场景 Handler 挂载与初始化检查。扫描 Unity 场景文件 (.unity YAML)，检查所有 Handler 是否挂载、是否在场景初始化脚本中调用 InitData()、是否注册了对应的 UIEnum 映射。新增 Handler 后或场景跑不通时使用。
---

# il-scene-init-check

**客栈传说 · 场景初始化完整性校验 Skill**

新增系统时，`<系统>Handler` 必须：
1. 作为 GameObject 挂载在场景中（通常挂在 `Handlers` 节点下）
2. 在场景初始化脚本（如 `GameSceneInit.cs`）中调用 `Handler.Instance.manager.InitData()`
3. 若有 UI，对应 `UIEnum` 已注册且 `UIHandler` 中已注册 Addressable 路径

任何一环遗漏都会导致系统"看起来代码完整但运行时静默失败"。本 skill 静态校验整条链路。

---

## 用法

```
/il-scene-init-check [范围]
```

**范围参数：**

| 范围 | 说明 |
|---|---|
| `all`（默认） | 全量审计所有场景与所有 Handler |
| `scene <场景名>` | 仅检查单个场景（如 `GameInnScene`） |
| `handler <Handler名>` | 仅检查单个 Handler 的注册情况 |
| `ui` | 仅检查 UI 注册完整性（UIEnum + Addressable） |
| `init-missing` | 仅找未调用 InitData 的 Handler |
| `mount-missing` | 仅找未挂载到场景的 Handler |

**示例：**
```
/il-scene-init-check
/il-scene-init-check scene GameInnScene
/il-scene-init-check handler BreweryHandler
/il-scene-init-check ui
/il-scene-init-check init-missing
```

---

## 背景知识

### 场景列表
| 场景 | 用途 |
|---|---|
| GameInnScene | 客栈主场景 |
| GameTownScene | 城镇场景 |
| GameArenaScene | 竞技场场景 |
| GameCourtyardScene | 庭院场景 |
| GameMountainScene | 山地探索 |
| GameInfiniteTowersScene | 无尽之塔 |

### 场景初始化脚本约定
每个场景通常有 `<场景名>Init.cs` 或 `<场景名>Handler.cs` 作为入口，负责：
- 调用所有相关 Handler 的 `InitData()`
- 拉起 UI 主面板

例：
```csharp
// Assets/Scripts/Component/Scenes/GameInnSceneInit.cs (假设)
void Start() {
    GameDataHandler.Instance.manager.InitData();
    InnHandler.Instance.manager.InitData();
    NpcInfoHandler.Instance.manager.InitData();
    BreweryHandler.Instance.manager.InitData();   // <-- 这一行漏了，酿酒系统就静默失败
    ...
}
```

### Handler 命名约定
- 文件：`Assets/Scripts/Component/Handler/<系统>/<系统>Handler.cs`
- 类继承：`BaseHandler<THandler, TManager>`
- 单例访问：`<系统>Handler.Instance`

### UIEnum 注册约定
- 文件：`Assets/Scripts/Enums/Base/UIEnum.cs`
- 每个 UI View 必须有对应枚举项
- `UIHandler` 内部维护 `Map<UIEnum, AddressableAddress>`，需手动注册

---

## 执行步骤

### Step 1 — 提取所有 Handler

Glob：`Assets/Scripts/Component/Handler/**/*Handler.cs`

对每个文件读取类声明，提取：
```
{
  name: BreweryHandler,
  file: 路径,
  manager: BreweryManager,
  hasInitData: bool   // 检查 manager 文件中是否有 public void InitData()
}
```

### Step 2 — 扫描场景文件挂载情况

`Assets/Scenes/*.unity` 是 YAML 文本，包含 `GameObject` + `MonoBehaviour` 组件，组件 `m_Script` 字段记录脚本 GUID。

**流程：**
1. 解析每个 `.unity` 文件，提取所有 `MonoBehaviour` 节点的 `m_Script: {fileID: -1, guid: <GUID>, type: 3}`
2. 通过 `<.cs>.meta` 文件中的 `guid:` 字段反查脚本类名
3. 建立 `sceneToHandlers : Map<scene_name, Set<HandlerClass>>`

> 提示：Unity YAML 解析建议直接 grep `m_Script:` 行 + 下一行的 `guid:`，不需要完整 YAML 库。

**对每个 Handler：**
```
mountedIn : Map<HandlerClass, List<scene>>
```
若 `mountedIn[handler]` 为空 → `🔴 未挂载到任何场景`

### Step 3 — InitData 调用追踪

在 `Assets/Scripts/Component/Scenes/` 与 `Assets/Scripts/Component/Manager/Scene/`（按已观察的目录结构）查找场景初始化脚本：
- 文件名含 `*SceneInit*` / `*SceneHandler*` / `*SceneManager*`

对每个初始化脚本 grep：
```regex
([A-Z]\w+Handler)\.Instance\.manager\.InitData\s*\(
```

建立：
```
initCalls : Map<scene_name, Set<HandlerClass>>
```

**审计：**
```
foreach scene:
  foreach handler in mountedIn-scene:
    if handler not in initCalls[scene]:
      -> 🟠 已挂载但未在场景 Init 中调用 InitData
```

> 注：若 Handler 在 Manager 的 `Awake()` 内自动调用 `InitData()`，需在该 Manager 中找到证据，否则按"未调"算。

### Step 4 — UI 注册完整性（操作 ui）

#### 4a. 收集 UI View 类
Glob：`Assets/Scripts/Component/UI/**/*.cs`，过滤继承 `BaseUIComponent` / `UIBaseOne` / `BaseUIView` / `DialogView` 的类。

#### 4b. 检查 UIEnum 注册
读取 `Assets/Scripts/Enums/Base/UIEnum.cs`、`DialogEnum.cs`、`PopupEnum.cs`，提取所有枚举名。

```
foreach uiClass:
  expected = uiClass 名去前缀 "UI"，转大写
  if expected not in UIEnum/DialogEnum/PopupEnum:
    -> 🟠 UI 类已存在但未在枚举中注册
```

#### 4c. 检查 UIHandler 中的地址映射
`UIHandler.cs` 中通常有：
```csharp
RegisterUI(UIEnum.BREWERY, "UI/UIBrewery");
```
grep 提取所有注册项，对比 UIEnum 中的枚举值是否都已注册。

#### 4d. 检查 Addressable 地址存在
联动 Addressable Group（参考 `il-addressable-audit`）：
- 注册的地址（如 `UI/UIBrewery`）必须在 Group 中实际存在
- 缺失 → `🔴 UIHandler 中注册的 Addressable 地址 'UI/UIBrewery' 在 Group 中找不到`

### Step 5 — 跨场景挂载冲突

某些 Handler 是「全局单例」，应该常驻一个 `DontDestroyOnLoad` 节点；某些是「场景级」，挂在每个具体场景。

诊断：
- 同一 Handler 出现在 2+ 场景 → 提示「请确认是单例预加载还是每场景独立」
- 仅出现在 1 个场景但被多个场景的初始化脚本调用 → `🔴 跨场景访问未挂载的 Handler`

### Step 6 — 输出报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · 场景初始化完整性报告
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

扫描：
  场景：6 个
  Handler：42 个
  UI View 类：87 个
  UIEnum 项：91 个

🔴 未挂载到任何场景（3 个 Handler）：
  - BreweryHandler            （file: .../BreweryHandler.cs）
    建议：将其拖入 GameInnScene 的 "Handlers" 节点
  - FestivalHandler
  - InfiniteTowerHandler

🟠 已挂载但未调用 InitData（5 处）：
  GameInnScene：
    BreweryHandler 已挂载，但 GameInnSceneInit.cs 中未发现
    BreweryHandler.Instance.manager.InitData() 调用
    → 建议在 GameInnSceneInit.cs 的 Start() 中追加：
       BreweryHandler.Instance.manager.InitData();

🔴 跨场景访问未挂载的 Handler（1 处）：
  TownSceneInit.cs:45 调用 BreweryHandler.Instance.manager.GetXxx()，
    但 BreweryHandler 仅挂载在 GameInnScene。从 Town 场景进入时
    会触发懒加载创建，可能导致 InitData 时机错误。

UI 注册检查：
  🟠 UI 已实现但未在 UIEnum 中注册（2 处）：
    - UIBrewery (BREWERY)
    - UIFestivalDetail (FESTIVAL_DETAIL)

  🔴 UIEnum 中注册了但 UIHandler 中无地址映射（3 处）：
    - UIEnum.OLD_DEPRECATED
    - UIEnum.NEW_FEATURE_X

  🔴 UIHandler 中地址在 Addressable Group 中不存在（1 处）：
    - UIHandler.cs:122 → "UI/UINewWindow"
      → 联动 /il-addressable-audit 修复

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  总计问题：13 处（🔴 严重 7 / 🟠 重要 6）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

### 操作 `handler <Handler名>` 单 Handler 详细

```
Handler 注册追踪：BreweryHandler

📁 文件：Assets/Scripts/Component/Handler/Brewery/BreweryHandler.cs
📁 Manager：BreweryManager
   public void InitData()  ✅ 已定义

🏠 场景挂载：
  ❌ 未挂载（建议挂在 GameInnScene/Handlers 节点）

🚀 InitData 调用：
  ❌ 在 6 个场景初始化脚本中均未发现调用

📋 跨系统访问：
  发现 5 处其他文件调用 BreweryHandler.Instance.manager.XXX：
    - UIBrewery.cs:45         GetAllRecipes()
    - AchievementHandler.cs:78  GetCompletedCount()
    ...
  → 若 Handler 未挂载，这些调用都会失败

待修复步骤：
  1. 在 GameInnScene 中找到 "Handlers" 节点，新建子 GameObject "BreweryHandler"，
     挂载 BreweryHandler 组件
  2. 在 GameInnSceneInit.cs 的 Start() 中追加：
       BreweryHandler.Instance.manager.InitData();
  3. 保存场景，提交 .unity 文件
```

---

## 注意事项

- **只读 skill**：不修改场景文件、不修改任何代码。Unity 场景修改必须在 Editor 中完成，本 skill 只输出待操作清单。
- **.unity YAML 解析**：场景文件可能数 MB，逐行 grep 即可，不需要完整解析。重点找 `m_Script:` 块。
- **GUID 反查**：建议建立 `<.cs.meta> → cs 文件` 索引一次，重用即可。
- **Handler 多挂载警告**：场景中同一 Handler 挂载多次会破坏单例语义，需重点警告。
- **场景初始化脚本路径不固定**：项目里既可能在 `Component/Scenes/` 也可能在 `Component/Manager/Scene/`，按全项目 grep `SceneInit` / `SceneStart` 找入口。
- **Editor-only Handler**：若 Handler 类有 `#if UNITY_EDITOR` 包裹，跳过审计。
- **关联工具**：
  - 新增 Handler 应配合 `/il-handler-gen` 生成
  - UI 注册问题可联动 `/il-addressable-audit`
  - 场景文件本身可用 `/il-unity-scene` 进一步操作
