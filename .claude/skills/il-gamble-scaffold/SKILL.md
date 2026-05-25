---
name: il-gamble-scaffold
description: 客栈传说 · 赌博子游戏脚手架。一键生成新 Gamble 子游戏：Bean（继承 GambleBaseBean）+ UI（继承 UIBaseGamble）+ Item 组件 + 枚举追加清单 + 文本 ID 建议。比 MiniGame 体系轻量，适合骰子/卡牌/掷币等无场景对抗的玩法。新增赌博类玩法时使用。
---

# il-gamble-scaffold

**客栈传说 · 赌博子游戏脚手架 Skill**

项目已有 2 种赌博子游戏（TrickyCup 三杯猜球 / TrickySize 大小猜拳）。本 skill 一次性生成新 Gamble 子游戏的全套结构。

---

## 用法

```
/il-gamble-scaffold <游戏名> [Item组件名...]
```

**示例：**
```
/il-gamble-scaffold Dice Dice Cup
/il-gamble-scaffold Card Card Deck
/il-gamble-scaffold CoinFlip Coin Result
```

---

## 背景知识

### 现有 Gamble 体系

```
[Bean 层]
GambleBaseBean                  (Bean/MVC/Gamble/GambleBaseBean.cs)
  └─ gambleType (GambleTypeEnum)
  └─ gambleStatus (GambleStatusType)
  └─ betForMoney L/M/S + betMaxForMoney L/M/S
  └─ isWin + winRewardRate
  └─ ResetData()/SetIsWin()/SetGambleStatus()/SetBetMax()/GetGambleName()

子类示例：GambleTrickyCupBean / GambleTrickySizeBean

[UI 层]
UIBaseGamble                    (Component/UI/Gamble/UIBaseGamble.cs)
  └─ 通用赌博 UI 基类，含下注/重置/确认按钮处理

子类示例：UIGambleTrickyCup / UIGambleTrickySize

[组件层]
Component/Gamble/<X>/
  └─ Gamble<X>Item.cs 等场景元素
```

### 枚举约定

- `Assets/Scripts/Enums/Gamble/GambleTypeEnum.cs` —— 游戏类型枚举
- `Assets/Scripts/Enums/Gamble/GambleStatusType.cs` —— Prepare/Playing/Settle 等状态
- 文本 ID：`GetTextById(601)` = TrickyCup、`GetTextById(602)` = TrickySize（建议新增按递增）

### 与 MiniGame 体系的区别

| 维度 | MiniGame | Gamble |
|---|---|---|
| Handler | 有，继承 BaseMiniGameHandler | 通常无独立 Handler |
| Builder | 有，加载场景对象 | 无，UI 内自管理 |
| 角色 NPC | 有，多角色 AI 对抗 | 无（玩家 vs 庄家） |
| 场景 Prefab | Builder Prefab | 仅 UI Prefab |
| 经验加成 | 有 | 无 |
| 复杂度 | 高 | 低 |

**判断标准：** 若新玩法需场景对抗/NPC AI，应该用 `/il-minigame-scaffold` 而非本 skill。

---

## 执行步骤

### Step 1 — 解析输入

- 游戏名 `<X>`：PascalCase（如 `Dice`、`CoinFlip`）
- Item 组件名列表 → 决定生成几个 `Gamble<X><Item>Item.cs`

### Step 2 — 生成 Bean

**路径：** `Assets/Scripts/Bean/MVC/Gamble/Gamble<X>Bean.cs`

