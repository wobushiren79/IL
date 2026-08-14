---
name: framework-core-system
description: 框架核心基础类开发指南（共享框架层）。使用此SKILL当需要理解或修改框架核心类、创建新的Manager/Handler、使用单例模式、理解继承体系或AutoLinkUI机制等，包括BaseMonoBehaviour、BaseSingleton、BaseSingletonMonoBehaviour、BaseManager、BaseHandler<T,M>配对模式、BaseMVCService、BaseComponent、BaseUIInit、BaseUIView、BaseUIComponent、BaseEvent等核心基类。
watched_files:
  - Assets/FrameWork/Scripts/Base/
  - Assets/FrameWork/Scripts/Component/Manager/
  - Assets/FrameWork/Scripts/Component/Handler/
  - Assets/FrameWork/Scripts/Component/UI/
  - Assets/FrameWork/Scripts/Component/Other/
---

# 框架核心系统开发指南

## 核心继承体系

### MonoBehaviour 继承链

```
UnityEngine.MonoBehaviour
    │
    ▼
BaseMonoBehaviour                      # 通用方法：Instantiate, Find, AutoLinkUI
    │
    ├── BaseManager                    # 资源加载、数据管理
    │   ├── UIManager / AudioManager / GameDataManager / EffectManager ...
    │
    ├── BaseComponent                  # 组件基类
    ├── BaseUIComponent                # UI 组件基类
    │
    └── BaseUIInit                     # UI 初始化，自动链接控件
        └── BaseUIView                 # UI 视图基类
```

### 单例体系

```
BaseSingleton<T> where T : new()       # 非 Mono 单例（双重检查锁）
    └── EventHandler                   # 全局事件管理器

BaseSingletonMonoBehaviour<T>          # Mono 单例
    └── BaseHandler<T, M>             # Handler 基类（自动持有 Manager）
        ├── GameDataHandler / AudioHandler / EffectHandler / UIHandler ...
```

---

## BaseMonoBehaviour - 所有组件的基类

**文件**: `Assets/FrameWork/Scripts/Base/BaseMonoBehaviour.cs`

### 核心方法

```csharp
public class BaseMonoBehaviour : MonoBehaviour
{
    // 实例化 GameObject
    public GameObject Instantiate(GameObject objContent, GameObject objModel);
    public GameObject Instantiate(GameObject objContent, GameObject objModel, Vector3 position);

    // 通过名称查找组件
    public T Find<T>(string name);
    public T FindInChildren<T>(string name);

    // 通过标签查找
    public T FindWithTag<T>(string tag, string name = null);
    public List<T> FindListWithTag<T>(string tag);

    // 通过反射自动链接 ui_ 前缀的 UI 控件
    public void AutoLinkUI();
}
```

### 使用示例

```csharp
public class MyComponent : BaseMonoBehaviour
{
    private void Start()
    {
        // 自动链接所有 ui_ 前缀的控件
        AutoLinkUI();
        
        // 实例化预制体
        GameObject obj = Instantiate(prefabContent, prefabModel);
        
        // 查找子组件
        Button btn = FindInChildren<Button>("ui_Submit");
    }
}
```

---

## BaseSingleton\<T\> - 非Mono单例

**文件**: `Assets/FrameWork/Scripts/Base/BaseSingleton.cs`

### 实现原理

```csharp
public abstract class BaseSingleton<T> where T : new()
{
    protected static T instance;
    protected static object syncRoot = new Object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)  // 双重检查锁，线程安全
                {
                    if (instance == null)
                        instance = new T();
                }
            }
            return instance;
        }
    }
}
```

### 创建新的非Mono单例

```csharp
public class MyService : BaseSingleton<MyService>
{
    private Dictionary<string, object> cache = new Dictionary<string, object>();

    public void CacheData(string key, object data)
    {
        cache[key] = data;
    }

    public object GetCachedData(string key)
    {
        cache.TryGetValue(key, out var data);
        return data;
    }
}

// 使用
MyService.Instance.CacheData("config", myConfig);
var config = MyService.Instance.GetCachedData("config");
```

---

## BaseSingletonMonoBehaviour\<T\> - Mono单例

**文件**: `Assets/FrameWork/Scripts/Base/BaseSingletonMonoBehaviour.cs`

```csharp
public class BaseSingletonMonoBehaviour<T> : BaseMonoBehaviour
    where T : BaseMonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 场景中查找现有实例
                _instance = FindObjectOfType<T>();
                // 如果找不到，创建新的 GameObject
                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
    }
}
```

---

## BaseManager - 管理器基类

