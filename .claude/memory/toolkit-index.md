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

## 关联文档
- `.claude/md/framework.md` — 框架技术文档
- `.claude/md/project.md` — 业务代码文档
- 项目根 `CLAUDE.md` — 操作权限、路径规范与任务总结规则
