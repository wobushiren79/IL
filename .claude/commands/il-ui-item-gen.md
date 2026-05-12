# il-ui-item-gen

**客栈传说 · UI ListItem 生成器 Agent**

生成 ScrollGrid 列表项组件脚本，支持只读展示、可点击回调、可选中三种交互模式。

---

## 用法

```
/il-ui-item-gen <类名> <Bean名> [模块] [--click] [--selectable]
```

**选项：**
- `[模块]`：文件所在子目录（Town / Game / MiniGame / Dialog / Popup 等）
- `[--click]`：生成点击回调版本（`Action<T>` 委托传递给父组件）
- `[--selectable]`：生成选中状态版本（适用于单选 / 多选列表）

**示例：**
```
/il-ui-item-gen ItemBrewRecipeList BrewRecipeBean Town --click
/il-ui-item-gen ItemWorkerList CharacterBean Town --click --selectable
/il-ui-item-gen ItemInventorySlot GameItemsBean --click
/il-ui-item-gen ItemAchievement AchievementInfoBean Game
/il-ui-item-gen ItemMenuInfo MenuInfoBean Town --click --selectable
```

---

## 文件路径规则

| 情况 | 路径 |
|---|---|
| 有模块参数 | `Assets/Scripts/Component/UI/ListItem/<模块>/<类名>.cs` |
| 无模块 / 通用 | `Assets/Scripts/Component/ListItem/<类名>.cs` |

---

## 执行步骤

### Step 1 — 分析 Bean 字段

读取 `Assets/Scripts/Bean/` 目录下对应的 `<Bean名>Bean.cs`，找出：
- 是否有 `name_language` 属性（多语言名称，直接显示）
- 是否有 `icon_id` / `sprite_id` 类字段（需要异步加载图标）
- 是否有 `desc_language` / `content_language`（描述文本）
- 是否有 `count` / `amount` / `level` / `exp`（数值字段）

若 Bean 文件不存在，直接使用标准骨架，在 `RefreshUI()` 中用 `// TODO` 注释占位。

### Step 2 — 生成 ListItem 文件

#### 基础版（只读展示）

```csharp
using UnityEngine;
using UnityEngine.UI;

public class <类名> : BaseUIComponent
{
    // ─── Inspector 绑定字段 ───
    public Image ivIcon;
    public Text tvName;
    // public Text tvDesc;
    // public Text tvCount;

    private <Bean名>Bean _data;

    public override void Awake()
    {
        base.Awake();
    }

    public void SetData(<Bean名>Bean data)
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

        // 名称（若 Bean 有 name_language 属性）
        if (tvName != null)
            tvName.text = _data.name_language;

        // 图标（若 Bean 有 icon_id 字段）
        // if (ivIcon != null && _data.icon_id > 0)
        //     <IconLoader>.Instance.LoadIcon(_data.icon_id, sprite => ivIcon.sprite = sprite);
    }
}
```

#### 含点击回调版（`--click`）

在基础版基础上，`SetData()` 增加回调参数，生成点击处理方法：

```csharp
    // 额外字段
    private System.Action<<Bean名>Bean> _onClickCallback;

    public void SetData(<Bean名>Bean data, System.Action<<Bean名>Bean> onClick = null)
    {
        _data = data;
        _onClickCallback = onClick;
        if (data == null) { gameObject.SetActive(false); return; }
        gameObject.SetActive(true);
        RefreshUI();
    }

    // 在 Awake() 中为整个 Item 或按钮绑定点击：
    // GetComponent<Button>().onClick.AddListener(OnClickItem);
    // 或对指定按钮绑定

    private void OnClickItem()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        _onClickCallback?.Invoke(_data);
    }
```

#### 含选中状态版（`--selectable`）

在点击版基础上，追加选中状态切换方法：

```csharp
    // 额外字段
    private bool _isSelected;

    public void SetSelected(bool selected)
    {
        _isSelected = selected;
        // TODO: 切换选中样式，如：
        // objSelected.SetActive(selected);
        // imgBackground.color = selected ? selectedColor : normalColor;
    }
```

### Step 3 — 输出摘要

```
已生成：
  <文件完整路径>/<类名>.cs

Bean：<Bean名>Bean
模式：<只读 / 点击回调 / 点击+选中>
识别到的 Bean 字段：
  ✅ name_language → tvName.text 已接入
  ⚠️ icon_id 存在 → ivIcon 加载逻辑已注释，请补充实际 IconLoader 调用
  ⚠️ desc_language 存在 → tvDesc 注释已添加，请取消注释并绑定

使用方式（父组件中）：
  // 单个绑定
  item.SetData(bean);

  // 带点击回调（--click 版）
  item.SetData(bean, (data) => { /* 处理选中逻辑 */ });

  // 带选中状态（--selectable 版）
  item.SetData(bean, onSelect);
  item.SetSelected(true);

后续步骤：
  1. 在 ScrollGrid 的 Cell Prefab 上挂载此脚本
  2. 在 Inspector 中拖拽绑定 Image / Text 组件引用
  3. 若有图标加载需求，补充实际的图标加载管理器调用
```
