# il-handler-gen

**客栈传说 · Handler + Manager 生成器 Agent**

为指定系统生成 Handler（单例访问入口）+ Manager（业务逻辑层）组合，支持配置加载、存档读写、事件广播三种工作模式。

---

## 用法

```
/il-handler-gen <系统名> [描述关键词...]
```

**描述关键词影响生成内容：**
- `配置 / cfg / 静态` → `InitData()` 走 `<Bean>Cfg.GetAllData()`
- `存档 / 保存 / save` → 额外生成 `SaveData()` 方法
- `事件 / 通知 / event` → 操作方法末尾补充 `EventHandler` 广播代码骨架
- `列表 / list` → 数据缓存用 `List<T>` 而非仅 `Dictionary`
- `定时 / timer` → Manager 中添加 `Update()` 骨架

**示例：**
```
/il-handler-gen Brewery 配置加载 存档保存 事件通知
/il-handler-gen Festival 配置加载
/il-handler-gen InnRoom 存档保存 列表 事件通知
/il-handler-gen GameTime 定时 事件通知
```

---

## 执行步骤

### Step 1 — 确定文件路径

```
Handler：Assets/Scripts/Component/Handler/<系统名>/<系统名>Handler.cs
Manager：Assets/Scripts/Component/Manager/<系统名>/<系统名>Manager.cs
```

> 若 `<系统名>` 以已有子目录名开头（如 `InnBuild`、`MiniGame`、`NPC`），路径中使用对应子目录：
> - `InnBuild*` → `Handler/Inn/<系统名>Handler.cs`
> - `NPC*` / `Character*` → `Handler/Character/<系统名>Handler.cs`
> - `GameTime` / `GameWeather` → `Handler/Game/<系统名>Handler.cs`

### Step 2 — 生成 Handler 文件

Handler 极简，只起单例访问与 Manager 注入作用，**不在 Handler 中写任何业务逻辑**：

```csharp
public class <系统名>Handler : BaseHandler<<系统名>Handler, <系统名>Manager>
{

}
```

### Step 3 — 生成 Manager 文件

根据描述关键词生成对应骨架：

```csharp
public class <系统名>Manager : BaseManager
{
    // ━━━━━━━━━━━━━━━━━━ 1. 数据缓存 ━━━━━━━━━━━━━━━━━━

    // 按 ID 查找：Dictionary（默认生成）
    public Dictionary<long, <Bean>Bean> map<Bean>Data = new Dictionary<long, <Bean>Bean>();

    // 按顺序访问：List（描述含"列表"时额外生成）
    // public List<<Bean>Bean> list<Bean>Data = new List<<Bean>Bean>();

    // ━━━━━━━━━━━━━━━━━━ 2. Service 字段 ━━━━━━━━━━━━━━━━━━

    private <系统名>Service _service;

    // ━━━━━━━━━━━━━━━━━━ 3. 生命周期 ━━━━━━━━━━━━━━━━━━

    private void Awake()
    {
        _service = new <系统名>Service();
    }

    // 定时模式额外生成（描述含"定时"）：
    // private void Update()
    // {
    //     // TODO: 定时逻辑
    // }

    // ━━━━━━━━━━━━━━━━━━ 4. 数据初始化 ━━━━━━━━━━━━━━━━━━

    // 模式 A：从 SQLite Service 加载（默认，无"配置"关键词）
    public void InitData()
    {
        var list = _service.QueryAllData();
        map<Bean>Data.Clear();
        if (list == null) return;
        foreach (var item in list)
            map<Bean>Data[item.id] = item;
    }

    // 模式 B：从 Cfg 静态类加载（描述含"配置"时替换）
    // public void InitData()
    // {
    //     var dic = <Bean>Cfg.GetAllData();
    //     map<Bean>Data.Clear();
    //     if (dic == null) return;
    //     foreach (var kv in dic)
    //         map<Bean>Data[kv.Key] = kv.Value;
    // }

    // ━━━━━━━━━━━━━━━━━━ 5. 查询方法 ━━━━━━━━━━━━━━━━━━

    public <Bean>Bean Get<Bean>ById(long id)
    {
        map<Bean>Data.TryGetValue(id, out var data);
        return data;
    }

    public List<<Bean>Bean> GetAll<Bean>List()
    {
        return new List<<Bean>Bean>(map<Bean>Data.Values);
    }

    // ━━━━━━━━━━━━━━━━━━ 6. 操作方法 ━━━━━━━━━━━━━━━━━━

    public void Add<Bean>(<Bean>Bean data)
    {
        _service.InsertData(data);
        map<Bean>Data[data.id] = data;

        // 事件广播骨架（描述含"事件/通知"时生成）：
        // EventHandler.Instance.TriggerEvent(MsgEnum.MSG_<系统名大写>_UPDATE);
    }

    public void Remove<Bean>ById(long id)
    {
        _service.DeleteDataById(id);
        map<Bean>Data.Remove(id);

        // EventHandler.Instance.TriggerEvent(MsgEnum.MSG_<系统名大写>_UPDATE);
    }

    public void Update<Bean>(<Bean>Bean data)
    {
        _service.UpdateData(data);
        map<Bean>Data[data.id] = data;

        // EventHandler.Instance.TriggerEvent(MsgEnum.MSG_<系统名大写>_UPDATE);
    }

    // 存档模式额外生成（描述含"存档/保存"）：
    // public void SaveData()
    // {
    //     foreach (var kv in map<Bean>Data)
    //         _service.UpdateData(kv.Value);
    // }
}
```

### Step 4 — 输出摘要

```
已生成：
  Assets/Scripts/Component/Handler/<系统名>/<系统名>Handler.cs
  Assets/Scripts/Component/Manager/<系统名>/<系统名>Manager.cs

检测到的关键词：<配置加载 / 存档保存 / 事件通知 / 列表 / 定时>

手动注册（必须完成）：
  1. 将 <系统名>Handler GameObject 挂载到场景中的 "Handlers" 节点
     （或对应场景的 Handler 挂载节点，如 GameSceneHandlers / TownHandlers）

  2. 在场景初始化脚本中调用 InitData()：
       <系统名>Handler.Instance.manager.InitData();
     通常在以下文件中：
       - 主游戏场景：GameSceneInit.cs
       - 城镇场景：TownSceneInit.cs  
       - 主菜单场景：MainSceneInit.cs

  3. 在 MsgEnum.cs 中添加事件枚举（若使用事件广播）：
       MSG_<系统名大写>_UPDATE   = <值>,
       MSG_<系统名大写>_COMPLETE = <值>,

  4. 如尚未创建 Service，执行：/il-service-gen <系统名>Service <主表名>
```