**文件**: `Assets/FrameWork/Scripts/Component/Manager/BaseManager.cs`

### 核心职责

| 职责 | 说明 |
|------|------|
| 资源加载 | 同步/异步加载和管理资源缓存 |
| 数据管理 | 持有和操作相关数据 |
| 对象池 | 管理 GameObject 复用 |

### 核心方法

```csharp
public class BaseManager : BaseMonoBehaviour
{
    // 数据初始化
    public void InitData<T>();

    // 同步加载资源
    public T GetModel<T>(string path) where T : UnityEngine.Object;

    // Addressables 异步加载单个
    public void GetModelForAddressables<T>(string key, Action<T> callback);

    // Addressables 异步加载批量
    public void GetModelsForAddressables<T>(string label, Action<IList<T>> callback);

    // 从 SpriteAtlas 加载精灵
    public Sprite GetSpriteByName(string atlasName, string spriteName);
}
```

### 创建新的 Manager

```csharp
// Assets/FrameWork/Scripts/Component/Manager/MyFeatureManager.cs
public class MyFeatureManager : BaseManager
{
    // 资源缓存字典
    private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();

    // 数据
    private MyFeatureData featureData;

    public void LoadFeaturePrefab(string prefabName, Action<GameObject> callback)
    {
        // 检查缓存
        if (prefabCache.TryGetValue(prefabName, out var cached))
        {
            callback?.Invoke(cached);
            return;
        }

        // 异步加载
        GetModelForAddressables<GameObject>(prefabName, (prefab) =>
        {
            if (prefab != null)
            {
                prefabCache[prefabName] = prefab;
            }
            callback?.Invoke(prefab);
        });
    }
}
```

---

## BaseHandler\<T, M\> - Handler-Manager 配对模式

**文件**: `Assets/FrameWork/Scripts/Component/Handler/BaseHandler.cs`

### 核心设计

Handler 作为单例提供全局访问点（逻辑处理），自动创建/获取 Manager 实例（资源管理）。

```csharp
public class BaseHandler<T, M> : BaseSingletonMonoBehaviour<T>
    where M : BaseManager
    where T : BaseMonoBehaviour
{
    private M _manager;

    public M manager
    {
        get
        {
            if (_manager == null)
                _manager = gameObject.AddComponentEX<M>();
            return _manager;
        }
    }
}
```

### Handler-Manager 配对模式详解

```
┌───────────────────────┐         ┌───────────────────────┐
│  Handler (单例)        │         │  Manager (MonoBehaviour)│
│  - 纯逻辑处理          │ ──持有──►│  - 资源加载与缓存       │
│  - 外部调用入口         │         │  - 状态数据维护         │
│  - 自动创建 Manager    │         │  - 生命周期管理         │
└───────────────────────┘         └───────────────────────┘

调用方式：XXXHandler.Instance.DoSomething()
内部访问：handler.manager.GetResource()
```

### 创建新的 Handler-Manager 对

```csharp
// 1. 创建 Manager
public class MyFeatureManager : BaseManager
{
    public void LoadData(Action<MyData> callback)
    {
        // 资源/数据加载逻辑
    }
}

// 2. 创建 Handler
public class MyFeatureHandler : BaseHandler<MyFeatureHandler, MyFeatureManager>
{
    // 对外暴露的逻辑方法
    public void DoSomething()
    {
        // 通过 manager 访问资源
        manager.LoadData((data) =>
        {
            // 处理逻辑
        });
    }
}

// 3. 使用
MyFeatureHandler.Instance.DoSomething();
```

### 已有的 Handler-Manager 配对一览（框架层，按项目实际配置增减）

| Handler | Manager | 职责 |
|---------|---------|------|
| `UIHandler` | `UIManager` | UI管理 |
| `AudioHandler` | `AudioManager` | 音频系统 |
| `CameraHandler` | `CameraManager` | 摄像机 |
| `EffectHandler` | `EffectManager` | 特效 |
| `SpineHandler` | `SpineManager` | Spine动画 |
| `GameDataHandler` | `GameDataManager` | 游戏数据 |
| `InputHandler` | `InputManager` | 输入管理 |
| `IconHandler` | `IconManager` | 图标管理 |
| `TextHandler` | `TextManager` | 多语言文本 |
| `AIHandler` | `AIManager` | AI管理 |
| `CShaderHandler` | `CShaderManager` | 自定义Shader |
| `ModHandler` | `ModManager` | Mod资源 |
| `VolumeHandler` | `VolumeManager` | 音量 |

