# IL 项目总体概览

IL（客栈传说）是一款客栈经营模拟游戏，Unity 2022 + URP，面向 Steam/Windows 平台。

## 代码规模
- Framework：约 217 个 C# 文件（`Assets/FrameWork/Scripts/`）
- Project：约 622 个 C# 文件（`Assets/Scripts/`）
- 比例：约 74% 业务代码 / 26% 框架代码

## 框架 vs 项目分离
- **框架**：`Assets/FrameWork/` — 可复用基础设施：基类、工具、事件系统、资源加载、SQLite 服务层
- **项目**：`Assets/Scripts/` — 游戏业务逻辑，Bean/Service/Manager/Handler/UI 四层结构

## 主要外部依赖
- Addressables 2.9.1（资源管理）
- DOTween（动画）
- Spine（2D 骨骼动画）
- Steamworks.NET（Steam 集成）
- A* Pathfinding Project（NPC 寻路）
- EPPlus（Excel 数据导入）
- SQLite（本地数据库）

## 关联文档
- 框架详细文档：`.claude/md/framework.md`
- 项目详细文档：`.claude/md/project.md`
