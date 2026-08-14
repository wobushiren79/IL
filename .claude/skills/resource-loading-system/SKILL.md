---
name: resource-loading-system
description: 资源加载系统开发指南（共享框架层）。使用此SKILL当需要加载/管理/缓存游戏资源、使用Addressables系统、Resources加载、AssetBundle管理、Spine资源加载、Sprite/图标加载等，包括LoadAddressablesUtil、LoadResourcesUtil、LoadAssetUtil、Manager中的资源缓存机制等。
watched_files:
  - Assets/FrameWork/Scripts/Utils/LoadAddressablesUtil.cs
  - Assets/FrameWork/Scripts/Utils/LoadResourcesUtil.cs
  - Assets/FrameWork/Scripts/Utils/LoadAssetUtil.cs
  - Assets/FrameWork/Scripts/Utils/LoadAssetBundleUtil.cs
  - Assets/FrameWork/Scripts/Utils/LoadWWWUtil.cs
  - Assets/FrameWork/Scripts/Component/Manager/BaseManager.cs
  - Assets/FrameWork/Scripts/Component/Manager/GameDataManager.cs
  - Assets/FrameWork/Scripts/Component/Manager/IconManager.cs
  - Assets/FrameWork/Scripts/Component/Manager/SpineManager.cs
---

# 资源加载系统开发指南

## 核心概念

项目提供 **4 种资源加载方式**，通过工具类封装，Manager 层统一缓存管理。

### 加载方式概览

| 加载方式 | 工具类 | 适用场景 | 特性 |
|---------|--------|---------|------|
| **Addressables** | `LoadAddressablesUtil` | 主要加载方式，动态资源 | 异步、支持远程更新、Catalog管理 |
| **Resources** | `LoadResourcesUtil` | 小型内置资源 | 同步、简单直接 |
| **AssetBundle** | `LoadAssetUtil` / `LoadAssetBundleUtil` | 旧版资源包 | 同步/异步、支持依赖加载 |
| **WWW** | `LoadWWWUtil` | 网络资源 | HTTP加载图片/文本 |

### 资源缓存架构

```
每个 Manager 维护:
    Dictionary<string, T> 资源字典    → 避免重复加载
    SpriteAtlas 懒加载               → 精灵图集按需加载

加载流程:
    1. 检查缓存字典
    2. 缓存未命中 → 发起异步加载
    3. 加载完成 → 存入缓存 → 回调返回
```

---

## Addressables 加载（主要方式）

**文件**: `Assets/FrameWork/Scripts/Utils/LoadAddressablesUtil.cs`

### 加载单个资源

```csharp
// 通过 key 加载（回调）
LoadAddressablesUtil.LoadAssetAsync<GameObject>("prefab_key", (prefab) =>
{
    if (prefab != null)
    {
        Instantiate(prefab);
    }
});

// 通过 key 加载（await）
GameObject prefab = await LoadAddressablesUtil.LoadAssetAsync<GameObject>("prefab_key");

// 通过 AssetReference 加载
LoadAddressablesUtil.LoadAssetAsync<Texture2D>(assetReference, (texture) =>
{
    rawImage.texture = texture;
});
```

### 批量加载

```csharp
// 通过 label 批量加载
LoadAddressablesUtil.LoadAssetsAsync<GameObject>("characters", (list) =>
{
    foreach (var obj in list)
    {
        LogUtil.Log($"加载完成: {obj.name}");
    }
});

// 加载多个指定 key
List<string> keys = new List<string> { "Character_01", "Character_02", "Character_03" };
LoadAddressablesUtil.LoadAssetsAsync<GameObject>(keys, (dict) =>
{
    foreach (var kvp in dict)
    {
        LogUtil.Log($"{kvp.Key} -> {kvp.Value.name}");
    }
});
```

### 释放资源

```csharp
// 释放单个资源
LoadAddressablesUtil.Release(handle);

// 释放通过 key 加载的资源
LoadAddressablesUtil.ReleaseAsset("prefab_key");
```

---

## BaseManager 中的资源加载方法

**文件**: `Assets/FrameWork/Scripts/Component/Manager/BaseManager.cs`

### 内部加载方法

```csharp
// 同步加载单个资源
public T GetModel<T>(string path) where T : UnityEngine.Object;

// Addressables 异步加载单个
public void GetModelForAddressables<T>(string key, Action<T> callback);

// Addressables 批量加载
public void GetModelsForAddressables<T>(string label, Action<IList<T>> callback);

// 从 SpriteAtlas 加载精灵
public Sprite GetSpriteByName(string atlasName, string spriteName);
```

### 在 Manager 中实现资源缓存

