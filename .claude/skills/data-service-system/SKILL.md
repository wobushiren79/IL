---
name: data-service-system
description: 数据服务系统开发指南（共享框架层 + 项目适配）。使用此SKILL当需要创建或修改数据持久化、JSON读写、SQLite查询、配置数据管理、存档服务等，包括框架层BaseDataService<T>泛型基类、BaseMVCService(SQLite)基类、项目层BaseDataStorageImpl<T>存档基类、GameDataService、数据存储(DataStorage)等。
watched_files:
  - Assets/FrameWork/Scripts/DataStorage/
  - Assets/FrameWork/Scripts/Base/BaseMVCService.cs
  - Assets/FrameWork/Scripts/BaseSystem/Sqlite/
  - Assets/Scripts/DataStorage/
  - Assets/Scripts/MVC/Service/
  - Assets/Scripts/Bean/MVC/
  - Assets/FrameWork/Scripts/Utils/JsonUtil.cs
---

# 数据服务系统开发指南

## 核心概念

IL 项目（客栈传说）数据层分**两层**，服务基类都在框架层，业务服务在项目层：

```
框架层（Assets/FrameWork/Scripts/）
├── DataStorage/BaseDataService.cs     # 泛型 JSON 文件服务基类（Load/Save/Delete）
├── DataStorage/BaseDataRead.cs        # 数据读取基类
├── DataStorage/BaseDataStorage.cs     # 数据存储基类
└── Base/BaseMVCService.cs             # SQLite 服务基类（链表查询/删除/插入）

项目层（Assets/Scripts/）
├── DataStorage/BaseDataStorageImpl.cs # 存档 JSON 基类（BaseSave/Load/DeleteFolder）
├── MVC/Service/                       # 30+ 业务数据服务（SQLite 为主）
│   ├── BaseDataService.cs             # 基础数据读取（读 BaseInfo 配置表）
│   ├── GameDataService.cs             # 游戏主存档（继承 BaseDataStorageImpl<GameDataBean>）
│   ├── GameListDataService.cs         # 游戏列表存档
│   ├── UserRevenueService.cs          # 玩家营收
│   └── ...（ItemsInfoService/NpcInfoService/MenuInfoService 等）
└── Bean/MVC/                          # 与 SQLite 表对应的 Bean（snake_case 字段名）
```

### 持久化方式

| 方式 | 适用场景 | 关键类 |
|------|---------|--------|
| JSON (Newtonsoft.Json) | 复杂存档、配置（`BaseDataService<T>` / `BaseDataStorageImpl<T>`） | `JsonUtil` / `UnityNewtonsoftJsonSerializer` |
| SQLite | 大量结构化数据（`BaseMVCService`） | `SQLiteHandle` / `SQliteHandle` |
| PlayerPrefs | 简单键值对 | — |
| Excel (EPPlus) | 配置表导入导出 | `ExcelUtil` |

---

## 框架层 BaseDataService\<T\> 泛型基类

**文件**: `Assets/FrameWork/Scripts/DataStorage/BaseDataService.cs`

封装 JSON 文件的 Load / Save / Delete，替代传统 MVC 的 Model + Controller + IView 层，让 Manager/Service 直接操作数据。

### 核心 API

```csharp
public class BaseDataService<T> where T : class, new()
{
    public string FileName { get; protected set; }              // JSON 文件名（构造传入）
    public JsonTypeEnum JsonType { get; set; } = JsonTypeEnum.Net;  // 序列化类型
    public string StoragePath { get; set; } = Application.persistentDataPath; // 目录路径

    public BaseDataService(string fileName);   // 构造函数传入文件名（不含路径）

    public virtual T Load(bool isShowLog = true);  // 从 {StoragePath}/{FileName} 读取
    public virtual void Save(T data);              // 写回 JSON 文件
    public virtual void Delete();                  // 删除数据文件
}
```

> ⚠️ 注意与 DLR 旧版 `InitData(fileName, dir)` + `GetData/SetData` 接口不同：本版是**构造函数传 fileName** + `Load/Save/Delete`，`StoragePath`/`FileName` 均为 public 可直接覆写。

### 创建新的数据服务

```csharp
// 1. 定义数据 Bean（字段 snake_case，与存储/表列一致）
[Serializable]
public class MyFeatureDataBean
{
    public int version = 1;
    public Dictionary<string, int> settings = new Dictionary<string, int>();
    public List<string> records = new List<string>();
}

// 2. 直接复用泛型基类（无需为每个类型建子类）
var service = new BaseDataService<MyFeatureDataBean>("MyFeatureData")
{
    StoragePath = Application.persistentDataPath,
};

// 3. 读写
var data = service.Load();
data.records.Add("record-1");
service.Save(data);
```

---

## 项目层 BaseDataStorageImpl\<T\> 存档基类

**文件**: `Assets/Scripts/DataStorage/BaseDataStorageImpl.cs`

