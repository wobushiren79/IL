# il-excel-sync

**客栈传说 · Excel ↔ Bean 同步 Agent**

精确比对 Excel 表头字段与对应 Bean 类字段的差异，安全地执行新增/删除/类型修正，并同步维护 Partial 文件中的相关解析方法。

---

## 用法

```
/il-excel-sync <目标>
```

**目标** 可以是：
- Excel 文件名（含或不含路径和扩展名）
- Bean 类名（含或不含 Bean 后缀）

**示例：**
```
/il-excel-sync excel_text_talk[对话文本].xlsx
/il-excel-sync TextTalkBean
/il-excel-sync TextStoryBean
/il-excel-sync NpcInfoBean
/il-excel-sync excel_items_info[物品信息].xlsx
```

---

## 执行步骤

### Step 1 — 定位文件对

根据目标参数，同时找到：
1. `Assets/Data/Excel/` 下匹配的 `.xlsx` 文件
2. `Assets/Scripts/Bean/MVC/` 下匹配的 `*Bean.cs` 文件

**匹配逻辑：**
- 输入 Excel 文件名 → 从文件名中提取核心名称（如 `text_talk`），在 Bean 目录中搜索 `TextTalkBean.cs`
- 输入 Bean 类名 → 从 PascalCase 转 snake_case（如 `TextTalkBean` → `text_talk`），在 Excel 目录中搜索含 `text_talk` 的 xlsx 文件
- 若找到多个候选，列出让用户确认

### Step 2 — 读取 Excel 结构

读取 `.xlsx` 文件的**第一个 Sheet**：
- **第 1 行**：字段名（= SQLite 列名 = Bean 字段名）
- **第 2 行**（若有）：类型注释（`int` / `long` / `string` / `float`）

提取所有非空列，构建 `Map<字段名, 类型>` 表。

### Step 3 — 读取 Bean 现有字段

读取 `*Bean.cs` 主文件，提取所有 `public` 字段声明，排除：
- `[JsonIgnore]` 修饰的属性（多语言缓存属性，由 `LanguageCache` 派生，不是独立字段）
- `private` 字段（如 `_name_language`）
- 继承自 `BaseBean` 的字段（`id` 通常在基类中）

构建 Bean 字段 `Map<字段名, 类型>` 表。

### Step 4 — 差异比对

以表格形式展示完整对比：

| 字段名 | Excel 类型 | Bean 类型 | 状态 |
|---|---|---|---|
| `id` | long | long | ✅ 一致 |
| `valid` | int | int | ✅ 一致 |
| `name` | long | long | ✅ 一致（有 LanguageCache） |
| `new_field` | string | — | ⚠️ **待新增** |
| `old_field` | — | int | ⚠️ **待删除** |
| `type` | string | int | ❌ **类型不符** |

统计差异数量：`新增 X 项 / 删除 X 项 / 类型修正 X 项`

若无差异，输出"✅ Bean 与 Excel 完全一致，无需同步"并结束。

### Step 5 — 用户确认

若有差异，询问：
> 发现 X 处差异。是否执行同步？（可选择逐项确认或全部执行）

**注意：删除操作前额外确认**，因为删除字段可能导致已有存档数据丢失。

### Step 6 — 执行同步

按确认结果逐项处理：

#### 新增字段（追加到主文件末尾，`[JsonIgnore]` 字段之前）

```csharp
// 普通字段
public <类型> <字段名>;

// long 类型且名含 name/content/title/desc → 额外生成 LanguageCache 属性对
public long <字段名>;
[JsonIgnore]
public string <字段名>_language
{
    get => _<字段名>_language.Get(() => TextHandler.Instance.GetTextById(<BeanName>Cfg.fileName, <字段名>));
    set => _<字段名>_language.Set(value);
}
private LanguageCache _<字段名>_language;
```

同时检查 Partial 文件：
- `string` 类型且名含 `_ids`/`_types`/`_data` → 在 Partial 追加解析方法骨架 + `// TODO`

#### 删除字段（从主文件删除声明）

- 删除字段声明行
- 若有对应的 `LanguageCache` 属性对，一并删除
- 在 `*BeanPartial.cs` 中搜索使用该字段的方法体，在方法头部追加注释：
  ```csharp
  // TODO: 字段 <field_name> 已从 Excel 删除，请更新此方法
  ```
- **不直接删除 Partial 方法**（防止编译错误）

#### 类型修改

- 修改字段类型声明
- 若该字段有 `LanguageCache` 属性对，检查新类型是否仍为 `long`（否则删除 LanguageCache）

### Step 7 — 完成摘要

```
Excel 同步完成：
  Assets/Data/Excel/<excel文件名>
  ↕
  Assets/Scripts/Bean/MVC/<目录>/<Bean名>Bean.cs
  Assets/Scripts/Bean/MVC/<目录>/<Bean名>BeanPartial.cs

变更：
  + 新增字段: new_field (string) → Partial 中已添加 GetNewFieldList() 骨架
  + 新增字段: desc (long) → 已生成 LanguageCache 属性对 desc_language
  - 已删除字段: old_field (int)
  ~ 类型修改: type: int → string
  ⚠️ BeanPartial 中有 2 处 // TODO 需手动检查

注意：
  若已有存档数据中包含被删除字段的值，下次读档时将被忽略（SQLite 列不会自动删除）。
  若要彻底清理数据库，需在 SQLite 文件中手动执行 ALTER TABLE。
```
