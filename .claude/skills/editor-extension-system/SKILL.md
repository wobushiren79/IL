---
name: editor-extension-system
description: 编辑器扩展系统开发指南（共享框架层）。使用此SKILL当需要创建或修改Unity编辑器工具、代码生成器、配置表导出工具、Inspector扩展等，包括ExcelEditorWindow、MVCEditorWindow、BaseUICreateWindow、SpineWindow、Inspector扩展、Hierarchy扩展、节点编辑器等。
watched_files:
  - Assets/FrameWork/Editor/
  - Assets/Editor/
  - Assets/FrameWork/Editor/Base/Window/
  - Assets/FrameWork/Editor/ScriptsTemplates/
---

# 编辑器扩展系统开发指南

## 核心概念

项目提供丰富的编辑器扩展工具，覆盖代码生成、配置管理、资源管理、美术工具、测试工具等。

### 编辑器工具体系

```
EditorWindow (Unity)
├── ExcelEditorWindow              # Excel 配置导出
├── MVCEditorWindow                # MVC 代码生成
├── UIEditorWindow                 # UI 代码生成
├── BaseUICreateWindow             # UI 脚本创建向导
├── AddressableWindow              # Addressable 管理
├── SpineWindow                    # Spine 工具
├── NodeBaseEditorWindow           # 节点编辑器
├── SearchEditorWindow             # 搜索编辑器
├── AnimSearchWindow               # 动画搜索
├── ImageResWindow                 # 图片资源窗口
├── CubemapGeneratorWindow         # Cubemap 生成器
├── FBXEditorWindow                # FBX 编辑器
├── SkinMeshEditorWindow           # 皮肤网格编辑器
├── ProjectAssetCollectorWindow    # 项目资源收集器
├── PixelArtPreviewWindow          # 像素图预览工具（多文件夹拖拽→Grid预览→点击Ping定位；虚拟滚动+AssetPreview异步缩略图）
├── StyleBaseWindow                # 样式基础窗口
└── PixelDaEditorWindow            # PixelDa 像素美术生成 (AI 文生图/图编辑/图生视频/抽帧/音乐)

项目编辑器（Assets/Editor/，游戏专属）：
├── StoryCreateEditor              # 故事/剧情创建
├── DatabaseResourceWindowsEditor  # 数据库资源窗口
├── StudyWindowsEditor             # 研究窗口
└── ChangeTextureImportSettings    # 纹理导入设置批量修改
```

---

## Excel 配置导出 (ExcelEditorWindow)

**文件**: `Assets/FrameWork/Editor/Base/Window/ExcelEditorWindow.cs`

### 功能

- Excel 配置表 → JSON 文件导出
- 多语言文本导出
- 支持增量导出

### 打开方式

```
菜单: Custom/工具弹窗/Excel编辑器
```

### 使用流程

```
1. 编辑 Assets/Data/Excel/*.xlsx
2. 打开 ExcelEditorWindow
3. 选择需要导出的 Sheet
4. 点击导出，生成 JSON 到 Resources/JsonText/
```

### `valid` 有效性列约定（生成器内置过滤）

`CreateEntity` 生成 `*Bean.cs` 时会检测工作表是否存在列名为 **`valid`** 的字段（`cellName.Equals("valid")` → `hasValid`）。**只有含该列的表**才会在生成的 `Cfg` 里附带过滤逻辑，其它表生成结果完全不变（按需启用、对存量表零影响）。

约定语义：`valid` 为 `int`，**0=无效（不进入运行时列表），1=有效**。`hasValid` 为真时生成器会：

- `GetAllArrayData()`：`GetInitData` 后追加 `arrayData = System.Array.FindAll(arrayData, itemData => itemData.valid != 0);`，过滤后再缓存。
- `GetItemData(key)`：改走 `InitData(GetAllArrayData())`（而非直接 `GetInitData`），使字典/单点查询都复用同一份已过滤数据 —— 即无效行 `GetItemData(无效id)` 也返回 `null`，运行时彻底不存在。

注意点：