> 完整清单以 `Assets/FrameWork/Scripts/Component/Handler/` 与 `Manager/` 目录为准；业务侧新增系统时按此配对模式新建 Handler + Manager。

---

## BaseMVCService（SQLite 服务基类）

**文件**: `Assets/FrameWork/Scripts/Base/BaseMVCService.cs`

项目数据层大量使用 SQLite，业务服务（角色/物品/NPC/菜单/技能/成就等 30+ 服务）继承此基类，通过 `SQLiteHandle` 读写 `ProjectConfigInfo.DATA_BASE_INFO_NAME` 数据库。

```csharp
public class BaseMVCService
{
    public BaseMVCService(string tableName);                          // 单表
    public BaseMVCService(string tableName, string leftDetailsTableName); // 链表

    public List<T> BaseQueryAllData<T>();
    public List<T> BaseQueryAllData<T>(string leftId);
    public List<T> BaseQueryData<T>(string key, string value);
    public bool BaseInsertData<T>(string tableName, T itemData);
    public bool BaseInsertDataWithLeft<T>(T itemData, List<string> listLeftName);
    public bool BaseDeleteDataById(long id);
    public bool BaseDeleteDataWithLeft(string mainName, string leftName, string value);
    // ... 多条件查询/删除重载
}
```

> 该基类的完整使用方式（业务服务示例、Bean 约定、JSON 存档基类）见 data-service-system SKILL。另有 `IBaseMVCView.cs`（视图接口，保留基类）。

---

## UI 基类体系

### BaseUIInit

**文件**: `Assets/FrameWork/Scripts/Base/BaseUIInit.cs`

```csharp
public class BaseUIInit : BaseMonoBehaviour
{
    // 自动链接 UI 控件（ui_ 前缀）
    public void AutoLinkUI();

    // 注册所有按钮点击回调
    public void RegisterButtons();

    // 打开/关闭 UI
    public virtual void OpenUI();
    public virtual void CloseUI();

    // 按钮点击回调
    public virtual void OnClickForButton(Button viewButton);

    // 生命周期
    public virtual void RefreshUI(bool isOpenInit = false);

    // 按键输入回调
    public virtual void OnInputActionForStarted(InputActionUIEnum inputType, CallbackContext callback);
}
```

### BaseUIView

**文件**: `Assets/FrameWork/Scripts/Base/BaseUIView.cs`

```csharp
public class BaseUIView : BaseUIInit
{
    protected RectTransform rectTransform;   // RectTransform 缓存
    protected Vector2 uiSizeOriginal;        // 原始 UI 大小

    // 子类在此实现具体 UI 逻辑
}
```

### BaseUIComponent

**文件**: `Assets/FrameWork/Scripts/Base/BaseUIComponent.cs`

```csharp
public class BaseUIComponent : BaseUIInit
{
    protected UIManager uiManager;            // UI 管理器引用
    protected UICloseTypeEnum uiCloseType;    // 关闭类型 (Hide/Destory)
}
```

---

## AutoLinkUI 机制

通过反射自动绑定所有 `ui_` 前缀的控件，无需手动拖拽赋值。

### 控件命名规范

```
ui_Submit      -> public Button ui_Submit;
ui_Title       -> public Text ui_Title;
ui_Icon        -> public Image ui_Icon;
ui_Input       -> public InputField ui_Input;
ui_List        -> public ScrollRect ui_List;
ui_Toggle      -> public Toggle ui_Toggle;
ui_Slider      -> public Slider ui_Slider;
```

### 使用方式

```csharp
public partial class UIExample : BaseUIView
{
    // 在 Component 文件中声明控件（partial class）
}

public partial class UIExample
{
    public Button ui_Submit;
    public Text ui_Title;
    public Image ui_Icon;
    public InputField ui_InputName;

    public override void Awake()
    {
        base.Awake();
        // AutoLinkUI 在 BaseUIInit.Awake() 中自动调用
        // 会自动将 Prefab 中名为 "ui_Submit" 的物体上的 Button 组件赋值给 this.ui_Submit
    }
}
```

### 自动绑定流程

```
Awake() 被调用
    │
    ▼
AutoLinkUI()
    │  使用 ReflexUtil.AutoLinkDataForChild(this, "ui_")
    │  遍历所有 FieldInfo
    │  检查字段名是否以 "ui_" 开头
    │
    ├── 字段名: "ui_Submit", 类型: Button
    │   → 在子物体中查找名为 "ui_Submit" 的 GameObject
    │   → 获取 Button 组件，赋值给 this.ui_Submit
    │
    ├── 字段名: "ui_Title", 类型: Text
    │   → 在子物体中查找名为 "ui_Title" 的 GameObject
    │   → 获取 Text 组件，赋值给 this.ui_Title
    │
    └── ... 依次处理所有 ui_ 前缀字段
```

