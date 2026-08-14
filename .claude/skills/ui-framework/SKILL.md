---
name: ui-framework
description: UI框架开发指南（共享框架层）。使用此SKILL当需要创建或修改UI界面、弹窗(Dialog)、气泡(Popup)、提示(Toast)等，包括UI脚本创建、UI管理、UI事件处理等。
watched_files:
  - Assets/FrameWork/Scripts/Base/BaseUIInit.cs
  - Assets/FrameWork/Scripts/Base/BaseUIView.cs
  - Assets/FrameWork/Scripts/Base/BaseUIComponent.cs
  - Assets/FrameWork/Scripts/Component/UI/DialogView.cs
  - Assets/FrameWork/Scripts/Component/UI/PopupShowView.cs
  - Assets/FrameWork/Scripts/Component/UI/ToastView.cs
  - Assets/FrameWork/Scripts/Component/Manager/UIManager.cs
  - Assets/FrameWork/Scripts/Component/Handler/UIHandler.cs
  - Assets/FrameWork/Scripts/Enums/BaseGameEnum.cs
  - Assets/FrameWork/Editor/Base/Window/BaseUICreateWindow.cs
  - Assets/FrameWork/Editor/ScriptsTemplates/
  - Assets/Scripts/Component/UI/
  - Assets/Resources/UI/
---

# UI框架开发指南

> 📌 **关联文档**: `.claude/md/project.md` - 项目 UI 结构与各业务系统说明
>
> ⚠️ **更新提示**: 新增 UI 后请同步更新本文档与项目业务文档 `.claude/md/project.md`

---

## 核心概念

### UI类型体系

```
UITypeEnum
├── UIBase = 0       // 普通UI（游戏主界面、功能界面）
├── Dialog = 1       // 弹窗（确认框、选择框）
├── Toast = 2        // 提示（浮动通知）
├── Popup = 3        // 气泡（悬浮详情）
├── Overlay = 4      // 遮罩（屏幕锁定、加载遮罩）
└── Model3D = 5      // 3D模型展示
```

### UI继承体系

```
BaseUIInit                              // UI初始化基类
│   - AutoLinkUI() 自动绑定UI控件
│   - RegisterButtons() 注册按钮点击
│   - OpenUI() / CloseUI() 打开/关闭
│   - 事件系统：RegisterEvent / TriggerEvent
│
├── BaseUIComponent                     // UI组件基类
│   │   - uiManager 引用
│   │   - uiCloseType 关闭类型 (Hide/Destory)
│   │
│   └── UIViewXXX (各类UI组件)
│
└── BaseUIView                          // UI视图基类
    │   - rectTransform 缓存
    │   - uiSizeOriginal 原始大小
    │
    ├── 普通UI (UIMain, UIMiniGame, UITown...)
    ├── DialogView                      // 弹窗基类
    ├── PopupShowView                   // 气泡基类
    └── ToastView                       // 提示基类
```

### 命名前缀规范

| 类型 | 前缀 | 示例 |
|------|------|------|
| 普通UI | `UI` | `UIMain`, `UIMiniGame`, `UITown` |
| 弹窗 | `UIDialog` | `UIDialogAchievement`, `UIDialogFindCharacter` |
| 气泡 | `UIPopup` | `UIPopupItemInfo` |
| 提示 | `UIToast` | `UIToastNormal` |
| 组件 | `UIView` | `UIViewListItem` |

---

## 创建新UI

### 方式一：使用编辑器工具（推荐）

1. **打开UI创建工具**
   - 菜单：`Custom/工具弹窗/UI脚本创建`
   - 或点击Toolbar上的`UI脚本创建`按钮

2. **选择脚本类型**
   - `UI 脚本` - 普通UI（继承BaseUIView）
   - `View 脚本` - UI组件（继承BaseUIComponent）
   - `Dialog 脚本` - 弹窗（继承DialogView）
   - `Popup 脚本` - 气泡（继承PopupShowView）
   - `Toast 脚本` - 提示（继承ToastView）
   - `Common 脚本` - 通用组件

3. **设置模块名和路径**
   - 模块名：用于生成子目录
   - 生成路径：脚本保存位置

4. **点击生成**
   - 自动生成脚本文件
   - 自动添加组件到Prefab

### 方式二：手动创建

#### 1. 创建普通UI

