# il-unity-scene

**客栈传说 · Unity 场景管理 Agent**

通过 Unity Editor REST API（unity-skills）管理场景：查询场景状态、查找 GameObject / 组件、分析场景结构、辅助调试。

---

## 用法

```
/il-unity-scene <操作> [参数...]
```

**操作列表：**

| 操作 | 说明 |
|---|---|
| `list` | 列出项目所有场景文件及其路径 |
| `open <场景名>` | 打开指定场景（在 Unity Editor 中） |
| `info` | 查看当前打开场景的基本信息（名称、GameObject 数量、层级结构） |
| `find <名称>` | 在当前场景中查找 GameObject（支持模糊匹配） |
| `hierarchy` | 展示当前场景的 GameObject 层级树（前 3 层） |
| `handlers` | 列出 Handlers 节点下所有挂载的 Handler 脚本 |
| `missing` | 查找当前场景中缺少脚本引用（Missing Script）的 GameObject |
| `check-init <场景名>` | 检查指定场景的初始化脚本中，哪些 Handler 已调用 InitData() |

**示例：**
```
/il-unity-scene list
/il-unity-scene open GameInnScene
/il-unity-scene info
/il-unity-scene find NpcManager
/il-unity-scene hierarchy
/il-unity-scene handlers
/il-unity-scene missing
/il-unity-scene check-init GameInnScene
```

---

## 场景文件速查（项目已知场景）

| 场景名 | 文件路径 | 用途 |
|---|---|---|
| MainScene | Assets/Scenes/MainScene.unity | 主菜单 / 开始界面 |
| LoadingScene | Assets/Scenes/LoadingScene.unity | 加载过渡页 |
| GameTownScene | Assets/Scenes/GameTownScene.unity | 城镇中心枢纽 |
| GameInnScene | Assets/Scenes/GameInnScene.unity | 客栈内部 |
| GameCourtyardScene | Assets/Scenes/GameCourtyardScene.unity | 庭院 / 花园 |
| GameMountainScene | Assets/Scenes/GameMountainScene.unity | 山区探索 |
| GameArenaScene | Assets/Scenes/GameArenaScene.unity | 擂台 / 竞技场 |
| GameInfiniteTowersScene | Assets/Scenes/GameInfiniteTowersScene.unity | 无限塔楼 |
| TestDataScene | Assets/Scenes/TestDataScene.unity | 数据测试用 |

---

## 执行步骤

### 操作 `list`

调用 unity-skills 搜索 `Assets/Scenes/**/*.unity`，列出所有场景文件路径和名称。

### 操作 `open <场景名>`

调用 unity-skills Editor API 打开指定场景（若 Unity 编辑器已打开）。
若场景名不含路径，从上方速查表或文件搜索中匹配完整路径。

### 操作 `info`

调用 unity-skills 获取当前激活场景信息：
- 场景名称与路径
- 根 GameObject 列表
- 总 GameObject 数量
- 是否有未保存更改

### 操作 `find <名称>`

在当前场景层级中搜索名称包含 `<名称>` 的 GameObject，返回：
- 完整路径（如 `Root/Handlers/BreweryHandler`）
- 挂载的组件列表（脚本名）

### 操作 `hierarchy`

展示当前场景根节点及其前 2-3 层子节点，格式：
```
Scene: GameInnScene
├── GameCamera
├── Handlers
│   ├── GameDataHandler
│   ├── InnHandler
│   ├── NpcHandler
│   └── ...
├── Managers
├── UI_Root
│   ├── UIGameMain
│   └── ...
└── ...
```

### 操作 `handlers`

定位 `Handlers` 节点，列出所有子 GameObject 及其挂载的 Handler 脚本类名。
对比 `Assets/Scripts/Component/Handler/` 目录中的所有 Handler 文件，指出：
- 已在场景中注册的 Handler
- 存在于代码中但**未挂载**到场景的 Handler（可能是遗漏）

### 操作 `missing`

扫描当前场景，找出 Missing Script 的 GameObject，按层级路径列出，方便定位清理。

### 操作 `check-init <场景名>`

1. 读取对应的场景初始化脚本（如 `GameSceneInit.cs` / `TownSceneInit.cs`）
2. 列出其中所有调用了 `InitData()` 的 Handler
3. 对比 Handlers 节点中的 Handler 列表，指出：
   - 已在 InitData 中初始化的 Handler ✅
   - 挂载了但**未调用 InitData()**的 Handler ⚠️

---

## 注意事项

- 本 Agent 只做**读取和分析**，不直接修改场景文件（`.unity` 是 YAML 格式，手动编辑风险高）
- 场景修改（如挂载 Handler）应在 Unity Editor 中手动完成
- 若 Unity Editor 未运行，unity-skills REST API 不可用，将改为读取 `.unity` 文件的 YAML 结构进行静态分析
