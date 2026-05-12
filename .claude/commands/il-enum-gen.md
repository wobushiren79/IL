# il-enum-gen

**客栈传说 · 枚举管理 Agent**

安全地新建枚举文件或向已有枚举追加枚举项，自动处理数值连续性、分段规则和存档兼容性。

---

## 用法

```
/il-enum-gen <操作> <目标> [枚举项...]
```

**操作：**
- `new` — 新建独立枚举文件
- `append` — 向已有枚举文件追加新枚举项

**目标：**
- `MsgEnum` — 消息事件枚举（追加到 `Assets/Scripts/Enums/Base/MsgEnum.cs`）
- `UIEnum` — UI 面板枚举（追加到 `Assets/Scripts/Enums/Base/UIEnum.cs`）
- `DialogEnum` — 对话框枚举（追加到 `Assets/Scripts/Enums/Base/DialogEnum.cs`）
- `PopupEnum` — 弹窗枚举（追加到 `Assets/Scripts/Enums/Base/PopupEnum.cs`）
- `AudioSoundEnum` — 音效枚举（追加到对应文件）
- `<自定义名>` — 新建独立枚举文件（需配合 `new` 操作）

**枚举项格式：** `名称=值` 或仅 `名称`（值自动递增）

**示例：**
```
/il-enum-gen append MsgEnum MSG_BREWERY_UPDATE MSG_BREWERY_COMPLETE
/il-enum-gen append UIEnum BREWERY
/il-enum-gen append DialogEnum BREWERY_CONFIRM
/il-enum-gen new BrewStatusEnum None=0 Idle=1 Brewing=2 Done=3 Failed=4
/il-enum-gen new FestivalTypeEnum None=0 Spring=1 Summer=2 Autumn=3 Winter=4
/il-enum-gen append MsgEnum MSG_FESTIVAL_START=5001 MSG_FESTIVAL_END=5002
```

---

## 执行步骤

### Step 1 — 定位目标文件

| 目标名 | 文件路径 |
|---|---|
| `MsgEnum` | `Assets/Scripts/Enums/Base/MsgEnum.cs` |
| `UIEnum` | `Assets/Scripts/Enums/Base/UIEnum.cs` |
| `DialogEnum` | `Assets/Scripts/Enums/Base/DialogEnum.cs` |
| `PopupEnum` | `Assets/Scripts/Enums/Base/PopupEnum.cs` |
| `AudioSoundEnum` | 搜索 `Assets/Scripts/Enums/` 下的对应文件 |
| 其他（`new` 操作） | `Assets/Scripts/Enums/<目标名>.cs`（新建） |

对于 `new` 操作且目标含 `Gamble` → `Assets/Scripts/Enums/Gamble/<目标名>.cs`
对于 `new` 操作且目标含 `MiniGame` → `Assets/Scripts/Enums/MiniGame/<目标名>.cs`

### Step 2 — 操作 `new`：新建枚举文件

直接写入，格式与项目其他枚举文件一致（无 `using`，无命名空间）：

```csharp
public enum <目标名>
{
    <Item1> = 0,
    <Item2> = 1,
    // ...
}
```

> 若用户未指定值，从 0 开始自增。
> 若用户指定了部分值，未指定的项接续前一项 +1。

### Step 3 — 操作 `append`：向已有枚举追加

1. **读取目标枚举文件**，提取当前所有枚举项和值

2. **确定追加值：**
   - 若用户显式指定了值（`NAME=VALUE`）：使用用户值，检查是否与现有值冲突
   - 若未指定：取当前枚举中的**最大值 +1**
   - 对于 `MsgEnum`，读取后检查是否有分段约定（如 1000-1999 = 游戏消息，2000-2999 = UI 消息），在正确的分段范围内追加，并告知用户

3. **追加格式：**
   ```csharp
   // 在最后一个枚举项之后，最后一个 } 之前插入
   // 确保前一项末尾有逗号
   <NEW_ITEM> = <值>,   // <描述，若用户提供了描述>
   ```

4. **冲突检测：**
   - 若新枚举项名称已存在 → 报错，不追加
   - 若新枚举项数值已存在 → 警告，询问是否仍然追加（存档兼容风险）

5. **绝对不删除已有枚举项**（删除会破坏存档兼容性）

### Step 4 — MsgEnum 分段说明

读取 MsgEnum.cs 后，识别现有分段（基于注释或数值跳跃），展示给用户：

```
当前 MsgEnum 分段（示例）：
  游戏消息：MSG_GAME_* = 100x 段
  UI 消息：MSG_UI_* = 200x 段
  NPC 消息：MSG_NPC_* = 300x 段
  ...

建议新增 MSG_BREWERY_* 在 <X>0x 段（当前最大值 + 留余量）
```

若用户未指定值，自动选择合适分段的起始值。

### Step 5 — 输出摘要

```
已修改：
  <文件路径>

变更：
  + MSG_BREWERY_UPDATE   = 2001
  + MSG_BREWERY_COMPLETE = 2002

注意：
  ⚠️ 枚举值一旦确定不可随意修改（已有存档中保存的是整数值）
  ⚠️ 不要删除已有枚举项，只做追加

后续步骤：
  - 在 Manager 操作方法中取消注释 EventHandler.Instance.TriggerEvent() 调用
  - 在对应 UI Handler/View 中注册消息监听
```
