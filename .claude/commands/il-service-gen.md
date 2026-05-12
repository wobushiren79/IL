# il-service-gen

**客栈传说 · Service 生成器 Agent**

为指定 Bean 生成 SQLite 数据服务类（继承 `BaseMVCService`），自动选择单表 / 双表 JOIN / 动态表名三种模式。

---

## 用法

```
/il-service-gen <服务名> <主表名> [副表名模板] [--dynamic]
```

**参数说明：**
- `<服务名>`：类名，通常为 `<系统名>Service`，如 `BreweryService`
- `<主表名>`：SQLite 主表名（snake_case）
- `[副表名模板]`：可选，含 `<language>` 占位符，如 `npc_info_details_<language>`
- `[--dynamic]`：可选标志，表示运行时动态切换表名（如 TextInfoService 模式）

**示例：**
```
/il-service-gen InnRoomService inn_room
/il-service-gen NpcInfoService npc_info npc_info_details_<language>
/il-service-gen TextInfoService --dynamic
/il-service-gen BreweryService brewery_recipe
```

---

## 模式选择逻辑

```
有 --dynamic 标志？
  → 模式 C（动态表名）
有副表名参数？
  → 模式 B（双表 LEFT JOIN，多语言）
否则
  → 模式 A（单表，固定表名）
```

---

## 执行步骤

### Step 1 — 确定 Bean 类名

从服务名推断 Bean 类名（去掉 `Service` 后缀），或在 `Assets/Scripts/Bean/` 下搜索匹配文件确认：
- `BreweryService` → `BreweryBean`
- `NpcInfoService` → `NpcInfoBean`
- `InnRoomService` → `InnRoomBean`

### Step 2 — 生成 Service 文件

**路径：** `Assets/Scripts/MVC/Service/<服务名>.cs`

#### 模式 A — 单表，固定表名

```csharp
public class <服务名> : BaseMVCService
{
    public <服务名>() : base("<主表名>") { }

    public List<<Bean>Bean> QueryAllData()
    {
        return BaseQueryAllData<<Bean>Bean>();
    }

    public List<<Bean>Bean> QueryDataById(long id)
    {
        return BaseQueryData<<Bean>Bean>("id", id + "");
    }

    public void InsertData(<Bean>Bean data)
    {
        BaseInsertData("<主表名>", data);
    }

    public void DeleteDataById(long id)
    {
        BaseDeleteDataById(id);
    }

    public void UpdateData(<Bean>Bean data)
    {
        DeleteDataById(data.id);
        InsertData(data);
    }
}
```

#### 模式 B — 双表 LEFT JOIN（主表 + 多语言副表）

```csharp
public class <服务名> : BaseMVCService
{
    public <服务名>() : base(
        "<主表名>",
        "<副表名前缀>_" + GameDataHandler.Instance.manager.GetGameConfig().language
    ) { }

    public List<<Bean>Bean> QueryAllData()
    {
        return BaseQueryAllData<<Bean>Bean>("<join_key>");
    }

    public List<<Bean>Bean> QueryDataById(long id)
    {
        return BaseQueryData<<Bean>Bean>(
            "<join_key>",
            tableNameForMain + ".id", id + ""
        );
    }

    public void InsertData(<Bean>Bean data)
    {
        // leftNames：副表独有字段（多语言字段 + 外键字段）
        List<string> leftNames = new List<string> { "name", "<join_key>" };
        BaseInsertDataWithLeft(data, leftNames);
    }

    public void DeleteData(<Bean>Bean data)
    {
        BaseDeleteDataWithLeft("id", "<join_key>", data.id + "");
    }

    public void UpdateData(<Bean>Bean data)
    {
        DeleteData(data);
        InsertData(data);
    }
}
```

> `<join_key>` 通常为 `<主表名>_id`，如 `npc_info_id`、`menu_info_id`。
> 如主表名本身就是外键前缀（如 `npc_info`），join_key 即为 `npc_info_id`。

#### 模式 C — 动态表名（运行时切换，适合多语言文本表）

```csharp
public class <服务名> : BaseMVCService
{
    public <服务名>() : base("", "") { }

    private void InitTable(/* 切换参数，如 TextTypeEnum type */)
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        switch (/* 参数 */)
        {
            case /* 值1 */:
                tableNameForMain = "<表名1>";
                tableNameForLeft = "<表名1>_details_" + gameConfig.language;
                break;
            case /* 值2 */:
                tableNameForMain = "<表名2>";
                tableNameForLeft = "<表名2>_details_" + gameConfig.language;
                break;
            default:
                tableNameForMain = "";
                tableNameForLeft = "";
                break;
        }
    }

    public List<<Bean>Bean> QueryDataBy<Key>(/* 参数类型 */ key, /* 切换参数 */ type)
    {
        InitTable(type);
        return BaseQueryData<<Bean>Bean>(
            "<join_key>",
            tableNameForMain + ".<column>", key + ""
        );
    }
}
```

### Step 3 — 常用查询方法参考

根据 Bean 字段名自动为 Step 2 生成的 Service 补充常用查询方法：

```csharp
// 按单字段精确查询
BaseQueryData<T>("join_key", "table.col", "value")

// 按多字段 AND 查询
BaseQueryData<T>("join_key", "table.col1", "val1", "table.col2", "val2")

// IN 集合查询
BaseQueryData<T>("join_key", "table.col", "IN", "(1,2,3)")

// 范围查询
BaseQueryData<T>("join_key", "table.col", ">=", "100")
BaseQueryData<T>("join_key", "table.col", "<=", "200")

// 查全表
BaseQueryAllData<T>()            // 单表
BaseQueryAllData<T>("join_key")  // 双表 JOIN
```

### Step 4 — 输出摘要

```
已生成：
  Assets/Scripts/MVC/Service/<服务名>.cs

模式：<A 单表 / B 双表JOIN / C 动态>
主表：<主表名>
副表：<副表名>（若有）

后续步骤：
  1. 在对应 Manager 的 Awake() 中实例化：
       private <服务名> _service;
       private void Awake() { _service = new <服务名>(); }
  2. 若为新 Bean，先执行：/il-bean-gen db <Bean名> <主表名> <字段列表>
  3. 若无 Handler/Manager，执行：/il-handler-gen <系统名>
```