项目级抽象存档基类，封装 `JsonUtil` + `FileUtil` 的文件读写，路径默认 `Application.persistentDataPath`。业务存档服务继承它：

```csharp
public abstract class BaseDataStorageImpl<T>
{
    protected string dataStoragePath;            // 默认 Application.persistentDataPath
    public string GetDataStoragePath();
    public void SetDataStoragePath(string path);

    public void BaseSaveData(string fileName, T dataBean);          // 存单个
    public T BaseLoadData(string fileName);                          // 读单个
    public void BaseSaveDataForList(string fileName, List<T> list);  // 存列表
    public List<T> BaseLoadDataForList(string fileName);             // 读列表
    public void BaseDeleteFile(string fileName);                     // 删文件
    public void BaseDeleteFolder(string folderName);                 // 删文件夹
}
```

### 示例：GameDataService

```csharp
public class GameDataService : BaseDataStorageImpl<GameDataBean>
{
    public GameDataBean QueryDataByUserId(string userId)
        => BaseLoadData(userId + "/Base");          // 按 userId 分目录

    public void UpdateDataByUserId(string userId, GameDataBean gameData)
    {
        FileUtil.CreateDirectory(dataStoragePath + "/" + userId);
        BaseSaveData(userId + "/Base", gameData);
    }

    public void DeleteDataByUserId(string userId)
        => BaseDeleteFolder(userId);
}
```

---

## SQLite 服务基类 BaseMVCService

**文件**: `Assets/FrameWork/Scripts/Base/BaseMVCService.cs`

IL 绝大多数业务数据（角色/物品/NPC/菜单/技能/成就等 30+ 服务）走 SQLite，基类是 `BaseMVCService`，通过 `SQLiteHandle` 读写 `ProjectConfigInfo.DATA_BASE_INFO_NAME` 指定的数据库。

### 核心 API

```csharp
public class BaseMVCService
{
    public BaseMVCService(string tableName);                          // 单表
    public BaseMVCService(string tableName, string leftDetailsTableName); // 链表

    public List<T> BaseQueryAllData<T>();                              // 查全表
    public List<T> BaseQueryAllData<T>(string leftId);                 // 链表查全
    public List<T> BaseQueryData<T>(string key, string value);         // 单条件查询
    public List<T> BaseQueryData<T>(string leftId, string key, string value); // 链表单条件
    public bool BaseInsertData<T>(string tableName, T itemData);       // 插入
    public bool BaseInsertDataWithLeft<T>(T itemData, List<string> listLeftName); // 链表插入
    public bool BaseDeleteDataById(long id);                           // 按 id 删除
    public bool BaseDeleteDataWithLeft(string mainName, string leftName, string value); // 链表删除
    // ... 多条件重载
}
```

### 业务服务示例

```csharp
// Assets/Scripts/MVC/Service/ItemsInfoService.cs
public class ItemsInfoService : BaseMVCService
{
    public ItemsInfoService() : base("items_info") { }

    public List<ItemBean> QueryAllItems() => BaseQueryAllData<ItemBean>();

    public List<ItemBean> QueryItemById(long id) => BaseQueryData<ItemBean>("id", id.ToString());
}
```

> 业务 Bean 对应数据库表字段，字段名使用 `snake_case`（与 SQLite 列名一致）。完整业务服务/Bean 清单见 `.claude/md/project.md`。

---

## JSON 工具类 (JsonUtil)

**文件**: `Assets/FrameWork/Scripts/Utils/JsonUtil.cs`

```csharp
// 序列化
string json = JsonUtil.ToJson(myObject);
string jsonNet = JsonUtil.ToJson(myObject, JsonTypeEnum.Net);

// 反序列化
MyClass obj = JsonUtil.FromJson<MyClass>(jsonString);

// 使用 Unity 的 Newtonsoft.Json 序列化器
UnityNewtonsoftJsonSerializer.Serialize(obj);
UnityNewtonsoftJsonSerializer.Deserialize<T>(json);
```

---

## SQLite 操作（框架层）

**目录**: `Assets/FrameWork/Scripts/BaseSystem/Sqlite/`

### SQliteHandle 核心方法

```csharp
// 初始化数据库
SQliteHandle handle = new SQliteHandle(dbPath);

// 执行查询
List<T> results = handle.Query<T>("SELECT * FROM TableName WHERE id = ?", id);

// 执行非查询
int affected = handle.Execute("INSERT INTO TableName VALUES (?, ?)", value1, value2);

// 批量操作
handle.BeginTransaction();
// ... 多次 Execute ...
handle.Commit();
```

> 业务层通常不直接 new SQliteHandle，而是通过 `BaseMVCService` 封装好的 `BaseQuery*/BaseInsert*/BaseDelete*` 方法读写。

### 使用 SQLiteHelper

