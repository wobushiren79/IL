---
name: utils-system
description: 工具类系统开发指南（共享框架层）。使用此SKILL当需要创建或修改静态工具类(Util)、扩展方法(Extension)、工具类(Tools)等，包括框架层工具(Assets/FrameWork/Scripts/Utils,Extension,Tools)的分层规范、纯函数原则、命名约定、避免热路径分配、多语言 textId 抽取、Sphere/Box/Ray 搜索工具等。
watched_files:
  - Assets/FrameWork/Scripts/Utils/
  - Assets/FrameWork/Scripts/Extension/
  - Assets/FrameWork/Scripts/Tools/
  - Assets/Scripts/Utils/
---

# 工具类系统开发指南

## 核心概念

项目工具代码按「Utils / Extension / Tools」三类区分用途，集中在框架层（与业务无关）：

```
Assets/FrameWork/Scripts/
├── Utils/         静态工具函数
├── Extension/     this 扩展方法
└── Tools/         构造/序列化工具
```

> IL 项目当前无独立游戏层工具目录（`Assets/Scripts/Utils/` 为空）。若出现依赖 Handler/Manager 的业务工具，再按需在 `Assets/Scripts/Utils/` 建立；扩展方法仍统一放框架层 `Extension/`。

### 分类原则

| 类别 | 关键字 | 用途 | 示例 |
|------|--------|------|------|
| **Util** | `static class XxxUtil` | 纯函数，按业务领域聚合 | `MathUtil`、`RayUtil`、`AnimUtil` |
| **Extension** | `static class XxxExtension` + `this T` | 给已有类型挂方法，调用更自然 | `StringExtension.IsNull()`、`GameObjectExtension.SetActiveEX()` |
| **Tool** | `static class XxxTools` | 构造性/工厂性的工具集合 | `CreateTools`、`DataTools` |

---

## 框架层工具类清单

### Utils（`Assets/FrameWork/Scripts/Utils/`）

| 文件 | 用途 |
|------|------|
| **数据处理** | |
| JsonUtil | JSON 序列化/反序列化 |
| ExcelUtil | Excel 配置表读取 |
| BeanUtil | Bean 对象通用处理 |
| TypeConversionUtil | 类型转换 |
| GeneralDataUtil | 通用数据工具 |
| **图形渲染** | |
| TextureUtil | 纹理处理 |
| MeshUtil | 网格处理 |
| UGUIUtil | UGUI 工具 |
| **数学/随机** | |
| MathUtil | 数学计算 |
| RandomUtil | 随机数 |
| FastNoise | 快速噪声 |
| SimplexNoiseUtil | Simplex 噪声 |
| **游戏通用** | |
| GameUtil | 游戏通用工具 |
| SceneUtil | 场景工具 |
| RayUtil | 射线/范围检测（`OverlapToSphere/Box`、`RayToCastAllNonAlloc`（共享缓冲零 GC）等） |
| VectorUtil | 向量计算 |
| CptUtil | Component 工具 |
| DGEaseUtil | DOTween Ease 工具 |
| AnimUtil | 通用 Animator 工具（`GetAnimClipLength` 按片段名关键字读取动画时长）。`partial class`，如需业务侧动画方法可在 `Assets/Scripts/Utils/AnimUtil.cs` 以同名 partial 扩展 |
| **系统工具** | |
| FileUtil | 文件读写 |
| LogUtil | 日志（替代 Debug.Log） |
| SystemUtil | 系统信息 |
| TimeUtil | 时间格式化 |
| UnitUtil | 单位转换 |
| **反射/类型** | |
| ReflexUtil | 反射工具 |
| ClassUtil | 类型工具 |
| CheckUtil | 空值/有效性检查 |
| **资源加载** | |
| LoadAddressablesUtil | Addressables 加载 |
| LoadAssetUtil | 通用资源加载入口 |
| LoadResourcesUtil | Resources 加载 |
| LoadAssetBundleUtil | AssetBundle 加载 |
| LoadWWWUtil | WWW 加载 |
| UnityNewtonsoftJsonSerializer | Unity 适配的 Newtonsoft 序列化 |

### Extension（`Assets/FrameWork/Scripts/Extension/`）

| 文件 | 典型方法 |
|------|---------|
| CheckExtension | `IsNull` / `IsNotNull` |
| ColorExtension | `ToHexString` / `ToRGBAString` |
| ComponentExtension | `AddComponentEX` |
| EnumExtension | `GetEnumName` |
| GameObjectExtension | `SetActiveEX` / `FindChild` / `ShowObj` |
| ListArrayDicExtension | `ForEach` / `AddRange` |
| MonoExtension | `StartCoroutineEx` |
| RandomExtension | `RandomRange` / `RandomItem` |
| StringExtension | `IsNullOrEmpty` / `ToLong` |
| TypeExtension | 类型反射扩展 |
| VectorExtension | `ToVector2` / `ToVector3` |
| IEnumeratorAwaitExension | 协程 await 适配 |

### Tools（`Assets/FrameWork/Scripts/Tools/`）

| 文件 | 用途 |
|------|------|
| CreateTools | 通用对象构造工具 |
| DataTools | 数据处理工具 |
| RandomTools | 随机工具 |
| Serialization | 序列化工具 |
| WorldRandTools | 世界随机（确定性 seed） |

---

## 游戏层工具类（`Assets/Scripts/Utils/`）

