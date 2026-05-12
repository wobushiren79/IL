# new-service

新增一个 SQLite 数据服务类（继承 `BaseMVCService`）。

## 用法

```
/new-service <服务名> <主表名> [副表名]
```

**示例：**
- `/new-service InnRoomService inn_room`（单表，无多语言）
- `/new-service NpcInfoService npc_info npc_info_details_<language>`（双表，含多语言副表）
- `/new-service TextInfoService`（动态表名，构造函数传空，运行时设置）

## 三种模式选择

### 模式A：单表，表名固定
适用：`GameDataService`、`ItemsInfoService` 等

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
}
```

### 模式B：双表 LEFT JOIN（主表 + 多语言副表）
适用：`NpcInfoService`、`MenuInfoService` 等

```csharp
public class <服务名> : BaseMVCService
{
    public <服务名>() : base(
        "<主表名>",
        "<副表名>_" + GameDataHandler.Instance.manager.GetGameConfig().language
    ) { }
    
    public List<<Bean>Bean> QueryAllData()
    {
        // 第二参数为副表 JOIN key（通常是 <主表名>_id 或 npc_id）
        return BaseQueryAllData<<Bean>Bean>("<join_key>");
    }
    
    public List<<Bean>Bean> QueryDataById(long id)
    {
        return BaseQueryData<<Bean>Bean>("<join_key>",
            tableNameForMain + ".id", id + "");
    }
    
    public void InsertData(<Bean>Bean data)
    {
        // leftNames：副表独有字段列表（如 name、title 等多语言字段）
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

### 模式C：动态表名（运行时切换）
适用：`TextInfoService`（根据语言/类型动态设置表名）

```csharp
public class <服务名> : BaseMVCService
{
    public <服务名>() : base("", "") { }
    
    private void InitTable(<参数>)
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        switch (<参数>)
        {
            case <值1>:
                tableNameForMain = "<表名1>";
                tableNameForLeft = "<表名1>_details_" + gameConfig.language;
                break;
            // ... 其他分支
        }
    }
    
    public List<<Bean>Bean> QueryDataBy<Key>(<类型> value)
    {
        InitTable(<参数>);
        return BaseQueryData<<Bean>Bean>("<join_key>",
            tableNameForMain + ".<column>", value + "");
    }
}
```

## 常用查询方法模板

根据用户描述的查询需求从以下选择：

```csharp
// 精确匹配单字段
BaseQueryData<T>("join_key", "table.col", "value")

// 多条件 AND
BaseQueryData<T>("join_key", "table.col1", "value1", "table.col2", "value2")

// 带操作符（IN / <= / >= 等）
BaseQueryData<T>("join_key", "table.col", "IN", "(1,2,3)")
BaseQueryData<T>("join_key", "table.col", "<=", "100")

// 查全部
BaseQueryAllData<T>()                    // 单表
BaseQueryAllData<T>("join_key")          // 双表 JOIN

// 删除
BaseDeleteDataById(id)                   // 按主键
BaseDeleteData("col", "value")           // 按字段
BaseDeleteDataWithLeft("id","fk","val")  // 同步删副表
```

生成后提示：在对应的 `<名称>Manager.cs` 的 `Awake()` 中实例化此 Service。