---

## BaseComponent 和 BaseControl

### BaseComponent

```csharp
public class BaseComponent : BaseMonoBehaviour
{
    // 通用游戏组件基类
    // 提供组件的基本生命周期管理
}
```

### BaseControl

```csharp
public class BaseControl : BaseMonoBehaviour
{
    // 基础控制类
    // 处理输入、控制逻辑等
    // 子类: ControlForGameBase, ControlForGameFight
}
```

---

## 通用残影 (Afterimage / 虚影拖尾) 效果

**目录**: `Assets/FrameWork/Scripts/Component/Other/`

框架层通用「残影/虚影拖尾」能力（恶魔城式冲刺残影即用此）。基类封装公共流程，子类按**不同渲染类型**实现快照差异。首个接入方是控制系统的空格突进（`ControlForGameBase` 用 `AfterimageGhostMesh`）。

### 继承结构与分工

```
AfterimageGhostBase (abstract)              # 通用:对象池 + 生成节奏 + 淡出 + 清理
    ├── AfterimageGhostMesh                 # MeshFilter+MeshRenderer(几何已烘焙进CPU Mesh):Spine/静态/程序化
    ├── AfterimageGhostSkinnedMesh          # SkinnedMeshRenderer(3D骨骼):必须 BakeMesh 取当前姿态(非 sharedMesh 的 bind pose)
    └── AfterimageGhostSprite               # SpriteRenderer(2D精灵):复制 sprite + 降 color 淡出
```

**基类 `AfterimageGhostBase` 负责（子类不用重写）**：
- 对象池：`listActive`/`poolIdle`/`listAll`，残影淡出即回池复用**不销毁**（高频触发不反复 Instantiate/Destroy）。
- 生成节奏：`StartSpawn(count, duration)` 按 `spawnInterval=duration/count` 把 count 个残影均匀铺满；`StopSpawn()` 停新增；`ClearAll()` 统一销毁（挂起/切场景调）；`OnDestroy` 兜底。
- 淡出：每帧按 `age/ghostLifetime` 算系数 `t`(1→0) 调子类 `ApplyFade`。

**子类只实现 5 个抽象方法**：`CanCapture()` / `CreateGhost()` / `CaptureInto(item)` / `ApplyFade(item,t)` / `DestroyGhost(item)`；各自 `Init(源渲染器)` 绑定。

### 用法

```csharp
// 绑定(挂到目标物体上,懒创建亦可)
var ghost = go.AddComponent<AfterimageGhostMesh>();
ghost.Init(skeletonAnimation.gameObject);   // 或 Init(MeshRenderer, MeshFilter)
ghost.ghostTint = new Color(0.6f,0.75f,1f,0.55f);   // 冷色半透明(可选)
// 触发:数量与时长由业务决定(如随等级)
ghost.StartSpawn(count: 9, duration: 0.2f);
// 结束 / 挂起
ghost.StopSpawn();
ghost.ClearAll();
```

### 关键点与坑

- **网格快照必须深拷贝**（`AfterimageGhostMesh.CopyMesh`）：Spine/SkeletonRenderer 每帧双缓冲，直接引用 `sharedMesh` 下一帧被覆盖。
- **蒙皮必须 BakeMesh**：`SkinnedMeshRenderer.sharedMesh` 是 bind pose，`BakeMesh` 才是当前动作帧。
- **材质零克隆**：网格/蒙皮变体共享源 `sharedMaterials` + 用 `MaterialPropertyBlock` 覆盖 `_Color` 淡出（PMA 整体乘 `ghostTint*t`），不改本体、无材质泄漏；精灵变体直接改 `SpriteRenderer.color`。
- **翻转**：残影用源 `lossyScale`（含翻转的负 X）。
- **排序**：残影 `sortingOrder = 源-1` 压身后，关阴影。
- **适用边界**：此套只适合「当前显示几何已在 CPU 侧（Mesh 或 Sprite）」的对象。**顶点着色器/GPU 驱动形变（风摆、GPU 粒子）冻结不住**——ghost 用同材质会按当前时间重新形变；真要通用需改「渲染纹理捕获(CommandBuffer.DrawRenderer → RenderTexture → 贴图残影)」，属另一条更重的技术路线，本框架暂未实现，需要时再作为新变体扩展。

### 文件