- **默认值坑**：JSON 反序列化 `int` 缺省为 `0`=无效。给某表新增 `valid` 列后，必须把现有每行填 `1` 并重新导出 JSON，否则该表全部数据消失。
- **链式 parent_id**：若某表用 `parent_id` 串等级链，把链中间某级标 0 会令该级 `GetItemData` 返回 null、链从此断开 —— 通常标无效即意图弃用该族，符合预期。

---

## MVC 代码生成 (MVCEditorWindow)

**文件**: `Assets/FrameWork/Editor/Base/Window/MVCEditorWindow.cs`

### 功能

- 根据配置表自动生成 Bean + Service 代码
- 支持字段类型映射
- 自动生成多语言 Property

### 使用流程

```
1. 配置 Excel 表结构（定义字段名和类型）
2. 打开 MVCEditorWindow
3. 选择目标 Excel
4. 配置命名空间和类名
5. 点击生成 → 生成 Bean 类、Service 类
```

### 生成的代码示例

```csharp
// 自动生成的 Bean
[Serializable]
public partial class MyFeatureInfoBean : BaseBean
{
    public long name;           // 名称文本ID
    public long value;          // 数值
    public string class_name;   // 类名

    [JsonIgnore]
    public string name_language
    {
        get { return TextHandler.Instance.GetTextById("MyFeatureInfo", name); }
    }
}

// 自动生成的 Cfg
public partial class MyFeatureInfoCfg : BaseCfg<long, MyFeatureInfoBean>
{
    public static string fileName = "MyFeatureInfo";
    protected static Dictionary<long, MyFeatureInfoBean> dicData = null;

    public static MyFeatureInfoBean GetItemData(long key)
    {
        if (dicData == null)
        {
            MyFeatureInfoBean[] arrayData = GetInitData(fileName);
            InitData(arrayData);
        }
        return GetItemData(key, dicData);
    }

    public static void InitData(MyFeatureInfoBean[] arrayData)
    {
        dicData = new Dictionary<long, MyFeatureInfoBean>();
        for (int i = 0; i < arrayData.Length; i++)
        {
            var itemData = arrayData[i];
            dicData.Add(itemData.id, itemData);
        }
    }
}
```

---

## UI 脚本创建工具 (BaseUICreateWindow)

**文件**: `Assets/FrameWork/Editor/Base/Window/BaseUICreateWindow.cs`

### 功能

- 根据 Prefab 自动生成 UI 脚本
- 支持多种 UI 类型（UI/Dialog/Popup/Toast/View/Common）
- 自动添加脚本组件到 Prefab

### 打开方式

```
菜单: Custom/工具弹窗/UI脚本创建
Toolbar: UI脚本创建 按钮
```

### 脚本模板

| 模板文件 | 生成类型 |
|---------|---------|
| `UI_BaseUI.txt` | 普通 UI（继承 BaseUIView） |
| `UI_BaseUIView.txt` | View 组件（继承 BaseUIComponent） |
| `UI_BaseUIDialog.txt` | 弹窗（继承 DialogView） |
| `UI_BaseUIPopup.txt` | 气泡（继承 PopupShowView） |
| `UI_BaseUIToast.txt` | 提示（继承 ToastView） |

### 使用流程

```
1. 创建 UI Prefab（GameObject 名 = 类名）
2. 在 Prefab 子物体中命名控件（ui_xxx 前缀）
3. 打开 BaseUICreateWindow
4. 拖入 Prefab，选择脚本类型和模块名
5. 点击生成 → 自动创建脚本 + 添加到 Prefab
```

---

## Inspector 扩展

### InspectorBaseUIComponent

**文件**: `Assets/FrameWork/Editor/Base/Inspector/InspectorBaseUIComponent.cs`

自动显示和绑定 `ui_` 前缀的控件：
- 支持 Text, Button, Image, Slider 等常用组件
- 支持自定义组件类型
- 缺失控件高亮提示

### InspectorBaseUIView

**文件**: `Assets/FrameWork/Editor/Base/Inspector/InspectorBaseUIView.cs`

