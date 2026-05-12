# new-ui-item

新增一个列表项组件脚本（ScrollGrid 中的 Cell 或单独的 ListItem）。

## 用法

```
/new-ui-item <类名> <数据Bean名> [所属模块]
```

**示例：**
- `/new-ui-item ItemBrewRecipeList BrewRecipeBean Town` — 酿酒配方列表项
- `/new-ui-item ItemWorkerList CharacterBean Town` — 员工列表项
- `/new-ui-item ItemInventorySlot GameItemsBean` — 背包格子

## 执行步骤

参考项目 ListItem 组件的实际模式：

### 文件：`Assets/Scripts/Component/UI/ListItem/<模块>/<类名>.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : BaseUIComponent
{
    // Inspector 绑定字段
    public Image ivIcon;
    public Text tvName;
    // public Text tvDesc;
    // public Button btAction;

    private <Bean名称>Bean _data;

    public override void Awake()
    {
        base.Awake();
        // if (btAction != null) btAction.onClick.AddListener(OnClickForAction);
    }

    /// <summary>
    /// 外部调用：设置数据并刷新显示
    /// </summary>
    public void SetData(<Bean名称>Bean data)
    {
        _data = data;
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (_data == null) return;
        
        if (tvName != null)
            tvName.text = _data.name_language;
        
        // TODO: 加载图标
        // if (ivIcon != null)
        //     <IconManager>.Instance.manager.GetIcon(_data.icon_id, sprite => ivIcon.sprite = sprite);
    }

    // private void OnClickForAction()
    // {
    //     AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
    //     // TODO: 点击回调，通常通过 Action<T> 委托传给父组件
    // }
}
```

### 含点击回调的扩展版（适用于可交互列表）

在 `SetData` 中额外接收 `Action` 参数：

```csharp
private Action<<Bean名称>Bean> _onClickCallback;

public void SetData(<Bean名称>Bean data, Action<<Bean名称>Bean> onClick = null)
{
    _data = data;
    _onClickCallback = onClick;
    // ... 其余同上
}

private void OnClickForAction()
{
    AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
    _onClickCallback?.Invoke(_data);
}
```

### 含选中状态的扩展版（适用于 RadioGroup / 多选）

```csharp
public void SetSelected(bool selected)
{
    // TODO: 切换选中样式（如改变背景色或显示选中标记）
    // objSelected.SetActive(selected);
}
```

## 文件路径规则

| 情况 | 路径 |
|---|---|
| 有明确模块 | `Assets/Scripts/Component/UI/ListItem/<模块>/<类名>.cs` |
| 无模块/通用 | `Assets/Scripts/Component/ListItem/<类名>.cs` |

生成后提示：
- 在 ScrollGrid 的 Cell Prefab 上挂载此组件
- 父组件中通过 `item.SetData(bean)` 或 `item.SetData(bean, onClick)` 调用