| 文件 | 说明 |
|------|------|
| `Component/Other/AfterimageGhostBase.cs` | 残影基类(池化/节奏/淡出/清理) |
| `Component/Other/AfterimageGhostMesh.cs` | 网格快照残影(Spine/静态/程序化) |
| `Component/Other/AfterimageGhostSkinnedMesh.cs` | 蒙皮残影(3D骨骼 BakeMesh) |
| `Component/Other/AfterimageGhostSprite.cs` | 精灵残影(2D SpriteRenderer) |

---

## 常用代码模板

### 创建新的 Handler-Manager 配对

```csharp
// === Manager ===
// Assets/Scripts/Component/Manager/MyFeatureManager.cs
public class MyFeatureManager : BaseManager
{
    private List<MyData> dataList = new List<MyData>();

    public void LoadFeatureData(Action<bool> callback)
    {
        // 加载逻辑
        callback?.Invoke(true);
    }

    public MyData GetData(int index)
    {
        return index < dataList.Count ? dataList[index] : null;
    }
}

// === Handler ===
// Assets/Scripts/Component/Handler/MyFeatureHandler.cs
public class MyFeatureHandler : BaseHandler<MyFeatureHandler, MyFeatureManager>
{
    public void InitFeature(Action<bool> callback)
    {
        manager.LoadFeatureData(callback);
    }

    public MyData QueryData(int index)
    {
        return manager.GetData(index);
    }
}

// === 使用 ===
MyFeatureHandler.Instance.InitFeature((success) =>
{
    if (success)
    {
        var data = MyFeatureHandler.Instance.QueryData(0);
    }
});
```

### 创建新的 BaseUIView UI

```csharp
// === UI 类 ===
// Assets/Scripts/Component/UI/Game/MyModule/UIMyFeature.cs
public class UIMyFeature : BaseUIView
{
    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI(true);
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        // 刷新UI数据
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Close)
        {
            UIHandler.Instance.CloseUI<UIMyFeature>();
        }
    }
}

// === Component 文件 ===
// Assets/Scripts/Component/UI/Game/MyModule/UIMyFeatureComponent.cs
public partial class UIMyFeature
{
    public Button ui_Close;
    public Text ui_Title;
}
```

---

## 文件位置速查

| 功能 | 文件路径 |
|------|----------|
| BaseMonoBehaviour | `Assets/FrameWork/Scripts/Base/BaseMonoBehaviour.cs` |
| BaseSingleton | `Assets/FrameWork/Scripts/Base/BaseSingleton.cs` |
| BaseSingletonMonoBehaviour | `Assets/FrameWork/Scripts/Base/BaseSingletonMonoBehaviour.cs` |
| BaseMVCService | `Assets/FrameWork/Scripts/Base/BaseMVCService.cs` |
| IBaseMVCView | `Assets/FrameWork/Scripts/Base/IBaseMVCView.cs` |
| BaseManager | `Assets/FrameWork/Scripts/Component/Manager/BaseManager.cs` |
| BaseHandler | `Assets/FrameWork/Scripts/Component/Handler/BaseHandler.cs` |
| BaseComponent | `Assets/FrameWork/Scripts/Base/BaseComponent.cs` |
| BaseUIInit | `Assets/FrameWork/Scripts/Base/BaseUIInit.cs` |
| BaseUIView | `Assets/FrameWork/Scripts/Base/BaseUIView.cs` |
| BaseUIComponent | `Assets/FrameWork/Scripts/Base/BaseUIComponent.cs` |
| BaseEvent | `Assets/FrameWork/Scripts/Base/BaseEvent.cs` |
| BaseObservable | `Assets/FrameWork/Scripts/Base/BaseObservable.cs` |
| BaseControl | `Assets/FrameWork/Scripts/Component/Control/BaseControl.cs` |

---

## 注意事项

1. **框架层不得依赖游戏层**: FrameWork/Scripts/ 中的代码不能引用 Scripts/ 下的类。
2. **修改基类要评估影响**: 基类的修改会影响所有子类，务必检查所有继承者。
3. **泛型约束**: BaseHandler<T, M> 的泛型约束必须正确：T 是 BaseMonoBehaviour，M 是 BaseManager。
4. **单例生命周期**: MonoBehaviour 单例的 Awake 中处理了重复实例的销毁，子类重写 Awake 时必须调用 base.Awake()。
5. **AutoLinkUI 依赖命名**: UI 控件的 GameObject 名称必须与字段名严格一致（如字段 `ui_Submit`，GameObject 名必须是 `ui_Submit`）。
6. **partial class 模式**: UI 类通常使用 partial class 分离声明和逻辑，Component 文件存放控件声明。
