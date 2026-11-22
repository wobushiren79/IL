---
name: pixel-image-editor
description: IL 共享框架的「像素图编辑器」(原名像素完美转换器)编辑器工具开发指南。使用此 SKILL 当需要创建或修改 PixelImageEditorWindow（把 AI 伪像素画重采样为像素对齐图）的功能：五步式工作流(设置/定位转换/编辑导出/辅助功能/颜色调节)、6 种取色算法(最常用/偏亮/偏暗/平均/邻域/最近邻)、画笔/橡皮/魔棒(洪水填充)编辑、撤销重做、调色板颜色替换(全局换色)、帧动画排版/精灵表重排、多图合并图集、双图颜色调节、PNG 导出(x1/x4/x8 另存为/覆盖原图/同目录/按列×行拆分)、网格与背景明暗切换等。类拆分为多个 partial(PixelImageEditorWindow.cs + Step1~Step5.cs)。触发关键词：像素图编辑器、像素完美、Pixel Perfect、AI像素画对齐、颜色替换、调色板替换、颜色调节、帧动画排版、精灵表重排、图片合并、单图合并图集、PixelImageEditorWindow、PixelPerfectConverterWindow。
watched_files:
  - Assets/FrameWork/Editor/Base/Window/PixelImageEditorWindow.cs
  - Assets/FrameWork/Editor/Base/Window/PixelImageEditorWindowStep1.cs
  - Assets/FrameWork/Editor/Base/Window/PixelImageEditorWindowStep2.cs
  - Assets/FrameWork/Editor/Base/Window/PixelImageEditorWindowStep3.cs
  - Assets/FrameWork/Editor/Base/Window/PixelImageEditorWindowStep4.cs
  - Assets/FrameWork/Editor/Base/Window/PixelImageEditorWindowStep5.cs
---

# 像素图编辑器开发指南

## 概述

> **原名「像素完美转换器」，已改名为「像素图编辑器」**：命名空间 `PixelPerfectTool`→`PixelImageEditor`、类 `PixelPerfectConverterWindow`→`PixelImageEditorWindow`、菜单文字全部改为「像素图编辑器」。类由单文件拆成多个 `partial`。

