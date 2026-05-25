---
name: il-build-prep
description: 客栈传说 · Steam 上线前置检查。打包前一次性校验：版本号一致性、steam_appid 配置、Addressables 是否需要重新 Build、调试代码残留、Logger 开关、Debug 场景、敏感字符串、TODO 遗留、平台开关等。每次出版本时使用。
---

# il-build-prep

**客栈传说 · 上线打包前置体检 Skill**

避免上线后才发现"忘记关 LogUtil"、"Addressables 内容过期"、"版本号没改"等低级错误。本 skill 一次性扫描所有打包前必须确认的项，输出待办清单。

---

## 用法

```
/il-build-prep [级别] [选项]
```

**级别参数：**

| 级别 | 检查项 |
|---|---|
| `quick`（默认） | 关键项（版本号、AppID、Logger、调试 Scene） |
| `full` | 全项目深度扫描 |
| `version-only` | 只看版本相关 |
| `addressables-only` | 只看资源构建 |
| `safety-only` | 只看代码安全/敏感字符串 |

**选项：**
- `--target=Windows64` ：指定目标平台（默认 Windows64）
- `--strict` ：严格模式，TODO 注释视为错误

**示例：**
```
/il-build-prep
/il-build-prep full
/il-build-prep version-only
/il-build-prep full --strict
```

---

## 背景知识

### 项目打包目标
- **平台**：Steam / Windows（`steam_appid.txt` 已存在）
- **渲染**：URP
- **资源管理**：Addressables 2.9.1

### 关键文件
| 路径 | 用途 |
|---|---|
| `ProjectSettings/ProjectSettings.asset` | bundleVersion / productName / companyName |
| `steam_appid.txt` | Steamworks App ID |
| `Assets/AddressableAssetsData/AddressableAssetSettings.asset` | Addressables 配置 |
| `Assets/AddressableAssetsData/AssetGroups/*.asset` | Group 配置 |
| `Library/com.unity.addressables/` | Addressables 构建缓存 |
| `ProjectSettings/EditorBuildSettings.asset` | 场景构建列表 |

### Logger 配置
项目用 `LogUtil` 封装 `Debug.Log`。`LogUtil.cs` 中通常有 `IS_LOG_ENABLE` 开关或 `[Conditional("DEBUG")]` 属性，发布版应关闭。

---

## 执行步骤

### Step 1 — 版本号一致性

#### 1a. ProjectSettings.asset 中的版本
读取 `ProjectSettings/ProjectSettings.asset`，提取：
```yaml
bundleVersion: 0.4.9
buildNumber:
  Standalone: 1
```

#### 1b. GameData 中的更新说明文件
Glob `GameData/客栈传说 v*.txt` 取最新版本号，对比是否一致。

#### 1c. 代码中的版本常量
grep `VERSION\s*=\s*"` 在 `Assets/Scripts/Common/` 与 `Assets/FrameWork/Scripts/`，检查是否有硬编码版本号需同步。

#### 1d. Steam Build Settings
若有 Steam 发布配置文件（VDF 等），核对 BuildID。

**输出：**
```
版本号一致性：
  ProjectSettings.bundleVersion : 0.4.9
  最新更新说明                  : 客栈传说 v0.5.0 更新说明.txt   ⚠️ 不一致
  ProjectConfigInfo.VERSION    : "0.4.9"                       ✅
  → 建议：将 bundleVersion 提升至 0.5.0
```

### Step 2 — steam_appid.txt 检查

读取 `steam_appid.txt`，确认：
- 文件存在
- 内容为 1~9 位数字（合法 AppID 格式）
- 不为 `480`（默认测试 AppID，发布前必须替换为真实 AppID）

### Step 3 — Addressables 构建状态

#### 3a. 检查 Addressables 设置
读取 `AddressableAssetSettings.asset`：
- `m_BuildRemoteCatalog` 设置
- `m_ActivePlayerDataBuilderIndex` 是否为 BuildScriptPackedMode（发布模式）
- `m_overridePlayerVersion`

#### 3b. 检查 catalog 时间
比较：
- `Library/com.unity.addressables/aa/Windows/catalog_*.json` 的修改时间
- `Assets/AddressableAssetsData/AssetGroups/*.asset` 任一文件的修改时间

若 Group 文件比 catalog 新 → `🔴 Group 修改后未重新 Build Addressables`

#### 3c. Group 中的 Static / Local 配置
- 本地资源应在 Local Group
- 是否有意外的 Remote 配置（无远程 CDN 项目应全 Local）

#### 3d. Build Report
若 `Library/com.unity.addressables/aa/Windows/buildlogtep.json` 存在，提取错误数量。

### Step 4 — 调试代码残留扫描（操作 safety）

#### 4a. Logger 开关
读取 `LogUtil.cs`，找到 `IS_LOG_ENABLE` / `IS_DEBUG` / `IS_LOG_OPEN` 常量：
- 若为 `true` → `🟠 发布前请置为 false`
- 若用 `[Conditional("DEBUG")]` 且 ScriptingDefineSymbols 含 `DEBUG` → `🟠 移除 DEBUG 宏`

#### 4b. Debug.Log 残留
grep `Debug.Log` / `Debug.LogError`（**不**走 LogUtil 封装的）：
- 列出所有直接调用 `Debug.Log*` 的位置
- 建议改用 `LogUtil` 封装

#### 4c. 测试场景
读取 `ProjectSettings/EditorBuildSettings.asset`，提取场景列表：
- 名含 `Test` / `Demo` / `Sample` 的场景 → 警告

#### 4d. 调试用快捷键
grep `Input\.GetKeyDown\s*\(\s*KeyCode\.F\d+|KeyCode\.[A-Z]\)`，列出按键调试代码（如 F1 跳关、F5 增加金币）。

