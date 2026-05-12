# il-unity-asset

**客栈传说 · Unity 资源管理 Agent**

通过 unity-skills 管理 Addressable 资源：查询资源状态、分析引用关系、辅助资源命名规范检查、生成资源加载代码骨架。

---

## 用法

```
/il-unity-asset <操作> [参数...]
```

**操作列表：**

| 操作 | 说明 |
|---|---|
| `list <类型>` | 列出指定类型的 Addressable 资源（prefab / sprite / audio / material） |
| `find <名称>` | 在 Addressable 组中搜索包含指定名称的资源 |
| `check-naming` | 检查 Addressable 资源命名是否符合项目规范 |
| `ref <资源名>` | 分析指定资源被哪些脚本引用（Addressable 地址字符串搜索） |
| `load-code <资源地址> <类型>` | 生成该资源的 Addressables 异步加载代码骨架 |
| `missing-ref` | 扫描 `Assets/Scripts/` 中硬编码的资源路径字符串，找出可能失效的引用 |

**示例：**
```
/il-unity-asset list prefab
/il-unity-asset find InnRoom
/il-unity-asset check-naming
/il-unity-asset ref UI/UIBrewery
/il-unity-asset load-code Prefabs/NPC/Customer GameObject
/il-unity-asset missing-ref
```

---

## 项目 Addressable 资源规范（背景知识）

本项目使用 **Addressables 2.9.1** 作为主要资源加载方式：

**资源加载工具：** `LoadAssetUtil`（位于 `Assets/FrameWork/Scripts/Utils/LoadAssetUtil.cs`）

**常见加载模式（参考现有代码）：**
```csharp
// 加载 Prefab 并实例化
LoadAssetUtil.SyncLoadAddressable<GameObject>("资源地址", (obj) => {
    // obj 是加载到的 GameObject
    Instantiate(obj);
});

// 加载 Sprite
LoadAssetUtil.SyncLoadAddressable<Sprite>("资源地址", (sprite) => {
    ivIcon.sprite = sprite;
});

// 加载 AudioClip
LoadAssetUtil.SyncLoadAddressable<AudioClip>("资源地址", (clip) => {
    audioSource.clip = clip;
});
```

---

## 执行步骤

### 操作 `list <类型>`

调用 unity-skills 查询 Addressable 资源组，筛选指定类型（`GameObject` / `Sprite` / `AudioClip` / `Material`）。

按资源组分类输出：
```
Group: UI
  ├── UI/UIBrewery          (Prefab)
  ├── UI/UIFestival         (Prefab)
  └── ...

Group: NPC
  ├── NPC/Customer/NpcBase  (Prefab)
  └── ...
```

### 操作 `find <名称>`

在所有 Addressable 资源的 Address（键）和文件名中搜索包含 `<名称>` 的资源，返回：
- 资源 Address（Addressable 键）
- 实际文件路径
- 所属 Addressable Group

### 操作 `check-naming`

读取 Addressable 资源列表，按以下规范检查命名：

| 资源类型 | 期望命名格式 | 示例 |
|---|---|---|
| UI Prefab | `UI/<类名>` | `UI/UIBrewery` |
| NPC Prefab | `NPC/<类型>/<名称>` | `NPC/Customer/NpcBase` |
| 音效 | `Audio/<分类>/<名称>` | `Audio/BGM/MainTheme` |
| 粒子特效 | `Effect/<名称>` | `Effect/Blood` |

输出不符合规范的资源列表，**不自动修改**，由用户手动在 Unity Editor 中调整 Address。

### 操作 `ref <资源名>`

在 `Assets/Scripts/` 目录下 grep 搜索包含 `"<资源名>"` 的字符串（Addressable 地址引用），列出：
- 文件路径
- 行号
- 上下文代码片段

### 操作 `load-code <资源地址> <类型>`

根据类型生成对应的 Addressables 加载代码骨架：

```csharp
// 加载 <类型> 资源：<资源地址>
LoadAssetUtil.SyncLoadAddressable<<类型>>("<资源地址>", (asset) =>
{
    if (asset == null)
    {
        LogUtil.LogError("资源加载失败: <资源地址>");
        return;
    }
    // TODO: 使用 asset
});
```

### 操作 `missing-ref`

扫描 `Assets/Scripts/` 中所有 `.cs` 文件，搜索形如 `"Prefabs/..."` / `"Assets/..."` / `"Resources/..."` 的硬编码路径字符串（非 Addressable 方式），列出：
- 文件路径 + 行号
- 硬编码路径内容
- 建议：将其改为 Addressable Address

---

## 注意事项

- 本 Agent 以**查询和分析**为主，不自动修改 Addressable 配置（修改需要在 Unity Editor 的 Addressables Groups 窗口中完成）
- `load-code` 操作只生成代码骨架供参考，不自动写入文件
- unity-skills REST API 需要 Unity Editor 处于运行状态；若未运行，将改为静态文件分析模式