```csharp
public class MyCustomManager : BaseManager
{
    // 资源缓存
    private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    // 加载并缓存预制体
    public void LoadPrefab(string prefabKey, Action<GameObject> callback)
    {
        // 1. 检查缓存
        if (prefabCache.TryGetValue(prefabKey, out var cached))
        {
            callback?.Invoke(cached);
            return;
        }

        // 2. 加载并缓存
        GetModelForAddressables<GameObject>(prefabKey, (prefab) =>
        {
            if (prefab != null)
            {
                prefabCache[prefabKey] = prefab;
            }
            callback?.Invoke(prefab);
        });
    }

    // 清理缓存
    public void ClearCache()
    {
        prefabCache.Clear();
        spriteCache.Clear();
    }
}
```

---

## Resources 加载

**文件**: `Assets/FrameWork/Scripts/Utils/LoadResourcesUtil.cs`

### 基础用法

```csharp
// 加载单个资源（泛型，路径相对 Resources 目录）
TextAsset config = LoadResourcesUtil.Load<TextAsset>("JsonText/AudioInfo");

// 加载 GameObject（UI 预制体在 Resources/UI 下）
GameObject prefab = LoadResourcesUtil.Load<GameObject>("UI/Dialog/UIDialogNormal");

// 加载所有资源
TextAsset[] configs = LoadResourcesUtil.LoadAll<TextAsset>("JsonText");

// 检查资源是否存在
bool exists = LoadResourcesUtil.Exists("JsonText/MyConfig");
```

### Resources 目录结构（IL 实际布局）

```
Assets/Resources/
├── JsonText/          # JSON 文本（配置表导出：AudioInfo / BaseInfo / ItemsInfo / UIText 等）
├── UI/                # UI 预制体（按类型分目录）
│   ├── Dialog/        # 弹窗（UIDialogAchievement / UIDialogFindCharacter ...）
│   ├── Popup/         # 气泡
│   ├── Toast/         # 提示
│   ├── Item/          # 列表项
│   ├── Show/          # 展示类
│   └── Other/         # 其他 UI
└── ...                # 字体等其他内置资源按需放置
```

---

## AssetBundle 加载

**文件**: `Assets/FrameWork/Scripts/Utils/LoadAssetUtil.cs` / `LoadAssetBundleUtil.cs`

### 基础用法

```csharp
// 同步加载 bundle 中的资源
GameObject prefab = LoadAssetUtil.LoadAsset<GameObject>(bundleName, assetName);

// 异步加载
LoadAssetUtil.LoadAssetAsync<GameObject>(bundleName, assetName, (prefab) =>
{
    if (prefab != null)
    {
        Instantiate(prefab);
    }
});

// 加载 bundle
AssetBundle bundle = LoadAssetBundleUtil.LoadBundle(bundlePath);

// 卸载 bundle
LoadAssetBundleUtil.UnloadBundle(bundleName, unloadAllLoadedObjects: true);
```

---

## 网络资源加载 (WWW)

**文件**: `Assets/FrameWork/Scripts/Utils/LoadWWWUtil.cs`

```csharp
// 加载图片
LoadWWWUtil.LoadTexture(url, (texture) =>
{
    if (texture != null)
    {
        rawImage.texture = texture;
    }
});

// 加载文本
LoadWWWUtil.LoadText(url, (text) =>
{
    if (!string.IsNullOrEmpty(text))
    {
        // 处理文本
    }
});
```

---

## Icon 加载系统

**文件**: `Assets/FrameWork/Scripts/Component/Manager/IconManager.cs`

### 图标加载

```csharp
// 通过 IconHandler 加载图标
IconHandler.Instance.LoadIcon(iconId, (sprite) =>
{
    if (sprite != null)
    {
        image.sprite = sprite;
    }
});

// 同步获取（如果已缓存）
Sprite sprite = IconManager.Instance.GetCachedIcon(iconId);
```

---

## Spine 资源加载

**文件**: `Assets/FrameWork/Scripts/Component/Manager/SpineManager.cs`

### Spine 资源预加载

```csharp
// 预加载多个 Spine 资源
List<string> assetNames = new List<string> { "Character_01", "Character_02" };
SpineHandler.Instance.PreLoadSkeletonDataAsset(assetNames, (dicData) =>
{
    // 加载完成
});

// 同步获取
SkeletonDataAsset data = SpineManager.Instance.GetSkeletonDataAssetSync("Character_01");

// 异步获取
SpineManager.Instance.GetSkeletonDataAsset("Character_01", (data) =>
{
    skeletonAnimation.skeletonDataAsset = data;
    skeletonAnimation.Initialize(true);
});
```

---

## Mod 资源加载

**文件**: `Assets/FrameWork/Scripts/Component/Manager/ModManager.cs`

```csharp
// 从 Mod 加载资源
ModHandler.Instance.LoadAsset<Sprite>("ModName", "icon_asset", (sprite) =>
{
    image.sprite = sprite;
});

// 同步加载
SkeletonDataAsset data = ModHandler.Instance.LoadAssetSync<SkeletonDataAsset>(
    "ModName", 
    "character_skeletondata"
);

// 检查是否为 Mod 资源
bool isMod = ModHandler.Instance.IsModAsset("asset_key");
```

---

## 常用代码模板

### 加载 UI Prefab

