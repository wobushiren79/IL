---
name: excel-io
description: 使用 openpyxl 读取和写入 Excel (.xlsx) 配置表。当需要直接读取或修改 Assets/Data/Excel/ 下的配置表数据时使用此 SKILL。
tools: Read, Write, Edit, Glob, Grep, Bash
---

# Excel 读写操作指南 (openpyxl)

## 核心规则

- **唯一允许的库**：`openpyxl`，禁止使用 `xlrd`、`xlwt`、`xlwings`、`pandas.read_excel`
- **编码**：UTF-8（openpyxl 默认行为，无需额外指定）
- **脚本存放位置**：`.claude/scripts/`
- **写入前必须确认**：修改配置表前先读取确认内容，写入后告知用户已变更的行列
- **写入前建议加 `--backup`**：自动生成 `*.bak`；任务完成后视情况清理或保留
- **新增数据按 id 升序插入**：新增配置行时，不要直接追加到表格末尾，而要根据 id 由小到大插入到中间对应位置，保证整张表 id 始终升序。`excel_add_row.py` 已默认按 id 排序插入（仅当新 id 比所有现有 id 都大时才落到末尾）；除特殊需求外，禁止用 `--append` 强制追加打乱排序。

## 配置表行布局（重要规范）

**所有配置表统一使用 3 行表头**，数据从第 4 行开始：

| 行号 | 用途 | 示例 |
|------|------|------|
| 1 | **列名**（英文） | `id`, `class_entity`, `hp` |
| 2 | **数据类型** | `long`, `string`, `int`, `float` |
| 3 | **中文说明** | `序号`, `类型1:攻击模块...` |
| 4+ | **实际数据** | `1001`, `BuffPreEntityForHPRateLess`, ... |

所有项目脚本默认 `--header-rows 3`，无需手动指定；如某张表布局不同可显式覆盖该参数。

## 配置表目录

```
Assets/Data/Excel/                   # 原始 Excel 配置表（IL 项目 22 张，文件名含中文方括号）
Assets/Resources/JsonText/           # 导出的 JSON 文本（由编辑器工具生成，.txt 文件名=Sheet 名）
```

## 通用工具脚本

| 脚本 | 用途 | 说明 |
|------|------|------|
| `.claude/scripts/excel_read.py` | 读取 Excel 全表/列/前 N 行 | 输出包含 3 行表头，便于查看类型与说明 |
| `.claude/scripts/excel_schema.py` | 查看 Sheet 列表 / 单 Sheet 表头 / 样例数据 | 自动跳过类型行/说明行展示数据样例 |
| `.claude/scripts/excel_find.py` | 按列条件查询/过滤行 | 支持精确/包含/范围匹配，跳过表头 |
| `.claude/scripts/excel_add_row.py` | 新增配置行 | 默认 id 查重，并按 id 由小到大插入正确位置（非追加末尾） |
| `.claude/scripts/excel_write.py` | 修改已有单元格 | 支持按行列、按 ID 单列、按 ID 多列三种模式 |
| `.claude/scripts/excel_delete_row.py` | 删除配置行 | 表头行受保护，支持 `--dry-run` 预览 |

> 所有脚本调用 Python 时若直接 `python` 不可用，请使用绝对路径：
> `C:\Users\<USER>\AppData\Local\Programs\Python\Python312\python.exe`

## 1. 查看表结构 (excel_schema.py)

```bash
# 列出文件内所有 Sheet 及行列数
python .claude/scripts/excel_schema.py --path "Assets/Data/Excel/excel_items_info[物品信息].xlsx"

# 查看指定 Sheet 的表头（列名 + 类型 + 中文说明）
python .claude/scripts/excel_schema.py --path "Assets/Data/Excel/excel_items_info[物品信息].xlsx" \
  --sheet ItemsInfo --sample 3
```

## 2. 读取数据 (excel_read.py)

