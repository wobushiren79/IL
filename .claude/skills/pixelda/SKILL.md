---
name: pixelda
description: IL 共享框架的「PixelDa 像素美术生成」编辑器工具开发指南。使用此 SKILL 当需要创建或修改 PixelDaEditorWindow（纯 C# 直连豆包/通义生成游戏像素美术）的功能：文生图、图编辑(图生图)、图生视频(5秒动画)、视频抽帧、纯色去背景、精灵表合成、AI 音乐(ABC→chiptune 方波)、生成历史、双提供商(豆包Doubao/通义Tongyi)与端点/模型设置。触发关键词：PixelDa、像素美术生成、文生图、图生视频、ABC 音乐、视频抽帧、豆包 Ark、通义 DashScope、PixelDaEditorWindow。
watched_files:
  - Assets/FrameWork/Editor/Base/Window/PixelDa/
---

# PixelDa 像素美术生成 开发指南

## 概述

`Assets/FrameWork/Editor/Base/Window/PixelDa/`（命名空间 `PixelDa`，菜单 **Custom/AI/像素图生成**）是一个 `EditorWindow`，
用**纯 C#（无 Python 依赖）**复刻开源工具 [dada-x/pixelda](https://github.com/dada-x/pixelda)（原 FastAPI 后端 + Angular 前端）。
通过 HttpClient 直连**豆包(火山引擎 Ark)** 与**通义(阿里百炼 DashScope)** 的 REST 接口，把原工具的网页前端替换为编辑器窗口。

## 文件结构

| 文件 | 职责 |
| --- | --- |
| [PixelDaEditorWindow.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaEditorWindow.cs) | 主窗口：7 页签 UI + 流程编排 + 编辑器内音频试听(反射 `AudioUtil`)；含样式系统(`InitStyles`)与绘制助手(`BeginCard`/`EndCard`/`Section`/`AccentButton`/`PromptField`)：品牌横幅、卡片式分区、强调色主按钮、动态进度条状态栏 |
| [PixelDaCore.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaCore.cs) | `PixelDaConfig`(项目 JSON 共享 + EditorPrefs 本地)、`PixelDaDispatcher`(主线程队列)、`PixelDaProvider` 枚举 |
| [PixelDaApi.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaApi.cs) | REST 客户端 + 后台任务/回调 + 错误提取 |
| [PixelDaImageUtil.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaImageUtil.cs) | 纯色去背景、精灵表合成(横向单行 + 列×行网格重载)、纹理/PNG 读写 |
| [PixelDaFrameUtil.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaFrameUtil.cs) | ffmpeg 抽帧(路径主线程解析、并发读流防死锁+超时)、zip 打包 |
| [PixelDaMusicUtil.cs](Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaMusicUtil.cs) | ABC 解析 + 方波合成 + WAV/ABC 保存 |

## 七个页签

### ① 文生图（`DrawImageTab` / `StartImageGeneration`）
提示词、负向(仅通义)、尺寸(`SizeOptions`)、种子 → `PixelDaApi.GenerateImage` → 返回远程 URL → 下载到 `Out/PixelDa/images/` 并预览。结果可"用作图编辑源 / 用作视频首帧"。

### ② 图编辑（`DrawEditTab` / `StartImageEdit`）
图片源(拖拽工程图片/外部文件，或填 URL，或"用上次文生图结果") + 提示词 → `PixelDaApi.EditImage`（豆包走 `image` 字段，通义走 `image2image` + `description_edit` 功能）。本地图经 `ResolveImageSource` 转 base64 data URI 传入。

### ③ 图生视频（`DrawVideoTab` / `StartVideoGeneration`）
首帧图源(拖拽/URL) + 提示词 + 分辨率(`ResolutionOptions`) → `PixelDaApi.GenerateVideo`（异步任务 + 5 秒轮询，可取消）→ 下载 mp4 到 `Out/PixelDa/videos/`。结果可一键"去抽帧"。

