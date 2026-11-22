---
name: system-audio
description: 音频系统开发：AudioHandler/AudioManager、音效播放、背景音乐、环境音、循环音效、音量管理、AudioView/ButtonAudio。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Component/Handler/AudioHandler.cs
  - Assets/FrameWork/Scripts/Component/Manager/AudioManager.cs
  - Assets/FrameWork/Scripts/Bean/MVC/AudioInfoBean.cs
  - Assets/FrameWork/Scripts/Bean/MVC/AudioInfoBeanPartial.cs
  - Assets/FrameWork/Scripts/Component/UI/AudioView.cs
  - Assets/FrameWork/Scripts/Component/UI/ButtonAudio.cs
  - Assets/FrameWork/Scripts/Enums/BaseGameEnum.cs
---

# 音频系统开发代理

负责 IL 共享框架的音频播放、资源缓存、音源池和 UI 音频组件。业务侧只遵循 IL 当前封装，不套用其他项目的业务枚举或场景规则。

## 核心类

- [AudioHandler.cs](Assets/FrameWork/Scripts/Component/Handler/AudioHandler.cs)：对外播放与暂停/停止接口。
- [AudioManager.cs](Assets/FrameWork/Scripts/Component/Manager/AudioManager.cs)：AudioSource、AudioListener、AudioClip 缓存与循环音源池。
- [AudioInfoBean.cs](Assets/FrameWork/Scripts/Bean/MVC/AudioInfoBean.cs)：音频 ID、资源名、类型和音量缩放配置。
- [AudioView.cs](Assets/FrameWork/Scripts/Component/UI/AudioView.cs) / [ButtonAudio.cs](Assets/FrameWork/Scripts/Component/UI/ButtonAudio.cs)：本地 UI 音频组件。

## 三类音频与资源

`AuidoTypeEnum`（保留源码拼写）分为：

| 类型 | 用途 | 资源目录 |
|---|---|---|
| `Sound = 0` | 瞬时音效 | `Assets/LoadResources/Audio/Sound/` |
| `Music = 1` | 背景音乐 | `Assets/LoadResources/Audio/Music/` |
| `Environment = 2` | 循环环境音 | `Assets/LoadResources/Audio/Environment/` |

资源经 Addressables 加载并缓存；新增资源需核对实际地址和 Group 注册。配置修改遵循 IL 的 Excel 源表与导出流程，不只改派生 JSON。

## 通用 API

- 音乐：`PlayMusicForLoop`、音乐列表轮播、暂停/恢复/停止。
- 音效：`PlaySound`、`PlaySoundForInterval`；默认按音频 ID 做 0.1 秒重复播放判定，限频与默认判定共用时间记录。
- 环境音：`PlayEnvironment`、`PauseEnvironment`、`RestoreEnvironment`、`StopEnvironment`。
- 循环音效：`PlayLoopSound`、`StopLoopSound`、`StopAllLoopSound`、暂停/恢复与状态查询。按 ID 独立起停，支持多路并发和 pitch；异步加载以 token 防止“停止先于回调”导致孤儿音源。
- 定时淡出：`PlaySoundTimedFade`，使用独立音源，在指定时长内播放并线性淡出。
- 可截断单次音效：`PlaySoundOnce`、`StopSoundOnce`，自然结束自动回收，停止请求可取消尚未完成的加载。

框架接口使用 `long` 音频 ID。业务层调用时优先使用 IL 已有的音频枚举和封装，新增音频后同步配置、资源与枚举。

## 修改约束

- 播放统一经过 AudioHandler，不在业务代码散落创建 AudioSource。
- 音量缩放区分绝对音量、运行时倍率与配置 `volume_scale`，修改前读取实际实现。
- 池化回收必须停止播放、清理 clip 并复位 pitch/volume；异步回调必须核对 token/canceled。
- `VolumeHandler`/`VolumeManager` 管 URP 后处理，不管音频音量。
- 修改自动生成 Bean 时遵循 IL 的生成/Partial 分工；修改上述框架文件后同步本 agent。
