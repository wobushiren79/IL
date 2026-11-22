# .claude 工具链索引

IL 项目的 Claude Code 工具链分三层：**Commands**（结构化代码生成）/ **Skills**（领域审计与流程）/ **Agents**（独立任务子代理）。位于项目根的 `.claude/` 目录。

**用法：** 用户提需求时，先查此索引看是否有现成工具命中。若有则直接用 `/<工具名>` 调用或 spawn agent，而非临时手工实现。

## 生成器 Commands（修改代码 / 创建文件）

| 命令 | 用途 |
|---|---|
| `/il-bean-gen` | Bean 主文件 + Partial（cfg/db 两种模式） |
| `/il-service-gen` | SQLite Service（单表 / 双表 JOIN / 动态表名） |
| `/il-handler-gen` | Handler + Manager 组合 |
| `/il-enum-gen` | 枚举新建/追加（MsgEnum/UIEnum 等） |
| `/il-ui-view-gen` | UI View（full/base/dialog/popup 四类） |
| `/il-ui-item-gen` | ListItem 脚本 |
| `/il-system-scaffold` | 整套系统一键脚手架（链式调用上述） |
| `/excel-sync` / `/il-excel-sync` | Excel ↔ Bean 字段同步 |
| `/il-unity-asset` / `/il-unity-scene` | Unity 资源与场景操作 |

还有同义短名：`new-cfg-bean` / `new-db-bean` / `new-handler` / `new-service` 等。

## 审计 Skills（只读分析 / 流程指引）

| Skill | 用途 |
|---|---|
| `/il-addressable-audit` | Addressables 地址注册/孤立/命名/Group 归属 |
| `/il-spine-setup` | Spine 角色资源接入流程指引 |
| `/il-localization-audit` | 多语言文本覆盖 + Bean LanguageCache + 硬编码中文 + TextEnum |
| `/il-event-flow-trace` | MsgEnum 全局事件发布/订阅链路追踪 |
| `/il-sqlite-schema-check` | SQLite 表结构 vs Bean 字段一致性 |
| `/il-scene-init-check` | Handler 场景挂载 + InitData 调用检查 |
| `/il-build-prep` | Steam 上线前置体检 |
| `/il-data-analyst` | Excel 配置表数据完整性 / 引用 / 数值平衡 |
| `/il-code-reviewer` | 代码规范六维度审查 |
| `/il-minigame-scaffold` | 新 MiniGame 全套脚手架（Handler/Builder/Bean/UI/Cpt） |
| `/il-minigame-state-audit` | 小游戏状态机/事件配对/enum switch/Builder Prefab 审计 |
| `/il-gamble-scaffold` | 赌博子游戏脚手架（Bean/UI/Item，轻量） |
| `/il-datetime-check` | 日期/时间系统审计（gameTime 初始化哨兵、历法常量一致性、季节映射、特殊日期比对、日历显示链路） |

## 框架层开发 Skills（共享框架，从同源框架项目迁移）

| Skill | 用途 |
|---|---|
| `/framework-core-system` | 框架核心基类（BaseMonoBehaviour / BaseSingleton / BaseManager / BaseHandler 配对 / BaseMVCService / BaseUI* / AutoLinkUI） |
| `/event-system` | 事件系统（EventHandler 全局 / BaseEvent 实例 / EventsInfo 常量） |
| `/resource-loading-system` | 资源加载（Addressables / Resources / AssetBundle / WWW + 缓存） |
| `/ui-framework` | UI 框架（BaseUIInit / BaseUIView / UIHandler / 弹窗气泡提示） |
| `/utils-system` | 工具类分层规范（Utils / Extension / Tools） |
| `/data-service-system` | 数据服务（BaseDataService\<T\> / BaseMVCService / BaseDataStorageImpl） |
| `/editor-extension-system` | 编辑器扩展（窗口 / Inspector / Hierarchy / 代码生成） |
| `/excel-io` | openpyxl 读写 Excel 配置表（3 行表头规范） |