BaseUIView 的 Inspector 扩展，显示 UI 层级和动画信息。

### InspectorEffectBase / InspectorMaskUIView

特效和遮罩的 Inspector 扩展。

---

## Hierarchy 扩展

### HierarchySelect

**文件**: `Assets/FrameWork/Editor/Base/Hierarchy/HierarchySelect.cs`

Hierarchy 窗口中显示额外信息（如组件图标、状态标识等）。

### HierarchySelectPopupSelect

Popup 类型的 Hierarchy 扩展。

---

## 节点编辑器 (NodeBaseEditorWindow)

**文件**: `Assets/FrameWork/Editor/Base/NodeEditor/NodeBaseEditorWindow.cs`

基于节点的可视化编辑器基类，可用于：
- 对话系统编辑
- AI 行为树编辑
- 技能序列编辑

---

## PixelDa 像素美术生成 (PixelDaEditorWindow)

**菜单**: `Custom/AI/像素图生成`

**目录**: `Assets/FrameWork/Editor/Base/Window/PixelDa/`（纯 C# 实现，复刻开源工具 dada-x/pixelda 的全部功能，无 Python 依赖）

| 文件 | 职责 |
|------|------|
| `PixelDaEditorWindow.cs` | 主窗口：文生图/图编辑/图生视频/视频抽帧/音乐/历史/设置 七个页签 |
| `PixelDaCore.cs` | `PixelDaConfig`(EditorPrefs 持久化：双提供商 Key/端点/模型) + `PixelDaDispatcher`(后台任务回主线程) |
| `PixelDaApi.cs` | HttpClient 直连豆包(Ark)/通义(DashScope) REST：文生图、图编辑、图生视频(异步轮询)、音乐 ABC |
| `PixelDaImageUtil.cs` | 纯色背景剔除(洪水填充)、精灵表横向合成、PNG 读写 |
| `PixelDaFrameUtil.cs` | 调系统 ffmpeg 按均匀时间戳抽帧、zip 打包 |
| `PixelDaMusicUtil.cs` | ABC 记谱解析 → 方波(chiptune)合成 WAV/AudioClip |

### 功能与提供商

- 支持**豆包**(`doubao-seedream-4-0`/`seedance`/`seed-1-6`) 与**通义**(`wan2.5-t2i`/`wanx2.1-imageedit`/`wan2.5-i2v`/`qwen-plus`)，设置页填各自 API Key 并可切换。
- 端点 URL 与模型名在「设置→高级」可改（防官方接口变动写死失效）。
- 输出统一存 `Assets/Out/PixelDa/<images|videos|frames|sprites|music|zips>/`，自动 `AssetDatabase.Refresh` 导入工程。

### 与原工具的纯 C# 实现差异

- **去背景**为「纯色背景剔除」(采样四角主色 + 阈值 + 边缘洪水填充)，适配纯色背景像素图，非原工具的 rembg/u2net AI 抠图。
- **抽帧**依赖系统 ffmpeg（设置页可配路径），非原工具的 OpenCV。
- **音乐**用方波合成 ABC 记谱模拟 8-bit chiptune（实现常见 ABC 子集），非原工具的 music21+8bit 音色库渲染。

> 该工具有专属开发文档：[pixelda](../pixelda/SKILL.md) skill 与 [pixelda](../../agents/pixelda.md) agent，详细功能/接口/线程模型见其中。

---

## 创建新的编辑器窗口

### 继承 EditorWindow 创建编辑器工具

```csharp
// Assets/FrameWork/Editor/Base/Window/MyEditorWindow.cs
public class MyEditorWindow : EditorWindow
{
    [MenuItem("Custom/工具弹窗/我的工具")]
    public static void ShowWindow()
    {
        var window = GetWindow<MyEditorWindow>("我的工具");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }

    private string inputData = "";
    private Vector2 scrollPos;

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("配置区域", EditorStyles.boldLabel);

        inputData = EditorGUILayout.TextField("输入数据", inputData);

        EditorGUILayout.Space(10);

        if (GUILayout.Button("执行操作", GUILayout.Height(30)))
        {
            ExecuteOperation();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private void ExecuteOperation()
    {
        // 编辑器逻辑
        EditorUtility.DisplayDialog("结果", $"处理完成: {inputData}", "确定");
    }
}
```

