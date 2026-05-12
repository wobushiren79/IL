# il-data-analyst

**客栈传说 · 游戏数据分析 Agent**

分析 Excel 配置表数据的完整性、合理性和引用关系，发现配置错误、孤立数据、数值平衡问题，为策划调优提供依据。

---

## 用法

```
/il-data-analyst <操作> [参数...]
```

**操作列表：**

| 操作 | 说明 |
|---|---|
| `validate <表名>` | 校验指定 Excel 表的数据完整性（必填项、类型、值域） |
| `ref-check <表名>` | 检查该表中所有 `*_id` 外键字段是否在对应表中存在 |
| `balance <表名> <数值字段>` | 对指定数值字段做统计分析（min/max/avg/分布） |
| `orphan <表名>` | 查找 `valid=0` 或未被任何地方引用的孤立数据行 |
| `diff <表名> <旧版本路径>` | 对比两个版本 Excel 的数据差异（新增/删除/修改的行） |
| `summary` | 输出所有 Excel 表的基本统计（行数、字段数、有效行数） |
| `text-coverage` | 检查所有文本 ID（name/content/desc 的 long 型字段值）在多语言文本表中的覆盖率 |

**示例：**
```
/il-data-analyst validate excel_npc_info[NPC信息].xlsx
/il-data-analyst ref-check excel_items_info[物品信息].xlsx
/il-data-analyst balance excel_skill_info[技能信息].xlsx damage
/il-data-analyst orphan excel_menu_info[菜单信息].xlsx
/il-data-analyst summary
/il-data-analyst text-coverage
```

---

## Excel 表速查（项目已知 30 张配置表）

| 文件名 | 用途 |
|---|---|
| excel_npc_info[NPC信息].xlsx | NPC 基础属性配置 |
| excel_npc_team[NPC队伍].xlsx | NPC 队伍组合 |
| excel_items_info[物品信息].xlsx | 物品基础信息 |
| excel_items_intact_info[物品完整信息].xlsx | 物品完整属性 |
| excel_menu_info[菜单信息].xlsx | 菜品菜单配置 |
| excel_skill_info[技能信息].xlsx | 技能数据 |
| excel_achievement_info[成就信息].xlsx | 成就配置 |
| excel_story_info[故事信息].xlsx | 故事/任务配置 |
| excel_story_info_details[故事信息详情].xlsx | 故事详情 |
| excel_text_story[故事文本].xlsx | 故事对话文本 |
| excel_text_talk[对话文本].xlsx | 普通对话文本 |
| excel_text_look[外观文本].xlsx | 外观描述文本 |
| excel_cooking_theme[烹饪主题].xlsx | 烹饪主题配置 |
| excel_build_item[建筑物品].xlsx | 建筑/装饰物品 |
| excel_seed_info[种子信息].xlsx | 种植种子配置 |
| excel_date_info[日期信息].xlsx | 节日/日期事件 |
| excel_weather_info[...天气信息].xlsx | 天气配置 |
| excel_store_info[商店信息].xlsx | 商店货架配置 |
| excel_language[多语言_FrameWork].xlsx | 框架多语言文本 |
| excel_ui_text[UI文本_FrameWork].xlsx | UI 界面文本 |

---

## 执行步骤

### 操作 `validate <表名>`

读取指定 `.xlsx` 文件，对每行数据执行以下校验：

1. **必填检查：** `id` 字段不为空，`valid` 字段存在且为 0 或 1
2. **类型检查：** 根据第 2 行类型注释，验证每个单元格值是否符合声明类型（int / long / string / float）
3. **值域检查（按字段名推断）：**
   - `valid` → 只能是 0 或 1
   - `*_time` → 应为正数
   - `level` → 通常在 1~99 范围
   - `*_price` / `*_cost` → 应为非负数
   - `*_id` 外键 → 应为正整数或 0（表示无引用）
4. **重复 ID 检查：** `id` 字段不允许重复

输出问题清单：
```
校验结果：excel_npc_info[NPC信息].xlsx
  总行数：45（有效：42，无效 valid=0：3）

  ⚠️ 第 12 行：age 字段类型不符（期望 int，实际为空）
  ⚠️ 第 18 行：valid 字段值为 2（期望 0 或 1）
  ❌ 第 27 行：id 重复（与第 15 行 id=1027 重复）
  ✅ 其余 42 行无问题
```

### 操作 `ref-check <表名>`

1. 读取目标表，找出所有 `*_id` 字段（外键字段，排除主键 `id`）
2. 根据字段名推断关联表（如 `npc_id` → `excel_npc_info`，`item_id` → `excel_items_info`）
3. 读取关联表，检查每个外键值是否在关联表的 `id` 列中存在且 `valid=1`

```
外键检查：excel_store_info[商店信息].xlsx
  item_id → excel_items_info[物品信息].xlsx
    ✅ 38 条记录全部有效
    ⚠️ item_id=1099（第 22 行）在物品表中不存在
    ⚠️ item_id=2003（第 31 行）在物品表中 valid=0（已下线物品）
```

### 操作 `balance <表名> <数值字段>`

对指定数值字段做统计分析：

```
数值分析：excel_skill_info[技能信息].xlsx → damage 字段

  有效行数：32
  最小值：  10（id=101, 普通攻击）
  最大值：  850（id=332, 终极技能）
  平均值：  245.6
  中位数：  180

  分布（按区间）：
    0~100：   5 条（15.6%）
    101~300：14 条（43.8%）
    301~500：  8 条（25.0%）
    501~1000：5 条（15.6%）

  ⚠️ 离群值（超过平均值 3 倍标准差）：
    id=332, damage=850
```

### 操作 `orphan <表名>`

找出以下两类"孤立"数据：
1. `valid=0` 的行（已软删除）
2. 有 `valid=1` 但未被任何其他表的 `*_id` 字段引用的行（孤立活跃记录）

```
孤立数据：excel_menu_info[菜单信息].xlsx

  valid=0（软删除，共 5 条）：id = 101, 205, 306, 412, 508

  未被引用（在其他表中找不到对应 id 的外键引用，共 3 条）：
    id=601（黑椒牛排）
    id=602（奶油蘑菇汤）
    id=603（水果拼盘）
  → 建议确认是否为新增待接入的菜品，或可 valid=0
```

### 操作 `summary`

读取所有 Excel 文件，输出汇总表：

```
Excel 配置表总览（共 30 张）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  表名                    总行数  有效行  字段数
  ──────────────────────────────────────
  excel_npc_info          45      42      18
  excel_items_info        128     120     22
  excel_menu_info         56      51      15
  excel_skill_info        32      32      12
  ...
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  合计：30 张表，1842 行有效数据
```

### 操作 `text-coverage`

扫描所有 Excel 表中 `long` 类型的 `name`/`content`/`title`/`desc` 字段值（这些是文本 ID），在 `excel_text_story`、`excel_text_talk`、`excel_language` 等文本表中检查覆盖情况：

```
文本 ID 覆盖率检查
  扫描到的文本 ID 总数：456
  ✅ 已覆盖：441（96.7%）
  ❌ 未找到对应文本（14 条）：
    ID 10234（来自 excel_npc_info 第 12 行 name 字段）
    ID 20156（来自 excel_skill_info 第 8 行 desc 字段）
    ...
```

---

## 注意事项

- 本 Agent 只做读取和分析，**不修改 Excel 文件**
- 外键关联表推断基于字段名惯例（`npc_id` → npc_info 表），若命名不规范可能误判
- `text-coverage` 依赖文本表的 `id` 列作为查找键