```csharp
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Gamble<X>Bean : GambleBaseBean
{
    // ─── 游戏专属配置 ───
    // 示例：骰子游戏
    // public int diceCount = 3;
    // public List<int> currentDiceValues = new List<int>();
    // public int targetSum;  // 玩家押注的数值

    // 示例：卡牌
    // public List<int> playerHand = new List<int>();
    // public List<int> dealerHand = new List<int>();

    public Gamble<X>Bean() : base(GambleTypeEnum.<X>)
    {
        // 初始化游戏专属字段
        ResetGameData();
    }

    /// <summary>
    /// 重置游戏数据（每局开始前调用）
    /// </summary>
    public void ResetGameData()
    {
        ResetData();  // 调用父类重置下注与状态
        // TODO: 重置游戏专属字段
    }

    /// <summary>
    /// 摇/抽/掷……（生成本局随机结果）
    /// </summary>
    public void Roll()
    {
        // TODO: 使用 RandomUtil 生成结果，赋值给游戏专属字段
    }

    /// <summary>
    /// 判定胜负（根据玩家押注与本局结果）
    /// </summary>
    public void CheckResult()
    {
        // TODO: 计算 isWin
        // SetIsWin(...);
    }

    /// <summary>
    /// 计算奖励金额（基于下注 × winRewardRate）
    /// </summary>
    public void GetRewardMoney(out int rewardL, out int rewardM, out int rewardS)
    {
        rewardL = (int)(betForMoneyL * winRewardRate);
        rewardM = (int)(betForMoneyM * winRewardRate);
        rewardS = (int)(betForMoneyS * winRewardRate);
    }
}
```

### Step 3 — 生成 UI

**路径：** `Assets/Scripts/Component/UI/Gamble/UIGamble<X>.cs`

```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGamble<X> : UIBaseGamble
{
    // ─── Inspector 绑定 ───
    // public Button btRoll;
    // public Button btReset;
    // public Transform itemContainer;
    // public List<Gamble<X><Item1>Item> listItems;
    // public Text tvResult;

    private Gamble<X>Bean _gambleData;

    public override void Awake()
    {
        base.Awake();
        // if (btRoll != null)  btRoll.onClick.AddListener(OnClickForRoll);
        // if (btReset != null) btReset.onClick.AddListener(OnClickForReset);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitGambleData();
        RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// 初始化游戏数据（每次打开 UI 时调用）
    /// </summary>
    private void InitGambleData()
    {
        if (_gambleData == null)
            _gambleData = new Gamble<X>Bean();
        else
            _gambleData.ResetGameData();

        // 设置下注上限（按玩家当前金钱）
        // GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        // _gambleData.SetBetMax(gameData.moneyL, gameData.moneyM, gameData.moneyS);

        SetGambleData(_gambleData);  // UIBaseGamble 基类方法
    }

    /// <summary>
    /// 刷新 UI（下注金额、状态、玩家钱包）
    /// </summary>
    public void RefreshUI()
    {
        if (_gambleData == null) return;
        // TODO: 刷新具体 UI 元素
    }

    /// <summary>
    /// 点击「掷骰子 / 翻牌 / 揭晓」
    /// </summary>
    private void OnClickForRoll()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);

        // 1. 扣除玩家下注金额
        // GameDataHandler.Instance.manager.SubMoney(
        //     _gambleData.betForMoneyL, _gambleData.betForMoneyM, _gambleData.betForMoneyS);

        // 2. 生成结果
        _gambleData.SetGambleStatus(GambleStatusType.Playing);
        _gambleData.Roll();

        // 3. 播放动画（与 Item 组件交互）
        StartCoroutine(PlayRollAnimation());
    }

    private System.Collections.IEnumerator PlayRollAnimation()
    {
        // TODO: Item 组件动画
        yield return new WaitForSeconds(1.5f);

        // 4. 揭晓
        _gambleData.CheckResult();
        _gambleData.SetGambleStatus(GambleStatusType.Settle);

        // 5. 发奖（赢则按倍率回款）
        if (_gambleData.isWin)
        {
            _gambleData.GetRewardMoney(out int rL, out int rM, out int rS);
            // GameDataHandler.Instance.manager.AddMoney(rL, rM, rS);
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Correct);
        }
        else
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Wrong);
        }

        RefreshUI();
    }

    /// <summary>
    /// 点击「再玩一局」
    /// </summary>
    private void OnClickForReset()
    {
        _gambleData.ResetGameData();
        RefreshUI();
    }
}
```

### Step 4 — 生成 Item 组件

对每个 Item 名，路径：`Assets/Scripts/Component/Gamble/<X>/Gamble<X><Item>Item.cs`

