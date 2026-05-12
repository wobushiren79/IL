# excel-sync

对比 Excel 表头与对应 Bean 字段的差异并同步更新。

## 用法

```
/excel-sync <excel文件名或Bean类名>
```

**示例：**
- `/excel-sync excel_text_talk[对话文本].xlsx`
- `/excel-sync TextTalkBean`
- `/excel-sync TextStoryBean`

## 执行步骤

### Step 1 — 定位文件

1. 在 `Assets/Data/Excel/` 下找到对应 `.xlsx` 文件
2. 在 `Assets/Scripts/Bean/MVC/` 下找到对应 `*Bean.cs`

### Step 2 — 读取 Excel 结构

使用文件读取工具检查 `.xlsx`，重点看：
- 第 1 行：字段名（= SQLite 列名 = Bean 字段名）
- 第 2 行（若有）：数据类型注释（int / long / string / float）

### Step 3 — 读取 Bean 现有字段

读取 `*Bean.cs` 主文件，列出所有 `public` 字段（排除 `[JsonIgnore]` 的语言缓存属性）。

### Step 4 — 差异比对输出

以表格形式展示：

| 字段名 | Excel | Bean | 状态 |
|---|---|---|---|
| `id` | long | long | ✅ 一致 |
| `valid` | int | int | ✅ 一致 |
| `new_field` | string | — | ⚠️ 待新增 |
| `old_field` | — | int | ⚠️ 待删除 |
| `type` | string | int | ❌ 类型不符 |

### Step 5 — 用户确认后执行

只修改用户确认的项，规则：

**新增字段：**
- 追加到已有字段末尾（`[JsonIgnore]` 字段之前）
- `long` 类型且名含 `name`/`content`/`title` → 同时生成 `LanguageCache` 属性对
- `string` 类型且名含 `_ids`/`_data` → 在 `*BeanPartial.cs` 中追加解析方法骨架 + `// TODO` 注释

**删除字段：**
- 主文件删除字段声明及对应 `LanguageCache`
- Partial 文件中使用该字段的方法体加 `// TODO: 字段已删除，请更新此方法` 注释（不直接删方法，防止编译错误）

**类型修改：**
- 修改字段类型声明
- 若有 `LanguageCache` 属性对，同步检查类型是否仍匹配

### Step 6 — 完成摘要

```
已同步:
  + 新增字段: new_field (string)
  - 已删除字段: old_field
  ~ 类型修改: type: int → string
  ⚠️ Partial 中有 2 处需手动检查（已标注 // TODO）
```
