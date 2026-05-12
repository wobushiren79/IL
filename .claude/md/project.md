# Project — 游戏业务代码文档（IL / 客栈传说）

游戏本体的所有业务逻辑代码。依赖 `Assets/FrameWork/` 提供的基础设施，不包含通用框架代码。

代码位置：`Assets/Scripts/`
框架文档：参见 `.claude/md/framework.md`

## 目录结构

```
Assets/Scripts/
├── Bean/           # 数据模型（DTO）
│   ├── Common/     # 跨系统共用 Bean
│   └── MVC/        # 与 SQLite 表对应的 Bean
│       ├── Arena/      # 竞技场
│       ├── Character/  # 角色
│       ├── Gamble/     # 赌博小游戏
│       ├── Game/       # 主游戏数据
│       ├── Inn/        # 客栈管理
│       ├── Menu/       # 菜单/菜品
│       ├── MiniGame/   # 小游戏
│       ├── Order/      # 订单
│       └── User/       # 玩家数据
├── MVC/
│   └── Service/    # SQLite 数据服务（继承 BaseMVCService）
├── Component/      # 游戏组件
│   ├── Base/       # 项目级基类
│   ├── Manager/    # 管理器（按系统分子目录）
│   ├── Handler/    # 事件处理器（按系统分子目录）
│   ├── Builder/    # 建造器（NPC、小游戏对象构建）
│   ├── UI/         # UI 界面与组件
│   ├── Character/  # 角色行为
│   ├── NPC/        # NPC 系统（顾客、员工）
│   ├── MiniGame/   # 小游戏实现
│   ├── Gamble/     # 赌博游戏
│   ├── Food/       # 食物系统
│   ├── Element/    # 游戏元素
│   ├── Control/    # 玩家输入控制
│   ├── Interactive/# 可交互对象
│   ├── ParticleSystem/ # 粒子特效脚本
│   ├── Scenes/     # 场景专属脚本
│   ├── Create/     # 制作/合成系统
│   ├── InnBuild/   # 客栈建造系统
│   └── ListItem/   # 列表项组件
├── Enums/          # 游戏枚举
├── Common/         # 项目公共工具
├── CallBack/       # 项目回调
├── DataStorage/    # 存档 & 序列化
├── Utils/          # 项目工具类
├── Anim/           # 动画控制脚本
└── Test/           # 测试脚本
```

## 分层架构

```
UI 层        Component/UI/           BaseUIInit / BaseUIView
              ↕ 事件 / 方法调用
业务层        Component/Manager/      域管理器（Game/Data/NPC/Inn 等）
              Component/Handler/      事件驱动处理器
              ↕ 服务调用
数据层        MVC/Service/            SQLite 数据服务
              Bean/MVC/               数据模型（映射数据库表）
```

## 主要游戏系统

### 角色系统（`Component/Character/`、`Bean/MVC/Character/`）
- `CharacterBean` — 角色基础信息
- `CharacterAttributesBean` — 属性（体力/攻击/防御等）
- `CharacterEquipBean` — 装备槽
- `CharacterBodyBean` — 外观/体型
- `CharacterFavorabilityBean` — 好感度关系
- `CharacterForFamilyBean` — 家族/团队归属
- `CharacterWorker*Bean` — 各工种员工扩展数据（厨师/会计/接待等）

### 客栈管理（`Component/Manager/Inn/`、`Bean/MVC/Inn/`）
- 建造系统（`InnBuild/`）：摆放/升级设施
- 员工管理：招募、分配岗位、薪资
- 营收追踪：`UserRevenueService`

### 菜品 & 烹饪（`Bean/MVC/Menu/`、`MVC/Service/MenuInfoService`）
- 菜单配置与解锁
- 烹饪主题（`CookingThemeService`）
- 食材/种子系统（`SeedInfoBean`）

### NPC 系统（`Component/NPC/`）
- `Customer/` — 顾客行为（点单、用餐、离开）
- `Worker/` — 员工 AI（搬运、烹饪、接待）
- `Details/` — NPC 详细属性
- A\* Pathfinding 驱动移动

