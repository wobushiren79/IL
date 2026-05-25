---
name: il-localization-audit
description: 客栈传说 · 多语言文本审计。检查 Excel 文本 ID 在多语言文本表中的覆盖率、Bean 中 LanguageCache 字段使用规范、代码中硬编码中文字符串、TextEnum 注册情况。上线前与新增系统时使用。
---

# il-localization-audit

**客栈传说 · 多语言（i18n）审计 Skill**

项目使用 `Excel 文本ID(long) → SQLite text_*表 → TextHandler.GetTextById() → LanguageCache 属性` 的多语言机制。本 skill 静态审计整条链路，找出断点和泄漏。

---

## 用法

```
/il-localization-audit [范围]
```

**范围参数：**

| 范围 | 说明 |
|---|---|
| `all`（默认） | 全量审计 |
| `coverage` | 仅检查 Excel 数据表中文本 ID 在多语言表中的覆盖率 |
| `bean` | 仅检查 Bean 中 `long name` 类字段是否配 `LanguageCache` 属性 |
| `hardcode` | 仅扫描代码中硬编码中文字符串（漏未本地化） |
| `enum` | 仅检查 `TextEnum.cs` 中的 ID 在文本表中是否存在 |
| `orphan` | 仅查找文本表中未被任何 Excel/代码引用的孤立文本 |

**示例：**
```
/il-localization-audit
/il-localization-audit coverage
/il-localization-audit hardcode
/il-localization-audit bean
```

---

## 背景知识

### 多语言机制（项目固定模式）

```
[Excel 配置表]                       [SQLite 文本表]
excel_npc_info[NPC信息].xlsx          excel_text_talk[对话文本].xlsx
  id   valid   name(long) ...          id   valid   content(zh)   content(en) ...
  101  1       10101                   10101 1      "店小二"        "Waiter"
                |
                +---------> long 字段值就是文本 ID

[Bean 主文件] Assets/Scripts/Bean/MVC/Character/NpcInfoBean.cs
  public long name;                       // 数据库字段
  [JsonIgnore]
  public string name_language
  {
    get => _name_language.Get(() => TextHandler.Instance.GetTextById(NpcInfoCfg.fileName, name));
    set => _name_language.Set(value);
  }
  private LanguageCache _name_language;   // 缓存避免重复查询

[UI 使用]
  tvName.text = npcBean.name_language;    // 自动按当前语言取值
```

### TextHandler 调用入口
- `TextHandler.Instance.GetTextById(fileName, textId)` —— 按表名 + ID 取多语言文本
- `TextEnum` —— 文本 ID 常量类（避免代码里散落数字字面量）
- 多语言切换：`GameDataHandler.Instance.manager.GetGameConfig().language`

### 文本表分类
| 表 | 用途 |
|---|---|
| excel_text_talk[对话文本].xlsx | 对话/剧情文本 |
| excel_text_story[故事文本].xlsx | 故事任务文本 |
| excel_text_look[外观文本].xlsx | 外观描述 |
| excel_language[多语言_FrameWork].xlsx | 框架通用文本 |
| excel_ui_text[UI文本_FrameWork].xlsx | UI 按钮/提示 |

---

## 执行步骤

### Step 1 — 建立文本 ID 索引

读取所有 `excel_text_*.xlsx` 与 `excel_language*.xlsx`、`excel_ui_text*.xlsx`，建立：
```
allTextIds : Set<long>       — 全部已注册的文本 ID
textIdToTable : Map<long, string>  — ID 来源表名
textIdToZh : Map<long, string>     — ID 对应中文（用于硬编码反查）
```

> 这些 Excel 在 `Assets/Data/Excel/` 下，第一行字段名，第二行类型，第三行起为数据。

### Step 2 — Excel 配置表文本 ID 提取（操作 coverage）

扫描 `Assets/Data/Excel/excel_*.xlsx`（**排除** `excel_text_*` 自身和 `excel_language*`、`excel_ui_text*`），找出：
- 字段名包含 `name` / `content` / `title` / `desc` / `text` 且
- 类型为 `long`

记录每个文本 ID 引用：`{excel: 表名, row: 行号, field: 字段名, value: ID}`。

**审计：** 引用了但 `allTextIds` 中不存在 → 标记为「未注册文本 ID」。

> 文本 ID 值为 0 视为"未填写"，列为警告而非错误。

### Step 3 — Bean LanguageCache 一致性（操作 bean）

扫描 `Assets/Scripts/Bean/MVC/**/*Bean.cs`（**不**含 Partial），匹配：
- `public long <name>;` 且 `<name>` 含 `name`/`content`/`title`/`desc` 之一
- 检查同文件是否有对应的 `<name>_language` 属性（getter 走 `TextHandler.GetTextById`）和 `private LanguageCache _<name>_language;`

**审计：**
- 有 long 字段但**缺**对应属性对 → `🟠 缺失 LanguageCache 属性`
- 有 `_xxx_language` 但 long 字段为非 `long` 类型（如 string）→ `🔴 类型不匹配`
- 有 `_xxx_language` 但其 getter 中调用的 `Cfg.fileName` 不是当前 Bean 对应的文件名 → `🟠 文件名错配`

### Step 4 — 代码硬编码中文扫描（操作 hardcode）

