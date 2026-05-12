# FrameWork — 技术框架文档

可复用的 Unity 游戏框架，独立于具体项目，设计为可在多个游戏项目中共享使用。

代码位置：`Assets/FrameWork/`
项目业务文档：参见 `.claude/md/project.md`

## 目录结构

```
Assets/FrameWork/
├── Scripts/
│   ├── Base/           # 核心基类（继承层级根）
│   ├── BaseSystem/     # 基础系统（Event、SQLite、Steam）
│   ├── Component/      # 通用组件（Handler、Manager、UI、Control、Effect）
│   ├── Extension/      # 扩展方法
│   ├── Utils/          # 工具类库
│   ├── Bean/           # 框架级 DTO
│   ├── CallBack/       # 回调接口
│   ├── DataStorage/    # 数据持久化
│   ├── AI/             # AI 工具
│   ├── Web/            # 网络工具
│   ├── MVC/            # MVC 基础层
│   ├── Tools/          # 杂项工具
│   └── Enums/          # 框架枚举
└── Assets/
    ├── Addons/         # 第三方插件（DOTween、Spine、SerializableDictionary）
    ├── Editor/         # 编辑器工具
    ├── Plugins/        # 原生库（Steamworks.NET、EPPlus）
    ├── Prefabs/        # 框架预制体
    ├── Resources/      # 框架资源
    ├── Shader/         # 自定义 Shader
    └── Sound/          # 框架音效
```

## 核心基类体系

### MonoBehaviour 继承链
```
MonoBehaviour
└── BaseMonoBehaviour          # 基础 Mono，提供扩展 & 生命周期钩子
    ├── BaseManager            # 资源加载管理器基类
    ├── BaseHandler            # 事件处理器基类
    ├── BaseComponent          # 通用组件基类
    ├── BaseUIInit             # UI 生命周期（Awake/Open/Close/Refresh）
    │   └── BaseUIView         # UI 视图基类
    │       └── BaseUIComponent# UI 子组件基类
    └── BaseUIManager          # UI 管理器基类
```

### 非 Mono 基类
```
BaseSingleton<T>               # 纯 C# 单例
BaseSingletonMonoBehaviour<T>  # MonoBehaviour 单例
BaseMVCService                 # SQLite 数据服务基类
BaseEvent                      # 局部事件管理（自动清理）
BaseObservable                 # 观察者模式基类
```

## 关键系统

### 资源加载（`BaseManager` / `LoadAssetUtil`）
- 统一接口，后端可切换：**Addressables**（主）/ AssetBundle / Resources
- 编辑器模式下支持直接路径加载（`LoadAssetAtPathForEditor`）
- 内置缓存：`Dictionary<string/long, T>` 参数传入即可自动缓存
- 支持同步 / 异步 / 回调三种加载方式
- SpriteAtlas 懒加载 + 缓存

```csharp
// 同步加载并缓存
T asset = GetModelForAddressablesSync(dicCache, keyName);

// 异步 + 回调
GetModelForAddressables(dicCache, id, keyName, result => { ... });

// SpriteAtlas sprite
GetSpriteByName(dicIcon, ref spriteAtlas, atlasName, path, spriteName);
```

### MVC 数据服务（`BaseMVCService`）
SQLite CRUD 封装，支持主表 + 副表（LEFT JOIN）模式。

```csharp
// 声明服务
public class MyService : BaseMVCService {
    public MyService() : base("main_table", "detail_table") {}
}

// 查询
List<T> all   = service.BaseQueryAllData<T>();
List<T> byKey = service.BaseQueryData<T>("column", "value");
List<T> join  = service.BaseQueryAllData<T>("foreign_key_value");

// 增删（通过反射自动映射字段）
service.BaseInsertData("table_name", beanObject);
service.BaseDeleteDataById(id);
```

数据库名称由 `ProjectConfigInfo.DATA_BASE_INFO_NAME` 统一配置。

### 事件系统
- **全局事件**：`EventHandler`（单例）—— 跨系统松耦合通信
- **局部事件**：`BaseEvent`（实例）—— 随对象销毁自动清理监听

### UI 系统（`BaseUIInit` 体系）
- 生命周期：`Awake` → `Open` → `Refresh` → `Close`
- 按钮自动绑定：基于命名约定（Tag），无需手动拖拽
- UI 打开动画：DOTween 驱动
- `ScrollGrid`：可复用的虚拟列表组件

## 扩展方法（`Extension/`）
| 文件 | 功能 |
|---|---|
| `StringExtension` | IsNull、字符串处理 |
| `ComponentExtension` | GetOrAdd 组件 |
| `ListArrayDicExtension` | IsNull、安全操作 |
| `VectorExtension` | 向量计算 |
| `TypeExtension` | 类型转换 |
| `MonoExtension` | 协程工具 |
| `ColorExtension` | 颜色工具 |
| `CheckExtension` | 数值校验 |
| `IEnumeratorAwaitExension` | 协程转 async/await |

## 工具类库（`Utils/`，32 个类）
| 工具类 | 用途 |
|---|---|
| `LoadAssetUtil` | AssetBundle / Editor 路径加载 |
| `LoadAddressablesUtil` | Addressables 加载（同步/异步/回调） |
| `LoadResourcesUtil` | Resources.Load 封装 |
| `SQLiteHandle` | SQLite 增删改查底层 |
| `JsonUtil` | JSON 序列化 |
| `ExcelUtil` | Excel 数据解析（EPPlus） |
| `FileUtil` | 文件 I/O |
| `LogUtil` | 统一日志（封装 Debug.Log） |
| `GameUtil` | 游戏通用工具 |
| `MathUtil` | 数学计算 |
| `RandomUtil` | 随机数 |
| `BeanUtil` | IconBean / DTO 工具 |
| `ReflexUtil` | 反射（字段名→值映射，供 Service 使用） |

## 第三方依赖
| 库 | 版本/位置 | 用途 |
|---|---|---|
| DOTween | `Addons/DOTween` | UI & 对象动画 |
| Spine | `Addons/Spine` | 2D 骨骼动画 |
| SerializableDictionary | `Addons/Rotary Heart` | Inspector 可见字典 |
| Steamworks.NET | `Plugins/Steamworks.NET` | Steam API |
| EPPlus | `Plugins/EPPlus` | Excel 读写 |

## 开发约定
- 资源加载优先走 `BaseManager` 的泛型方法，不直接调用 `LoadAddressablesUtil`
- 新增数据服务继承 `BaseMVCService`，通过构造函数传表名
- 新增 UI 继承 `BaseUIInit` / `BaseUIView`，实现对应生命周期方法
- 事件命名使用 `MsgEnum`（全局）或局部字符串常量
- 编辑器专用代码包裹在 `#if UNITY_EDITOR` 中