```csharp
using UnityEngine;
using DG.Tweening;

public class Gamble<X><Item>Item : MonoBehaviour
{
    // ─── 视觉元素 ───
    // public SpriteRenderer spriteRenderer;
    // public Animator animator;

    private int _value;

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(int value)
    {
        _value = value;
        // TODO: 根据 value 切换显示
    }

    /// <summary>
    /// 播放动画（被 UI 调用）
    /// </summary>
    public Tween PlayShake()
    {
        return transform.DOShakePosition(0.5f, 0.1f);
    }
}
```

### Step 5 — 输出注册清单

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
  Gamble<X> 子游戏脚手架已生成
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

已创建文件：
  ✅ Bean/MVC/Gamble/Gamble<X>Bean.cs
  ✅ UI/Gamble/UIGamble<X>.cs
  ✅ Gamble/<X>/Gamble<X><Item1>Item.cs
  ✅ Gamble/<X>/Gamble<X><Item2>Item.cs

待手动操作：

  [ ] 1. 在 GambleTypeEnum.cs 追加：
         /il-enum-gen append GambleTypeEnum <X>

  [ ] 2. 若 GambleBaseBean.GetGambleName() 有 switch，添加 case：
            case GambleTypeEnum.<X>:
                gambleName = TextHandler.Instance.GetTextById(<新文本ID>);
                break;
         （已知约定：601=TrickyCup, 602=TrickySize，建议 603 起递增）

  [ ] 3. UIEnum.cs 追加：
         /il-enum-gen append UIEnum GAMBLE_<X大写>

  [ ] 4. 在 excel_ui_text 中追加文本：
         | id  | content_zh   | content_en | content_jp |
         | 603 | <游戏中文名> | <英文名>   | <日文名>   |
         （以及游戏内提示文本）

  [ ] 5. 创建 UI Prefab：UIGamble<X>.prefab，并在 Inspector 中绑定字段：
         - btRoll / btReset / itemContainer / tvResult / listItems

  [ ] 6. 在 Addressables 中注册 UI Prefab：
         Group: Default Local Group
         Address: UI/UIGamble<X>

  [ ] 7. 在 UIHandler（或对应 Manager）中注册 UI 地址：
            RegisterUI(UIEnum.GAMBLE_<X>, "UI/UIGamble<X>");

  [ ] 8. 在 NPC 互动入口（如赌场 NPC 的 Interactive 脚本）添加打开入口：
            UIHandler.Instance.OpenUIAndCloseOther<UIGamble<X>>();

  [ ] 9. 若有 Item 组件需要美术资源，准备 Sprite/AnimatorController 并绑定

  [ ] 10. （可选）在 Steam 成就系统注册"首次胜利"/"连胜 N 次"等成就

后续审计：
  /il-addressable-audit unregistered
  /il-localization-audit coverage
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 注意事项

- **不自动改基类**：`GambleTypeEnum`、`GambleBaseBean` 的修改必须手动（值由 `/il-enum-gen` 控制，避免冲突）。
- **不创建 Prefab**：UI Prefab 必须在 Unity Editor 中创建。
- **下注上限默认按当前金钱**：生成的 UI 模板使用 `GameDataHandler` 取当前金钱设置上限，确保玩家不会下注超过持有数。
- **奖励倍率默认 2x**：`GambleBaseBean.winRewardRate = 2`。若新游戏需要不同倍率（如 3x、4x），在子类构造或 `ResetGameData()` 中调整。
- **随机性测试**：本 skill 生成的 `Roll()` 留空，建议实现后用 `RandomUtil` 并跑统计测试，确认期望值与倍率匹配（避免设定不当让赌场永远亏钱或永远赚钱）。可使用 spawn `il-minigame-balance-analyst` 做平衡分析（虽然偏向 MiniGame，赌博奖励/下注分析同样适用）。
- **关联工具：**
  - 数值平衡：spawn `il-minigame-balance-analyst`（也覆盖 Gamble 奖励倍率分析）
  - 多语言：`/il-localization-audit coverage`
  - 资源审计：`/il-addressable-audit`
- **不要把 Gamble 强行套 MiniGame 体系**：项目的设计选择是 Gamble 更轻量，没有 Handler/Builder/Cpt 层级。若强行套用会增加冗余。
