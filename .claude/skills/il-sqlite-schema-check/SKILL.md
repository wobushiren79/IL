---
name: il-sqlite-schema-check
description: 客栈传说 · SQLite 表结构与 DB Bean 字段一致性审计。读取 StreamingAssets/SQLiteDataBase 中的 .db 文件 schema，对比 Assets/Scripts/Bean/MVC 下 Bean 字段，找出类型不符、字段缺失、表缺失、孤立字段。新增/修改数据库表后或上线前使用。
---

# il-sqlite-schema-check

**客栈传说 · SQLite ↔ Bean 一致性审计 Skill**

项目数据库走 SQLite，Bean 通过反射映射列名（`ReflexUtil`）。当 Excel/数据库表结构变化但 Bean 未同步（或反过来）时，会在运行时 SELECT/INSERT 出现"找不到列"或"类型转换异常"。本 skill 通过解析 `.db` 文件 schema，与 Bean 字段做静态对比。

---

## 用法

```
/il-sqlite-schema-check [范围] [选项]
```

**范围参数：**

| 范围 | 说明 |
|---|---|
| `all`（默认） | 扫描所有 .db 与所有 Bean，全量对比 |
| `db <文件名>` | 仅检查单个 .db 文件 |
| `bean <Bean名>` | 仅检查单个 Bean 对应的表 |
| `table <表名>` | 仅检查单个表 |
| `missing-cols` | 仅找 Bean 中有但表中无的字段（运行时 INSERT 报错） |
| `missing-fields` | 仅找表中有但 Bean 中无的列（数据丢失） |
| `type-mismatch` | 仅找类型不匹配 |
| `orphan-tables` | 仅找表存在但无 Bean / Bean 存在但无表 |

**选项：**
- `--include-cfg` ：也对比 Cfg Bean（从 JSON 读取的静态配置 Bean，默认不对比）
- `--snake-case` ：严格要求字段名为 snake_case（默认仅警告）

**示例：**
```
/il-sqlite-schema-check
/il-sqlite-schema-check db game_data.db
/il-sqlite-schema-check bean InnRoomBean
/il-sqlite-schema-check missing-cols
/il-sqlite-schema-check type-mismatch
```

---

## 背景知识

### 数据库文件位置
```
Assets/StreamingAssets/SQLiteDataBase/
  ├── <DATA_BASE_INFO_NAME>.db    # 主存档数据库
  └── ...
```
具体名称由 `ProjectConfigInfo.DATA_BASE_INFO_NAME` 配置（需读取该常量确认）。

### Bean → 表映射规则
- `<XxxBean>` 类 → 表名 = Service 构造时传入的字符串（如 `inn_room`）
- 字段名 = SQLite 列名（snake_case）
- C# 类型 → SQLite 类型对照：

| C# 类型 | SQLite 类型 |
|---|---|
| `int` | INTEGER |
| `long` | INTEGER |
| `string` | TEXT |
| `float` / `double` | REAL |
| `bool` | INTEGER (0/1) |

### 反射注入机制
`ReflexUtil.GetFieldsAndValues(bean)` 通过反射读取 Bean 的 public 字段名 → 拼接 SQL。字段名错配 / 类型错配会在运行时抛 SQLiteException。

---

## 执行步骤

### Step 1 — 定位 .db 文件与 schema 提取

通过 Glob 找 `Assets/StreamingAssets/SQLiteDataBase/*.db`。

**Schema 提取方案：**

方案 A（推荐）—— 使用 `sqlite3` CLI：
```bash
sqlite3 game_data.db ".schema"
sqlite3 game_data.db "SELECT name, sql FROM sqlite_master WHERE type='table';"
```

方案 B —— 用 Python 解析（环境无 sqlite3 时）：
```python
import sqlite3
conn = sqlite3.connect("game_data.db")
for row in conn.execute("SELECT name FROM sqlite_master WHERE type='table'"):
    table = row[0]
    cols = conn.execute(f"PRAGMA table_info({table})").fetchall()
    # col: (cid, name, type, notnull, dflt_value, pk)
```

**输出结构：**
```
schemaMap : Map<table_name, List<{ col_name, col_type, not_null, pk, default }>>
```

### Step 2 — 找 Bean ↔ 表映射

#### 2a. 找所有 Service 文件，解析构造函数提取表名

`Assets/Scripts/MVC/Service/*.cs` 中 grep：
```regex
public\s+\w+Service\s*\([^)]*\)\s*:\s*base\s*\(\s*"([^"]+)"
```

得到 `serviceToTable : Map<ServiceClass, table_name>` 以及双表的 leftTable。

#### 2b. 从 Bean 文件名/类名推断对应 Service

- `InnRoomBean` → `InnRoomService` → 表名 `inn_room`
- `NpcInfoBean` → `NpcInfoService` → 表名 `npc_info`

若 Bean 找不到匹配的 Service，记录为「无 Service 的 Bean」（可能是 Cfg Bean 走 JSON）。

#### 2c. 提取 Bean 字段

```regex
public\s+(int|long|string|float|double|bool)\s+([a-z_][a-z0-9_]*)\s*;
```

只匹配 **非 partial 的主 Bean 文件**（Partial 中无字段定义），且字段不在 `[JsonIgnore]` 上方。

输出：
```
beanFields : Map<BeanClass, List<{ field_name, field_type, file, line }>>
```