```csharp
// Assets/Scripts/Component/UI/Game/UIExample.cs（IL：按业务分子目录 Main/Town/Game/MiniGame/Mountain/Gamble 等）
using UnityEngine;
using UnityEngine.UI;

public class UIExample : BaseUIView
{
    // UI控件（命名规范：ui_xxx）
    public Button ui_Submit;
    public Text ui_Title;
    public Image ui_Icon;
    
    // 数据
    private ExampleData data;

    public override void Awake()
    {
        base.Awake();
        // 初始化代码
    }

    public override void OpenUI()
    {
        base.OpenUI();
        // 打开时逻辑
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        // 刷新UI数据
        if (data == null) return;
        ui_Title.text = data.title;
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Submit)
        {
            OnClickSubmit();
        }
    }

    public void SetData(ExampleData data)
    {
        this.data = data;
        RefreshUI();
    }

    private void OnClickSubmit()
    {
        // 按钮点击逻辑
        UIHandler.Instance.CloseUI<UIExample>();
    }
}
```

#### 2. 创建弹窗

```csharp
// Assets/Scripts/Component/UI/View/Dialog/UIDialogExample.cs（IL：弹窗脚本在 View/Dialog/）
using UnityEngine;

public class UIDialogExample : DialogView
{
    // 自定义控件
    public InputField ui_Input;
    
    public override void SetData(DialogBean dialogData)
    {
        base.SetData(dialogData);
        // 自定义数据设置
        if (dialogData.customData is string text)
        {
            ui_Input.text = text;
        }
    }

    public override void SubmitOnClick()
    {
        // 获取输入值
        string inputValue = ui_Input.text;
        dialogData.actionSubmit?.Invoke(this, dialogData);
        if (dialogData.isDestroySubmit)
            DestroyDialog();
    }
}
```

#### 3. 创建气泡

```csharp
// Assets/Scripts/Component/UI/View/Popup/UIPopupExample.cs（IL：气泡脚本在 View/Popup/）
using UnityEngine;

public class UIPopupExample : PopupShowView
{
    public Text ui_Content;
    
    public override void SetData(PopupBean popupData)
    {
        base.SetData(popupData);
        ui_Content.text = popupData.content;
    }
}
```

#### 4. 创建View组件

```csharp
// Assets/Scripts/Component/UI/Child/UIViewExampleItem.cs（IL：列表项组件在 Child/ 或 ListItem/）
using UnityEngine;
using UnityEngine.UI;

public class UIViewExampleItem : BaseUIComponent
{
    public Image ui_Icon;
    public Text ui_Name;
    
    private ItemData itemData;

    public void SetData(ItemData data)
    {
        this.itemData = data;
        ui_Name.text = data.name;
        ui_Icon.sprite = data.icon;
    }
}
```

### 3. Prefab放置规范（IL：UI 预制体在 Resources/UI 下）

| UI类型 | Prefab路径 | 命名规范 |
|--------|-----------|---------|
| 普通UI | `Assets/Resources/UI/` | `UIExample` |
| 弹窗 | `Assets/Resources/UI/Dialog/` | `UIDialogExample` |
| 气泡 | `Assets/Resources/UI/Popup/` | `UIPopupExample` |
| 提示 | `Assets/Resources/UI/Toast/` | `UIToastExample` |
| 列表项 | `Assets/Resources/UI/Item/` | `UIViewExampleItem` |

---

## UI管理器使用

### UIHandler 核心API

