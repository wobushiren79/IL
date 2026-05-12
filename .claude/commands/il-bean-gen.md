# il-bean-gen

**客栈传说 · Bean 生成器 Agent**

根据描述自动判断 Bean 类型（Cfg 只读 / DB 存档），生成规范的 Bean 主文件与 Partial 文件。

---

## 用法

```
/il-bean-gen <模式> <Bean名称> <主表名或文件名> <字段列表>
```

**模式：**
- `cfg` — 静态配置数据，从 JSON/Excel 读取，只读（走 `BaseCfg` + `Cfg` 静态类）
- `db`  — 运行时存档数据，从 SQLite 读写（走 `BaseBean`，无 Cfg 类）

**字段列表格式：** `字段名:类型`，空格分隔

**示例：**
```
/il-bean-gen cfg FestivalInfo FestivalInfo id:long valid:int name:long type:int start_day:int end_day:int reward_ids:string
/il-bean-gen cfg BrewRecipe BrewRecipe id:long valid:int name:long material_ids:string output_id:long brew_time:int
/il-bean-gen db InnRoom inn_room id:long name:string capacity:int level:int unlock_price:int
/il-bean-gen db CharacterSkill character_skill id:long character_id:long skill_id:long level:int exp:int
```

---

## 执行步骤

### Step 1 — 解析输入

从参数中提取：
- 模式（cfg / db）
- Bean 名称（PascalCase，不含 Bean 后缀）
- 主表名 / 文件名（snake_case）
- 字段列表（字段名 + 类型）

**自动推断系统目录：**
从 Bean 名称前缀推断所属系统子目录，规则如下：
| 前缀 | 目录 |
|---|---|
| Character / Char | Character |
| Npc | Character（共存） |
| Inn / InnBuild | Inn |
| Menu | Menu |
| Arena | Arena |
| Gamble | Gamble |
| MiniGame | MiniGame |
| Order | Order |
| User | User |
| Text / Story / Talk | Game |
| 其他 / 无法识别 | Game |

### Step 2 — 字段分析

逐字段分析，标记特殊处理：

| 判断条件 | 处理 |
|---|---|
| `long` 类型 + 字段名含 `name`/`content`/`title`/`desc` | 主文件生成 `LanguageCache` 多语言属性对 |
| `string` 类型 + 字段名含 `_ids`/`_types`/`_data` | Partial 生成 `GetXxxList()` 解析方法骨架 + `// TODO` |
| `int` 类型 + 字段名含 `type`/`status` | Partial 生成枚举转换方法骨架 |
| 字段名含 `_id`（外键，非主键） | Partial 注释说明关联系统 |

### Step 3 — 生成主文件

**路径：** `Assets/Scripts/Bean/MVC/<系统目录>/<Bean名称>Bean.cs`

#### 模式 `cfg` — 静态配置 Bean

```csharp
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public partial class <Bean名称>Bean : BaseBean
{
    public int valid;
    // ... 其他字段（按输入顺序排列）

    // 若有多语言 long 字段：
    public long name;
    [JsonIgnore]
    public string name_language
    {
        get => _name_language.Get(() => TextHandler.Instance.GetTextById(<Bean名称>Cfg.fileName, name));
        set => _name_language.Set(value);
    }
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
        foreach (var item in arrayData)
            dicData.Add(item.id, item);
    }
}
```

#### 模式 `db` — 存档 Bean

```csharp
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public partial class <Bean名称>Bean : BaseBean
{
    public int valid;
    // ... 其他字段（按输入顺序排列）

    // 若有多语言 long 字段（db 模式较少见，视字段情况生成）：
    // public long name;
    // [JsonIgnore]
    // public string name_language { ... }
    // private LanguageCache _name_language;
}
```

> DB 模式无 Cfg 静态类，数据由 Service 从 SQLite 读取。

### Step 4 — 生成 Partial 文件

**路径：** `Assets/Scripts/Bean/MVC/<系统目录>/<Bean名称>BeanPartial.cs`

```csharp
using System;
using System.Collections.Generic;

public partial class <Bean名称>Bean
{
    public <Bean名称>Bean() { }

    // string _ids / _data 字段的解析方法骨架（自动生成对应字段）：
    // public List<long> Get<FieldName>List()
    // {
    //     // TODO: 解析 <field_name> 字符串，参考 GameUtil.GetListByStr()
    //     return new List<long>();
    // }

    // int type / status 字段的枚举转换骨架：
    // public <TypeEnum> Get<FieldName>Type() => (<TypeEnum>)<fieldName>;
}

// cfg 模式时同步生成：
public partial class <Bean名称>Cfg
{
}
```

### Step 5 — 输出摘要

```
已生成：
  Assets/Scripts/Bean/MVC/<系统目录>/<Bean名称>Bean.cs
  Assets/Scripts/Bean/MVC/<系统目录>/<Bean名称>BeanPartial.cs

字段清单：
  ✅ id        long
  ✅ valid     int
  ✅ name      long  → 已生成 LanguageCache 多语言属性
  ⚠️ type      int   → Partial 中有 GetType() 枚举转换骨架，请填写枚举类名
  ⚠️ item_ids  string → Partial 中有 GetItemIdsList() 解析骨架，请实现解析逻辑

后续步骤：
  1. 创建对应 Service：/il-service-gen <Bean名称>Service <主表名>
  2. 补全 Partial 中标注 // TODO 的方法
  3. （cfg 模式）在 Manager 的 InitData() 中调用 <Bean名称>Cfg.GetAllData()
```
