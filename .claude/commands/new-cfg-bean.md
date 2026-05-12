# new-cfg-bean

新增一个从 JSON/Excel 配置文件读取的静态配置 Bean（Cfg 模式）。
适用于只读的游戏配置数据，如对话文本、NPC信息、菜品配置等。

## 用法

```
/new-cfg-bean <Bean名称> <文件名> <字段列表>
```

**示例：**
- `/new-cfg-bean FestivalInfo FestivalInfo id:long name:long valid:int type:int start_day:int end_day:int`
- `/new-cfg-bean BrewRecipe BrewRecipe id:long valid:int name:long material_ids:string output_id:long brew_time:int`

## 执行步骤

根据项目已有的 `TextTalkBean.cs` / `TextStoryCfg` 模式生成两个文件：

### 文件1：`Assets/Scripts/Bean/MVC/Game/<Bean名称>Bean.cs`

```csharp
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class <Bean名称>Bean : BaseBean
{
    // 字段（snake_case，与 JSON/Excel 列名一致）
    public int valid;
    // ... 其他字段
    
    // 多语言字段示例（如有 long 类型的 name/content 字段则生成）：
    public long name;
    [JsonIgnore]
    public string name_language { get => _name_language.Get(() => TextHandler.Instance.GetTextById(<Bean名称>Cfg.fileName, name)); set => _name_language.Set(value); }
    private LanguageCache _name_language;
}
public partial class <Bean名称>Cfg : BaseCfg<long, <Bean名称>Bean>
{
    public static string fileName = "<文件名>";
    protected static Dictionary<long, <Bean名称>Bean> dicData = null;

    public static Dictionary<long, <Bean名称>Bean> GetAllData()
    {
        if (dicData == null) { var arr = GetAllArrayData(); InitData(arr); }
        return dicData;
    }
    public static <Bean名称>Bean[] GetAllArrayData()
    {
        if (arrayData == null) arrayData = GetInitData(fileName);
        return arrayData;
    }
    public static <Bean名称>Bean GetItemData(long key)
    {
        if (dicData == null) { var arr = GetInitData(fileName); InitData(arr); }
        return GetItemData(key, dicData);
    }
    public static void InitData(<Bean名称>Bean[] arrayData)
    {
        dicData = new Dictionary<long, <Bean名称>Bean>();
        foreach (var item in arrayData) dicData.Add(item.id, item);
    }
}
```

### 文件2：`Assets/Scripts/Bean/MVC/Game/<Bean名称>BeanPartial.cs`

```csharp
using System;
using System.Collections.Generic;
public partial class <Bean名称>Bean
{
    public <Bean名称>Bean() { }

    // 如字段有枚举映射则生成 Get 方法，例如：
    // public <TypeEnum> GetType() => (<TypeEnum>)type;
}
public partial class <Bean名称>Cfg
{
}
```

### 判断规则

- `long` 类型且字段名含 `name`/`content`/`title`/`desc` → 生成 `LanguageCache` 多语言属性对
- `int` 类型且字段名为 `type`/`*_type` → 在 Partial 中生成对应的 `Get*()` 枚举转换方法（枚举名由用户确认）
- `string` 类型且字段名含 `_ids`/`_data` → 在 Partial 中生成解析方法骨架，加 `// TODO: 实现解析逻辑` 注释

生成后提示：需在对应的 Manager 或 Handler 中调用 `<Bean名称>Cfg.GetItemData(id)` 初始化数据。