```csharp
// 单例访问
UIHandler.Instance

// ==================== 打开/关闭UI ====================

// 打开UI
UIHandler.Instance.OpenUI<UIExample>();

// 打开UI并设置数据
UIHandler.Instance.OpenUI<UIExample>((ui) =>
{
    ui.SetData(data);
});

// 打开UI并指定层级
UIHandler.Instance.OpenUI<UIExample>(layer: 1);

// 关闭UI
UIHandler.Instance.CloseUI<UIExample>();

// 关闭指定名称的UI
UIHandler.Instance.CloseUI("UIExample");

// 关闭所有UI
UIHandler.Instance.CloseAllUI();

// 打开UI并关闭其他
UIHandler.Instance.OpenUIAndCloseOther<UIExample>();

// ==================== 获取UI ====================

// 获取UI实例
UIExample ui = UIHandler.Instance.GetUI<UIExample>();

// 获取当前打开的UI
BaseUIComponent openUI = UIHandler.Instance.GetOpenUI();

// 获取当前打开UI的名称
string uiName = UIHandler.Instance.GetOpenUIName();

// ==================== 刷新UI ====================

// 刷新指定UI
UIHandler.Instance.RefreshUI<UIExample>();

// 刷新当前打开的UI
UIHandler.Instance.RefreshUI();

// 刷新所有UI
UIHandler.Instance.RefreshAllUI();

// ==================== 弹窗 ====================

// 显示普通弹窗
DialogBean dialogData = new DialogBean("标题", "内容", "确定", "取消");
dialogData.actionSubmit = (dialog, data) => { /* 确认操作 */ };
UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);

// 显示选择弹窗
DialogBean selectData = new DialogBean(DialogEnum.Select, "选择", "选项1|选项2|选项3");
selectData.actionSubmit = (dialog, data) => 
{
    int selectedIndex = dialogData.selectIndex;
};
UIHandler.Instance.ShowDialog<UIDialogSelect>(selectData);

// 关闭所有弹窗
UIHandler.Instance.manager.CloseAllDialog();

// ==================== Toast提示 ====================

// 普通提示
UIHandler.Instance.ToastHint<UIToastNormal>("保存成功！");

// 带图标的提示
UIHandler.Instance.ToastHint<UIToastNormal>(iconSprite, "获得道具");

// 指定显示时间
UIHandler.Instance.ToastHint<UIToastNormal>("提示内容", 3f);

// ==================== 气泡Popup ====================

// 显示气泡
PopupBean popupData = new PopupBean(PopupEnum.ItemInfo, targetTransform);
UIHandler.Instance.ShowPopup<UIPopupItemInfo>(popupData);

// 隐藏气泡
UIHandler.Instance.HidePopup(PopupEnum.ItemInfo);

// ==================== 屏幕锁定 ====================

// 锁定屏幕（禁止点击）
UIHandler.Instance.ShowScreenLock();

// 解锁屏幕
UIHandler.Instance.HideScreenLock();
```

---

## ⚠️ 通用控件优先原则（强制约束）

**凡是有通用解决方案的 UI 需求，必须优先调用 UIHandler 上的现成方法，禁止在业务 UI 中自己造轮子。**

业务侧每次写"动画期间挂个 CanvasGroup 拦点击"、"自己拼一个确认框"、"自加 Toast Text"这类代码前，**先翻 UIHandler.cs 找现成方法**。

### 常见通用需求 → 框架方法对照表

| 业务需求 | ❌ 不要这样做 | ✅ 必须这样做 |
|---------|------------|------------|
| 动画 / 异步流程中阻挡点击 | 自挂 `CanvasGroup` + `interactable=false`、自加全屏 `Image(raycastTarget)`、自维护 isAnimating + 关闭按钮 interactable | `UIHandler.Instance.ShowScreenLock()` / `HideScreenLock()` |
| 普通提示信息 | 自写飘字、自做 Tween 隐藏 | `UIHandler.Instance.ToastHint<UIToastNormal>(content)` |
| 确认 / 选择 / 输入弹窗 | 自拼 UI + 按钮回调 | `UIHandler.Instance.ShowDialog<UIDialogNormal/UIDialogSelect/...>(dialogBean)` |
| 悬浮详情气泡 | 自加 Tooltip 子物体 + 跟随逻辑 | `UIHandler.Instance.ShowPopup<UIPopupXXX>(popupBean)` |
| 打开新 UI 并关掉当前所有 | 手动逐个 CloseUI 再 OpenUI | `UIHandler.Instance.OpenUIAndCloseOther<T>()` |
| 刷新所有打开 UI | 自维护刷新列表 | `UIHandler.Instance.RefreshAllUI()` |
| 关闭所有弹窗 | 自循环关 | `UIHandler.Instance.manager.CloseAllDialog()` |

### 判断流程

新代码遇到 UI 通用需求时：

1. **先查** `UIHandler.cs` 是否已有同类方法（关键字：`Show*` / `Hide*` / `Open*` / `Close*` / `Toast*` / `Popup*` / `Dialog*` / `ScreenLock`）。
2. **再查** `BaseUIView` / `BaseUIComponent` / `BaseUIInit` / `BaseUIManager` 基类是否已封装。
3. **都没有**再考虑业务侧实现 —— 若评估为"通用能力"，应**沉淀到 UIHandler / 基类**而非在业务 UI 内私有实现，并同步更新本文档的对照表。

