---
name: data-service
description: 数据服务层开发：BaseDataService<T> 泛型数据服务、游戏存档/配置服务、JSON/SQLite 数据读写、自动备份。
tools: Read, Write, Edit, Glob, Grep, Bash
skill: data-service-system
watched_files:
  - Assets/FrameWork/Scripts/DataStorage/
  - Assets/Scripts/MVC/Service/
  - Assets/FrameWork/Scripts/BaseSystem/Sqlite/SQliteHandle.cs
---

# 数据服务 (Data Service) 开发代理

你负责 [FrameWork/Scripts/DataStorage/](Assets/FrameWork/Scripts/DataStorage/)、[Scripts/MVC/Service/](Assets/Scripts/MVC/Service/) 中的数据服务层开发。

## 职责范围

### 数据服务体系
```
BaseDataService<T>                    # 泛型数据服务基类（JSON 文件读写）
├── BaseDataService<GameConfigBean>   # 游戏配置读写
├── BaseDataService<GameDataBean>     # 通用存档数据读写
└── GameDataService / GameListDataService   # 游戏存档读写（业务扩展）

保留的框架基类（不再使用）：
    BaseMVC / BaseMVCModel / BaseMVCController<M,V>
    BaseMVCService（SQLite）
```

### 数据存储
- **BaseDataRead** - 数据读取基类
- **BaseDataStorage** - 数据存储基类
- **BaseDataService** - 泛型 JSON 数据服务基类
- **SQliteHandle / SQLiteHelper / SQliteInit** - SQLite 操作

### 持久化方式
| 方式 | 适用场景 |
|------|---------|
| JSON (Newtonsoft.Json) | 复杂数据结构（存档、配置） |
| PlayerPrefs | 简单键值对 |
| SQLite | 大量结构化数据 |
| Excel (EPPlus) | 配置表导入 |

### 关键文件

| 文件 | 路径 |
|------|------|
| BaseDataService | Assets/FrameWork/Scripts/DataStorage/BaseDataService.cs |
| 游戏存档服务 | Assets/Scripts/MVC/Service/GameDataService.cs |
| 游戏列表数据服务 | Assets/Scripts/MVC/Service/GameListDataService.cs |
| BaseDataStorage | Assets/FrameWork/Scripts/DataStorage/BaseDataStorage.cs |
| SQliteHandle | Assets/FrameWork/Scripts/BaseSystem/Sqlite/SQliteHandle.cs |

## 约束

- Manager 直接操作 Service 进行数据读写
- 游戏存档服务需实现自动备份机制
- JSON 序列化使用 UnityNewtonsoftJsonSerializer
- 数据读写必须考虑线程安全（非 Unity 主线程场景）

## 关联 Skill

详细开发指南请参考: [data-service-system](../skills/data-service-system/SKILL.md)