### Step 3 — 三向对比

对每个 `(Bean, Service, Table)` 三元组：

#### missing-cols：Bean 有 / 表无
```
foreach field in beanFields[bean]:
  if field.name not in schemaMap[table]:
    -> 🔴 严重：INSERT/UPDATE 会失败
        "BeanFile.cs:23 字段 'brew_speed' 未在表 'brewery' 中定义"
```

#### missing-fields：表有 / Bean 无
```
foreach col in schemaMap[table]:
  if col.name not in beanFields[bean]:
    -> 🟠 重要：SELECT * 后该列数据丢失（除非 SELECT 列名显式列出）
        "表 'brewery' 列 'last_modify_time' 在 BreweryBean 中无对应字段"
```

#### type-mismatch：类型对不上
```
foreach field in beanFields[bean]:
  if field.name in schemaMap[table]:
    sqlType = schemaMap[table][field.name].col_type
    expected = csharpTypeToSqlite(field.type)
    if sqlType != expected:
      -> 🔴 严重：运行时类型转换异常
```

#### snake_case 检查
```
foreach field in beanFields[bean]:
  if field.name contains uppercase OR field.name starts with digit:
    -> 🟡 命名违规
```

#### orphan-tables / orphan-beans
```
tablesWithoutBean : schemaMap.keys - serviceTables
beansWithoutTable : beanFields.keys - (serviceBeans ∪ cfgBeans)
```

### Step 4 — 输出报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · SQLite Schema 审计报告
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

扫描：
  .db 文件：1 个 (game_data.db)
  表：68 张
  Bean：123 个（DB 类：87 个，跳过 Cfg：36 个）
  Service：54 个

🔴 字段缺失（Bean 有但表无，共 4 处）：
  BreweryBean.cs:23  字段 'brew_speed' (int)
    → 表 'brewery' 中无此列；INSERT 时 ReflexUtil 会传入未知列，SQLite 抛
       "no such column" 异常
    建议：在 Excel 中追加 brew_speed 列后跑 /il-excel-sync，或从 Bean 中移除该字段

🟠 字段丢失（表有但 Bean 无，共 7 处）：
  表 'inn_room' 列 'last_modify_time' (INTEGER)
    → InnRoomBean 中无对应字段，SELECT 后该列被忽略，UPDATE 时该列保持原值
    建议：若不再使用此列，从表删除；若仍需使用，在 Bean 中补字段

🔴 类型不匹配（共 2 处）：
  CookingThemeBean.cs:18  字段 'unlock_day' (string)
    → 表中类型为 INTEGER，运行时反射赋值会抛 InvalidCastException
    建议：将 Bean 字段改为 int

🟡 命名违规（snake_case 违规，共 3 处）：
  CharacterBean.cs:45  字段 'lastLoginTime' 应为 'last_login_time'

🗑️ 孤立表（表存在但无 Service / Bean，共 2 张）：
  - test_temp_data         （疑似废弃测试表，建议清理）
  - legacy_user_settings

🗑️ 孤立 Bean（DB Bean 但无对应表，共 1 个）：
  - PlannedFeatureBean     （尚未在数据库建表？请创建表或转为 Cfg）

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  总计问题：17 处（🔴 严重 6 / 🟠 重要 7 / 🟡 建议 3 / 🗑️ 清理 3）
  建议优先修复 🔴 严重项
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

### 操作 `bean <Bean名>` 单 Bean 详细对比

```
Bean ↔ Table 对照：InnRoomBean ↔ inn_room

字段对比：
  Bean 字段              C# 类型     表列            SQLite 类型     状态
  ──────────────────────────────────────────────────────────────────
  id                     long       id              INTEGER PK     ✅
  valid                  int        valid           INTEGER        ✅
  name                   string     name            TEXT           ✅
  capacity               int        capacity        INTEGER        ✅
  level                  int        level           INTEGER        ✅
  unlock_price           int        unlock_price    INTEGER        ✅
  ─                      ─          last_modify_time INTEGER       🟠 Bean 缺
  brew_speed             int        ─               ─              🔴 表缺

主键：id  ✅
副表（如有）：—
```

---

## 注意事项

- **只读 skill**：不修改 Bean、表或 schema。修改 schema 必须在 Unity Editor 通过 SQLite 工具或迁移脚本完成。
- **运行时数据库 vs 模板数据库**：`StreamingAssets/SQLiteDataBase/*.db` 是初始模板。玩家存档可能位于 `Application.persistentDataPath/<DB_NAME>.db`，本 skill 只检查 StreamingAssets 中的模板，因为那是版本控制中的"权威源"。
- **副表（多语言）**：Mode B 的 Service 有 `tableNameForLeft`，多语言副表名按 `<前缀>_<language>` 规则。审计时认为 `xxx_zh`、`xxx_en` 等是同一逻辑副表，取 `xxx_zh` 做对比即可。
- **动态表名 Service**：Mode C 的 Service 表名在运行时切换，无法静态判定对应 Bean。在报告中单独列出。
- **`#if UNITY_EDITOR` 字段**：仅编辑器存在的字段不应进 DB，若被检测到，报警告。
- **关联工具**：
  - 字段不匹配可联动 `/excel-sync <Bean>` 从 Excel 同步
  - 类型变更涉及存档迁移：建议手写 SQL ALTER 脚本，本 skill 不自动生成