### 典型案例：动画期间防止多次点击

❌ **错误做法**（自维护遮罩，散落各处难维护）：

```csharp
private CanvasGroup canvasGroup;

public void OnClickForSelect(...)
{
    if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    canvasGroup.interactable = false;
    selectedView.AnimForSelect(() =>
    {
        canvasGroup.interactable = true;
        actionForSelect?.Invoke(data);
    });
}
```

✅ **正确做法**（走框架统一通道）：

```csharp
public void OnClickForSelect(...)
{
    if (isAnimating) return;
    isAnimating = true;
    UIHandler.Instance.ShowScreenLock();

    selectedView.AnimForSelect(() =>
    {
        UIHandler.Instance.HideScreenLock();
        actionForSelect?.Invoke(data);
    });
}
```

`ShowScreenLock` 会同时：
- 拉起 Overlay 层的全透明 `UIScreenLock`（`raycastTarget=1`）拦截所有 UI 点击；
- 把 `UIManager.CanClickUIButtons` / `CanInputActionStarted` 置 false，键盘 / 手柄输入一起锁；
- `HideScreenLock` 对称恢复。**比自挂 CanvasGroup 更彻底、可在跨 UI 流程中复用。**

> 配套保留 `bool isAnimating` 做"同帧重入"双保险（ShowScreenLock 是异步 OpenUI，本帧内仍可能被点到第二次）。

---

## UI生命周期与事件

### 生命周期方法

```csharp
public class UIExample : BaseUIView
{
    public override void Awake()
    {
        base.Awake();
        // 初始化：自动绑定UI控件、注册按钮
    }

    public override void OnEnable()
    {
        base.OnEnable();
        // UI启用时：注册输入事件
    }

    public override void OpenUI()
    {
        base.OpenUI();
        // 打开UI：显示、刷新、播放动画
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        // 刷新UI数据
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        // 处理按钮点击
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType, CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);
        // 处理快捷键输入
    }

    public override void OnDisable()
    {
        base.OnDisable();
        // UI禁用时
    }

    public override void CloseUI()
    {
        base.CloseUI();
        // 关闭UI：隐藏、注销事件
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        // 销毁时：清理资源
    }
}
```

### UI事件系统

```csharp
// 注册事件
RegisterEvent(EventsInfo.XXX, OnEventTriggered);
RegisterEvent<int>(EventsInfo.XXX, OnEventWithParam);
RegisterEvent<int, string>(EventsInfo.XXX, OnEventWithParams);

// 触发事件
TriggerEvent(EventsInfo.XXX);
TriggerEvent(EventsInfo.XXX, data);

// 注销事件（通常不需要手动调用，CloseUI时自动注销）
UnRegisterAllEvent();
```

---

## 常用代码模板

### 带列表的UI

```csharp
public class UIExampleList : BaseUIView
{
    public ScrollGridVertical ui_List;
    public GameObject pf_Item;
    
    private List<Data> dataList;

    public override void OpenUI()
    {
        base.OpenUI();
        InitList();
    }

    private void InitList()
    {
        ui_List.SetData(dataList.Count, (index, objItem) =>
        {
            UIViewExampleItem item = objItem.GetComponent<UIViewExampleItem>();
            item.SetData(dataList[index]);
        });
    }
}
```

### 带Tab切换的UI

```csharp
public class UIExampleTab : BaseUIView
{
    public Button ui_Tab1;
    public Button ui_Tab2;
    public GameObject ui_Content1;
    public GameObject ui_Content2;
    
    private int currentTab = 0;

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Tab1) SwitchTab(0);
        if (viewButton == ui_Tab2) SwitchTab(1);
    }

    private void SwitchTab(int tabIndex)
    {
        currentTab = tabIndex;
        ui_Content1.SetActive(tabIndex == 0);
        ui_Content2.SetActive(tabIndex == 1);
    }
}
```

### 带确认关闭的UI

