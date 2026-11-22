---
name: system-effect
description: 特效系统开发：EffectHandler/EffectManager、一次性与持久特效、对象池回收、EffectBase/BaseEffectView。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Component/Handler/EffectHandler.cs
  - Assets/FrameWork/Scripts/Component/Manager/EffectManager.cs
  - Assets/FrameWork/Scripts/Component/Effect/EffectBase.cs
  - Assets/FrameWork/Scripts/Component/UI/BaseEffectView.cs
---

# 特效系统开发代理

负责 IL 共享框架的特效加载、实例化、播放与回收。业务特效 ID、战斗表现和专用 shader 不属于本通用 agent 的固定规则。

## 核心类

- [EffectHandler.cs](Assets/FrameWork/Scripts/Component/Handler/EffectHandler.cs)：通过 `ShowEffect` 等入口驱动特效。
- [EffectManager.cs](Assets/FrameWork/Scripts/Component/Manager/EffectManager.cs)：`GetEffect`、`GetEffectForEnduring`、`GetEffectModelSync` 与 `DestoryEffect`（保留源码拼写）。
- [EffectBase.cs](Assets/FrameWork/Scripts/Component/Effect/EffectBase.cs)：特效生命周期和粒子/视图基础能力。
- [BaseEffectView.cs](Assets/FrameWork/Scripts/Component/UI/BaseEffectView.cs)：UI 特效视图基类。

## 通用约束

- 特效通过 EffectHandler/EffectManager 创建和管理，业务侧不散落实例化与销毁逻辑。
- 一次性与持久特效使用对应通道；需要独立多实例时，不得误用按资源名复用的持久单例通道。
- `GetEffectModelSync` 只取模型资源，不等于完成特效实例化、入池或播放。
- 播放前确认 prefab 的组件、粒子空间、方向、持续时间与池化语义；重播持久粒子时注意旧粒子是否应保留。
- 回收或销毁必须清理回调、实例状态和引用；场景切换时检查常驻对象是否残留。
- 热路径避免重复加载和不必要的 GC；修改后验证首次播放、重复播放、并发、回收和场景切换。
- 修改上述框架文件后同步本 agent。