`PixelImageEditorWindow`（`partial class`，命名空间 `PixelImageEditor`，菜单 **Custom/工具弹窗/像素图编辑器**；另在 Project 窗口选中图片后右键 **Assets/像素图编辑器** 可直接用该图打开）是一个 `EditorWindow`，
主流程忠实移植自开源网页工具 [Void8Bit / Pixel-Perfect-AI-Art-Converter](https://github.com/Void8Bit/Pixel-Perfect-AI-Art-Converter)。
它把 AI 生成的「看起来像像素画、实则没有像素对齐」的图，按网格重采样为真正逐像素对齐的像素图，并可编辑、导出、排版与调色。

> 区别于已删除的旧工具 PixelArtConverterWindow（量化+调色板一次性导出）：本工具是完整的多步式工作流，
> 且已把旧工具的「调色板颜色替换」能力整合进步骤③。

### partial 文件拆分（每步单独成文件）

同一 `PixelImageEditorWindow` 拆成 6 个文件：
- **PixelImageEditorWindow.cs**（核心）：枚举/常量/共享字段/生命周期(OnGUI 分发/OnDestroy)/菜单/快捷键/横幅步骤条(`DrawStepBar`/`CanGoToStep`)/步骤切换与源图载入(`EnterStep2/3`、`LoadSource`、`ResetAll`)/颜色工具/UI 辅助/拖拽读取。
- **Step1.cs** 步骤①设置 · **Step2.cs** 步骤②定位转换+转换核心+智能网格检测+Sprite Fusion 网格校正 · **Step3.cs** 步骤③编辑导出+调色板+撤销重做+渲染导出 · **Step4.cs** 步骤④帧排版/图片合并 · **Step5.cs** 步骤⑤颜色调节。
- 新增步骤：写 `PixelImageEditorWindowStepN.cs` partial + 核心文件三处接线（`OnGUI` switch `case N`、`DrawStepBar` names 数组与 `reachable`、`CanGoToStep(N)`）。步骤④⑤为独立工具，`CanGoToStep` 恒 true。

## 工作流（五步）

### 步骤① 设置（`DrawStep1`）
- 网格宽/高下拉：档位 `kGridOptions` = `{16,32,48,64,80,96,112,128,256,512,1024}`，默认目标大小 `_gridWidth/_gridHeight` = **32×32**。
- 源图：拖拽区(`DrawSourceDropArea`)或 ObjectField，支持工程内 Texture / 外部图片文件。
- `LoadSource`：读可读像素(`ReadSourcePixels`，按文件解码，回退 RenderTexture) → 按 `kMaxImageSize`=1024 等比缩小 → 生成自上而下采样数组 `_srcTopDown` 与显示纹理 `_srcDisplayTex`。

### 右键快捷入口（`OpenFromSelection`）
- `[MenuItem("Assets/像素图编辑器")]`：在 Project 窗口选中一张 `Texture2D` 图片后右键，直接打开窗口并把该图设为源图、自动 `LoadSource` + `EnterStep2`，跳过步骤①手动选图。
- `OpenFromSelectionValidate`（`[MenuItem(..., true)]` 校验函数）：仅当 `Selection.activeObject is Texture2D` 时菜单项可用，否则置灰。

### 步骤② 定位与转换（`DrawStep2`）
- 预览格子尺寸 `_previewCellSize`(4~16)：画布每格的「画布像素」数（= canvas.width/gridWidth）。
- 源图缩放 `_zoomPercent`(10~300)：`_imageScale = 值/100`，以画布中心为锚（`ApplyZoom`）。
- 拖拽定位：`HandleStep2Drag` 把屏幕位移 / 显示缩放换算成 `_offsetX/_offsetY`。
- `RefitImage`：居中适配并把缩放滑杆复位到 100（与原工具一致）。
- 算法下方可设：相似度阈值 `_similarityThreshold`(0~100)、限制最终颜色数 `_limitColors` + `_maxColors`(1~256)。
- 选算法后 `Convert` 生成 `_pixels`（末尾按需 `QuantizeToMaxColors`）。

#### 智能网格检测（`DrawStep2AutoDetect`，移植自 theamusing/perfectPixel）
- **来源与取舍**：移植自 [theamusing/perfectPixel](https://github.com/theamusing/perfectPixel) 的纯 numpy 后端 `perfect_pixel_noCV2.py`。原实现「FFT 频谱检测为主 + 梯度法回退」，本移植**只采用梯度法为主检测器**，未移植 2D-FFT（避免手写 FFT 的复杂度/正确性风险；FFT 主检测器留作后续增强）。`refine_grids`/`find_best_grid`/采样/`fix_square` 忠实移植；采样直接复用上述 6 种取色算法。
- **UI 卡片**：位于步骤②手动设置卡与预览卡之间。选项 `_autoUseRefine`(边缘对齐开关，默认 true)、`_refineIntensity`(对齐强度 0~0.5，默认 0.25)、`_autoFixSquare`(近正方形强制正方，默认 true)；两个按钮 + `_autoMessage`(HelpBox 结果提示)。`ResetAll` 复位这些字段。
- **仅检测网格尺寸**（`AutoDetectGridSizeOnly`）：`DetectGridScale` 算格子数 → 填入 `_gridWidth/_gridHeight` + `RefitImage`，供预览核对/微调，不立即转换。
- **智能一键转换 → 步骤③**（`AutoDetectAndConvert`）：`DetectGridScale` →（`_autoUseRefine` ? `RefineGrids` : `BuildUniformGrid`）得网格线坐标 → `ConvertByCoords` 逐格采样生成 `_pixels` → `EnterStep3`，全程无需手动拖拽定位。
- **检测链方法**（全在「智能网格检测 - perfectPixel 移植」region，均源图空间运算，不涉及画布 offset/scale）：
  - `RgbToGray`(0.299/0.587/0.114) → `SobelAbsProjections`(3×3 Sobel，边缘 clamp，输出按列求和 `gxSum[W]` 与按行求和 `gySum[H]`)。
  - `EstimateGridGradient`：`FindProjectionPeaks`(局部峰，`relThr`=0.2、`minInterval`=4) + `MedianInterval`(峰间距中位数)，任一轴峰 <4 判失败。
  - `DetectGridScale`：梯度尺寸 → 像素块边长（长宽比 >1.5 取 min 否则取均值）→ 回推格子数。
  - `RefineGrids`：从中心向两侧按格宽步进，每条网格线用 `FindBestGrid`(±`intensity`×格宽内取最强梯度峰)吸附；`Sort`+`DedupSortedCoords` 去重防零宽格；带循环 guard 防死循环。
  - `ConvertByCoords`：逐格 `SampleSourceRect`(复用 `SampleAverage/SampleMostUsed/SampleWeighted/SampleNearestNeighbor`，邻域算法四周外扩 25%、最近邻取矩形中心像素) → `FixSquare`(|nx-ny|==1 时按奇偶裁末列/行或复制首行/列) → 同步 `_gridWidth/_gridHeight`，按需 `QuantizeToMaxColors`。

#### Sprite Fusion 网格校正（`DrawSpriteFusionSnap`）

- 在步骤②选「Sprite Fusion 网格校正」。该入口直接处理已载入源图，预览区的拖拽和缩放用于下方手动转换。自动检测从横纵边缘峰值估算间距，也可输入源图中像素块的边长（px），或选择「固定当前画布尺寸」使用当前网格数。固定尺寸沿两轴重采样，16×16 图标宜载入正方形构图。检测不到周期时，提示改用后两种方式。
- `ConvertSpriteFusionSnap` 通过 `BuildSnapEdgeProfiles` 比较相邻像素的预乘颜色及 alpha，以 `EstimateSnapPitch` 估计间距。 `SnapGridCuts` 从均匀网格向局部强边缘吸附，各线独立，保留输出格子数。实现参考 [Sprite Fusion Pixel Snapper](https://github.com/Hugo-Dz/spritefusion-pixel-snapper) 的边缘投影、吸附和主色取样，在 Unity 内使用 C#。
- `SampleSnapGrid` 逐格取主色。alpha≥128 的像素须占半数以上才生成可见像素，输出为二值透明。固定色板接受 1–256 种六位 RGB（可带 `#`，逗号或空格分隔），以最近 RGB 映射可见像素。留空时沿用上方「限制最终颜色数」设置。
- 点击「校正并转换 → 步骤③」沿用原有编辑、撤销和导出。要保存原生尺寸，选择「×1」。透明边缘以源图 alpha 为依据，纯色不透明背景可在步骤③用魔棒处理。

### 步骤③ 编辑与导出（`DrawStep3`）
- 三栏布局：左侧工具面板 + 中间实时编辑画布(`DrawEditCanvas`，渲染 Point 纹理 `_artTex`，带网格/笔刷高亮) + 右侧「最终效果图」(`DrawResultPreview`，同 `_artTex` 但**无网格/无高亮**，缩放 `_resultZoom`(1~20)，宽高超限时内部滚动)。
- 底部全宽「调色板/颜色替换」面板(`DrawPaletteSection`)。

### 步骤④ 辅助功能（`DrawStep4` → 页签切换两个子工具）
`DrawStep4` 顶部用 `GUILayout.Toolbar` 切换 `_auxMode`(`AuxMode.Relayout`/`Merge`)，派发到 `DrawStep4Relayout`（帧排版）/ `DrawStep4Merge`（图片合并）。
- **入口**：`DrawStepBar` 的 names 数组含「④ 辅助功能」「⑤ 颜色调节」；`CanGoToStep(4)`/`CanGoToStep(5)` 恒为 true、`reachable = step<=_step || step==4 || step==5`，随时可进入、不依赖步骤①~③数据。

#### ④-A 帧排版（`DrawStep4Relayout`）
独立于主流程的精灵表重排工具：把按「列×行」帧排布的精灵表重排为另一种「列×行」布局，**单帧像素尺寸不变，仅改变帧的行列排布**。
例：256×32 原图填原图帧数 8×1、输出帧数 4×2 → 单帧 32×32，结果拆成 128×64。

- **4 个参数**：原图帧数 `_auxSrcCols`×`_auxSrcRows`、输出帧数 `_auxOutCols`×`_auxOutRows`（默认 8×1 → 4×2），改任一值自动 `RebuildAuxResult`。
- **单帧尺寸** = `_auxSrcW/_auxSrcCols` × `_auxSrcH/_auxSrcRows`（整除；不整除时取整并忽略右/下边缘多余像素，HelpBox 警告）。
- **重排核心 `RebuildAuxResult`**：行优先(从左到右、从上到下)，第 f 帧从源 `(f%sc, f/sc)` 整块搬到输出 `(f%oc, f/oc)`；输出尺寸 = `oc*frameW × or*frameH`，空帧位填 `kTransparent`；输出帧位 < 原帧数时多余帧丢弃并警告。
- **独立源图 + 拖拽替换**：`_auxSourceTexture`/`_auxSourceExternalPath`；`DrawAuxDropArea`/`AcceptAuxDraggedImage`/`LoadAuxExternalImageRef` 复用 `IsDragValid`/`IsImagePath`/`LoadAsProjectAsset`；`LoadAuxSource` 复用 `ReadSourcePixels` 读像素并转自上而下数组 `_auxSrcTopDown`，同时建显示纹理 `_auxDisplayTex`。
- **实时预览**：`_auxResultTex`(`BuildAuxResultTexture`)，`_auxResultZoom`(1~16) 缩放，超高内部滚动。
- **导出**（均经 `BuildAuxResultPng`→`BuildPngFromTopDown` 把自上而下数组翻成 Texture 自下而上后 `EncodeToPNG`）：`ExportAuxAs`（不覆盖导出/另存为，`SaveFilePanel`）、`ExportAuxOverwrite`（覆盖原图导出，弹确认）、`ExportAuxToSourceDir`（同原图目录，名=`原图名_relayout_列x行.png`）；后两者需 `GetAuxSourceFilePath()` 有磁盘文件，否则按钮禁用。

#### ④-B 图片合并（`DrawStep4Merge`）
帧排版的逆操作：把多张单图按「列×行」拼成一张图集。例：4 张 32×32 填 2×2 → 64×64；填 4×1 → 128×32。

- **先设布局再填图**：`_mergeCols`×`_mergeRows`(各 1~32)，改动经 `EnsureMergeSlotCount` 把 `_mergeSlotTex`/`_mergeSlotPath` 两平行 List 对齐到 列×行 长度(保留已有槽内容)后 `RebuildMergeResult`。
- **槽位网格 `DrawMergeSlotGrid`/`DrawMergeSlot`**：按行优先每行 `cols` 个格；每格 = 棋盘底 + 缩略图 + `#序号`角标 + `ObjectField`，支持拖入工程内 `Texture2D` 或外部图片(`IsMergeDragValid`/`AcceptMergeDraggedImage`，外部经 `DecodeExternalImage`→临时纹理并记 `_mergeSlotPath`)；「清空所有槽位」一键复位。
- **合并核心 `RebuildMergeResult`**：格子尺寸 = 所有非空图的**最大宽×最大高**，每张图在格子内**居中**(`ox=(cellW-w)/2`)、空白 `kTransparent`；像素读取复用 `ReadSourcePixels`；尺寸不一时提示已按最大格子居中。结果 `_mergeResultTopDown`/`_mergeResultW/H`，预览 `_mergeResultTex`(`BuildMergeResultTexture`, `_mergeResultZoom` 1~16)。
- **导出**（经 `BuildPngFromTopDown`）：`ExportMergeAs`（另存为，名=`merged_列x行.png`）、`ExportMergeToFirstDir`（导出到首张有磁盘文件单图目录，`GetMergeFirstSourceDir`/`GetSlotFilePath`，无文件禁用）；**无覆盖原图选项**（合并无单一源图）。

- 状态字段带 `_aux`/`_merge` 前缀，独立于主流程；`OnDestroy` 额外释放 `_auxDisplayTex`/`_auxResultTex`/`_mergeResultTex`。

### 步骤⑤ 像素图颜色调节（`DrawStep5`，PixelImageEditorWindowStep5.cs）

并排载入 **两张互相独立** 的图（`_caImageA`/`_caImageB`），各自提取该图出现的所有颜色并可编辑（编辑某色即全局替换该图所有该色像素，同步骤③调色板语义），每张图各有 2 个导出按钮。**两图各持独立调色板**，互不影响。与步骤④一样独立于主流程（`CanGoToStep(5)` 恒 true）。

- **`ColorAdjustImage` 嵌套类**（每图一份）：`sourceTexture`/`externalPath`（外部图解码的临时纹理，非空表示需自行释放）、`topDown`（自上而下像素，既预览也导出的真实数据源）、`w`/`h`、`displayTex`、`palette`/`paletteCounts`/`pixelPaletteIndices`（按 RGBA 提取、忽略透明、按使用量降序）、`previewZoom`(1~16)/`previewScroll`。
- **载入（可拖拽设置）**：`DrawColorAdjustColumn` 每列 = 拖放区(`DrawColorAdjustDropArea`/`AcceptColorAdjustDraggedImage`) + `ObjectField` + 预览 + 调色板 + 导出；复用 `IsDragValid`/`IsImagePath`/`LoadAsProjectAsset`/`DecodeExternalImage`。`LoadColorAdjustImage` 复用 `ReadSourcePixels` 转自上而下数组、建预览纹理、`ExtractColorAdjustPalette`。替换源图时 `FreeColorAdjustTempTexture` 释放上一张外部临时纹理。
- **调色板编辑**：`DrawColorAdjustPalette`/`DrawColorAdjustPaletteCell`（ColorField 替换、A0/A1 透明开关、占比）→ `ReplaceColorAdjustByIndex(img,i)` 按索引整体替换 `img.topDown` 并 `BuildColorAdjustDisplayTex` 重建预览。**无撤销栈**（比步骤③精简）；「重新提取颜色」按钮可手动 `ExtractColorAdjustPalette`。
- **吸管取色（快捷取色，无需打开颜色选择器）**：每个色块带「吸」按钮 → 进入取色模式（`_caPickImage`/`_caPickIndex` 记目标图与槽位，按钮变「吸取中…」并高亮）→ 在**任一图预览**上单击，`HandleColorAdjustPickSample` 按屏幕坐标换算像素采样该色 → 写入目标槽位 → `ReplaceColorAdjustByIndex` → `CancelColorAdjustPick` 退出。取色模式下预览加 `MouseCursor.Link` 光标 + accent 描边 + 顶部 HelpBox 提示；Esc 或再点「吸」取消；`LoadColorAdjustImage` 重载目标图时自动取消（避免索引指向旧调色板）。
- **两个导出（每图独立，参考步骤③）**：`ExportColorAdjustOverwrite`（覆盖原图，弹确认，写 `BuildPngFromTopDown(img.topDown,w,h)`）、`ExportColorAdjustToSourceDir`（同原图目录，名=`原图名_recolor.png`）；均需 `GetSlotFilePath(sourceTexture,externalPath)` 有磁盘文件，否则禁用。
- `OnDestroy` 调 `DisposeColorAdjustImage(_caImageA/_caImageB)` 释放临时源纹理与预览纹理。

## 6 种取色算法（`ConvMethod` / `Convert`）

| 枚举 | 含义 | 实现 |
| --- | --- | --- |
| `Most` | 最常用色 | `SampleMostUsed`：精确直方图 → 阈值 `_similarityThreshold` 欧氏距离聚类 → 主簇内最高频色 |
| `MostLight` | 最常用(偏亮) | `SampleWeighted`：亮度加权聚类，权重 `0.25+0.50*(亮度/255)`，用 `_similarityThreshold` |
| `MostDark` | 最常用(偏暗) | `SampleWeighted`：权重 `0.25+0.50*((255-亮度)/255)`，用 `_similarityThreshold` |
| `Average` | 平均色 | `SampleAverage`：格内非透明像素 RGB 均值（不用阈值） |
| `Neighbor` | 邻域色 | 在格子四周各外扩 25% 后做平均（不用阈值） |
| `NearestNeighbor` | 最近邻色 | `SampleNearestNeighbor`：取格中心映射到源图后最近的一个像素原色，不混合、保留原 alpha（不用阈值） |

- 亮度公式：`0.299*r + 0.587*g + 0.114*b`。
- 画布→源图坐标：`o = (canvasCoord - offset) / imageScale`，floor/ceil 后夹紧到源图范围；空区域置透明。

### 相似度阈值（可设置）

- `_similarityThreshold`（默认 `kDefaultSimilarityThreshold`=30）：相近色归并阈值（RGB 欧氏距离），在步骤②算法下方滑杆设置 0~100；越大越容易合并相近色。
- 仅 `Most`/`MostLight`/`MostDark` 聚类算法使用；`Average`/`Neighbor`/`NearestNeighbor` 选中时该滑杆禁用。

### 最终颜色数量限制（可设置）

- `_limitColors` 开关 + `_maxColors`(1~256)：步骤②算法下方设置。
- 转换末尾 `Convert` 调用 `QuantizeToMaxColors(_maxColors)`：直方图 → 频率加权最远点采样选种子 → K-means 精化（代表色取簇内最常用色）→ 每个非透明像素映射到最近代表色，保证生成图不同颜色数 ≤ 上限；透明像素保持透明。
- 仅转换时生效；步骤③手动笔刷/调色板替换不强制此上限。

## 编辑工具（`EditTool`）

- **画笔/橡皮**：`PaintAt` 按 `_brushSize`(1~5) 方形涂格；橡皮写 `kTransparent`(a==0)。改色自动切回画笔。
- **魔棒**：`DoMagicWand` 收集笔刷区域内所有非透明色，逐色用 `FloodFill`(4 向，`ColorDistance` ≤ `_magicWandThreshold`(0~30)) 抹透明。
- **撤销/重做**：`SaveHistory`/`Undo`/`Redo`，`_historyStack`/`_redoStack` 存像素深拷贝；快捷键 Ctrl+Z / Ctrl+Y（`HandleShortcutKeys`）。
- **最近颜色**：`AddRecentColor`，最多 6 个，点击回填为画笔色。

## 调色板颜色替换（移植自旧 PixelArtConverterWindow 的核心能力）

- `ExtractPalette`：从 `_pixels` 提取不同颜色（按 RGBA、忽略透明），按使用量降序填充 `_palette`/`_paletteCounts`，并记录 `_pixelPaletteIndices`（每像素→调色板索引，-1=透明）。
- `ReplaceByIndex(i)`：把映射到索引 i 的**所有像素**整体替换为 `_palette[i]`（alpha≈0 视为透明）。**按索引替换，天然避免颜色碰撞**，可反复编辑同一槽位（含从透明改回有色）。
- UI：色块网格(`DrawPaletteCell`) 提供 ColorField(替换)、A0/A1(透明开关)、「笔刷」(取此色作画)、占比。
- 同步时机：转换、进入步骤③、撤销/重做、笔刷收笔、魔棒后自动 `ExtractPalette`；笔刷编辑中置 `_paletteDirty` 提示刷新；拖拽色块期间 `_palettePendingHistory`，松手时 `SaveHistory` 合并为一次历史。
- **调色板编辑松手时【不】调用 `ExtractPalette`**：因 `ExtractPalette` 跳过透明像素，若刚把某色设为透明就重建调色板，该色槽会因无像素而消失（旧 bug：设透明 0.几秒后颜色从列表消失）。不重建即让透明槽常驻，`_pixelPaletteIndices` 仍指向原槽，可再改回有色恢复像素。透明槽会在下次笔刷收笔/魔棒/撤销重做/手动刷新时才随重建移除。

## 导出（导出 PNG 卡片）

所有导出底层共用 `BuildRegionPng(col0,row0,col1,row1,scale)`：把像素区域（顶部为 row0）按 scale 放大编码为 PNG 字节（每格 scale×scale 实色块，透明格留空，自上而下数组翻成 Texture 自下而上）。

- **另存为（`ExportImage(scale)`）**：`SaveFilePanel` 选路径，固定 ×1/×4/×8 导出整图。
- **导出倍数（`_exportScale`，IntPopup 1/2/4/8）**：覆盖原图 / 同目录 / 拆分导出三者共用的放大倍数。
- **覆盖原图导出（`ExportOverwriteOriginal`）**：用当前像素图（×_exportScale）写回 `GetSourceFilePath()` 指向的原始文件；destructive，弹确认框；原图无磁盘文件时按钮禁用。
- **同原图目录导出（`ExportToSourceDir`）**：导出到原图所在目录，文件名 = `原图名_输出宽x高.png`。
- **拆分导出（`_splitCols`/`_splitRows`）**：按列×行把整图切块（边界用整数比例 `i*grid/n` 计算，容忍不能整除），核心 `ExportSplitCore(cols,rows,scale,dir,baseName)` 逐块导出 `baseName_r{行}_c{列}.png`。两个入口：`ExportSplit`（`SaveFilePanel` 选基名）、`ExportSplitToSourceDir`（导出到原图目录、基名取原图名，原图无文件时禁用）。
- **`GetSourceFilePath()`**：取原图磁盘绝对路径（优先 `_sourceExternalPath` 外部文件，其次工程内资源 `AssetDatabase.GetAssetPath`→`GetAbsolutePath`）；无文件返回 null（覆盖/同目录导出据此禁用）。
- 写盘后均 `AssetDatabase.Refresh`。

## 关键约定

- `_pixels`：扁平数组，索引 `row*gridWidth + col`，**row0 在顶部**；`alpha==0` = 透明（哨兵 `kTransparent`）。
- 自上而下数据 → Texture2D（自下而上）需翻转：见 `BuildArtTexture`、`LoadSource`、`BuildRegionPng`。
- 外观快捷键：Alt+B 切棋盘背景明暗(`_isDarkBackground`)，Alt+G 切网格线明暗(`_isDarkGrid`)。
- 颜色键：`PackRGB`/`UnpackRGB`(0xRRGGBB)、`PackRGBA`(0xRRGGBBAA)。

## 开发规范

- 编辑器脚本置于 `Editor/` 目录；所有方法/属性带 `/// <summary>` XML 注释并用 `#region` 分类。
- 类为 `partial`，拆成 `PixelImageEditorWindow.cs` + `Step1~Step5.cs`；修改任一 partial（命中 `watched_files`）时，必须同步更新本 SKILL 与 [pixel-image-editor](../../agents/pixel-image-editor.md) agent。
