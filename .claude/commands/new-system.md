# new-system

为客栈传说项目搭建一个完整新系统的脚手架，按分层顺序依次创建所有文件。

## 用法

```
/new-system <系统名> <描述>
```

**示例：**
- `/new-system Brewery 酿酒系统：管理酒水配方（静态配置）与酿造进度（存档数据），支持玩家发起酿造、查看进度、完成领取`
- `/new-system Festival 节日系统：静态节日配置+活动奖励，运行时追踪当前节日状态`

## 执行步骤（严格按顺序，每步完成后继续）

### Step 1 — 数据层：配置 Bean（Cfg 模式，只读）

若系统有静态配置数据，调用内部逻辑等价于 `/new-cfg-bean` 命令：
- `Assets/Scripts/Bean/MVC/<系统名>/<系统名>InfoBean.cs` + Partial

### Step 2 — 数据层：存档 Bean（DB 模式，可读写）

若系统有运行时存档数据，调用内部逻辑等价于 `/new-db-bean` 命令：
- `Assets/Scripts/Bean/MVC/<系统名>/<系统名>Bean.cs` + Partial

### Step 3 — 数据层：Service

等价于 `/new-service` 命令：
- `Assets/Scripts/MVC/Service/<系统名>Service.cs`
- 根据是否有多语言副表选择模式 A / B

### Step 4 — 业务层：Handler + Manager

等价于 `/new-handler` 命令：
- `Assets/Scripts/Component/Handler/<系统名>/<系统名>Handler.cs`
- `Assets/Scripts/Component/Manager/<系统名>/<系统名>Manager.cs`
  - `Awake()` 实例化 Service
  - `InitData()` 从 Service/Cfg 加载数据到 Dictionary 缓存
  - 核心业务方法骨架（增删改查 + 触发事件）

### Step 5 — UI 层（询问用户是否需要）

如用户确认需要，等价于 `/new-ui-view` 命令：
- 主界面：`Assets/Scripts/Component/UI/Game/<系统名>View.cs`
- 列表项（若涉及列表）：`Assets/Scripts/Component/UI/ListItem/Game/Item<系统名>List.cs`

### Step 6 — 枚举项清单

输出需要**手动**添加的枚举项：

```
MsgEnum（建议新增）：
  MSG_<系统名大写>_UPDATE    // 数据变更通知 UI
  MSG_<系统名大写>_COMPLETE  // 操作完成（如酿造完成）

UIEnum / PopupEnum（如有 UI）：
  <系统名大写>              // UI 面板 ID
```

### Step 7 — 注册清单

打印所有已创建文件路径 + 需要手动注册的位置：

```
已创建文件：
  Assets/Scripts/Bean/MVC/<系统名>/...
  Assets/Scripts/MVC/Service/<系统名>Service.cs
  Assets/Scripts/Component/Handler/<系统名>/<系统名>Handler.cs
  Assets/Scripts/Component/Manager/<系统名>/<系统名>Manager.cs
  （可选）Assets/Scripts/Component/UI/...

手动注册：
  1. 将 <系统名>Handler 挂载到场景 Handlers 节点
  2. 在 GameSceneInit（或对应场景初始化）中调用
     <系统名>Handler.Instance.manager.InitData()
  3. 在 MsgEnum.cs 中添加事件枚举项
  4. 在 UIEnum.cs 中添加面板枚举项（如有 UI）
```
