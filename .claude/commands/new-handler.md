# new-handler

新增一个 Handler + Manager 组合（系统的单例访问入口 + 业务逻辑层）。

## 用法

```
/new-handler <系统名> [描述]
```

**示例：**
- `/new-handler Brewery 酿酒系统，管理配方加载和酿造进度`
- `/new-handler Festival 节日系统，管理当前节日状态和奖励`

## 执行步骤

参考 `TextInfoHandler.cs` + `TextInfoManager.cs` + `NpcInfoManager.cs` 模式：

### 文件1：`Assets/Scripts/Component/Handler/<系统名>/<系统名>Handler.cs`

```csharp
public class <系统名>Handler : BaseHandler<<系统名>Handler, <系统名>Manager>
{

}
```

> Handler 极简，只起到单例访问（`<系统名>Handler.Instance.manager`）和 Manager 注入的作用。
> 不在 Handler 中写业务逻辑。

### 文件2：`Assets/Scripts/Component/Manager/<系统名>/<系统名>Manager.cs`

根据描述生成对应骨架，参考以下规范：

```csharp
public class <系统名>Manager : BaseManager
{
    // 1. 数据缓存字段
    //    - 按 ID 查找 → Dictionary<long, XxxBean>
    //    - 列表展示 → List<XxxBean>
    public Dictionary<long, <Bean>Bean> map<Bean>Data = new Dictionary<long, <Bean>Bean>();

    // 2. Service 字段（在 Awake 中实例化）
    private <系统名>Service _service;

    private void Awake()
    {
        _service = new <系统名>Service();
    }

    // 3. 数据初始化（由场景初始化调用）
    public void InitData()
    {
        var list = _service.QueryAllData();
        map<Bean>Data.Clear();
        if (list == null) return;
        foreach (var item in list)
            map<Bean>Data[item.id] = item;
    }

    // 4. 业务查询方法（按需生成）
    public <Bean>Bean Get<Bean>ById(long id)
    {
        map<Bean>Data.TryGetValue(id, out var data);
        return data;
    }

    // 5. 业务操作方法骨架（增删改）
    public void Add<Bean>(<Bean>Bean data)
    {
        _service.InsertData(data);
        map<Bean>Data[data.id] = data;
        // TODO: 广播事件通知 UI 刷新
        // EventHandler.Instance.TriggerEvent(MsgEnum.MSG_<系统名大写>_UPDATE);
    }

    public void Remove<Bean>By Id(long id)
    {
        _service.DeleteDataById(id);
        map<Bean>Data.Remove(id);
    }
}
```

### 生成判断

- 若描述含"加载配置" → `InitData()` 走 `<Bean>Cfg.GetAllData()`（Cfg 模式）而非 Service
- 若描述含"存档/保存" → 添加 `SaveData()` 方法调用 `_service.UpdateData()`
- 若描述含"事件/通知" → 在操作方法末尾补充 `EventHandler` 广播代码骨架

生成后提示：
1. Handler GameObject 需挂载到场景中的 `Handlers` 节点
2. 在场景初始化脚本（`GameSceneInit` / `TownSceneInit` 等）中调用 `<系统名>Handler.Instance.manager.InitData()`
