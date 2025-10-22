# Quick Start Guide - AR Organ Puzzle Game
# 快速开始指南 - AR器官拼图游戏

## English Version

### Minimum Requirements
- Unity 6.0 or later
- Android device with ARCore support OR iOS device with ARKit support
- Printed reference image (10cm x 10cm)

### Quick Setup (5 steps)

1. **Open in Unity**
   - Open Unity Hub
   - Add project from `UnityProject` folder
   - Unity will automatically install required packages

2. **Create Reference Image**
   - Create/download a unique 10x10cm image
   - In Unity: Assets → Create → XR → Reference Image Library
   - Add your image, set size to 0.1m x 0.1m

3. **Setup Scene**
   - Create new scene
   - Add: GameObject → XR → AR Session
   - Add: GameObject → XR → AR Session Origin
   - Create empty GameObject "GameManager", add all three components:
     - GameManager
     - ARImageTracker
     - OrganManager

4. **Configure AR**
   - Select AR Session Origin → Add Component → AR Tracked Image Manager
   - Assign Reference Image Library
   - Assign GameManager reference

5. **Build and Run**
   - File → Build Settings → Select platform (Android/iOS)
   - Player Settings → XR Plug-in Management → Enable ARCore/ARKit
   - Build and deploy to device

### Game Flow
```
Scan Reference Image → Transparent Torso Appears →
Press Start → Organs Scatter → Drag to Correct Positions →
Game Complete (Timer Stops)
```

---

## 中文版本

### 最低要求
- Unity 6.0或更高版本
- 支持ARCore的Android设备 或 支持ARKit的iOS设备
- 打印的参考图（10cm x 10cm）

### 快速设置（5步）

1. **在Unity中打开**
   - 打开Unity Hub
   - 从`UnityProject`文件夹添加项目
   - Unity将自动安装所需包

2. **创建参考图**
   - 创建/下载一张独特的10x10厘米图片
   - 在Unity中：Assets → Create → XR → Reference Image Library
   - 添加图片，设置尺寸为0.1m x 0.1m

3. **设置场景**
   - 创建新场景
   - 添加：GameObject → XR → AR Session
   - 添加：GameObject → XR → AR Session Origin
   - 创建空GameObject "GameManager"，添加三个组件：
     - GameManager
     - ARImageTracker
     - OrganManager

4. **配置AR**
   - 选择AR Session Origin → Add Component → AR Tracked Image Manager
   - 分配Reference Image Library
   - 分配GameManager引用

5. **构建并运行**
   - File → Build Settings → 选择平台（Android/iOS）
   - Player Settings → XR Plug-in Management → 启用ARCore/ARKit
   - 构建并部署到设备

### 游戏流程
```
扫描参考图 → 透明胸腹出现 →
按开始 → 器官散开 → 拖到正确位置 →
游戏完成（计时停止）
```

---

## Key Files / 关键文件

| File | Purpose | 用途 |
|------|---------|------|
| `GameManager.cs` | Main game logic | 主游戏逻辑 |
| `ARImageTracker.cs` | AR image tracking | AR图像跟踪 |
| `OrganManager.cs` | Organ management | 器官管理 |
| `OrganDragHandler.cs` | Drag interaction | 拖拽交互 |
| `TransparentTorsoController.cs` | Torso visualization | 胸腹可视化 |
| `OrganData.cs` | Data structures | 数据结构 |

## Default Settings / 默认设置

- **Torso Size / 胸腹尺寸**: 30cm x 60cm x 25cm
- **Placement Threshold / 放置阈值**: 5cm
- **Transparency / 透明度**: 30%
- **Organs / 器官数量**: 9 organs

## Troubleshooting / 故障排除

| Problem | Solution |
|---------|----------|
| AR不工作 / AR not working | 检查设备兼容性和权限 / Check device compatibility and permissions |
| 无法检测图像 / Can't detect image | 确保光线充足，图片清晰 / Ensure good lighting and clear image |
| 器官无法拖动 / Can't drag organs | 确认游戏已开始 / Confirm game has started |
| 性能问题 / Performance issues | 降低图形质量 / Reduce graphics quality |

## Next Steps / 下一步

1. Read full README for detailed instructions
2. Customize organ positions in `OrganManager.cs`
3. Add custom 3D models for organs
4. Design educational content

---

For detailed documentation, see `README.md` or `README_zh-CN.md`
详细文档请参见 `README.md` 或 `README_zh-CN.md`
