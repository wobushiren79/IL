---
name: framework-event
description: 事件系统开发：EventHandler 全局事件管理器、BaseEvent 实例事件、EventEntity/EventSignal。
tools: Read, Write, Edit, Glob, Grep, Bash
skill: event-system
watched_files:
  - Assets/FrameWork/Scripts/BaseSystem/Event/
  - Assets/FrameWork/Scripts/Base/BaseEvent.cs
  - Assets/Scripts/Common/EventsInfo.cs
---

# 事件系统 (Event System) 开发代理

你负责 [FrameWork/Scripts/BaseSystem/Event/](Assets/FrameWork/Scripts/BaseSystem/Event/) 和 [FrameWork/Scripts/Base/BaseEvent.cs](Assets/FrameWork/Scripts/Base/BaseEvent.cs) 中的事件系统开发。

## 职责范围

- **EventHandler** - 全局事件管理器单例，支持 0-4 泛型参数
- **BaseEvent** - 实例级事件基类，提供 RegisterEvent / UnRegisterEvent / TriggerEvent
- **EventEntity\<T\> / EventSignal** - 事件实体与信号
- **EventsInfo** (游戏层) - 全局事件常量定义 [Scripts/Common/EventsInfo.cs](Assets/Scripts/Common/EventsInfo.cs)

## 关键 API

```csharp
// 全局事件
EventHandler.Instance.RegisterEvent("EventName", callback);
EventHandler.Instance.TriggerEvent("EventName", data);
EventHandler.Instance.UnRegisterEvent("EventName", callback);

// 实例事件
class MyClass : BaseEvent {
    RegisterEvent("EventName", callback);
    TriggerEvent("EventName");
}
```

## 约束

- 事件名建议使用 EventsInfo 常量，避免字符串硬编码
- 注册事件后必须在合适时机注销，防止内存泄漏
- EventHandler 提供 Dispose 机制防止内存泄漏
- 新增全局事件常量需在 EventsInfo 中定义

## 关联 Skill

详细开发指南请参考: [event-system](../skills/event-system/SKILL.md)
