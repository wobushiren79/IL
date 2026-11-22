---
name: aseprite-pixel-editor
description: Aseprite 源文件（.aseprite/.ase）检查、创建、像素修改、描边修复、精确换色、PNG 新图层、指定原图层 Cel 替换与导出流程。当用户要求检查或修改角色/道具 Aseprite 母版、增加独立装备图层、替换指定部位、批量处理图层/帧或创建新的像素源文件时使用。只负责 Aseprite 美术源文件，不处理游戏装备配置或 Unity 关联。
watched_files:
  - .claude/scripts/run-aseprite.ps1
  - .claude/scripts/aseprite_tool.lua
---

# Aseprite 像素源文件编辑

本文负责通用 Aseprite Cel 工具与像素源文件验收。具体装备款式、角色锚点和正式交付位置由用户与 IL 当前资源结构确定。

## 一、范围与现有资源

- 由用户指定或从 IL 当前资源结构定位目标 `.aseprite/.ase` 文件；不要假设固定的角色目录或母版名称。
- 本 Skill 只处理 Aseprite 母版和预览导出，不修改 Unity 配置表、Addressables、Spine 或游戏装备关联。
- 使用 Aseprite 官方 CLI + Lua，禁止用 Write/Edit 或自写二进制替换直接修改 `.aseprite`。

## 二、工具文件

- [.claude/scripts/run-aseprite.ps1](../../scripts/run-aseprite.ps1)：动态定位 Aseprite，依次检查 `ASEPRITE_EXE`、PATH、卸载注册表、Steam 主库与其他 Steam 库、常见安装目录；不写死盘符。通过输出管道等待 GUI 进程完整退出，调用返回后再检查输出文件。
- [.claude/scripts/aseprite_tool.lua](../../scripts/aseprite_tool.lua)：通用只读检查与非覆盖编辑操作。

所有命令从项目根目录执行，优先 PowerShell。标准调用形式：

```powershell
$runner = ".claude/scripts/run-aseprite.ps1"
$tool = ".claude/scripts/aseprite_tool.lua"
powershell.exe -NoProfile -File $runner -WhichOnly
```

`-WhichOnly` 只输出实际解析到的 `Aseprite.exe`。若自动定位失败，先让用户设置 `$env:ASEPRITE_EXE`，不要在文档或脚本里写死本机路径。

## 三、不可省略的安全规则

1. **原文件只读**：编辑统一输出到新的 `.aseprite`；`src` 与 `out` 相同会被脚本拒绝。
2. **默认不覆盖**：输出已存在时报错；只有用户明确允许覆盖中间产物时才传 `overwrite=true`，它仍不允许覆盖 `src`。
3. **先检查再修改**：先 `inspect` 记录尺寸、颜色模式、图层、帧、Cel、切片，再决定操作范围。
4. **编辑副本**：保存后重新 `inspect` 输出文件，并导出 PNG 做视觉核对；Aseprite 脚本异常也可能伴随退出码 0，必须同时检查预期输出与操作报告，不能仅凭退出码或 `ASEPRITE_TOOL_OK` 宣布图片正确。
5. **RGB 限制**：`create/recolor/add_layer` 当前只支持 RGB/RGBA。Indexed/Grayscale 只能 `inspect`；如需转换，必须在副本上另行确认颜色与调色板影响。
6. **坐标规则**：Cel Image 使用局部坐标；画布坐标需结合 `cel.position`。新增 PNG 的 `x/y` 是 Cel 在画布上的位置。
7. **动画规则**：未指定 `frames` 的换色会处理所有帧；链接 Cel 可能共享图像，动手前必须确认是同步修改还是断链后只改一帧。
8. **导出规则**：修改 `.aseprite` 不等于 PNG 已更新；需要预览时显式执行 Aseprite 原生 `--save-as` 导出。
9. **临时产物**：验证用的临时文件放 `$env:TEMP` 或明确的临时目录，验证后清理；通用脚本不能当临时脚本删除。

### 完美像素检查

