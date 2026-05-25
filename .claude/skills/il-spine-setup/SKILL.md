---
name: il-spine-setup
description: 客栈传说 · Spine 角色资源接入指引。引导将一套 Spine 资源（atlas/json/png）正确接入项目：放置目录、Addressable 标记、SpineManager 加载验证、动画状态枚举注册。新增 NPC/角色/特效时使用。
---

# il-spine-setup

**客栈传说 · Spine 资源接入流程 Skill**

将一套新的 Spine 资源（`.atlas.txt` + `.skel`/`.json` + `.png` + `_SkeletonData.asset`）接入项目，并打通到 `SpineManager`/`SpineHandler` 调用链路。

---

## 用法

```
/il-spine-setup <资源名> [类型]
```

**参数：**
- `<资源名>`：Spine 资源主名（不含扩展名），将作为 Addressable Address，例如 `customer_01`
- `[类型]`（可选）：`npc` / `character` / `effect`，决定目录归属与命名建议

**示例：**
```
/il-spine-setup customer_01 npc
/il-spine-setup boss_03 character
/il-spine-setup fire_burst effect
```

---

## 背景知识

**项目 Spine 体系（位于 Framework）：**
- `Assets/FrameWork/Scripts/Component/Manager/SpineManager.cs` — 缓存 `SkeletonDataAsset`、皮肤、动画名映射
- `Assets/FrameWork/Scripts/Component/Handler/SpineHandler.cs` — 业务调用入口：`AddSkeletonAnimation`、`SetSkeletonDataAsset` 等
- `Assets/FrameWork/Scripts/Bean/MVC/SpineAnimationStateBean.cs` — 动画状态 Bean
- `Assets/Scripts/Enums/SpineAnimationStateEnum.cs`（推测，按需核实）— 动画状态枚举

**加载入口：**
```csharp
// 同步
var asset = SpineManager.Instance.GetSkeletonDataAssetSync(assetName);

// 异步（推荐）
SpineManager.Instance.GetSkeletonDataAsset(assetName, (asset) => { ... });

// 业务侧加到 GameObject
SpineHandler.Instance.AddSkeletonAnimation(targetObj, assetName, skinData);
```

`assetName` 就是 Addressable Address，必须与 Group 中注册的地址一致。

---

## 执行步骤

### Step 1 — 检查源资源完整性

提示用户确认导入了完整的一套 Spine 资源（通常由美术从 Spine 软件导出）：

| 必需文件 | 说明 |
|---|---|
| `<资源名>.atlas.txt` | Spine 图集描述 |
| `<资源名>.json` 或 `<资源名>.skel.bytes` | 骨骼数据 |
| `<资源名>.png`（可能多张） | 图集贴图 |
| `<资源名>_Atlas.asset` | Unity 自动生成的 AtlasAsset |
| `<资源名>_SkeletonData.asset` | Unity 自动生成的 SkeletonDataAsset |

通过 Glob 在 `Assets/` 下搜索 `<资源名>*` 列出实际找到的文件，提示缺失项。

### Step 2 — 推荐放置目录

按类型推荐目录（不自动移动文件，输出建议路径）：

| 类型 | 推荐目录 |
|---|---|
| `npc` | `Assets/LoadResources/Spine/NPC/<资源名>/` |
| `character` | `Assets/LoadResources/Spine/Character/<资源名>/` |
| `effect` | `Assets/LoadResources/Spine/Effect/<资源名>/` |

> 实际目录可能与项目现有惯例不同，先 Glob 看现有同类 Spine 资源放在哪，跟随现有规律。

### Step 3 — Addressable 注册指引

输出操作步骤（手动在 Unity Editor 完成）：

```
1. 打开 Window > Asset Management > Addressables > Groups
2. 选中或新建 Group "Character"（角色/NPC）或对应 Group
3. 拖入 <资源名>_SkeletonData.asset
4. 将 Address 改为：<推荐 Address>
   - npc:       NPC/<资源名>
   - character: Character/<资源名>
   - effect:    Effect/Spine/<资源名>
5. 保存
```

### Step 4 — 动画状态枚举核查

检查 `Assets/Scripts/Enums/` 下 `SpineAnimationStateEnum`（或类似命名）枚举：
- 该资源的动画名（通常是 idle / walk / attack / die / ...）是否已包含
- 如缺失，提示通过 `/il-enum-gen` 或手动追加

读取 SkeletonData 的方法（如果 Unity Editor 在运行）：
- 调用 `SpineWindow` 编辑器工具或 `Assets/FrameWork/Editor/Base/SpineEditor.cs` 查看动画列表
- 静态分析：解析 `.json` 文件的 `animations` 字段列出动画名

### Step 5 — 生成调用示例代码

输出可粘贴到业务代码中的加载片段：

```csharp
// 加载并挂载到 targetObj
SpineHandler.Instance.AddSkeletonAnimation(targetObj, "<Address>");

// 异步预加载（推荐在场景初始化时调用）
SpineManager.Instance.GetSkeletonDataAsset("<Address>", (asset) =>
{
    if (asset == null) { LogUtil.LogError("Spine 资源加载失败: <Address>"); return; }
    // TODO: 缓存或使用 asset
});

// 切换动画
skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
```

### Step 6 — 输出验收清单

```
=== Spine 资源接入清单：<资源名> ===

源文件检查：
  ✅ <资源名>.atlas.txt
  ✅ <资源名>.json
  ✅ <资源名>.png
  ✅ <资源名>_SkeletonData.asset
  ⚠️  <资源名>_Atlas.asset 未找到，请检查 Spine 导入设置

推荐 Addressable 配置：
  Group:   Character
  Address: NPC/<资源名>

待手动操作：
  [ ] 将资源放入 Assets/LoadResources/Spine/NPC/<资源名>/
  [ ] 在 Addressables Groups 窗口注册并设置 Address
  [ ] 在 SpineAnimationStateEnum 中补齐缺失的动画状态名
  [ ] 验证 GameObject 加载效果（建议先用 SpineWindow 预览）

调用代码已生成（见上方代码块），可粘贴到对应 Handler 中。
```

---

## 注意事项

- **不自动移动文件**：Unity 资源移动必须经过 Editor 以更新 GUID/引用，本 skill 只给路径建议。
- **不自动注册 Addressable**：Addressable Group 是 Unity Editor 维护的 YAML 资源，手动操作更安全。
- **可与 /il-addressable-audit 联动**：接入后跑一次 audit 确认地址注册正确。
- **多语言资源名**：若资源名涉及多语言/UI 文本，遵循 `TextHandler` + `LanguageCache` 模式（参考 /il-bean-gen）。
