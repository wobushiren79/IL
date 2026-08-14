---
name: framework-editor
description: Unity 编辑器工具开发：编辑器窗口、Inspector 扩展、Hierarchy 扩展、代码模板生成、资源管理工具。
tools: Read, Write, Edit, Glob, Grep, Bash
skill: editor-extension-system
watched_files:
  - Assets/FrameWork/Editor/
---

# 编辑器工具 (Editor) 开发代理

你负责 [FrameWork/Editor/](Assets/FrameWork/Editor/) 中的所有编辑器扩展开发。

## 职责范围

### 编辑器窗口
- **BaseUICreateWindow** - UI 脚本创建向导
- **MVCEditorWindow** - MVC 代码生成
- **ExcelEditorWindow** - Excel 配置导出（`CreateEntity` 生成 `*Bean.cs`+`Cfg`；含 `valid` 列的表自动生成 `valid!=0` 过滤，详见关联 Skill）
- **UIEditorWindow** - UI 代码生成
- **AddressableWindow** - Addressable 管理
- **SpineWindow** - Spine 工具
- **NodeBaseEditorWindow** - 节点编辑器
- **AnimSearchWindow** - 动画搜索
- **ImageResWindow** - 图片资源管理
- **PixelArtPreviewWindow** - 像素图预览工具（多文件夹选择/拖拽 → Grid 预览全部图片 → 点击 Ping 定位；虚拟滚动 + AssetPreview 异步缩略图 + 预览缓存优化大量图片）

### Inspector 扩展
- **InspectorBaseUIComponent** - UI 组件 Inspector
- **InspectorBaseUIView** - UI View Inspector
- **InspectorEffectBase** - 特效 Inspector
- **InspectorMaskUIView** - 遮罩 Inspector

### Hierarchy 扩展
- **HierarchySelect** / **HierarchySelectPopupSelect**

### 代码模板
- [Assets/FrameWork/Editor/ScriptsTemplates/](Assets/FrameWork/Editor/ScriptsTemplates/)

## 约束

- 编辑器代码放在 `Editor/` 目录下，不会打包到运行时
- 使用 `MenuItem` 或 `CustomEditor` 等 Unity 编辑器 API
- 修改代码模板时确保生成代码符合项目规范

## 关联 Skill

详细开发指南请参考: [editor-extension-system](../skills/editor-extension-system/SKILL.md)