```csharp
// UIManager 中加载 UI Prefab（IL：UI 预制体在 Resources/UI 下，路径相对 Resources）
public void LoadUIPrefab(string uiName, Action<GameObject> callback)
{
    string prefabPath = $"UI/{uiName}";
    var prefab = LoadResourcesUtil.Load<GameObject>(prefabPath);
    if (prefab != null)
    {
        var uiObj = Instantiate(prefab, transform);
        callback?.Invoke(uiObj);
    }
}
```

### 预加载资源列表

```csharp
public class ResourcePreloader : BaseMonoBehaviour
{
    public IEnumerator PreloadResources(Action<float> onProgress, Action onComplete)
    {
        List<string> prefabsToLoad = new List<string>
        {
            "UI/UIMain", "UI/UIMiniGame", "UI/UISettlement",
            "UI/Dialog/UIDialogNormal", "UI/Toast/UIToastNormal"
        };

        int loaded = 0;
        foreach (var key in prefabsToLoad)
        {
            bool done = false;
            LoadAddressablesUtil.LoadAssetAsync<GameObject>(key, (prefab) =>
            {
                loaded++;
                onProgress?.Invoke((float)loaded / prefabsToLoad.Count);
                done = true;
            });

            yield return new WaitUntil(() => done);
        }

        onComplete?.Invoke();
    }
}
```

### 带 Fallback 的资源加载

```csharp
public void LoadSpriteWithFallback(string primaryKey, string fallbackKey, Action<Sprite> callback)
{
    LoadAddressablesUtil.LoadAssetAsync<Sprite>(primaryKey, (sprite) =>
    {
        if (sprite != null)
        {
            callback?.Invoke(sprite);
        }
        else
        {
            // Fallback 到备用资源
            LoadAddressablesUtil.LoadAssetAsync<Sprite>(fallbackKey, callback);
        }
    });
}
```

### 清理特定模块的资源

```csharp
public void UnloadModuleResources(string moduleName)
{
    // 释放 Addressables 句柄
    LoadAddressablesUtil.ReleaseByLabel(moduleName);
    
    // 清理本地缓存
    prefabCache.Clear();
    spriteCache.Clear();
    
    // 卸载未使用的资源
    Resources.UnloadUnusedAssets();
}
```

---

## 资源路径规范

| 资源类型 | 路径 | 加载方式 |
|---------|------|---------|
| UI Prefab | `Assets/Resources/UI/` | Resources（`LoadResourcesUtil`） |
| 场景 Prefab（角色/特效/物品等） | `Assets/Prefabs/` | Addressables（`LoadAddressablesUtil`） |
| 纹理 / 图集 | `Assets/LoadResources/Textures/` | Addressables |
| 音频 | `Assets/LoadResources/Audio/` | Addressables |
| JSON 配置 | `Assets/Resources/JsonText/` | Resources |

> 具体加载方式以代码中实际使用的 Util 为准（框架提供 `LoadAddressablesUtil` / `LoadResourcesUtil` / `LoadAssetUtil` / `LoadAssetBundleUtil` / `LoadWWWUtil` 五套）；本表为 IL 当前资源目录布局参考。

---

## 文件位置速查

| 功能 | 文件路径 |
|------|----------|
| Addressables 工具 | `Assets/FrameWork/Scripts/Utils/LoadAddressablesUtil.cs` |
| Resources 工具 | `Assets/FrameWork/Scripts/Utils/LoadResourcesUtil.cs` |
| Asset 加载工具 | `Assets/FrameWork/Scripts/Utils/LoadAssetUtil.cs` |
| AssetBundle 工具 | `Assets/FrameWork/Scripts/Utils/LoadAssetBundleUtil.cs` |
| WWW 加载工具 | `Assets/FrameWork/Scripts/Utils/LoadWWWUtil.cs` |
| BaseManager | `Assets/FrameWork/Scripts/Component/Manager/BaseManager.cs` |
| IconManager | `Assets/FrameWork/Scripts/Component/Manager/IconManager.cs` |
| SpineManager | `Assets/FrameWork/Scripts/Component/Manager/SpineManager.cs` |
| ModManager | `Assets/FrameWork/Scripts/Component/Manager/ModManager.cs` |
| Addressables 配置 | `Assets/AddressableAssetsData/` |

---

## 注意事项

1. **缓存管理**: Manager 中的资源缓存是内存占用大户，场景切换时应清理不再需要的缓存。
2. **异步回调时序**: Addressables 加载是异步的，注意回调中可能已经切换了场景或销毁了对象。
3. **释放资源**: 使用 LoadAddressablesUtil.Release 释放 Addressables 句柄，避免内存泄漏。
4. **Resources vs Addressables**: 新资源优先使用 Addressables，仅小型内置配置使用 Resources。
5. **同步加载警告**: 同步加载可能造成卡顿，仅用于确定已缓存的资源或小体积资源。
6. **Mod 优先加载**: 系统会自动检测并优先加载 Mod 中的资源，需注意资源覆盖行为。