#### 4e. 作弊代码
grep `cheat` / `cheating` / `god\s*mode` / `add_?money` / `unlock_?all`（不区分大小写）。

#### 4f. TODO / FIXME / HACK
grep `//\s*(TODO|FIXME|HACK|XXX)`：
- `--strict` 模式：视为错误
- 默认：列为建议清理

#### 4g. 硬编码 IP / URL
grep `https?://[a-z0-9.\-]+` 与 `\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}`：
- 排除注释行
- 排除已知白名单（如 unity.com）

### Step 5 — Player Settings 检查

读取 `ProjectSettings.asset`：
- `companyName` / `productName` 不为 "DefaultCompany" / "New Unity Project"
- `bundleVersion` 已更新
- 是否开启 `useStandardAssetsAnalytics`、`enableEditorAnalytics`（按需关闭）
- `ScriptingBackend` = IL2CPP（推荐 Release 用）
- `ApiCompatibilityLevel` 正确
- 图标已设置（不为默认 Unity logo）

### Step 6 — 平台开关

读取 ScriptingDefineSymbols：
- `UNITY_STANDALONE_WIN` 应启用
- `DEBUG` / `TEST` 应关闭
- `STEAMWORKS_NET` 应启用（项目用 Steam）
- 自定义符号（如 `CHEAT_MODE`）应关闭

### Step 7 — 资源大小预估

可选：调用 `du -sh Assets/AddressableAssetsData/` 与 `Library/com.unity.addressables/aa/Windows/` 输出资源总量预估。

### Step 8 — 输出报告

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  客栈传说 · 上线前置检查 · v0.4.9
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📌 版本号
  bundleVersion          : 0.4.9
  GameData/客栈传说 v*.txt : 0.5.0
  → ⚠️ 不一致，建议更新 ProjectSettings 至 0.5.0

📌 Steam
  steam_appid.txt        : 1234567   ✅
  ⚠️ 注意：若仍为 480 必须替换

📌 Addressables
  最近 Build              : 2025-12-01 14:23
  最近 Group 修改          : 2026-05-24 09:12  🔴 比 Build 新 175 天
  → 必须重新 Build Addressables（菜单：Window > Asset Management > Addressables > Groups > Build > New Build）
  Active Builder         : BuildScriptPackedMode  ✅
  Remote Catalog         : 未启用  ✅（全本地）

📌 Logger / 调试
  LogUtil.IS_LOG_ENABLE  : true       🟠 发布前请置为 false
  Debug.Log 直接调用      : 87 处     🟡 建议封装走 LogUtil
  调试快捷键             : 12 处     🟡 详见下方清单
  作弊代码               : 3 处      🟠 上线前移除
  TODO / FIXME           : 156 处    （strict 模式下视为错误）

📌 测试场景
  EditorBuildSettings.scenes 包含：
    Assets/Scenes/TestScene.unity        🟠 上线前移除
    Assets/Scenes/SampleScene.unity      🟠 上线前移除

📌 硬编码 URL/IP
  Assets/Scripts/Web/AnalyticsManager.cs:23
    http://test-api.local:8080         🔴 测试 URL，必须替换

📌 PlayerSettings
  companyName            : "GalaSports"        ✅
  productName            : "客栈传说"          ✅
  icon                   : 已设置              ✅
  ApiCompatibilityLevel  : .NET Standard 2.1   ✅
  ScriptingBackend       : Mono2x  🟡 建议改用 IL2CPP

📌 ScriptingDefineSymbols
  UNITY_STANDALONE_WIN   : ✅
  STEAMWORKS_NET         : ✅
  DEBUG                  : 🟠 已启用，发布前关闭
  CHEAT_MODE             : 🟠 已启用，发布前关闭

📌 资源大小预估
  AssetGroups 源资源     : 2.3 GB
  Build 输出             : 1.8 GB
  → 注意 Steam Build 大小上限

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  待办清单（按优先级）
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🔴 必须修复（5 项）：
  [ ] 更新 ProjectSettings.bundleVersion 至 0.5.0
  [ ] 重新 Build Addressables
  [ ] 替换 AnalyticsManager 中测试 URL
  [ ] 移除作弊代码（3 处见详情）
  [ ] EditorBuildSettings 移除 TestScene / SampleScene

🟠 强烈建议（4 项）：
  [ ] LogUtil.IS_LOG_ENABLE = false
  [ ] 关闭 DEBUG / CHEAT_MODE 宏
  [ ] 改用 IL2CPP 后端
  [ ] 移除 12 处调试快捷键

🟡 可选优化（3 项）：
  [ ] 87 处 Debug.Log 改为 LogUtil
  [ ] 清理 156 处 TODO 注释
  [ ] 检查 Player 启动画面

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 注意事项

- **只读 skill**：不修改任何代码或配置。所有修复需用户在 Unity Editor 或代码中手动执行。
- **执行 Addressables Build 不在本范围**：构建必须在 Unity Editor 的 Addressables 窗口完成。本 skill 仅检测是否需要重新构建。
- **版本号约定**：项目通过 `GameData/客栈传说 v*.txt` 命名追踪版本，作为权威源。
- **跨平台**：默认 Windows64。若多平台发布，需对每个平台分别 Build Addressables。
- **不删 TODO**：TODO 注释只标记，不批量删除（可能漏掉真实未完成项）。
- **关联工具**：
  - Addressables 问题可联动 `/il-addressable-audit`
  - 多语言完整性可联动 `/il-localization-audit`
  - 代码质量可联动 `/il-code-reviewer --diff`
  - 数据完整性可联动 `/il-data-analyst summary`
