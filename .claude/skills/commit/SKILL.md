---
name: commit
description: 按模块归类、生成结构化中文 git 提交信息。仅在用户显式输入 /commit 时触发：读取当前工作区改动（默认包含未暂存与已暂存），按 IL 的框架、业务、UI、数据、编辑器、资源等边界汇总，输出规范的中文 commit message 并执行 git commit。
disable-model-invocation: true
---

# /commit —— 规范化中文提交

为 IL（客栈传说）项目生成符合项目习惯的中文提交信息并完成提交。仅在用户显式输入 `/commit` 时触发。

## 适用场景

- 用户完成一组改动后，希望直接 `/commit` 一键生成提交信息并提交。
- 支持单文件小修或跨模块的大批量改动。
- 默认提交**工作区全部改动**（已暂存 + 未暂存）；参数含 `staged-only` 时只提交已暂存内容。

## 执行流程

### 1. 收集改动

```powershell
git status --porcelain
git diff --stat
git diff --name-status
git diff
```

如果参数包含 `staged-only`，将上述 diff 命令改为对应的 `git diff --cached` 形式，不读取未暂存 diff 作为提交内容。

发现与本次需求无关或来源不明的改动时，先向用户说明，不要静默混入提交。

### 2. 按模块归类

依据实际改动路径映射模块标签；未命中的路径使用通用名称，不臆造业务模块：

| 路径模式 | 模块标签 |
|---------|---------|
| `Assets/FrameWork/**` | 框架（可按 Editor/Scripts/Addons 等子目录细化） |
| `Assets/Scripts/Bean/**` 或 `**/*Bean*.cs` | 数据模型 |
| `Assets/Scripts/MVC/Service/**` 或 `**/*Service*.cs` | 数据服务 |
| `Assets/Scripts/Enums/**` | 枚举 |
| `Assets/Scripts/Component/UI/**` | UI |
| `Assets/Scripts/Component/MiniGame/**` | 小游戏 |
| `Assets/Scripts/Component/Handler/**` | Handler |
| `Assets/Scripts/Component/Manager/**` | Manager |
| `Assets/Scripts/Component/**` | 游戏业务（按下一级目录细化） |
| `Assets/Editor/**` 或 `**/Editor/**` | 编辑器工具 |
| `Assets/Data/Excel/**` | 配置表 |
| `Assets/Resources/**`、`Assets/LoadResources/**` 或 `Assets/AddressableAssetsData/**` | 资源 |
| `Packages/**` 或 `ProjectSettings/**` | 项目配置 |
| `.claude/**` | Claude 配置（Agent/Skill/Hook/脚本） |
| 其他 | 杂项 |

### 3. 生成提交信息

```text
<主标题：动词 + 一句话总览，不超过 50 字>

- [模块A] 简述本次该模块的关键改动
- [模块B] 简述本次该模块的关键改动
```

- 主标题动词取自：新增 / 修复 / 优化 / 重构 / 调整 / 完善 / 移除。
- 多模块标题概括主要改动；单模块标题直接写具体内容。
- 仅 `.claude` 改动时，标题前缀使用 `[claude]`。
- 提交信息只陈述实际改动和已验证结果，不写入未完成的验证。

### 4. 执行提交

使用 PowerShell 单引号 here-string，避免 `$` 被解释：

```powershell
git add -A                    # staged-only 模式跳过
git commit -m @'
<标题>

- [模块] ...
'@
```

`'@` 必须位于第 0 列并独占一行。不要改写或丢弃用户的既有改动。

### 5. 报告结果

- 使用 `git log -1 --oneline` 报告新提交。
- 列出涉及模块。
- 使用 `git show --stat --oneline HEAD` 报告文件统计。
- 若提交钩子失败，如实报告输出并排查，不绕过钩子。

## 注意事项

- 不使用 `--amend`，始终创建新提交。
- 不使用 `--no-verify`，不主动修改签名设置。
- 工作区为空时直接告知“无改动可提交”，不创建空提交。
- `staged-only` 模式没有已暂存改动时同样不创建空提交。