## 通用工作流与 Unity/美术工具 Skills

| Skill | 用途 |
|---|---|
| `/commit` | 按 IL 模块归类改动，生成中文提交信息并在显式调用时提交 |
| `/diagnosing-bugs` | 复现→最小化→假设→插桩→修复→回归的根因排查纪律 |
| `/grill` | 高歧义/高风险需求的决策分支盘问与收敛 |
| `/writing-great-skills` | Skill 新建、修改与体检的质量参考 |
| `/unity-mcp-connection` | Unity MCP 检测、连接、实例选择与 JSON-RPC 调用 |
| `/pixelda` | PixelDa AI 像素美术生成编辑器工具开发 |
| `/pixel-image-editor` | 像素图重采样、编辑、调色、排版与导出工具开发 |
| `/aseprite-pixel-editor` | Aseprite CLI/Lua 源文件检查、非覆盖编辑与预览验收 |
| `/aseprite-ai-artist` | Aseprite AI Artist 原生像素网格绘制、修复与检查 |
| `/blender-scene-from-reference` | 参考图重建 Blender 场景与 Unity 独立验收/入库流程 |

## Agents（独立 spawn 的子代理）

| Agent | 用途 |
|---|---|
| `unity-meta-checker` | Unity .meta 一致性 |
| `il-system-architect` | 新系统设计前的依赖分析与落地步骤 |
| `il-bug-hunter` | 异常堆栈 / 错误现象的根因定位 |
| `il-perf-profiler` | 性能隐患静态扫描（GC/Find/阻塞加载） |
| `il-ui-prefab-binder` | UI 脚本字段 vs Prefab 实际绑定 |
| `il-cn-text-extractor` | 中文硬编码字符串扫描与迁移建议 |
| `il-minigame-balance-analyst` | 小游戏 + 赌博 数值平衡分析（胜率/奖励/期望值/职业经验分布） |
| `il-minigame-flow-tracer` | 小游戏全流程追踪（触发→Init→Start→进行→End→Close 断点定位） |

### 框架层 Agents（共享框架，从同源框架项目迁移）

| Agent | 用途 |
|---|---|
| `framework-core` | 框架核心基础类 |
| `framework-editor` | Unity 编辑器工具 |
| `framework-event` | 事件系统 |
| `framework-resource` | 资源加载 |
| `framework-utils` | 工具类 / 扩展方法 |
| `ui-core` | UI 核心框架 |
| `ui-components` | UI 通用组件 |
| `data-bean` | 数据模型 Bean |
| `data-service` | 数据服务层 |
| `data-excel` | Excel 配置表 |

### 通用工具与共享系统 Agents

| Agent | 用途 |
|---|---|
| `unity-mcp` | Unity MCP 连接检测、实例选择与调用诊断 |
| `pixelda` | PixelDa 编辑器工具维护 |
| `pixel-image-editor` | 像素图编辑器工具维护 |
| `blender-scene-artist` | Blender 场景重建与 Unity 验收交付 |
| `system-network` | UnityWebRequest、网络回调与网络资源加载 |
| `system-steam` | Steamworks.NET、排行榜、统计与创意工坊 |
| `system-audio` | 音频播放、环境音、循环音源池与 UI 音频组件 |
| `system-camera` | 主/UI 摄像机引用与通用 Cinemachine 控制 |
| `system-effect` | 特效加载、播放、池化与回收 |
| `system-spine` | Spine 资源、动画、皮肤与 SpineWindow 开发 |
| `system-localization` | 文本读取、语言切换、UI 绑定与动态占位符 |
| `system-volume` | URP Volume、景深、内置雾与环境渲染状态 |

## 关联文档
- `.claude/md/framework.md` — 框架技术文档
- `.claude/md/project.md` — 业务代码文档
- 项目根 `CLAUDE.md` — 操作权限、路径规范与任务总结规则
