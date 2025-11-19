# Scene Setup Guide - AR Organ Puzzle Game
# 场景设置指南 - AR器官拼图游戏

This guide provides detailed instructions for setting up the main game scene in Unity.

本指南提供了在Unity中设置主游戏场景的详细说明。

---

## Step-by-Step Scene Setup

### 1. Create New Scene / 创建新场景

```
File → New Scene → Basic (Built-in) → Save as "MainScene.unity"
```

### 2. Setup AR Components / 设置AR组件

#### A. AR Session
```
GameObject → XR → AR Session
```
- This manages the AR lifecycle
- 管理AR生命周期
- No configuration needed
- 无需配置

#### B. XR Origin (AR Session Origin)
```
GameObject → XR → XR Origin
```
- Handles AR camera and tracking
- 处理AR相机和跟踪
- Contains Main Camera (will be AR Camera)
- 包含主相机（将成为AR相机）

#### C. AR Tracked Image Manager
```
Select "XR Origin" → Add Component → AR Tracked Image Manager
```

**Settings / 设置:**
- Serialized Library: Assign your Reference Image Library
  - 序列化库：分配参考图库
- Max Number Of Moving Images: 1
  - 最大移动图像数：1
- Tracked Image Prefab: Leave empty (handled by script)
  - 跟踪图像预制体：留空（由脚本处理）

### 3. Create Game Manager Hierarchy / 创建游戏管理器层级

```
Create Empty GameObject → Name: "GameManager"
```

#### Add Components to GameManager:
```
1. Add Component → Game Manager
2. Add Component → AR Image Tracker
3. Add Component → Organ Manager
```

#### Configure Components:

**AR Image Tracker:**
- Tracked Image Manager: Drag "XR Origin" here
- Torso Model Prefab: Will be created in Step 4

**Organ Manager:**
- Scatter Radius: 0.3 (30cm)
- Scatter Height Offset: 0.2 (20cm)
- Torso Dimensions: (0.3, 0.6, 0.25)

**Game Manager:**
- AR Image Tracker: Auto-assigned
- Organ Manager: Auto-assigned
- UI references: Assign after creating UI

### 4. Create Torso Prefab / 创建胸腹预制体

```
Create Empty GameObject → Name: "TorsoPrefab"
Add Component → Transparent Torso Controller
```

**Settings:**
- Dimensions: (0.3, 0.6, 0.25)
- Transparency: 0.3
- Torso Color: Light blue (R:0.8, G:0.9, B:1.0, A:0.3)

**Save as Prefab:**
```
Drag "TorsoPrefab" to Assets/Prefabs/ folder
Delete from Hierarchy
Assign to ARImageTracker's "Torso Model Prefab" field
```

### 5. Create UI Canvas / 创建UI画布

```
GameObject → UI → Canvas
```

#### Canvas Settings:
- Render Mode: Screen Space - Overlay
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920 x 1080

```
Add Component → Canvas Scaler
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920 x 1080
- Match: 0.5 (balance width/height)
```

### 6. Create UI Elements / 创建UI元素

#### A. Status Text (Top Center)
```
Right-click Canvas → UI → Text - TextMeshPro
Name: "StatusText"
```

**Settings:**
- Position: X:0, Y:450, Z:0
- Size: 600 x 100
- Font Size: 36
- Alignment: Center, Middle
- Text: "Status: Initializing..."
- Color: White

#### B. Timer Text (Top Right)
```
Right-click Canvas → UI → Text - TextMeshPro
Name: "TimerText"
```

**Settings:**
- Position: X:400, Y:450, Z:0
- Size: 400 x 100
- Font Size: 32
- Alignment: Right, Middle
- Text: "Time: 00:00:00"
- Color: Yellow

#### C. Instruction Text (Center)
```
Right-click Canvas → UI → Text - TextMeshPro
Name: "InstructionText"
```

**Settings:**
- Position: X:0, Y:0, Z:0
- Size: 800 x 200
- Font Size: 40
- Alignment: Center, Middle
- Text: "Scan the reference image to begin..."
- Color: White
- Auto Size: Enabled

#### D. Start Button (Bottom Center)
```
Right-click Canvas → UI → Button - TextMeshPro
Name: "StartButton"
```

**Settings:**
- Position: X:0, Y:-400, Z:0
- Size: 300 x 80
- Button Text: "Start Game"
- Font Size: 32
- Color: Green

#### E. Reset Button (Bottom Left)
```
Right-click Canvas → UI → Button - TextMeshPro
Name: "ResetButton"
```

**Settings:**
- Position: X:-300, Y:-400, Z:0
- Size: 200 x 80
- Button Text: "Reset"
- Font Size: 28
- Color: Orange
- Initial State: Inactive

### 7. Connect UI to Game Manager / 连接UI到游戏管理器

Select "GameManager" and assign:

```
Game Manager Component:
├── Status Text → StatusText
├── Timer Text → TimerText
├── Start Button → StartButton
├── Reset Button → ResetButton
└── Instruction Text → InstructionText
```

### 8. Configure Lighting / 配置光照

```
Window → Rendering → Lighting
```

**Recommended Settings:**
- Environment Lighting: Skybox
- Ambient Intensity: 1
- Realtime Global Illumination: Enabled
- Auto Generate: Enabled

### 9. Add Event System / 添加事件系统

Should be auto-created with Canvas. If not:
```
GameObject → UI → Event System
```

### 10. Final Hierarchy / 最终层级结构

```
Scene Hierarchy:
├── AR Session
├── XR Origin
│   ├── Camera Offset
│   └── Main Camera (AR Camera)
├── GameManager
│   ├── Game Manager (Component)
│   ├── AR Image Tracker (Component)
│   └── Organ Manager (Component)
├── Directional Light
├── Canvas
│   ├── StatusText
│   ├── TimerText
│   ├── InstructionText
│   ├── StartButton
│   └── ResetButton
└── EventSystem
```

---

## Testing in Editor / 在编辑器中测试

### Unity Remote (For Quick Testing)
1. Install Unity Remote app on device
2. Connect device via USB
3. Window → Analysis → Unity Remote → Device: Any Android/iOS Device
4. Press Play in Unity Editor

### Full AR Testing
1. Build and deploy to device
2. Use printed reference image
3. Test in good lighting conditions

---

## Verification Checklist / 验证清单

- [ ] AR Session exists in scene
- [ ] XR Origin with Main Camera
- [ ] AR Tracked Image Manager configured
- [ ] Reference Image Library assigned
- [ ] GameManager with all three components
- [ ] Torso Prefab created and assigned
- [ ] All UI elements created and positioned
- [ ] UI references connected to GameManager
- [ ] Event System present
- [ ] Scene saved

---

## Common Issues / 常见问题

### UI not visible
- Check Canvas render mode
- Verify camera is tagged "MainCamera"
- Check UI layer and camera culling mask

### AR not working in build
- Verify XR Plug-in Management enabled
- Check platform-specific settings (ARCore/ARKit)
- Ensure camera permissions in Android Manifest

### Organs not appearing
- Check Torso Prefab is assigned
- Verify Organ Manager is on GameManager
- Check console for errors

---

## Next Steps / 下一步

1. Create Reference Image Library
2. Add reference image (10x10cm)
3. Build to device
4. Test AR tracking
5. Play and enjoy!

For more details, see README.md or README_zh-CN.md
