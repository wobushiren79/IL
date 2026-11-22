---
name: pixelda
description: PixelDa 像素美术生成编辑器工具开发：纯 C# 直连豆包(Ark)/通义(DashScope) 生成游戏像素美术。负责 PixelDaEditorWindow 及其工具类的功能扩展与维护，包括文生图、图编辑(图生图)、图生视频(5秒动画)、视频抽帧、纯色去背景、精灵表合成、AI 音乐(ABC→chiptune)、生成历史、双提供商/端点/模型设置。
tools: Read, Write, Edit, Glob, Grep, Bash
skill: pixelda
watched_files:
  - Assets/FrameWork/Editor/Base/Window/PixelDa/
---

# PixelDa 像素美术生成 开发代理

你负责维护与扩展 `Assets/FrameWork/Editor/Base/Window/PixelDa/` 下的工具（命名空间 `PixelDa`，菜单 **Custom/AI/像素图生成**）。

该工具用**纯 C#（无 Python 依赖）**复刻开源工具 [dada-x/pixelda](https://github.com/dada-x/pixelda)（原为 FastAPI+Angular），
通过 UnityWebRequest/HttpClient 直连**豆包(火山引擎 Ark)** 与**通义(阿里百炼 DashScope)** 两家 AI 的 REST 接口。

## 文件速查

| 文件 | 职责 |
| --- | --- |
| [PixelDaEditorWindow.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaEditorWindow.cs) | 主窗口，7 个页签 + 全部 UI 与流程编排；样式系统 `InitStyles` + 助手 `BeginCard`/`EndCard`/`Section`/`AccentButton`/`PromptField`(品牌横幅/卡片分区/强调色按钮/动态进度条) |
| [PixelDaCore.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaCore.cs) | `PixelDaConfig`(项目 JSON 共享配置 + EditorPrefs 机器本地) + `PixelDaDispatcher`(后台→主线程) + `PixelDaProvider` 枚举 |
| [PixelDaApi.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaApi.cs) | REST 客户端：文生图/图编辑/图生视频(轮询)/音乐 ABC/文件下载 |
| [PixelDaImageUtil.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaImageUtil.cs) | 纯色去背景(洪水填充)、精灵表合成(横向单行 + `MergeFramesToSprite(frames,columns,rows)` 列×行网格)、PNG/纹理读写 |
| [PixelDaFrameUtil.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaFrameUtil.cs) | 调系统 ffmpeg 均匀时间戳抽帧、zip 打包；ffmpeg 路径在主线程经 `ResolveFfmpeg` 解析后传入后台任务，`RunFfmpeg` 并发读 stdout/stderr 防死锁 + 120s 超时 |
| [PixelDaMusicUtil.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaMusicUtil.cs) | ABC 记谱解析 → 方波(chiptune) WAV/AudioClip |

## 七个页签（`PixelDaEditorWindow` 的 `tabIndex`）

0. **文生图** `DrawImageTab`：提示词/负向/尺寸/种子 → `PixelDaApi.GenerateImage`。
1. **图编辑** `DrawEditTab`：图片源(拖拽工程图片/外部文件，或填 URL)+提示词 → `PixelDaApi.EditImage`。
2. **图生视频** `DrawVideoTab`：首帧图源(拖拽/URL)+提示词+分辨率 → `PixelDaApi.GenerateVideo`(异步轮询)。
3. **视频抽帧** `DrawFramesTab`：选视频(浏览/拖拽快捷区 `HandleVideoDrop`) + ffmpeg 抽帧 + 去背景开关 + 合成精灵表(可选列×行布局，`EnsureSpriteLayout` 默认取最近正方形分解 + 因子快捷按钮)/导出 zip。
4. **音乐生成** `DrawMusicTab`：描述/时长/风格/节奏/种子 → `PixelDaApi.GenerateMusic` → ABC → 方波合成试听/存 WAV/存 ABC。
5. **历史** `DrawHistoryTab`：扫描输出目录，按 全部/图片/视频/音频 过滤，缩略图+定位。
6. **设置** `DrawSettingsTab`：双 API Key、输出目录、ffmpeg 路径、(高级) 端点 URL 与模型名。