```csharp
SQLiteHelper.CreateTable<T>(dbPath);
SQLiteHelper.Insert(dbPath, myObject);
SQLiteHelper.InsertAll(dbPath, listObjects);
SQLiteHelper.Update(dbPath, myObject);
List<T> items = SQLiteHelper.Query<T>(dbPath, "WHERE column = ?", value);
```

---

## Excel 配置处理 (ExcelUtil)

**文件**: `Assets/FrameWork/Scripts/Utils/ExcelUtil.cs`

```csharp
// 从 Excel 读取配置数据（Editor 环境）
var items = ExcelUtil.GetExcelDataList<ItemsInfoBean>(
    "Assets/Data/Excel/excel_items_info[物品信息].xlsx",
    "ItemsInfo"
);

// 导出 Excel 数据为 JSON（使用编辑器窗口）
// 菜单: Custom/工具弹窗/Excel编辑器
```

### 配置表结构

```
Assets/Data/Excel/                # 原始 Excel 配置表（excel_*.xlsx，文件名含中文方括号）
Assets/Resources/JsonText/        # 导出的 JSON 文本（由编辑器工具生成）
```

---

## 常用代码模板

### 新增持久化数据模块（JSON）

```csharp
[Serializable]
public class MySaveData
{
    public int version = 1;
    public long last_save_time;
    public List<MyRecord> records = new List<MyRecord>();
}

[Serializable]
public class MyRecord
{
    public string id;
    public long timestamp;
    public string data;
}

// 直接复用泛型基类，不另建子类
var service = new BaseDataService<MySaveData>("MySaveData");
service.Save(myData);
myData = service.Load();
```

### 新增 SQLite 业务服务

```csharp
// 1. 定义 Bean（Assets/Scripts/Bean/MVC/xxx/MyBean.cs，字段 snake_case）
[Serializable]
public class MyBean
{
    public long id;
    public string name;
    public long value;
}

// 2. 创建 Service（Assets/Scripts/MVC/Service/MyService.cs）
public class MyService : BaseMVCService
{
    public MyService() : base("my_table") { }

    public List<MyBean> QueryAll() => BaseQueryAllData<MyBean>();
    public List<MyBean> QueryById(long id) => BaseQueryData<MyBean>("id", id.ToString());
}
```

### 使用 PlayerPrefs（简单设置）

```csharp
PlayerPrefs.SetInt("TutorialComplete", 1);
PlayerPrefs.SetFloat("MusicVolume", 0.8f);
PlayerPrefs.SetString("LastLoginTime", DateTime.Now.ToString());

int tutorialComplete = PlayerPrefs.GetInt("TutorialComplete", 0);
float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
PlayerPrefs.Save();
```

---

## 文件位置速查

| 功能 | 文件路径 |
|------|----------|
| 泛型 JSON 数据服务基类 | `Assets/FrameWork/Scripts/DataStorage/BaseDataService.cs` |
| 数据读取基类 | `Assets/FrameWork/Scripts/DataStorage/BaseDataRead.cs` |
| 数据存储基类 | `Assets/FrameWork/Scripts/DataStorage/BaseDataStorage.cs` |
| SQLite 服务基类 | `Assets/FrameWork/Scripts/Base/BaseMVCService.cs` |
| 项目存档 JSON 基类 | `Assets/Scripts/DataStorage/BaseDataStorageImpl.cs` |
| 游戏主存档服务 | `Assets/Scripts/MVC/Service/GameDataService.cs` |
| 游戏列表存档服务 | `Assets/Scripts/MVC/Service/GameListDataService.cs` |
| 基础数据读取服务 | `Assets/Scripts/MVC/Service/BaseDataService.cs` |
| 业务 Bean（SQLite 表） | `Assets/Scripts/Bean/MVC/` |
| JSON 工具 | `Assets/FrameWork/Scripts/Utils/JsonUtil.cs` |
| Excel 工具 | `Assets/FrameWork/Scripts/Utils/ExcelUtil.cs` |
| SQLite 操作 | `Assets/FrameWork/Scripts/BaseSystem/Sqlite/` |

---

## 注意事项

1. **分层职责**：框架层 `BaseDataService<T>`（JSON 泛型）与 `BaseMVCService`（SQLite）是基类，项目层服务继承其一；勿在框架层引入项目 Bean。
2. **JSON vs SQLite 选型**：少量/复杂嵌套存档用 JSON（`BaseDataService<T>` / `BaseDataStorageImpl<T>`）；大量结构化、需按列查询/链表关联的数据用 SQLite（`BaseMVCService`）。
3. **文件路径**：编辑器环境用 `Application.dataPath` 同级目录，打包后统一 `Application.persistentDataPath`。
4. **序列化**：使用 Newtonsoft.Json（`JsonUtil` / `UnityNewtonsoftJsonSerializer`）而非 Unity 内置 JsonUtility。
5. **数据版本**：Bean 中的 `version` 字段可用于数据迁移，升级时处理旧格式兼容。
6. **线程安全**：非主线程读写时注意线程安全（存档服务多为主线程调用）。