每次使用 Aseprite 绘制或修复像素图时，都在原生网格检查线条、描边和接合处。用铅笔自由绘制时可启用工具栏的 Pixel-perfect 模式。Lua 用 `app.useTool` 模拟笔画时可显式设置 `freehandAlgorithm=1`。字符网格、逐坐标写入或导入 PNG 时直接核对实际落点。[Aseprite 铅笔工具设置](https://aseprite.org/docs/context-bar/#pixel-perfect-mode)，[Aseprite Lua `app.useTool`](https://aseprite.org/api/app#appusetool)。

- 沿原稿的受光方向和材质逐段选边色，在原生网格上把通常为一像素宽的外轮廓接成连续阶梯。亮侧沿用原包边或集中高光，背光侧沿用局部暗色。发光、虚化、碎裂等有意间断的边缘保留原效果。
- 将透明单层与实际合成对照，分清外轮廓、内折线和材质暗面。用相接材质已有的局部暗阶交代帽口内缘、衣褶转折或部件接缝，只在多叠一层的边线上逐格收薄。连片的深部阴影承担体积，保留其宽度与形状。
- 对设定为 1 像素宽的细线、细描边和支架，检查转斜处多余的 L 形拐角、局部双厚、断点与孤点。按轮廓的坡度整理相邻阶梯的步长，例如把平滑斜边上突兀的短、长、短步长调匀。保留有意的粗细变化、结构转折和尖角。
- 去掉凸点、补齐断点或收薄暗边后，检查翼尖等细端是否连到主体、镂空是否保形、相邻图层是否在原锚点接合。用独立图层核对像素，再用实际合成检查遮挡与接缝。
- 将颜色调整和像素位置变化分开核对。对照原稿检查像素坐标、透明区域与实际轮廓，另看暗边宽度和色阶过渡。用 1× 看整体轮廓和线条是否清晰，用最近邻整数倍放大逐格检查转折、台阶和接合。

#### 保留原形的局部调色

- 用户只确认了新描边时，把确认的边线叠回原稿，保留它的走向和边色，内部以用户指定的原版本为准。只修描边时比较逐格差异，改动应落在指定边线和接缝内，Cel 坐标和透明轮廓与原稿一致。用户明确要求改形时，再按目标轮廓增减像素、调整剪影或开口。
- 用户同时要求收拢碎色时，先在原位标出亮面、暗面、纹样和材质交界。只把同一材质、同一明暗面里一两格来回跳色且不表示结构的像素并入相邻原色，沿原色面走向把相近色接成可读的连片，保留布褶、甲片朝向和原光源。细刺绣、铆钉、符文转角和原有高光即使只有一格，也按其作用保留。保持原明暗分区和纹样的位置、宽度与转折，避免整片重涂、锐化或沿色块新勾硬亮边。
- 在同一坐标切换原稿与修稿的透明单层，再看实际保存的原生 1× 合成和最近邻整数倍放大。先确认剪影、纹样位置与明暗分区重合，再逐处检查并色是否接回本材质、边线是否连续。整帧颜色数和孤点计数只能辅助定位。

## 四、操作 1：inspect 检查源文件

```powershell
$source = (Resolve-Path "<目标Aseprite文件路径>").Path
$report = Join-Path $env:TEMP "aseprite-inspect.json"
powershell.exe -NoProfile -File $runner -b `
  --script-param "op=inspect" `
  --script-param "src=$source" `
  --script-param "report=$report" `
  --script $tool
```

报告包含：画布宽高、`color_mode`、图层数量与名称、帧时长、每个 Cel 的帧号/图层/位置/尺寸、切片名称。未传 `report` 时 JSON 输出到控制台。

## 五、操作 2：create 创建 RGB 母版

输出目录必须已存在；默认创建一个全透明 Cel。

```powershell
$output = Join-Path $env:TEMP "aseprite-create.aseprite"
$createReport = Join-Path $env:TEMP "aseprite-create.json"
powershell.exe -NoProfile -File $runner -b `
  --script-param "op=create" `
  --script-param "out=$output" `
  --script-param "width=32" `
  --script-param "height=32" `
  --script-param "layer=Body" `
  --script-param "report=$createReport" `
  --script $tool
```

必填：`out/width/height`。可选：`layer`（默认 `Layer 1`）、`report`、`overwrite`。

## 六、操作 3：recolor 精确换色

```powershell
$recolored = Join-Path $env:TEMP "aseprite-recolor.aseprite"
$recolorReport = Join-Path $env:TEMP "aseprite-recolor.json"
powershell.exe -NoProfile -File $runner -b `
  --script-param "op=recolor" `
  --script-param "src=$source" `
  --script-param "out=$recolored" `
  --script-param "from=#112233FF" `
  --script-param "to=#445566FF" `
  --script-param "layer=Body" `
  --script-param "frames=1,2,3" `
  --script-param "report=$recolorReport" `
  --script $tool
```

- 颜色格式：`RRGGBB` 或 `RRGGBBAA`，`#` 可选；省略 alpha 时按 `FF`。
- `layer` 省略表示全部图层；`frames` 省略表示全部帧。
- 默认跳过直接标记为隐藏的图层；确需包含时传 `include_hidden=true`。
- 默认没有命中像素会报错，避免写错颜色后仍生成“成功”文件；确认需要空操作时才传 `allow_no_match=true`。
- 报告中的 `changed_pixels/changed_cels` 是实际修改数量，必须纳入验证。

## 七、操作 4：add_layer 从 PNG 新增独立图层

适合把已经绘制好的装备、武器或部件 PNG 加到母版，不直接画进身体层。

```powershell
$withLayer = Join-Path $env:TEMP "aseprite-layer.aseprite"
$layerReport = Join-Path $env:TEMP "aseprite-layer.json"
powershell.exe -NoProfile -File $runner -b `
  --script-param "op=add_layer" `
  --script-param "src=$source" `
  --script-param "out=$withLayer" `
  --script-param "png=$png" `
  --script-param "layer=Weapon_New" `
  --script-param "frame=1" `
  --script-param "x=0" `
  --script-param "y=0" `
  --script-param "report=$layerReport" `
  --script $tool
```

- 必填：`src/out/png/layer`。
- `frame` 默认 1，`x/y` 默认 0；PNG 与母版都必须是 RGB/RGBA。
- 当前每次新增一个顶层图层和一个 Cel；同名顶层图层会报错。默认位于栈顶，传 `after=现有图层名` 可插到该层之上，报告会记录 `after`。
- 打开并关闭临时 PNG Sprite 后必须执行 `app.activeSprite = sprite` 恢复母版，再进入事务；否则批处理可能中断且不生成输出。
- 多帧不同图片、链接 Cel、图层组内插入、像素矩阵绘制等需求，先扩展 Lua，不要用重复调用冒充已支持。

## 八、操作 5：set_cel 在原图层替换或补充 Cel

适合替换肩甲、胸带、腰布等已有装备，同时保留原图层顺序；目标层在该帧没有 Cel 时则新建。

```powershell
$withCel = Join-Path $env:TEMP "aseprite-cel.aseprite"
$celReport = Join-Path $env:TEMP "aseprite-cel.json"
powershell.exe -NoProfile -File $runner -b `
  --script-param "op=set_cel" `
  --script-param "src=$source" `
  --script-param "out=$withCel" `
  --script-param "png=$png" `
  --script-param "layer=Clothes_1_Body" `
  --script-param "frame=1" `
  --script-param "x=44" `
  --script-param "y=55" `
  --script-param "report=$celReport" `
  --script $tool
```

- 必填：`src/out/png/layer`；`frame` 默认 1。
- 已存在 Cel 时，`x/y` 默认沿用原位置；新建 Cel 时默认 0。报告用 `replaced_existing` 区分替换与新增。
- 每次只处理一个顶层图层的一帧。多帧装备可串联多个副本操作，但必须核对其他帧未因链接 Cel 被意外修改。

## 九、导出 PNG 预览

使用 Aseprite 原生 CLI，不在通用 Lua 里重复实现导出器：

```powershell
$preview = Join-Path $env:TEMP "aseprite-preview.png"
powershell.exe -NoProfile -File $runner -b `
  $withLayer --save-as $preview
```

动画可能输出多张带帧号的 PNG，应检查实际生成文件，不能假设只有 `$preview` 一张。

## 十、完成一次修改的验收清单

1. 原 `src` 的路径、时间戳和内容未被编辑流程覆盖。
2. 输出 `.aseprite` 能重新打开，`inspect` 的画布、颜色模式、图层和帧符合预期。
3. `recolor` 的修改像素数合理；`add_layer` 的图层名、帧号、PNG 尺寸和偏移合理。
4. PNG 预览中透明背景、边缘、颜色、遮挡、各帧位置正确。绘制或修复的像素通过[完美像素检查](#完美像素检查)。
5. 临时预览、报告与测试母版已清理；源文件和最终交付文件保留在明确位置。
6. 修改了 `run-aseprite.ps1` 或 `aseprite_tool.lua` 时，必须在同一任务同步更新本 Skill，并逐项核对 frontmatter 的 `watched_files` 与脚本相对链接。

## 十一、后续扩展规则

- 新操作在 `aseprite_tool.lua` 增加独立函数与 `op` 分支，默认仍是非覆盖输出。
- 每种新操作至少验证：新建临时母版、执行操作、重新 inspect、导出预览、清理临时文件；还要用中文路径检查 JSON 报告仍是有效 UTF-8。
- 复杂像素绘制优先增加“从 PNG/像素矩阵生成 Cel”，不要把大量像素坐标塞进命令行。
- 优先保持图层名、分组、顺序、帧时长、切片不变；需求明确要求调整时才修改这些元数据。
- 官方参考：[Aseprite CLI](https://www.aseprite.org/docs/cli/)、[Lua API](https://www.aseprite.org/api/)。
