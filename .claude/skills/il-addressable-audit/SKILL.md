---
name: il-addressable-audit
description: 客栈传说 · Addressables 资源审计。扫描 Assets/AddressableAssetsData/AssetGroups 与 Assets/Scripts 中的 Addressable 地址引用，找出未注册地址、孤立资源、命名违规、Group 归属异常等问题。用于例行体检或上线前检查。
---

# il-addressable-audit

**客栈传说 · Addressables 资源审计 Skill**

对项目 Addressables 2.9.1 配置进行静态审计，不依赖 Unity Editor 运行。

---

## 用法

```
/il-addressable-audit [范围]
```

**范围参数：**

| 范围 | 说明 |
|---|---|
| `all`（默认） | 全量审计：未注册地址 + 孤立资源 + 命名 + Group 归属 |
| `unregistered` | 只扫脚本里 `LoadAssetUtil` 用到但 Group 未注册的地址 |
| `orphan` | 只扫 Group 中注册了但全项目代码无引用的资源 |
| `naming` | 只检查命名规范（UI/、NPC/、Audio/ 等前缀） |
| `groups` | 只检查 Group 分布是否合理（音频是否都在 Music/Sound Group） |

**示例：**
```
/il-addressable-audit
/il-addressable-audit unregistered
/il-addressable-audit naming
```

---

## 背景知识

- Addressable 配置文件：`Assets/AddressableAssetsData/AssetGroups/*.asset`（YAML 文本）
- 已知 Group：Character、Music、Sound、SpriteAtlas、Effects、BuildItems、AnimEquip、AnimFood、MiniGame、Controls、Default Local Group 等
- 加载入口：`LoadAssetUtil.SyncLoadAddressable<T>(address, callback)` / 异步重载
- Spine 资源加载走 `SpineManager.GetSkeletonDataAsset(assetName, ...)`，地址即 assetName

---

## 执行步骤

### Step 1 — 解析 Addressable Group 文件

读取 `Assets/AddressableAssetsData/AssetGroups/*.asset`，提取每个 entry：
- `m_Address`（Addressable 地址）
- `m_GUID`
- `m_TargetAsset`（资源 GUID → 路径，需查 `.meta`）
- 所属 Group 名

建立 `address -> { group, assetPath, guid }` 映射表。

### Step 2 — 收集代码中的地址引用

在 `Assets/Scripts/` 与 `Assets/FrameWork/Scripts/` 中 grep：

```
LoadAssetUtil\.(Sync)?LoadAddressable\s*<[^>]+>\s*\(\s*[$"]([^"]+)
GetSkeletonDataAsset(?:Sync)?\s*\(\s*[$"]?([^",)]+)
```

提取字符串字面量地址。对 `$"..."` 插值字符串，记录前缀部分（无法静态判断完整地址，标记为"动态地址"另行汇总）。

### Step 3 — 审计

#### 未注册地址 (unregistered)
代码引用但 Group 中无 entry 的地址。
> 注意：动态地址（含变量插值）无法判定，列入"待人工核查"。

#### 孤立资源 (orphan)
Group 中注册但代码中无引用的地址。
> 部分资源可能通过反射/字符串拼接加载，输出时提醒"可能为动态加载，确认后再处理"。

#### 命名检查 (naming)

| 资源类型推断 | 期望前缀 | 示例 |
|---|---|---|
| UI Prefab（路径含 `/UI/` 或 `UIBase` 组件） | `UI/` | `UI/UIBrewery` |
| NPC Prefab | `NPC/` | `NPC/Customer/NpcBase` |
| 音乐（在 Music Group） | `Music/` | `Music/main_theme` |
| 音效（在 Sound Group） | `Sound/` | `Sound/click` |
| Spine 资源 | `Spine/` 或角色名 | `Spine/customer_01_SkeletonData` |
| 特效 | `Effect/` | `Effect/Blood` |

#### Group 归属 (groups)
- AudioClip 必须在 `Music` 或 `Sound` Group
- SpriteAtlas 必须在 `SpriteAtlas` Group
- 角色 Spine 资源必须在 `Character` Group
- UI Prefab 不应在 `Default Local Group`

### Step 4 — 输出报告

```
=== Addressables 审计报告 ===
扫描 Group: 11 个，Entry: <N> 条
扫描脚本: <M> 个文件，发现引用 <K> 处（动态地址 <D> 处）

❌ 未注册地址（代码引用但 Group 缺失）：
  - "UI/UINewWindow"  使用于  Assets/Scripts/MVC/.../FooHandler.cs:42

🗑️  孤立资源（Group 中注册但无代码引用）：
  - "UI/UIObsolete"   注册于  Group: Default Local Group
    （提示：如为动态地址加载，请忽略）

📝 命名不规范：
  - "main_theme"      Group: Music   建议改为 "Music/main_theme"
  - "customerBase"    Group: Character  建议改为 "NPC/Customer/customerBase"

🔀 Group 归属异常：
  - "click.wav"  在  Default Local Group，应移到 Sound

⚠️  动态地址（无法静态判定，请人工核查）：
  - $"NPC/Customer/{npcId}"   位置: NpcHandler.cs:88

总计问题：<count>
```

---

## 注意事项

- **只读 skill**：不修改任何 Group 配置或代码。Group 修改必须在 Unity Editor 的 Addressables Groups 窗口操作。
- **YAML 解析**：Group `.asset` 文件是 Unity YAML，可逐行解析 `m_Address:` / `m_GUID:` 字段，不要依赖额外库。
- **GUID 到路径**：通过查 `Assets/**/*.meta` 文件中的 `guid:` 字段反查资源路径。可一次性建索引。
- **大项目优化**：本项目脚本 800+ 文件，建议先对 grep 结果去重再分析。
