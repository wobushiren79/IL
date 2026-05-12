# new-enum

新增一个游戏枚举，或向已有枚举添加新的枚举项。

## 用法

```
/new-enum <操作> <枚举名或类型> [枚举项...]
```

**操作：**
- `add` — 新建枚举文件
- `append` — 向已有枚举追加枚举项

**示例：**
- `/new-enum add BrewStatusEnum Idle=0 Brewing=1 Done=2 Failed=3`
- `/new-enum add FestivalTypeEnum None=0 Spring=1 Summer=2 Autumn=3 Winter=4`
- `/new-enum append MsgEnum MSG_BREWERY_UPDATE MSG_BREWERY_COMPLETE`
- `/new-enum append UIEnum BREWERY`

## 操作 `add`：新建枚举文件

### 推断文件路径

| 枚举名规律 | 路径 |
|---|---|
| `Msg*Enum` | `Assets/Scripts/Enums/Base/MsgEnum.cs`（追加，不新建） |
| `UI*Enum` / `*UIEnum` | `Assets/Scripts/Enums/Base/UIEnum.cs`（追加，不新建） |
| `Dialog*Enum` | `Assets/Scripts/Enums/Base/DialogEnum.cs`（追加，不新建） |
| `Popup*Enum` | `Assets/Scripts/Enums/Base/PopupEnum.cs`（追加，不新建） |
| 其他 | `Assets/Scripts/Enums/<枚举名>.cs`（新建） |

### 新建格式

```csharp
public enum <枚举名>
{
    // 枚举项（值从用户输入，未指定则从 0 自增）
    <Item1> = 0,
    <Item2> = 1,
    // ...
}
```

> 不加 `using`，不加命名空间，与项目其他枚举文件风格一致。
> 不自动生成 `EnumTool` 工具类，除非用户明确要求。

## 操作 `append`：追加枚举项

1. 读取目标枚举文件
2. 定位枚举末尾（最后一个 `}` 前一行）
3. 若最后一项没有逗号，先补逗号
4. 追加新枚举项，值自动取当前最大值 +1（或由用户指定）
5. 给每个新枚举项加行注释说明用途（若用户有描述）

**MsgEnum 追加示例：**

```csharp
// 原有
MSG_GAME_START = 1001,
MSG_GAME_END   = 1002,

// 追加后（自动取最大值 + 1，或按用户指定的分段规则）
MSG_BREWERY_UPDATE   = 2001,   // 酿酒数据变更，通知 UI 刷新
MSG_BREWERY_COMPLETE = 2002,   // 酿造完成
```

## 注意事项

- `MsgEnum` / `UIEnum` 等基础枚举已有既定的数值分段，追加时先读取现有最大值或询问用户期望的数值范围
- 不删除已有枚举项（删除会导致存档兼容性问题），只做追加
- 若枚举名末尾不含 `Enum` 则自动补上