IL 项目当前**暂无游戏层工具类**（`Assets/Scripts/Utils/` 为空）。若后续出现依赖 Handler/Manager 的业务工具，按下面的分层规范新建：纯计算放框架层 `Utils/`，依赖业务单例的放 `Assets/Scripts/Utils/`（类名带 `Util` 后缀，方法 static，通过参数注入依赖而非内部单例）。

---

## 工具类编写规范

### 1. 类与方法签名

```csharp
public static class XxxUtil
{
    /// <summary>
    /// 方法用途说明
    /// </summary>
    /// <param name="xx">参数说明</param>
    /// <returns>返回值说明</returns>
    public static ReturnType MethodName(InputType input)
    {
        // ...
    }
}
```

- **类必须是 `static`**：禁止实例化
- **方法必须是 `static`**：纯函数风格
- **必须有 XML 注释**：遵循 CLAUDE.md「代码注释与分类规则」

### 2. 区域分组

按功能分 `#region`，不按可见性分：

```csharp
public static class MyFeatureUtil
{
    #region 颜色工具
    public static Color ParseHtmlColor(string colorStr) { /* ... */ }
    #endregion

    #region UI 辅助
    public static void SetUIGradient(Graphic graphic, string colorStr) { /* ... */ }
    #endregion
}
```

### 3. 扩展方法归属

```csharp
// ❌ 错误：扩展方法写在 Utils 下
public static class MyEnumUtil
{
    public static string GetName(this MyEnum value) { /* ... */ }
}

// ✅ 正确：扩展方法放 Extension 目录，类名带 Extension 后缀
// Assets/FrameWork/Scripts/Extension/MyEnumExtension.cs
public static class MyEnumExtension
{
    public static string GetName(this MyEnum value) { /* ... */ }
}
```

### 4. 多语言 textId 不要硬编码

```csharp
// ❌ 错误：散落的魔法数
return TextHandler.Instance.GetTextById(1001);

// ✅ 正确：抽取为 const
private const int TextId_Skin_Head = 1001;
return TextHandler.Instance.GetTextById(TextId_Skin_Head);
```

### 5. 热路径性能约定（搜索/碰撞工具）

```csharp
// 循环外缓存引用
var handler = MyHandler.Instance;
for (int i = 0; i < hits.Length; i++)
{
    // 用缓存，避免循环内重复取单例 / GetComponent / Find
    var target = handler.manager.GetDataById(...);
}

// 0 结果返回 null，不分配空 list
if (targetEntity == null) return null;
// 命中 1 个时按容量 1 分配，避免 List 默认 4 容量
return new List<T>(1) { targetEntity };
```

### 6. 禁止持有 UI 引用 / 可变静态状态

```csharp
// ❌ 错误：静态字段被多处修改
public static class GameUIUtil
{
    private static Graphic currentTarget;  // 不要这样做
}

// ✅ 正确：所有依赖通过参数传入
public static void SetGradientColor(Graphic graphic, string colorStr) { /* ... */ }
```

---

## 工具类 vs Handler/Manager 抉择

| 场景 | 选 Util | 选 Handler/Manager |
|------|---------|-------------------|
| 输入 → 输出的纯计算 | ✅ | ❌ |
| 不依赖 Unity 生命周期 | ✅ | ❌ |
| 需要持有缓存/状态 | ❌ | ✅ |
| 需要在 Inspector 暴露 | ❌ | ✅ |
| 跨场景常驻 | ❌ | ✅（单例） |
| 调用频率高、参数稳定 | ✅ | 看情况 |

---

## 常见反模式

### 1. Util 里调用 Handler 单例 → 隐藏依赖

```csharp
// ❌ Util 内部隐式依赖 Handler 单例，难以单测
public static class XxxUtil
{
    public static void Do() { MyHandler.Instance.xxx; }
}

// ✅ 把依赖作为参数传入
public static class XxxUtil
{
    public static void Do(MyManager manager) { manager.xxx; }
}
```

### 2. switch 漏 case → 静默返回 null

```csharp
// ❌ 枚举新增 case 后忘记同步外层 switch
switch (searchType)
{
    case SearchTypeEnum.AreaSphere: /* ... */ break;
    // 漏掉其他 case
}
return null;  // 调用方拿到 null 不知道是空结果还是漏分支

// ✅ default 抛异常或写 LogUtil 标记未覆盖
switch (searchType)
{
    case SearchTypeEnum.AreaSphere: /* ... */ break;
    default:
        LogUtil.Log($"[XxxUtil] 未处理的 searchType: {searchType}");
        return null;
}
```

### 3. 复制粘贴时类型交叉

搜索/碰撞工具若用枚举区分几何形状（Sphere/Box），新增 case 时要按枚举字面语义分组，避免枚举名与实际形状不一致（如某 `AreaBox*` 分支误走 `OverlapToSphere`）。可用枚举位运算掩码或独立的形状提取方法避免。

---

## 关联模块

- 框架核心基类：[framework-core-system](../framework-core-system/SKILL.md)
- 资源加载工具（LoadXxxUtil）：[resource-loading-system](../resource-loading-system/SKILL.md)
- 配置表工具（ExcelUtil）：[excel-io](../excel-io/SKILL.md)
- 数据服务（JsonUtil 等）：[data-service-system](../data-service-system/SKILL.md)
