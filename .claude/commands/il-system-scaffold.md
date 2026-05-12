# il-system-scaffold

**客栈传说 · 完整系统脚手架 Agent**

一条命令从零搭建一个完整游戏系统：Bean → Service → Handler/Manager → UI View → ListItem → 枚举清单 → 注册清单。
内部按顺序调用其他 `il-*` agent 的逻辑，完成后输出统一的集成摘要。

---

## 用法

```
/il-system-scaffold <系统名> <描述>
```

**描述** 应涵盖以下信息（自然语言，尽量完整）：
- 系统功能
- 是否有静态配置数据（cfg）和/或运行时存档数据（db）
- 是否需要多语言（name/title 字段）
- 是否需要 UI 界面
- 是否有列表展示需求

**示例：**
```
/il-system-scaffold Brewery 酿酒系统：有静态配方配置(id/name/material_ids/output_id/brew_time)，有运行时酿造进度存档(id/recipe_id/start_time/status)，需要完整 UI 界面展示配方列表和酿造进度
/il-system-scaffold Festival 节日系统：只有静态节日配置(id/name/type/start_day/end_day/reward_ids)，不需要存档，需要简单 UI 弹窗
/il-system-scaffold InnRoom 客房系统：只有存档数据(id/name/capacity/level/unlock_price)，需要房间列表 UI
```

---

## 执行步骤（严格顺序，逐步完成）

### Step 1 — 解析需求

从描述中提取：

| 项目 | 判断逻辑 |
|---|---|
| **有 cfg Bean？** | 描述含"静态配置"/"配方"/"配置表"/"cfg" |
| **有 db Bean？** | 描述含"存档"/"进度"/"运行时"/"保存"/"db" |
| **需要 UI？** | 描述含"UI"/"界面"/"展示" |
| **需要列表？** | 描述含"列表"/"list"/"多条" |
| **有多语言？** | 字段名含 name/title/content/desc |
| **副表？** | 有多语言 AND 为 db 模式 |

输出解析结果表格让用户确认，然后继续。

---

### Step 2 — 数据层：Cfg Bean（若有 cfg）

等价于执行 `/il-bean-gen cfg <系统名>Info <文件名> <字段列表>`

创建：
- `Assets/Scripts/Bean/MVC/<系统目录>/<系统名>InfoBean.cs`
- `Assets/Scripts/Bean/MVC/<系统目录>/<系统名>InfoBeanPartial.cs`

---

### Step 3 — 数据层：DB Bean（若有 db）

等价于执行 `/il-bean-gen db <系统名> <主表名> <字段列表>`

创建：
- `Assets/Scripts/Bean/MVC/<系统目录>/<系统名>Bean.cs`
- `Assets/Scripts/Bean/MVC/<系统目录>/<系统名>BeanPartial.cs`

---

### Step 4 — 数据层：Service

等价于执行 `/il-service-gen <系统名>Service <主表名> [副表名]`

创建：
- `Assets/Scripts/MVC/Service/<系统名>Service.cs`

模式选择：
- 有多语言副表 → 模式 B（双表 JOIN）
- 有 --dynamic 需求 → 模式 C
- 默认 → 模式 A（单表）

若同时有 cfg 和 db 数据，cfg 数据不需要 Service（走 Cfg 静态类），只为 db 数据生成 Service。

---

### Step 5 — 业务层：Handler + Manager

等价于执行 `/il-handler-gen <系统名> <描述关键词>`

创建：
- `Assets/Scripts/Component/Handler/<系统名>/<系统名>Handler.cs`
- `Assets/Scripts/Component/Manager/<系统名>/<系统名>Manager.cs`

Manager 内容：
- `Awake()`：实例化 Service
- `InitData()`：加载 cfg 数据到 Dictionary / 从 Service 加载 db 数据
- 核心 CRUD 方法骨架
- 若描述含"存档" → 额外生成 `SaveData()`
- 若描述含"事件" → 操作方法末尾有 EventHandler 广播骨架

---

### Step 6 — UI 层（若需要 UI）

**主界面：**
等价于执行 `/il-ui-view-gen UI<系统名> full Game [--with-list]`

创建：
- `Assets/Scripts/Component/UI/Game/UI<系统名>.cs`

**列表项（若需要列表）：**
等价于执行 `/il-ui-item-gen Item<系统名>List <Bean名>Bean Game --click`

创建：
- `Assets/Scripts/Component/UI/ListItem/Game/Item<系统名>List.cs`

---

### Step 7 — 枚举清单

输出需要**手动添加**的枚举项（不自动修改枚举文件，防止值冲突）：

```
待添加到 MsgEnum.cs：
  MSG_<系统名大写>_UPDATE   = <建议值>  // 数据变更，通知 UI 刷新
  MSG_<系统名大写>_COMPLETE = <建议值>  // 操作完成（如酿造完成）

待添加到 UIEnum.cs（若有 UI）：
  <系统名大写> = <建议值>              // 主界面 ID

待添加到新枚举文件（若有状态字段）：
  <系统名>StatusEnum：
    None    = 0
    <状态1> = 1
    <状态2> = 2
```

> 建议值基于当前枚举文件最大值 +1，或按系统分段建议（需读取枚举文件后确认）。

---

### Step 8 — 集成注册清单

打印所有已创建文件路径，以及所有**必须手动完成**的注册操作：

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · <系统名>系统 · 文件生成完毕
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

已创建文件：
  ✅ Assets/Scripts/Bean/MVC/<目录>/<系统名>InfoBean.cs
  ✅ Assets/Scripts/Bean/MVC/<目录>/<系统名>InfoBeanPartial.cs
  ✅ Assets/Scripts/Bean/MVC/<目录>/<系统名>Bean.cs          （若有 db）
  ✅ Assets/Scripts/Bean/MVC/<目录>/<系统名>BeanPartial.cs   （若有 db）
  ✅ Assets/Scripts/MVC/Service/<系统名>Service.cs
  ✅ Assets/Scripts/Component/Handler/<系统名>/<系统名>Handler.cs
  ✅ Assets/Scripts/Component/Manager/<系统名>/<系统名>Manager.cs
  ✅ Assets/Scripts/Component/UI/Game/UI<系统名>.cs           （若有 UI）
  ✅ Assets/Scripts/Component/UI/ListItem/Game/Item<系统名>List.cs（若有列表）

必须手动完成：
  [ ] 1. 将 <系统名>Handler 挂载到场景 "Handlers" 节点
  [ ] 2. 在 GameSceneInit.cs 中调用 <系统名>Handler.Instance.manager.InitData()
  [ ] 3. 在 MsgEnum.cs 中追加 MSG_<系统名大写>_UPDATE / COMPLETE
  [ ] 4. 在 UIEnum.cs 中追加 <系统名大写>（若有 UI）
  [ ] 5. 在 UIHandler 中注册 UI<系统名> 的 Addressable 路径
  [ ] 6. 在 Excel 中创建对应配置表，执行 /il-excel-sync 验证字段
  [ ] 7. 补全 BeanPartial.cs 中的 // TODO 解析方法
  [ ] 8. 检查 Manager 中事件广播枚举值是否已填入
```