```csharp
public class UIExample : BaseUIView
{
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton.name == "ui_Close")
        {
            ShowCloseConfirm();
        }
    }

    private void ShowCloseConfirm()
    {
        DialogBean dialog = new DialogBean("确认", "是否保存更改？", "保存", "不保存");
        dialog.actionSubmit = (d, data) => 
        {
            SaveAndClose();
        };
        dialog.actionCancel = (d, data) =>
        {
            UIHandler.Instance.CloseUI<UIExample>();
        };
        UIHandler.Instance.ShowDialog<UIDialogNormal>(dialog);
    }
}
```

### 多语言文本绑定

```csharp
public class UIExample : BaseUIView
{
    // 使用UITextLanguageView组件绑定文本key
    public UITextLanguageView ui_TitleText;
    
    public override void Awake()
    {
        base.Awake();
        // 或在代码中设置
        ui_TitleText.SetTextKey("UI_Example_Title");
    }
}
```

---

## 更新UI框架文档

### 何时更新

新增UI后必须同时更新：
1. **SKILL.md** - 添加代码模板和使用示例（如需要）
2. **.claude/md/project.md** - 添加业务 UI 清单条目

### 更新清单

- [ ] 在对应UI类型分类下添加新UI信息
- [ ] 更新目录结构
- [ ] 更新最后更新时间
- [ ] 更新更新记录表

---

## 文件位置速查

| 功能 | 文件路径 |
|------|----------|
| UI基类 | `Assets/FrameWork/Scripts/Base/BaseUIInit.cs` |
| UI视图基类 | `Assets/FrameWork/Scripts/Base/BaseUIView.cs` |
| UI组件基类 | `Assets/FrameWork/Scripts/Base/BaseUIComponent.cs` |
| 弹窗基类 | `Assets/FrameWork/Scripts/Component/UI/DialogView.cs` |
| 气泡基类 | `Assets/FrameWork/Scripts/Component/UI/PopupShowView.cs` |
| 提示基类 | `Assets/FrameWork/Scripts/Component/UI/ToastView.cs` |
| UI管理器 | `Assets/FrameWork/Scripts/Component/Manager/UIManager.cs` |
| UI处理器 | `Assets/FrameWork/Scripts/Component/Handler/UIHandler.cs` |
| UI类型枚举 | `Assets/FrameWork/Scripts/Enums/BaseGameEnum.cs` |
| UI编辑器 | `Assets/FrameWork/Editor/Base/Window/BaseUICreateWindow.cs` |
| UI脚本模板 | `Assets/FrameWork/Editor/ScriptsTemplates/` |
| 业务UI脚本目录 | `Assets/Scripts/Component/UI/`（Main/Town/Game/MiniGame/Mountain/Gamble/View/Child） |
| 弹窗/气泡脚本目录 | `Assets/Scripts/Component/UI/View/Dialog/`、`View/Popup/` |
| Prefab目录 | `Assets/Resources/UI/`（Dialog/Popup/Toast/Item/Show/Other） |
| 完整UI文档 | `.claude/md/project.md` |

---

## 相关事件

IL 项目 UI 事件通过 `EventHandler` 广播，事件常量在 `Assets/Scripts/Common/EventsInfo.cs`（`public static readonly string`）与 `Assets/Scripts/Enums/Base/MsgEnum` 中定义。事件名以实际代码为准，本 SKILL 示例仅作机制演示：

```csharp
// 小游戏类事件示例（IL EventsInfo 实际常量）
EventsInfo.MiniGame_GamePreCountDownStart     // 小游戏倒计时开始
EventsInfo.MiniGame_EventForOnClickClose      // 点击退出小游戏
EventsInfo.MiniGameCooking_CookingSettle      // 斗厨阶段结算
EventsInfo.MiniGameCooking_CookingSettlementClose // 结算界面关闭
```

---

## 业务 UI 速记

业务 UI 的详细清单与各系统专属约定以项目业务文档 `.claude/md/project.md` 为准，新增业务 UI 时按需在此补充速记。UI 脚本按业务分子目录存放：`Main/`（主界面）、`Town/`（城镇）、`Game/`（游戏内 HUD）、`MiniGame/`（小游戏）、`Mountain/`（山地探索）、`Gamble/`（赌博）、`View/Dialog/`（弹窗）、`View/Popup/`（气泡）、`Child/`（子组件）、`ListItem/`（列表项）。

---

*SKILL结束 - 完整 UI 清单请参考 [.claude/md/project.md](../../.claude/md/project.md)*