```bash
# 前 5 行（含类型/说明行）
python .claude/scripts/excel_read.py --path "Assets/Data/Excel/excel_items_info[物品信息].xlsx" --rows 5

# 只输出指定列
python .claude/scripts/excel_read.py --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --col id --col name
```

## 3. 查询过滤 (excel_find.py)

```bash
# 精确匹配
python .claude/scripts/excel_find.py --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --where id=1000100001 --col id --col name

# 多条件 AND
python .claude/scripts/excel_find.py --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --where type=1 --where level=3 --col id --col name[language]

# 包含匹配（子串）
python .claude/scripts/excel_find.py --path "Assets/Data/Excel/excel_menu_info[菜单信息].xlsx" \
  --like name=Melee --col id --col name

# 数值范围
python .claude/scripts/excel_find.py --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --gt id=1000000000 --lt id=1100000000 --col id --col name
```

## 4. 新增配置行 (excel_add_row.py)

```bash
# col=value 形式（推荐）
python .claude/scripts/excel_add_row.py \
  --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --set id=999001 --set name=测试技能 --set remark=测试备注 \
  --backup

# JSON 形式（适合大量字段；PowerShell 内层引号需转义）
python .claude/scripts/excel_add_row.py \
  --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --json '{"id":999001,"name":"测试技能","remark":"测试备注"}' \
  --backup
```

- 默认拒绝重复 `id`，可用 `--allow-duplicate-id` 关闭
- **默认按 `id` 由小到大插入到正确位置**（不再无脑追加末尾）；新 id 比所有现有 id 都大时才落到末尾
- 如确需强制追加到末尾（不排序），加 `--append`（一般不推荐，会打乱 id 升序）
- 未指定的列保持空值

## 5. 修改单元格 (excel_write.py)

```bash
# 模式一：按行列号修改
python .claude/scripts/excel_write.py \
  --path "Assets/Data/Excel/excel_items_info[物品信息].xlsx" \
  --row 5 --col 4 --value 500 --backup

# 模式二：按 ID 修改单列
python .claude/scripts/excel_write.py \
  --path "Assets/Data/Excel/excel_items_info[物品信息].xlsx" \
  --find-col id --find-id 1001 --set-col num --value 500 --backup

# 模式三：按 ID 批量修改同行多列（推荐用于多字段更新）
python .claude/scripts/excel_write.py \
  --path "Assets/Data/Excel/excel_items_info[物品信息].xlsx" \
  --find-col id --find-id 1001 --set num=500 --set price=80 --set name=测试物品 \
  --backup
```

## 6. 删除配置行 (excel_delete_row.py)

```bash
# 先预览（不写入）
python .claude/scripts/excel_delete_row.py \
  --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --id 1000100001 --dry-run

# 真正删除（表头第 1~3 行受保护，会被拒绝）
python .claude/scripts/excel_delete_row.py \
  --path "Assets/Data/Excel/excel_skill_info[技能信息].xlsx" \
  --id 1000100001 --backup
```

## 配置表速查（文件名 → Sheet名）

IL 项目配置表共 22 张，Sheet 名与导出的 JSON 文件名一致（`Assets/Resources/JsonText/*.txt`）。数据行数以实际文件为准：

