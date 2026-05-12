# il-ui-view-gen

**客栈传说 · UI View 生成器 Agent**

生成完整的 UI 界面脚本，支持四种视图类型：full（带货币栏完整界面）、base（普通界面）、dialog（对话框）、popup（弹窗）。

---

## 用法

```
/il-ui-view-gen <类名> <类型> [模块] [--with-list]
```

**类型：**
| 参数 | 继承基类 | 适用场景 |
|---|---|---|
| `full` | `UIBaseOne` | 带返回按钮 + 货币栏的主界面，如酿酒界面、背包界面 |
| `base` | `BaseUIComponent` | 无货币栏的普通界面或子面板 |
| `dialog` | `DialogView` | 弹出确认/输入对话框，带提交/取消按钮 |
| `popup` | `BaseUIComponent` | 放 `View/Popup/` 目录下的浮层弹窗 |

**选项：**
- `[模块]`：所属游戏模块（Game / Town / MiniGame / Main 等），决定文件路径
- `[--with-list]`：同时生成 ScrollGrid 列表绑定骨架

**示例：**
```
/il-ui-view-gen UIBrewery full Game --with-list
/il-ui-view-gen UIFestivalDetail base Town
/il-ui-view-gen CharacterDetailDialog dialog
/il-ui-view-gen BrewResultPopup popup
/il-ui-view-gen UIMiniGameAccount base MiniGame --with-list
```

---

## 文件路径规则

| 类型 | 路径 |
|---|---|
| `full` / `base` | `Assets/Scripts/Component/UI/<模块>/<类名>.cs` |
| `dialog` | `Assets/Scripts/Component/UI/View/Dialog/<类名>.cs` |
| `popup` | `Assets/Scripts/Component/UI/View/Popup/<类名>.cs` |

---

## 执行步骤

### Step 1 — 解析类型与路径

根据类型参数确定文件路径与基类，若无 `[模块]` 参数且类型为 `full`/`base`，默认放 `Game` 目录。

### Step 2 — 生成视图文件

#### 类型 `full` — 带货币栏完整界面

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : UIBaseOne
{
    // ─── Inspector 绑定字段 ───
    // public Button bt<功能>;
    // public Text tv<信息名>;
    // public Image iv<图标名>;
    // public GameObject obj<容器名>;

    // --with-list 时额外生成：
    // public ScrollRect scrollRect<列表名>;
    // private List<<Bean>Bean> _list<Bean>;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        // if (bt<功能> != null) bt<功能>.onClick.AddListener(OnClickFor<功能>);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void RefreshUI()
    {
        // TODO: 从 <系统名>Handler.Instance.manager 获取数据并刷新
        // --with-list 时额外生成：
        // RefreshList();
    }

    // --with-list 时额外生成：
    // private void RefreshList()
    // {
    //     _list<Bean> = <系统名>Handler.Instance.manager.GetAll<Bean>List();
    //     // TODO: 绑定到 ScrollGrid
    // }

    // private void OnClickFor<功能>()
    // {
    //     AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
    //     // TODO: 处理点击逻辑
    // }

    public override void OnClickForBack()
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }
}
```

#### 类型 `base` — 普通界面 / 子面板

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : BaseUIComponent
{
    // ─── Inspector 绑定字段 ───
    // public Button bt<功能>;
    // public Text tv<信息名>;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void RefreshUI()
    {
        // TODO: 刷新界面数据
    }
}
```

#### 类型 `dialog` — 对话框

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : DialogView
{
    // ─── Inspector 绑定字段（除继承的提交/取消按钮外）───
    // public Text tvTitle;
    // public Text tvContent;
    // public InputField inputField;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshData();
    }

    public void RefreshData()
    {
        if (dialogBean == null) return;
        // TODO: 使用 dialogBean 中的数据刷新 UI
        // if (tvTitle != null) tvTitle.text = dialogBean.title;
        // if (tvContent != null) tvContent.text = dialogBean.content;
    }

    protected override void OnClickSubmit()
    {
        base.OnClickSubmit();
        // TODO: 提交逻辑
    }

    protected override void OnClickCancel()
    {
        base.OnClickCancel();
    }
}
```

#### 类型 `popup` — 弹窗

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : BaseUIComponent
{
    // ─── Inspector 绑定字段 ───
    // public Button btClose;
    // public Button btConfirm;
    // public Text tvMessage;

    private System.Action _onConfirmCallback;

    public override void Awake()
    {
        base.Awake();
        // if (btClose != null)   btClose.onClick.AddListener(CloseUI);
        // if (btConfirm != null) btConfirm.onClick.AddListener(OnClickForConfirm);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void SetData(/* 数据参数 */, System.Action onConfirm = null)
    {
        _onConfirmCallback = onConfirm;
        RefreshUI();
    }

    private void RefreshUI()
    {
        // TODO: 刷新显示
    }

    private void OnClickForConfirm()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        _onConfirmCallback?.Invoke();
        CloseUI();
    }
}
```

### Step 3 — 输出摘要

```
已生成：
  <文件完整路径>/<类名>.cs

类型：<full / base / dialog / popup>
基类：<UIBaseOne / BaseUIComponent / DialogView>

手动注册（必须完成）：
  1. 在枚举文件中添加对应 ID：
       - full / base → UIEnum.cs：  <类名大写>  = <值>
       - dialog      → DialogEnum.cs：<类名大写> = <值>
       - popup       → PopupEnum.cs： <类名大写> = <值>

  2. 在 UIHandler（或对应 Handler）中注册面板的 Addressable 路径
     （参考同类已有注册项的格式）

  3. 在 Inspector 中拖拽绑定所有 public 字段

  4. full 类型：确认 OnClickForBack() 中的跳转目标界面正确
```
