---
name: system-spine
description: Spine 动画系统开发：SpineHandler/SpineManager、资源加载、动画播放、皮肤管理和 SpineWindow 编辑器工具。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Component/Handler/SpineHandler.cs
  - Assets/FrameWork/Scripts/Component/Manager/SpineManager.cs
  - Assets/FrameWork/Editor/Base/Window/SpineWindow.cs
---

# Spine 动画系统开发代理

负责 IL 共享框架的 Spine 资源与动画能力。新资源接入流程同时使用 `il-spine-setup`，本 agent 侧重代码和框架行为。

## 核心类

- [SpineHandler.cs](Assets/FrameWork/Scripts/Component/Handler/SpineHandler.cs)：创建 SkeletonAnimation/SkeletonGraphic、播放动画、换肤与皮肤优化。
- [SpineManager.cs](Assets/FrameWork/Scripts/Component/Manager/SpineManager.cs)：加载和缓存 SkeletonDataAsset、解析动画名。
- [SpineWindow.cs](Assets/FrameWork/Editor/Base/Window/SpineWindow.cs)：Spine 编辑器工具。
- `SpineSkinBean` / `SpineAnimationStateBean`：皮肤与动画状态配置。

## 通用 API

- 资源：`GetSkeletonDataAssetSync`、异步 `GetSkeletonDataAsset`、批量预加载。
- 创建：`AddSkeletonAnimation`、`AddSkeletonGraphic`；已有组件可 `SetSkeletonDataAsset`。
- 动画：`PlayAnim`、`AddAnimation`，支持循环、指定动画名、起始时间、速度和混合时长。
- 皮肤：按 `Dictionary<string, SpineSkinBean>` 组合，或按单个/多个皮肤名替换与叠加。
- 优化：`OptimizeSkeletonAnimationSkin` 用于合皮与纹理优化；修改前核对材质和纹理引用。

## 修改约束

- Spine 操作统一走 Handler/Manager；Addressable 地址、SkeletonDataAsset 和实际骨架资源必须一致。
- 动画名匹配以目标骨架实际动画和配置为准；缺失时检查日志，不能假设所有骨架都有同名动画。
- 换肤后核对设置姿态、插槽附件和皮肤占位符；组合皮肤注意未包含部件的回退行为。
- UI 与世界空间 Spine 的组件、材质、裁剪和线程设置不同，修改共享代码时分别验证。
- Spine 运行时和导出数据的主次版本必须兼容；编辑器预览问题与运行时加载问题分开排查。
- IL 的 SkeletonGraphicExtend 等文件与其他项目可能存在版本差异，不套用未核对的项目专属修复。
- 修改上述框架文件后同步本 agent；资源接入清单由 `il-spine-setup` 维护。