在 `Assets/Scripts/**/*.cs` 与 `Assets/FrameWork/Scripts/**/*.cs` 中 grep：
```regex
"[^"\n]*[一-鿿]+[^"\n]*"
```

对每个匹配的中文字面量：
1. 若位于 `Debug.Log`/`LogUtil.LogXxx` 内 → 列为 `⚪ 提示`（日志可保留中文）
2. 若位于 `// ` 或 `/* */` 注释中 → 跳过
3. 若位于 UI 赋值（`tv*.text = "..."`、`SetText("...")`、`.text = "..."`）→ `🔴 严重`
4. 若位于错误/异常消息（`throw new Exception("...")`）→ `🟡 建议`
5. 若位于常量定义（`const string XXX = "..."`、`public static readonly`）→ `🟠 重要`
6. 反查 `textIdToZh` 是否已有相同中文 → 提示用「该字符串已存在于 text 表 ID=xxx」

### Step 5 — TextEnum 注册检查（操作 enum）

读取 `Assets/Scripts/Enums/TextEnum.cs`，提取所有 `<NAME> = <ID>` 项：
- ID 不在 `allTextIds` 中 → `🔴 TextEnum 指向不存在的文本`
- 同一 ID 出现多次 → `🟠 重复定义`
- 命名规范（如要求全大写下划线）违反 → `🟡 命名建议`

### Step 6 — 孤立文本检查（操作 orphan）

`allTextIds - (allExcelRefs ∪ allCodeRefs ∪ allTextEnumRefs)` = 未被任何地方引用的文本：
- 在文本表中 `valid=1` 但全项目无引用 → 候选「可删除」（**不**自动删除，仅报告）
- 提示：动态拼接 ID（`textIdBase + offset`）的代码无法静态分析，需人工确认

### Step 7 — 输出报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · 多语言审计报告
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

文本表统计：
  excel_text_talk     1842 条
  excel_text_story    523  条
  excel_language       89  条
  excel_ui_text       210  条
  合计：2664 条已注册文本 ID

Excel 数据表引用：
  扫描 26 张 Excel，提取 1287 处文本 ID 引用
  ❌ 未注册 ID（15 处）：
     - excel_npc_info 第 12 行 name=10999    （文本表中无对应）
     - excel_menu_info 第 8  行 desc=20888
     ...
  ⚠️ 文本 ID = 0（38 处，可能为未填写）

Bean LanguageCache 检查：
  扫描 187 个 Bean 文件
  🟠 缺失 LanguageCache（5 处）：
     - Assets/Scripts/Bean/MVC/Inn/InnRoomBean.cs: long title 缺属性对
     ...
  🔴 类型/文件名错配（1 处）：
     - Assets/Scripts/Bean/MVC/Menu/MenuInfoBean.cs:23
       _desc_language 的 getter 使用 NpcInfoCfg.fileName，应为 MenuInfoCfg.fileName

代码硬编码中文：
  🔴 UI 文本直接赋值（12 处）：
     - Assets/Scripts/Component/UI/Game/UIBrewery.cs:88
       tvTitle.text = "酿酒"
       → 建议：迁移到 excel_ui_text，使用 TextHandler.Instance.GetTextById()
  🟠 常量字符串（3 处）：
     - Assets/Scripts/Component/Manager/.../XxxManager.cs:45
       private const string MSG = "客栈已满"
  🟡 异常消息（2 处）
  ⚪ 日志（28 处，可保留）

TextEnum 检查：
  扫描 89 个枚举项
  🔴 指向不存在的文本（2 处）：
     - TextEnum.NPC_GREET_DEFAULT = 99001  （文本表中无此 ID）
  🟠 重复定义（1 处）：
     - TextEnum.TIP_OK 与 TextEnum.CONFIRM 都指向 ID=10001

孤立文本（仅 valid=1 且无引用）：
  共 47 条，前 10 条：
     - ID 12345（excel_text_talk）"过时的对话文本"
     ...
  ⚠️ 提示：以下文本 ID 用于代码动态拼接，已自动排除：
     - TextHandler.Instance.GetTextById(file, baseId + offset)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  总计问题：38 处（🔴 严重 15 / 🟠 重要 9 / 🟡 建议 2 / ⚪ 提示 12）
  孤立文本：47 条（候选清理）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 注意事项

- **只读 skill**：不修改任何 Excel、Bean、文本表或代码。修复建议交由用户决定。
- **Excel 解析**：使用 EPPlus 不现实（无 Unity 上下文），改用 Python 第三方库或直接 unzip xlsx 解析 `xl/sharedStrings.xml`。若环境无 Python，提示用户在 Excel 中导出 CSV 后再分析。
- **动态文本 ID**：代码中 `GetTextById(file, idVar)` 这类动态引用无法静态判定，单独列出"未确认"区。
- **多语言完整性**：本 skill 只检查 ID 注册情况，**不**检查每种语言列是否都已翻译。语言翻译完整性需另一个工具（见 `/il-data-analyst text-coverage`）。
- **与 `/il-data-analyst text-coverage` 区别**：那个 command 偏向 Excel 单向覆盖率，本 skill 全链路（Excel + Bean + Code + Enum）一起扫。
- **TextEnum 路径推断**：若 `TextEnum.cs` 实际不存在或命名不同（如 `TextIdEnum`），按 Glob 在 `Assets/Scripts/Enums/` 下找一次后缓存。