> **本地图片源**：`DrawImageSourceField`(缩略图 + 拖拽区 + ObjectField + URL 兜底) + `HandleImageDrop`(收 Texture2D/Sprite 资源或 .png/.jpg/.webp 外部文件) + `AssetToAbsolutePath`(取资源磁盘路径)。`ResolveImageSource` 优先把本地图(`PixelDaImageUtil.FileToDataUri` 读原文件字节，兜底 `TextureToDataUri` EncodeToPNG)编码为 `data:image/*;base64,...` 直传，两家 API 均接收 base64(无需上传图床)；本地图存在时 URL 输入禁用。

### ④ 视频抽帧（`DrawFramesTab`）
- 选视频：文本框 + 「浏览」面板，或拖拽快捷区 `HandleVideoDrop`（接受 mp4/mov/avi/mkv/webm/m4v/flv/wmv，`IsVideoFile` 按扩展名判定）。
- `StartExtractFrames` → `PixelDaFrameUtil.ExtractFramesAsync`：在 [from,to] 区间均匀取 N 帧（`count==1` 取起点，否则含首尾均分，与原工具一致）。**ffmpeg 路径在主线程经 `ResolveFfmpeg` 解析后传入后台任务**（`EditorPrefs.GetString` 只能主线程调用）；`RunFfmpeg` 并发 `ReadToEndAsync` 读 stdout/stderr 防管道死锁，120s 超时强杀，退出码非 0 携 stderr 抛错。
- `framesRemoveBg` 开关：加载帧时调 `PixelDaImageUtil.RemoveSolidBackground` 去纯色背景。
- `MergeSprite`：可选 `列×行` 布局（`spriteCols`/`spriteRows`，`EnsureSpriteLayout` 按帧数默认取最近正方形分解、UI 提供因子快捷按钮如 8 帧→1×8/2×4/4×2/8×1，格子不足以容纳全部帧时禁用合成并警告）→ `MergeFramesToSprite(frames,columns,rows)` 网格拼接(格子取最大帧宽高、帧居中、第 0 帧左上)存 PNG；`ExportZip`：`ZipFrames` 打包(去背景时先另存处理后帧)。

### ⑤ 音乐生成（`DrawMusicTab` / `StartMusicGeneration`）
描述/时长/风格(`GenreOptions`)/节奏(`TempoOptions`)/种子 → `PixelDaApi.GenerateMusic`(聊天模型输出 ABC) → `SynthesizeMusic` 用 `PixelDaMusicUtil` 合成方波 → 试听/停止/保存 WAV/保存 ABC。ABC 文本框可手改后"重新合成"。

### ⑥ 历史（`DrawHistoryTab`）
`RefreshHistory` 递归扫描输出目录(png/jpg/mp4/wav/abc/zip)，按修改时间倒序，过滤(全部/图片/视频/音频)，图片显缩略图，可"定位/打开目录"。

### ⑦ 设置（`DrawSettingsTab`）
双 API Key(`PasswordField`)、输出目录、ffmpeg 路径(+检测)；高级折叠区可改豆包/通义的 Base URL 与所有模型名，"恢复默认"调 `ResetEndpoints`。提供"重新加载"按钮调 `PixelDaConfig.ReloadProjectData`(他人更新配置后刷新)。

## REST 接口与提供商（`PixelDaApi`）

| 功能 | 豆包(Ark) | 通义(DashScope) |
| --- | --- | --- |
| 文生图 | `POST {base}/images/generations` | `POST {base}/services/aigc/text2image/image-synthesis`(异步) |
| 图编辑 | 同上 + `image` 字段 | `.../image2image/image-synthesis` + `function=description_edit` |
| 图生视频 | `POST {base}/contents/generations/tasks` + 轮询 | `.../image2video/video-synthesis`(异步) |
| 任务轮询 | `GET .../tasks/{id}`，`status` ∈ succeeded/failed | `GET {base}/tasks/{id}`，`output.task_status` ∈ SUCCEEDED/FAILED |
| 音乐 ABC | `POST {base}/chat/completions` | `POST {chatBase}/chat/completions`(兼容模式) |