## REST 接口对照（端点/模型均在设置页可改）

- **豆包(Ark, `DoubaoBaseUrl`)**：`/images/generations`(文生图，含 `image` 字段即图编辑)、`/contents/generations/tasks`(+轮询，图生视频)、`/chat/completions`(音乐 ABC)。尺寸用 `x`(如 `1024x1024`)。
- **通义(DashScope, `TongyiBaseUrl`)**：`/services/aigc/text2image/image-synthesis`、`/image2image/image-synthesis`(图编辑)、`/image2video/video-synthesis`，均带 `X-DashScope-Async: enable`，统一 `GET /tasks/{id}` 轮询；音乐走兼容模式 `TongyiChatBaseUrl/chat/completions`。尺寸用 `*`(如 `1024*1024`)。
- 默认模型常量集中在 `PixelDaConfig.DEFAULT_*`；新增模型/接口变更优先改设置或常量，不要散落硬编码。

## 异步与线程模型

- 所有网络任务在 `Task.Run` 后台线程跑；回调通过 `PixelDaDispatcher.Enqueue` 回主线程再做 UI/资源操作。
- 视频/通义任务用 5 秒轮询 + `Func<bool> isCancelled`（窗口的 `cancelFlag`）支持取消。
- `isBusy` 控制按钮禁用与状态栏；进度文本走 `onProgress` 回调。

## 纯 C# 实现与原工具的差异（改动时须保留说明）

- **去背景**：纯色背景剔除（采样四角主色 + 阈值 + 边缘 4 向洪水填充），适配纯色背景像素图，**非** rembg/u2net AI 抠图。
- **抽帧**：依赖系统 ffmpeg（`PixelDaConfig.FfmpegPath` 或 PATH），**非** OpenCV。
- **音乐**：方波合成 ABC 记谱模拟 8-bit chiptune（实现常见 ABC 子集：L/M/Q/K + 单音/休止/时长/八度/临时升降号），**非** music21+8bit 音色库渲染。

## 约定与约束

- **配置分两层**：API Key/端点/模型/尺寸/输出目录存项目 JSON `Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaProjectConfig.json`(`PROJECT_CONFIG_PATH`，随 git 提交、团队共享，setter 值变化时才写盘)；ffmpeg 路径与当前提供商选择存 EditorPrefs(机器/个人，不入库)。密钥入库有泄露风险，仓库须私有。
- **本地图片喂 AI**：图编辑/图生视频支持拖拽工程内 Texture2D/Sprite 或外部图片文件作为图片源(`DrawImageSourceField`/`HandleImageDrop`)；提交时 `ResolveImageSource` 把本地图编码成 base64 data URI(`PixelDaImageUtil.FileToDataUri`/`TextureToDataUri`)直接传给豆包/通义(两家均接收 base64，无需图床)，本地图优先、远程 URL 兜底。
- 输出统一存 `PixelDaConfig.OutputFolder`(默认 `Assets/Out/PixelDa/<images|videos|frames|sprites|music|zips>/`)，保存后 `AssetDatabase.Refresh` 自动导入。
- 路径全部动态推导：`Application.dataPath`/`GetProjectRoot`/`ToUnityAssetPath`，禁止写死盘符。
- 调用付费 AI 前需用户已配置 API Key；本工具由用户在编辑器内手动点击触发，不在其他任务里"顺带"自动生成。
- 编辑器代码放 `Editor/` 目录；所有方法/属性带 `/// <summary>` XML 注释并用 `#region` 分类（项目规范）。
- 修改本目录文件命中 `watched_files` 时，必须同步更新本 agent、[pixelda](../skills/pixelda/SKILL.md) skill，以及 [editor-extension-system](../skills/editor-extension-system/SKILL.md) 中的工具清单。

## 关联 Skill

详细开发指南：[pixelda](../skills/pixelda/SKILL.md)