### 继承 Editor 创建 Inspector 扩展

```csharp
// Assets/FrameWork/Editor/Base/Inspector/InspectorMyComponent.cs
[CustomEditor(typeof(MyComponent))]
public class InspectorMyComponent : Editor
{
    private SerializedProperty propSpeed;
    private SerializedProperty propName;

    private void OnEnable()
    {
        propSpeed = serializedObject.FindProperty("speed");
        propName = serializedObject.FindProperty("displayName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propSpeed);
        EditorGUILayout.PropertyField(propName);

        EditorGUILayout.Space(10);

        if (GUILayout.Button("快速配置"))
        {
            propSpeed.floatValue = 10f;
            propName.stringValue = "Default";
        }

        serializedObject.ApplyModifiedProperties();
    }
}
```

---

## 脚本模板系统

**路径**: `Assets/FrameWork/Editor/ScriptsTemplates/`

| 模板文件 | 用途 |
|---------|------|
| `Excel_Bean.txt` | Excel 配置 Bean 模板 |
| `Excel_Cfg.txt` | Excel 配置管理类模板 |
| `Excel_LanguageEntity.txt` | 多语言实体 Bean 模板 |
| `UI_BaseUI.txt` | 普通 UI 脚本模板 |
| `UI_BaseUIView.txt` | View 组件脚本模板 |
| `UI_BaseUIDialog.txt` | 弹窗脚本模板 |
| `UI_BaseUIPopup.txt` | 气泡脚本模板 |
| `UI_BaseUIToast.txt` | 提示脚本模板 |
| `Service_Base.txt` | 数据服务模板 |

---

## 文件位置速查

| 功能 | 文件路径 |
|------|----------|
| 框架编辑器根目录 | `Assets/FrameWork/Editor/` |
| 编辑器窗口 | `Assets/FrameWork/Editor/Base/Window/` |
| Inspector 扩展 | `Assets/FrameWork/Editor/Base/Inspector/` |
| Hierarchy 扩展 | `Assets/FrameWork/Editor/Base/Hierarchy/` |
| 节点编辑器 | `Assets/FrameWork/Editor/Base/NodeEditor/` |
| 脚本模板 | `Assets/FrameWork/Editor/ScriptsTemplates/` |
| 编辑器工具类 | `Assets/FrameWork/Editor/Base/Utils/` |
| AssetBundle 工具 | `Assets/FrameWork/Editor/AssetBundles/` |
| 项目编辑器 | `Assets/Editor/` |
| PixelDa 像素生成工具 | `Assets/FrameWork/Editor/Base/Window/PixelDa/` |
| 故事/剧情创建 | `Assets/Editor/StoryCreateEditor.cs` |
| 数据库资源窗口 | `Assets/Editor/DatabaseResourceWindowsEditor.cs` |
| 研究窗口 | `Assets/Editor/StudyWindowsEditor.cs` |
| 纹理导入设置修改 | `Assets/Editor/ChangeTextureImportSettings.cs` |
| Excel 配置目录 | `Assets/Data/Excel/` |

---

## 注意事项

1. **Editor 代码隔离**: 编辑器代码必须放在 `Editor/` 目录下，打包时不会包含。
2. **UNITY_EDITOR 宏**: 运行时引用的编辑器代码需要 `#if UNITY_EDITOR` 包裹。
3. **EditorPrefs 持久化**: 编辑器中的参数使用 EditorPrefs 保存，跨会话有效。
4. **Application.isPlaying 检查**: 编辑器按钮的操作如果需要在运行时执行，需检查 `Application.isPlaying`。
5. **MenuItem 路径**: 菜单项路径格式为 `Custom/分类/功能名`。
6. **Prefab 修改**: 编辑器代码修改 Prefab 后需要调用 `EditorUtility.SetDirty()` 或 `PrefabUtility.SavePrefabAsset()`。
