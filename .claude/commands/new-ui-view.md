# new-ui-view

新增一个完整的 UI 界面脚本（继承项目基类，含完整生命周期）。

## 用法

```
/new-ui-view <类名> <类型> [所属模块]
```

**类型：**
- `full` — 带货币栏的完整界面（继承 `UIBaseOne`）
- `base` — 普通界面（继承 `BaseUIComponent`）
- `dialog` — 对话框（继承 `BaseDialogView` 或 `DialogView`）
- `popup` — 弹窗（继承 `BaseUIComponent`，放 `UI/View/Popup/`）

**示例：**
- `/new-ui-view UIBrewery full Game` — 酿酒界面，带货币栏
- `/new-ui-view CharacterDetailDialog dialog` — 角色详情对话框
- `/new-ui-view BrewResultPopup popup` — 酿造结果弹窗

## 执行步骤

### 类型 `full`（继承 UIBaseOne）

参考 `UIBaseRank.cs` 风格：

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : UIBaseOne
{
    // Inspector 绑定字段（公有，无属性）
    public Button bt<功能>;
    // public Text tv<信息>;
    // public GameObject obj<容器>;

    public override void Awake()
    {
        base.Awake();
        // 子组件初始化
    }

    public override void Start()
    {
        base.Start();
        if (bt<功能> != null) bt<功能>.onClick.AddListener(OnClickFor<功能>);
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

    private void OnClickFor<功能>()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        // TODO: 处理点击逻辑
    }

    public override void OnClickForBack()
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }
}
```

### 类型 `base`（继承 BaseUIComponent）

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : BaseUIComponent
{
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

### 类型 `dialog`

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : DialogView
{
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
    }

    protected override void OnClickSubmit()
    {
        base.OnClickSubmit();
    }

    protected override void OnClickCancel()
    {
        base.OnClickCancel();
    }
}
```

## 文件路径规则

| 类型 | 路径 |
|---|---|
| full / base | `Assets/Scripts/Component/UI/<模块>/<类名>.cs` |
| dialog | `Assets/Scripts/Component/UI/View/Dialog/<类名>.cs` |
| popup | `Assets/Scripts/Component/UI/View/Popup/<类名>.cs` |

生成后提示：
- 在 `UIEnum` / `DialogEnum` / `PopupEnum` 中添加对应枚举项
- 在 `UIHandler` 中注册（如需）
- 按钮通过 Inspector 拖拽绑定，框架按 Tag 命名自动绑定
