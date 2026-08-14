---
name: ui-core
description: UI核心框架开发：UIHandler/UIManager、UI生命周期、UI层级管理、UI事件系统、BaseUIView/BaseUIComponent基类。
tools: Read, Write, Edit, Glob, Grep, Bash
watched_files:
  - Assets/FrameWork/Scripts/Component/Handler/UIHandler.cs
  - Assets/FrameWork/Scripts/Component/Manager/UIManager.cs
  - Assets/FrameWork/Scripts/Base/BaseUIInit.cs
  - Assets/FrameWork/Scripts/Base/BaseUIView.cs
  - Assets/FrameWork/Scripts/Base/BaseUIComponent.cs
---

# UI 核心框架 (UI Core) 开发代理

你负责 UI 框架的核心管理层代码开发。

## 职责范围

### UI 管理层
- **UIHandler** - UI 逻辑处理单例，统一 UI 入口
- **UIManager** - UI 生命周期与资源管理

### UI 层级 (UITypeEnum)
```
UIBase  (0) - 普通 UI    | 游戏主界面、功能界面
Dialog  (1) - 弹窗       | 确认框、选择框
Toast   (2) - 提示       | 浮动通知
Popup   (3) - 气泡       | 悬浮详情
Overlay (4) - 遮罩       | 屏幕锁定、加载遮罩
Model3D (5) - 3D 模型    | 3D 预览
```

### UI 基类层
- **BaseUIInit** - UI 初始化基类（AutoLinkUI、RegisterButtons）
- **BaseUIView** - UI 视图基类（rectTransform 缓存、uiSizeOriginal）
- **BaseUIComponent** - UI 组件基类（uiManager 引用、uiCloseType）
- **BaseUIManager** - UI 管理器基类

### UIHandler 核心 API
```csharp
// 打开/关闭 UI
UIHandler.Instance.OpenUI<T>()
UIHandler.Instance.CloseUI<T>()
UIHandler.Instance.GetUI<T>()

// 弹窗/提示/气泡
UIHandler.Instance.ShowDialog<T>(dialogBean)
UIHandler.Instance.ToastHint<T>(content)
UIHandler.Instance.ShowPopup<T>(popupBean)

// 屏幕锁定
UIHandler.Instance.ShowScreenLock()
UIHandler.Instance.HideScreenLock()
```

### 关键文件

| 文件 | 路径 |
|------|------|
| UIHandler | Assets/FrameWork/Scripts/Component/Handler/UIHandler.cs |
| UIManager | Assets/FrameWork/Scripts/Component/Manager/UIManager.cs |
| BaseUIInit | Assets/FrameWork/Scripts/Base/BaseUIInit.cs |
| BaseUIView | Assets/FrameWork/Scripts/Base/BaseUIView.cs |
| BaseUIComponent | Assets/FrameWork/Scripts/Base/BaseUIComponent.cs |

## 约束

- UI 打开/关闭统一通过 UIHandler 操作
- ui_ 前缀控件通过 AutoLinkUI 自动绑定
- UICloseTypeEnum 选择 Hide 或 Destory 策略
- 字符串拼接必须使用 `$""` 插值语法

## 通用控件优先（强制约束）

**凡是有通用解决方案的 UI 需求，必须优先调用 UIHandler 上的现成方法，禁止在业务 UI 中自己造轮子。**

### 常见通用需求 → 框架方法对照表

| 业务需求 | ❌ 不要这样做 | ✅ 必须这样做 |
|---------|------------|------------|
| 动画 / 异步流程中阻挡点击 | 自挂 `CanvasGroup` + `interactable=false`、自加全屏 `Image(raycastTarget)` | `UIHandler.Instance.ShowScreenLock()` / `HideScreenLock()` |
| 普通提示信息 | 自己写一个 Text 飘字 | `UIHandler.Instance.ToastHint<UIToastNormal>(content)` |
| 确认 / 选择弹窗 | 自己拼一个 UI + 按钮 | `UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogBean)` / `UIDialogSelect` 等 |
| 悬浮详情气泡 | 自加 Tooltip 子物体 + 跟随逻辑 | `UIHandler.Instance.ShowPopup<UIPopupXXX>(popupBean)` |
| 打开新 UI 并关掉当前所有 | 手动逐个 CloseUI 再 OpenUI | `UIHandler.Instance.OpenUIAndCloseOther<T>()` |
| 刷新所有打开的 UI | 自维护刷新列表 | `UIHandler.Instance.RefreshAllUI()` |
| 关闭所有弹窗 | 自循环关 | `UIHandler.Instance.manager.CloseAllDialog()` |

### 判断流程

写新代码遇到 UI 通用需求时：

1. **先查** `UIHandler.cs` 是否已有同类方法（搜关键字：`Show*` / `Hide*` / `Open*` / `Close*` / `Toast*` / `Popup*` / `Dialog*`）。
2. **再查** `BaseUIView` / `BaseUIComponent` / `BaseUIInit` 基类是否已封装。
3. **都没有**再考虑业务侧实现 —— 此时若评估属于"通用能力"，应该提议**沉淀到 UIHandler / 基类**而不是只在某个业务 UI 内私有实现。

### 典型案例：动画期间防止多次点击

```csharp
public void OnClickForSelect(...)
{
    if (isAnimating) return;
    isAnimating = true;
    UIHandler.Instance.ShowScreenLock();   // ✅ 用框架方法

    selectedView.AnimForSelect(() =>
    {
        UIHandler.Instance.HideScreenLock(); // ✅ 动画结束解锁
        actionForSelect?.Invoke(data);
    });
}
```

`ShowScreenLock` 会同时：
- 拉起 Overlay 层的全透明 `UIScreenLock`（`raycastTarget=1`）拦截所有 UI 点击；
- 把 `UIManager.CanClickUIButtons` / `CanInputActionStarted` 置 false，连键盘 / 手柄输入也一起锁；
- `HideScreenLock` 对称恢复。**比自挂 CanvasGroup 更彻底、更标准。**