| 中文名 | 文件名（含方括号） | Sheet名 |
|--------|---------------|---------|
| 成就信息 | excel_achievement_info[成就信息] | AchievementInfo |
| 音频信息 | excel_audio_info[音频信息_FrameWork] | AudioInfo |
| 基础数据 | excel_base_info[基础数据_FrameWork] | BaseInfo |
| 建筑物品 | excel_build_item[建筑物品] | BuildItem |
| 烹饪主题 | excel_cooking_theme[烹饪主题] | CookingTheme |
| 日期信息 | excel_date_info[日期信息] | DateInfo |
| 物品信息 | excel_items_info[物品信息] | ItemsInfo |
| 物品完整信息 | excel_items_intact_info[物品完整信息] | ItemsIntactInfo |
| 多语言 | excel_language[多语言_FrameWork] | UIText + 各表 Language_*_cn/en |
| 菜单信息 | excel_menu_info[菜单信息] | MenuInfo |
| NPC信息 | excel_npc_info[NPC信息] | NpcInfo |
| NPC队伍 | excel_npc_team[NPC队伍] | NpcTeam |
| 种子信息 | excel_seed_info[种子信息] | SeedInfo |
| 技能信息 | excel_skill_info[技能信息] | SkillInfo |
| 商店信息 | excel_store_info[商店信息] | StoreInfo |
| 故事信息 | excel_story_info[故事信息] | StoryInfo |
| 故事信息详情 | excel_story_info_details[故事信息详情] | StoryInfoDetails |
| 外观文本 | excel_text_look[外观文本] | TextLook |
| 故事文本 | excel_text_story[故事文本] | TextStory |
| 对话文本 | excel_text_talk[对话文本] | TextTalk |
| UI文本 | excel_ui_text[UI文本_FrameWork] | UIText |
| 天气信息 | excel_weather_info[不同场景的天气信息] | WeatherInfo |

> 表结构或列含义不明确时，用 `excel_schema.py` 查看表头，勿臆测。

## 典型工作流

### 工作流 A：新增一条配置（如新增 Buff 前置条件）
1. `excel_schema.py --sheet <SheetName> --sample 1` 查看列结构和样例值
2. `excel_find.py` 检查目标 id 是否已存在
3. `excel_add_row.py --set ... --backup` 新增
4. `excel_find.py --where id=<新id>` 验证
5. Unity Editor → `Custom/工具弹窗/Excel编辑器` 重新导出 JSON

### 工作流 B：批量调整某行数值
1. `excel_find.py --where id=<id>` 看当前值
2. `excel_write.py --find-col id --find-id <id> --set col1=v1 --set col2=v2 --backup`
3. `excel_find.py --where id=<id>` 复核
4. Unity Editor 导出 JSON

### 工作流 C：删除废弃配置
1. `excel_find.py --where id=<id>` 确认要删的内容
2. `excel_delete_row.py --id <id> --dry-run` 预览
3. `excel_delete_row.py --id <id> --backup` 删除
4. Unity Editor 导出 JSON

## 内联 Python 写法（仅供特殊场景）

```python
import openpyxl, shutil

path = "Assets/Data/Excel/excel_items_info[物品信息].xlsx"
shutil.copy2(path, path + ".bak")
wb = openpyxl.load_workbook(path)
ws = wb.active

# 列名 -> 列号映射（基于第 1 行）
headers = {cell.value: cell.column for cell in ws[1]}
HEADER_ROWS = 3  # 项目统一表头规范

# 按 ID 修改
for row_idx in range(HEADER_ROWS + 1, ws.max_row + 1):
    if ws.cell(row=row_idx, column=headers["id"]).value == 1001:
        ws.cell(row=row_idx, column=headers["num"]).value = 500
        break

wb.save(path)
wb.close()
```

## 注意事项

1. **配置表修改后必须重新导出 JSON**：Unity 编辑器菜单 `Custom/工具弹窗/Excel编辑器`
2. **read_only=True**：只读场景下使用，性能更好，但不能修改
3. **空行/表头**：数据从第 4 行开始，第 2 行是类型、第 3 行是中文说明
4. **文件名含中文/方括号**：PowerShell 操作时使用 `Copy-Item -LiteralPath`、`Remove-Item -LiteralPath` 避免方括号被解析为通配符
5. **PowerShell 传 JSON**：内层双引号需转义，如 `--json '{\"id\":1,\"name\":\"x\"}'`；推荐用 `--set` 形式
6. **不要修改 Bean 文件**：`*InfoBean.cs` 和 `*Bean.cs` 是自动生成的，修改 Excel 后由编辑器工具重新生成
