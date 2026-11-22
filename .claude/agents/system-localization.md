---
name: system-localization
description: 多语言系统开发：TextHandler/TextManager、UITextLanguageView、语言切换、文本 ID、动态占位符和语言资源加载。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Component/Handler/TextHandler.cs
  - Assets/FrameWork/Scripts/Component/Manager/TextManager.cs
  - Assets/FrameWork/Scripts/Component/UI/UITextLanguageView.cs
  - Assets/FrameWork/Scripts/Bean/MVC/LanguageBeanPartial.cs
  - Assets/FrameWork/Scripts/Bean/GameConfigBean.cs
---

# 多语言系统开发代理

负责 IL 共享框架的文本读取、语言切换和 UI 绑定。项目文本覆盖率、LanguageCache 与硬编码检查使用 `il-localization-audit`。

## 核心类

- [TextHandler.cs](Assets/FrameWork/Scripts/Component/Handler/TextHandler.cs)：文本获取、语言切换和动态替换入口。
- [TextManager.cs](Assets/FrameWork/Scripts/Component/Manager/TextManager.cs)：按配置表、文本 ID 和语言加载内容。
- [UITextLanguageView.cs](Assets/FrameWork/Scripts/Component/UI/UITextLanguageView.cs)：Prefab 静态文本绑定与刷新。
- [LanguageBeanPartial.cs](Assets/FrameWork/Scripts/Bean/MVC/LanguageBeanPartial.cs)：语言配置的手写扩展。
- [GameConfigBean.cs](Assets/FrameWork/Scripts/Bean/GameConfigBean.cs)：用户语言偏好。

## 通用接口

- `GetTextById(id, contentIndex)`：读取通用 UI 文本。
- `GetTextById(cfgName, id, contentIndex)`：读取指定配置表的语言文本。
- `GetTextByIdNoBreakingSpace`：需要避免空格换行时使用。
- `ChangeLanguageEnum`：切换当前语言；检查 UI 刷新与配置持久化。
- `GetTextReplace`：以 `Dictionary<TextReplaceEnum, string>` 替换 `{枚举名}` 占位符。一个重载从通用文本表取模板，另一个直接处理传入字符串，调用前确认模板来源。

`contentIndex` 用于选择同一文本 ID 下的不同内容列；名称与详情是否共用 ID 应遵循 IL 当前表结构，不照搬其他项目的文本编号。

## 修改约束

- 文本真实源是 IL 的 Excel 配置；派生 JSON/TXT 由现有导出流程生成，不单独手改后当作完成。
- 新增文本时核对登记表、语言表、Bean 字段和实际使用位置；支持语言以 IL 当前 `LanguageEnum` 和 Excel 列为准。
- 自动生成 Bean 的扩展放入对应 Partial；UI 动态文本通过 TextHandler，静态文本优先使用 UITextLanguageView。
- 运行时数值使用统一占位符机制，不在各语言代码分支中拼接特殊格式。
- 语言切换后检查缓存清理、UI 刷新和存档偏好；缺文本、ID=0 与空内容按实际框架语义处理。
- 修改上述框架文件后同步本 agent；审计规则不在此复制，避免与 `il-localization-audit` 漂移。
