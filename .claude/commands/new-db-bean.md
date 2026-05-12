# new-db-bean

新增一个对应 SQLite 存档数据库表的运行时 Bean（DB 模式）。
适用于需要增删改查的运行时数据，如 NPC 信息、物品、玩家存档等。

## 用法

```
/new-db-bean <Bean名称> <主表名> <字段列表>
```

**示例：**
- `/new-db-bean InnRoom inn_room id:long name:string capacity:int level:int unlock_price:int`
- `/new-db-bean CharacterSkill character_skill id:long character_id:long skill_id:long level:int exp:int`

## 执行步骤

参考 `NpcInfoBean.cs` 模式，生成两个文件：

### 文件1：`Assets/Scripts/Bean/MVC/<系统名>/<Bean名称>Bean.cs`

系统名从字段或 Bean 名称推断（如 `InnRoom` → `Inn`，`CharacterSkill` → `Character`）。

```csharp
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public partial class <Bean名称>Bean : BaseBean
{
    // 字段（snake_case，与 SQLite 列名一致）
    public int valid;
    // ... 其他字段（按用户传入顺序）
    
    // 多语言字段（若有 long 类型的 name/title/content 字段）：
    // public long name;
    // [JsonIgnore]
    // public string name_language { ... }
    // private LanguageCache _name_language;
}
```

> 无 Cfg 类（DB Bean 不走 JSON 加载，通过 Service 从 SQLite 读取）

### 文件2：`Assets/Scripts/Bean/MVC/<系统名>/<Bean名称>BeanPartial.cs`

```csharp
using System;
using System.Collections.Generic;
public partial class <Bean名称>Bean
{
    public <Bean名称>Bean() { }

    // string 类型存储的复合数据解析方法骨架：
    // public List<long> Get<FieldName>List()
    // {
    //     // TODO: 解析 <field_name> 字符串
    //     return new List<long>();
    // }
    
    // 枚举转换方法（若有 int 类型的 type 字段）：
    // public <TypeEnum> Get<FieldName>Type() => (<TypeEnum>)<fieldName>;
}
```

### 判断规则

- `string` 类型且字段名含 `_ids`/`_types`/`_data` → Partial 中生成 `GetXxxList()` 解析骨架
- `int` 类型且字段名含 `type`/`status` → Partial 中生成枚举转换方法骨架
- 若有 `npc_id` / `character_id` 等外键字段 → 注释说明需要关联 JOIN 查询

生成后提示：
1. 需创建对应的 `<Bean名称>Service`（使用 `/new-service` 命令）
2. 若有多语言副表，表名约定为 `<主表名>_details_<language>`
