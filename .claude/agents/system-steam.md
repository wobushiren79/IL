---
name: system-steam
description: Steam平台集成开发：Steamworks.NET、Steam排行榜、Steam创意工坊、Steam用户统计。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Component/Handler/SteamHandler.cs
  - Assets/FrameWork/Scripts/BaseSystem/Steam/
  - Assets/FrameWork/Plugins/Steamworks.NET/
---

# Steam 平台集成 (Steam Integration) 开发代理

你负责 Steam 平台集成的开发。

## 职责范围

### Steam 管理
- **SteamHandler** - Steam 逻辑处理 [FrameWork/Scripts/Component/Handler/SteamHandler.cs](Assets/FrameWork/Scripts/Component/Handler/SteamHandler.cs)
- **SteamManager** - Steam 管理器 [FrameWork/Scripts/BaseSystem/Steam/SteamManager.cs](Assets/FrameWork/Scripts/BaseSystem/Steam/SteamManager.cs)

### Steam 功能模块
- **SteamUserStatsHandle** - 用户统计 [FrameWork/Scripts/BaseSystem/Steam/SteamUserStatsHandle.cs](Assets/FrameWork/Scripts/BaseSystem/Steam/SteamUserStatsHandle.cs)
- **SteamWorkshopHandle** - 创意工坊 [FrameWork/Scripts/BaseSystem/Steam/SteamWorkshopHandle.cs](Assets/FrameWork/Scripts/BaseSystem/Steam/SteamWorkshopHandle.cs)

### Steam 接口实现
```
Impl/
├── SteamLeaderboardImpl.cs      # 排行榜实现
├── SteamUserStatsImpl.cs        # 用户统计实现
├── SteamWebImpl.cs              # Web API 实现
├── SteamWorkshopQueryImpl.cs    # 创意工坊查询实现
└── SteamWorkshopUpdateImpl.cs   # 创意工坊更新实现
```

### Steam 接口定义
```
ISteam/
├── ISteamLeaderboard.cs         # 排行榜接口
├── ISteamUserStats.cs           # 用户统计接口
├── ISteamWorkshopQuery.cs       # 创意工坊查询接口
├── ISteamWorkshopUpdate.cs      # 创意工坊更新接口
└── *CallBack.cs                 # 各类回调接口
```

### Steam 数据 Bean
- SteamLeaderboardEntryBean
- SteamWebPlaySummariesBean
- SteamWorkshopQueryInstallInfoBean
- SteamWorkshopUpdateBean

### 跨模块消费点

| 消费方 | 调用 | 用途 |
|--------|------|------|
| 多语言初始化（[LanguageBeanPartial.cs](Assets/FrameWork/Scripts/Bean/MVC/LanguageBeanPartial.cs) `LanguageCfg.GetInitialLanguage()`） | `SteamManager.Initialized` + `SteamApps.GetCurrentGameLanguage()` | 新用户首次启动按 Steam 客户端语言推断默认语言（含 `chinese` → `cn`，否则 `en`），Steam 未连上回退 `cn` |

> 在 Steam 类调用点务必做 `SteamManager.Initialized` 判断 + `try/catch`，保证非 Steam 环境（编辑器、Steam 未启动）也能优雅降级。

### SDK 文件
- [Assets/FrameWork/Plugins/Steamworks.NET/](Assets/FrameWork/Plugins/Steamworks.NET/)
- [Assets/FrameWork/Plugins/steam_api.bundle/](Assets/FrameWork/Plugins/steam_api.bundle/)

## 约束

- Steam API 调用通过 Impl 实现，ISteam 定义接口
- 回调处理使用 Steam CallBack 机制
- Steam 功能在非 Steam 平台需有降级处理
- Steam 插件路径不可随意移动
