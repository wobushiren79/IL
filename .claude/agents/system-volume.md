---
name: system-volume
description: 后处理系统开发：VolumeHandler/VolumeManager、URP Volume、景深 DepthOfField、内置距离雾 RenderSettings.fog 与天空盒资源。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Component/Handler/VolumeHandler.cs
  - Assets/FrameWork/Scripts/Component/Manager/VolumeManager.cs
---

# 后处理系统开发代理

负责 IL 共享框架的 URP Volume 与场景环境渲染通用能力。与 `system-camera` 互补：摄像机决定观察方式，本 agent 负责景深、雾和相关渲染状态。

## 核心类

- [VolumeHandler.cs](Assets/FrameWork/Scripts/Component/Handler/VolumeHandler.cs)：`SetDepthOfField`、`SetDepthOfFieldActive`、`SetFog`、`SetFogActive` 等通用入口。
- [VolumeManager.cs](Assets/FrameWork/Scripts/Component/Manager/VolumeManager.cs)：懒加载 `volume`、`volumeProfile`、`depthOfField` 等组件和资源。

## 通用约束

- 景深通过 URP Volume 组件修改，核对 override 状态、参数和当前 Profile；不要在业务代码散落查找组件。
- 内置雾使用 `RenderSettings.fog`：Linear 使用起止距离，Exp/Exp2 使用 density；切换场景时检查全局状态是否残留。
- 内置雾只对实现对应 fog 宏的 shader 生效；自定义 shader 或 Spine 材质不生效时先检查 shader，而非重复调参数。
- Volume 是后处理系统，不是音频音量管理；音频由 `system-audio` 负责。
- 新增第三方雾、环境光或天空盒逻辑前，先确认 IL 当前包和场景消费方，不套用其他项目的场景配置字段。
- 修改后验证开关、参数边界、场景进入/退出和不同相机下的表现；修改上述框架文件后同步本 agent。
