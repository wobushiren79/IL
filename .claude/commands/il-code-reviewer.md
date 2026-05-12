# il-code-reviewer

**客栈传说 · 代码规范审查 Agent**

对新增或修改的代码进行架构一致性、命名规范、性能隐患、安全性和可维护性的全面审查，输出带优先级的问题清单和修改建议。

---

## 用法

```
/il-code-reviewer <目标>
```

**目标：**
- 文件路径：审查单个文件
- 目录路径：审查整个目录下的所有 `.cs` 文件
- `--staged`：审查 git staged 的所有变更文件
- `--diff`：审查当前分支相对 master 的所有变更
- 系统名：审查该系统的所有相关文件（Bean + Service + Handler + Manager + UI）

**示例：**
```
/il-code-reviewer Assets/Scripts/Component/Manager/Brewery/BreweryManager.cs
/il-code-reviewer Assets/Scripts/Bean/MVC/Brewery/
/il-code-reviewer --staged
/il-code-reviewer --diff
/il-code-reviewer Brewery
```

---

## 审查维度与检查项

### 维度 1 — 架构层级正确性（权重：高）

| 检查项 | 规则 |
|---|---|
| Bean 只含数据字段，不含业务逻辑 | Bean 文件中不应出现 `if`/`switch` 逻辑（Partial 中的解析方法除外） |
| Service 只做 CRUD | Service 方法中不应出现游戏逻辑、事件广播、Manager 调用 |
| Handler 极简 | Handler 只做单例路由，不写业务代码 |
| Manager 是业务核心 | Manager 直接操作 Service，不跨层访问 UI |
| UI 只读数据，不写数据 | UI View/ListItem 不应直接调用 `_service.InsertData()`，应通过 Manager |
| 单例访问规范 | 跨系统访问应通过 `XxxHandler.Instance.manager`，不直接 `new` Manager |

### 维度 2 — 命名规范（权重：中）

| 检查项 | 规则 |
|---|---|
| 类名 PascalCase | 所有类名使用 PascalCase |
| 私有字段 _camelCase | 私有字段以 `_` 开头小驼峰，如 `_service`、`_listData` |
| 公有字段 camelCase | 公有字段小驼峰，如 `mapBrewData`、`listRecipe` |
| Bean 字段 snake_case | Bean 中的数据字段使用 snake_case（匹配 SQLite 列名），如 `brew_time`、`recipe_id` |
| Inspector 绑定字段命名 | UI 中 Button 以 `bt` 前缀，Text 以 `tv`，Image 以 `iv`，GameObject 以 `obj` |
| Handler 文件名 | `<系统名>Handler.cs`，无前后缀 |
| Manager 文件名 | `<系统名>Manager.cs`，无前后缀 |
| Service 文件名 | `<系统名>Service.cs`，无前后缀 |

### 维度 3 — 性能隐患（权重：高）

| 检查项 | 规则 |
|---|---|
| Update() 中避免 GC 分配 | Update/FixedUpdate 中不应有 `new List<>`、字符串拼接、LINQ 查询 |
| 避免每帧 Find/GetComponent | `GameObject.Find()`、`GetComponent<>()` 不应在 Update 中调用 |
| Dictionary 查找优先于 List 遍历 | 按 ID 查找数据应用 `Dictionary.TryGetValue`，不用 `List.Find(lambda)` |
| Addressable 异步加载不阻塞主线程 | 资源加载应在回调中处理，不使用 `.WaitForCompletion()` |
| UI 刷新去重 | RefreshUI 中应有脏标记或在 `OpenUI()` 时刷新，避免每帧刷新 |

### 维度 4 — 多语言与文本（权重：中）

| 检查项 | 规则 |
|---|---|
| 不硬编码中文字符串到代码 | 代码中不应出现中文字面量赋值给 UI Text，应通过 `name_language` 属性 |
| LanguageCache 正确使用 | `long` 类型的 name/content 字段应配有 `LanguageCache` 属性对，而非每次调用 `TextHandler` |
| 文本 ID 不为 0 | Bean 中文本 ID 字段值为 0 时，`name_language` 会返回空字符串，应有空值保护 |

### 维度 5 — 存档安全性（权重：高）

| 检查项 | 规则 |
|---|---|
| 枚举值不可随意修改 | 不应出现对已有枚举项值的修改（如 `Idle=0` 改为 `Idle=2`） |
| Bean 字段不可随意删除 | 已有存档中保存了旧字段，删除字段会导致反序列化异常 |
| Service CRUD 操作有 null 检查 | `InsertData(null)` 会崩溃，应有防守性检查 |

### 维度 6 — 可维护性（权重：低）

| 检查项 | 规则 |
|---|---|
| TODO 注释不遗留 | `// TODO` 注释不应在完成功能后遗留 |
| 注释语言一致性 | 项目以中文注释为主，英文注释也可接受，但不应混用（同一方法内） |
| 不重复的 null 检查 | 同一变量在同一作用域内不需要多次 null 检查 |
| 避免空 catch | `catch` 块不应为空，至少要有 `LogUtil.LogError()` |

---

## 执行步骤

### Step 1 — 确定审查范围

- 单文件：直接读取
- 目录：Glob 搜索所有 `.cs` 文件
- `--staged`：运行 `git diff --cached --name-only` 获取文件列表
- `--diff`：运行 `git diff master...HEAD --name-only` 获取文件列表
- 系统名：搜索 Bean/Service/Handler/Manager/UI 目录下包含该系统名的文件

### Step 2 — 逐文件审查

对每个文件按六个维度逐项检查，记录发现的问题，标注：
- `🔴 严重`：会导致运行时崩溃或数据损坏
- `🟠 重要`：架构违规或性能隐患，应修复
- `🟡 建议`：命名或风格问题，建议修复
- `⚪ 提示`：可选优化，不强制

### Step 3 — 输出审查报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  代码审查报告：<目标>
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

BreweryManager.cs
  🔴 [存档安全] 第 45 行：InsertData(data) 无 null 检查，data 为空时崩溃
  🟠 [架构违规] 第 67 行：直接访问 InnHandler.Instance.manager._service，
         应通过 InnHandler.Instance.manager.GetXxx() 方法
  🟡 [命名规范] 第 23 行：私有字段 `listData` 应命名为 `_listData`
  ⚪ [可维护性] 第 89 行：TODO 注释疑似遗留，请确认是否已完成

BreweryBean.cs
  ✅ 无问题

BreweryService.cs
  🟡 [命名规范] 第 12 行：方法 `QueryAlldata()` 中 d 未大写，应为 `QueryAllData()`

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  总计：3 个文件，4 处问题（1 严重 / 1 重要 / 2 建议）
  建议优先修复：🔴 严重 1 处
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

### Step 4 — 询问是否修复

对于 `🔴 严重` 和 `🟠 重要` 问题，询问用户是否立即修复：
> 是否修复以上 2 处严重/重要问题？

若用户确认，逐项执行修改（使用 Edit 工具），修改后列出变更摘要。