### 小游戏（`Component/MiniGame/`）
| 目录 | 游戏 | 说明 |
|---|---|---|
| `GameCooking/` | 烹饪小游戏 | 时序操作，还原菜谱 |
| `GameAccount/` | 算账小游戏 | 数学计算，核对账目 |
| `GameBarrage/` | 弹幕小游戏 | 动作/节奏，躲避攻击 |
| `GameDebate/` | 辩论小游戏 | 对话选择，策略博弈 |

### 赌博系统（`Component/Gamble/`）
- `TrickyCup/` — 三杯猜球
- `TrickySize/` — 大小猜拳

### 竞技场（`Bean/MVC/Arena/`）
- `ArenaPrepareBean` — 竞技场准备数据

### 物品 & 背包（`Bean/Common/`、`MVC/Service/ItemsInfoService`）
- `ItemBean` — 通用物品
- `GameItemsBean` — 游戏内物品实例
- `BuildItemService` — 建筑物品

### 成就 & 统计（`MVC/Service/AchievementInfoService`）
- `AchievementInfoBean` — 成就定义
- Steam 成就同步

### 对话 & 剧情（`Bean/MVC/Game/`）
- `TextTalkBean` / `TextTalkBeanPartial` — 对话数据（partial class 拆分逻辑）
- `StoryInfoDetailsBean` — 剧情节点
- `TextInfoBean` — 多语言文本

### 天气 & 季节（`Bean/Common/WeatherBean`、`Enums/SeasonsEnum`）
- 昼夜循环
- 四季系统
- 天气效果

### 音频（`Enums/Audio*Enum`）
- `AudioMusicEnum` — 背景音乐
- `AudioSoundEnum` — 音效
- `AudioEnvironmentEnum` — 环境音

## 数据服务一览（`MVC/Service/`）
| 服务类 | 主表 | 说明 |
|---|---|---|
| `GameDataService` | game_data | 游戏主存档 |
| `AchievementInfoService` | achievement_info | 成就数据 |
| `BuildItemService` | build_item | 建筑物品 |
| `CookingThemeService` | cooking_theme | 烹饪主题 |
| `DateInfoService` | date_info | 游戏日期 |
| `ItemsInfoService` | items_info | 物品信息 |
| `MenuInfoService` | menu_info | 菜单信息 |
| `NpcInfoService` | npc_info | NPC 信息 |
| `SkillInfoService` | skill_info | 技能信息 |
| `UserRevenueService` | user_revenue | 玩家营收 |
| `BaseDataService` | — | 基础数据服务基类 |
| *(30+ 更多服务)* | | |

## UI 结构（`Component/UI/`）
```
UI/
├── Main/       # 主菜单、主界面
├── Town/       # 城镇界面
├── Game/       # 游戏内 HUD
├── MiniGame/   # 小游戏 UI
├── Mountain/   # 山地探索界面
├── Gamble/     # 赌博界面
├── View/
│   ├── Dialog/ # 对话框
│   └── Popup/  # 弹出窗口
├── Child/      # 子组件
└── ListItem/   # 列表项模板（Town/Dialog/MiniGame/Mountain/Popup）
```

## 枚举约定（`Enums/`）
- `Base/MsgEnum` — 全局事件 ID（跨系统通信用）
- `Base/UIEnum` — UI 面板 ID
- `Base/PopupEnum` — 弹窗 ID
- `Base/DialogEnum` — 对话框 ID
- `Base/AIIntentEnum` — NPC AI 意图状态
- `AttributesTypeEnum` — 角色属性类型
- `AchievementTypeEnum` / `AchievementStatusEnum` — 成就分类与状态

## 开发约定
- Bean 类对应数据库表字段，字段名使用 `snake_case`（与 SQLite 列名一致）
- 复杂 Bean 使用 `partial class` 拆分：主文件放字段，`*Partial.cs` 放方法
- Manager 负责业务状态与跨系统协调，Handler 负责事件响应
- UI 事件通过 `EventHandler`（`MsgEnum`）广播，不直接持有 Manager 引用
- 新增系统按现有目录分层：Bean → Service → Manager/Handler → UI
- 大型 Manager 使用 `partial class`（参考 `GameDataManager` + `GameDataManagerPartial`）