- 鉴权统一 `Authorization: Bearer <apiKey>`；通义异步接口加 `X-DashScope-Async: enable`。
- 尺寸：豆包发送前 `*`→`x`；通义直接用 `*`。
- 默认端点/模型见 `PixelDaConfig.DEFAULT_*`；**接口变动只改设置或常量，不要硬编码散落各处**。

## 线程与回调

- 网络在 `Task.Run` 后台线程；结果/进度经 `PixelDaDispatcher.Enqueue` 回主线程（`[InitializeOnLoad]` 挂 `EditorApplication.update` 抽干队列）。
- 回调类型：`UrlResultCallback`(图/视频)、`MusicResultCallback`(音乐)；下载用 `PixelDaApi.DownloadFile`。
- 轮询支持 `Func<bool> isCancelled`（窗口 `cancelFlag`），状态栏在视频页显示"取消"按钮。

## 纯 C# 实现细节（区别于原工具）

- **纯色去背景** `RemoveSolidBackground`：四角采样取主色 → 曼哈顿距离阈值 → 从边缘 4 向洪水填充置 alpha=0，只清边缘连通背景，**非** rembg/u2net AI 抠图。
- **抽帧**：`ffmpeg -ss {t} -i video -frames:v 1 out.png` 逐时间戳精确取帧，依赖系统 ffmpeg（`IsFfmpegAvailable` 检测）。
- **音乐合成** `PixelDaMusicUtil`：解析 ABC 头(L 默认长度/M 拍号/Q 速度/K 调号)与曲体(音名/休止/时长数字与斜杠/八度 `'`,/临时升降号 `^_=`)，按 `freq=440*2^((midi-69)/12)` 生成方波(占空比 50%)+5ms 淡入淡出，输出 16bit 单声道 WAV。属常见 ABC 子集，复杂记谱可能退化。

## 配置存储（两层）

- **项目共享层**：API Key/端点/模型/尺寸/输出目录序列化到 `PixelDaConfig.PROJECT_CONFIG_PATH`(=`Assets/FrameWork/Editor/Base/Window/PixelDa/PixelDaProjectConfig.json`)，用 `JsonUtility` 读写，`[Serializable] ProjectData` 承载；setter 仅在值变化时 `SaveProjectData` 写盘(避免 OnGUI 每帧写)；`ReloadProjectData` 手动重载。**随 git 提交**，成员拉取即用。⚠ 密钥入 git 历史有泄露风险，仓库须私有。
- **机器本地层**(EditorPrefs，前缀 `PixelDa.`)：`Provider`(提供商选择)、`FfmpegPath`(ffmpeg 路径)——因人/机而异，不入库。
- 新增配置项时按此分层抉择：团队需统一的进 `ProjectData`，因机/因人而异的留 EditorPrefs。

## 关键约定

- 输出目录 `PixelDaConfig.OutputFolder`(默认 `Assets/Out/PixelDa/`)，子目录 images/videos/frames/sprites/music/zips；保存后 `AssetDatabase.Refresh` + `PingObject` 定位。
- 路径动态化：`GetProjectRoot`/`GetOutputFolderAbsolute`/`ToUnityAssetPath`，禁止写死绝对路径。
- 文件名用 `DateTime.Now:yyyyMMdd_HHmmss` 时间戳；预览纹理在 `OnDisable` 释放。
- 依赖：`System.Net.Http`、`System.IO.Compression.ZipFile`、`System.Collections.Concurrent`、`Newtonsoft.Json`(项目已带)；项目 apiCompatibilityLevel=.NET Standard，`Assets/FrameWork/Editor/`(无 asmdef) 自动归入 `Assembly-CSharp-Editor`。

## 开发规范

- 编辑器脚本置于 `Editor/` 目录，不打包到运行时；所有方法/属性带 `/// <summary>` XML 注释并用 `#region` 分类。
- 付费 AI 调用由用户在窗口内手动触发，不在其他任务里"顺带"自动生成。
- 修改本目录文件（命中 `watched_files`）时，必须同步更新本 SKILL、[pixelda](../../agents/pixelda.md) agent，以及 [editor-extension-system](../editor-extension-system/SKILL.md) 的编辑器工具清单。
