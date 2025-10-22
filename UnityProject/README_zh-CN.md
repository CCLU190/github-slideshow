# AR器官拼图游戏

一个使用Unity 6开发的教育类AR（增强现实）拼图游戏，通过互动游戏方式教授人体解剖学知识。

## 游戏概述

这款AR游戏让玩家通过以下方式学习人体内部器官：
1. 扫描参考图（10x10厘米）
2. 查看包含器官的透明3D胸腹部模型
3. 将散落的器官重新排列回正确位置
4. 竞争最快完成时间

## 包含的器官

游戏包含9个主要器官：
- **肺** - 左肺和右肺
- **心** - 中心偏左上胸部
- **肝** - 右侧中腹部
- **胃** - 左侧上腹部
- **胰** - 中心，胃后方
- **脾** - 左侧，胃后方
- **小肠** - 中心下腹部
- **大肠** - 环绕小肠
- **膀胱** - 中心最低点

## 技术规格

### 尺寸
- **参考图**: 10厘米 x 10厘米
- **胸腹部模型**: 60厘米（高）x 30厘米（宽）x 25厘米（深）
- **注意**: 所有尺寸都可以在检查器中调整

### AR要求
- Unity 6.0 或更高版本
- AR Foundation 5.1.0
- ARCore（Android）或 ARKit（iOS）
- 支持AR功能的移动设备

## 项目结构

```
UnityProject/
├── Assets/
│   ├── Scripts/
│   │   ├── ARImageTracker.cs          # 处理AR图像跟踪
│   │   ├── GameManager.cs             # 主游戏逻辑和状态管理
│   │   ├── OrganData.cs               # 器官数据结构
│   │   ├── OrganManager.cs            # 管理所有器官及其位置
│   │   ├── OrganDragHandler.cs        # 处理拖放交互
│   │   └── TransparentTorsoController.cs  # 控制透明胸腹模型
│   ├── Scenes/
│   │   └── MainScene.unity            # 主游戏场景（待创建）
│   ├── Prefabs/                       # 游戏对象预制体
│   └── Materials/                     # 材质和着色器
├── ProjectSettings/                   # Unity项目设置
└── Packages/                          # 包依赖项
```

## 设置说明

### 1. 在Unity中打开项目
1. 从 [Unity Hub](https://unity.com/download) 下载并安装Unity 6.0或更高版本
2. 打开Unity Hub
3. 点击"添加"并选择 `UnityProject` 文件夹
4. 打开项目

### 2. 安装所需包
项目使用Unity Package Manager。所需包应自动安装：
- AR Foundation 5.1.0
- ARCore XR Plugin（用于Android）
- Input System 1.7.0
- UI Toolkit 2.0.0

如果包缺失：
1. Window → Package Manager
2. 搜索并安装上述所需包

### 3. 配置AR设置

#### 对于Android（ARCore）：
1. File → Build Settings → Android
2. 切换平台到Android
3. Player Settings → XR Plug-in Management
4. 启用"ARCore"
5. 设置最小API级别为24或更高

#### 对于iOS（ARKit）：
1. File → Build Settings → iOS
2. 切换平台到iOS
3. Player Settings → XR Plug-in Management
4. 启用"ARKit"
5. 设置部署目标为iOS 12.0或更高

### 4. 创建AR参考图
1. 准备一张10x10厘米的图片作为跟踪目标
2. 在Unity中：Assets → Create → XR → Reference Image Library
3. 将参考图添加到库中
4. 设置物理尺寸为0.1m x 0.1m（10cm x 10cm）

### 5. 设置主场景

#### 创建AR会话：
1. GameObject → XR → AR Session
2. GameObject → XR → AR Session Origin（或XR Origin）

#### 添加图像跟踪：
1. 选择AR Session Origin
2. Add Component → AR Tracked Image Manager
3. 分配参考图库
4. 设置"Max Number of Moving Images"为1

#### 添加游戏管理器：
1. 创建空GameObject，命名为"GameManager"
2. Add Component → Game Manager
3. Add Component → AR Image Tracker
4. Add Component → Organ Manager

#### 创建UI：
1. 创建Canvas（GameObject → UI → Canvas）
2. 设置Canvas为Screen Space - Overlay
3. 添加UI元素：
   - Text：状态文本（顶部中心）
   - Text：计时器文本（顶部右侧）
   - Button：开始游戏按钮（底部中心）
   - Button：重置按钮（底部左侧）
   - Text：说明文本（中心）

4. 在GameManager检查器中分配UI引用

### 6. 创建胸腹预制体
1. 创建空GameObject，命名为"TorsoPrefab"
2. Add Component → Transparent Torso Controller
3. 保存为预制体到Assets/Prefabs/
4. 分配到ARImageTracker的"Torso Model Prefab"字段

## 游戏玩法

### 设置：
1. 打印参考图（10x10厘米）
2. 在支持AR的设备上启动游戏
3. 将相机对准参考图

### 游戏流程：
1. 检测到图像后，透明胸腹模型出现
2. 按"开始游戏"按钮
3. 器官散落到胸腹周围的随机位置
4. 将每个器官拖回到正确位置
5. 当器官足够接近正确位置时，会自动吸附
6. 完成所有器官以完成游戏
7. 记录完成时间

### 控制：
- **触摸/点击**：选择器官
- **拖动**：移动选定的器官
- **松开**：放下器官

## 自定义

### 调整尺寸：
- 选择TransparentTorsoController组件
- 在检查器中修改"Dimensions"值
- 默认：(0.3, 0.6, 0.25) = 30cm x 60cm x 25cm

### 调整透明度：
- 选择TransparentTorsoController组件
- 修改"Transparency"滑块（0.0到1.0）
- 默认：0.3（70%透明）

### 调整器官位置：
- 编辑OrganManager.cs
- 修改InitializeOrganData()方法
- 更改每个器官的initialPosition值

### 调整放置阈值：
- 编辑OrganData.cs
- 更改placementThreshold值
- 默认：0.05（5厘米）

## 开发说明

### 脚本概览：

1. **ARImageTracker.cs**
   - 检测并跟踪参考图
   - 找到图像时生成胸腹模型
   - 在游戏过程中保持跟踪

2. **GameManager.cs**
   - 控制游戏状态和流程
   - 管理计时器和UI更新
   - 处理胜利条件

3. **OrganManager.cs**
   - 创建并初始化所有器官
   - 管理器官位置和状态
   - 检查放置准确性

4. **OrganDragHandler.cs**
   - 处理触摸/鼠标输入
   - 实现拖放功能
   - 提供视觉反馈

5. **TransparentTorsoController.cs**
   - 创建透明胸腹可视化
   - 管理胸腹外观
   - 可调整尺寸

6. **OrganData.cs**
   - 器官属性数据结构
   - 存储位置和状态信息

### 未来增强功能：
- 添加3D器官模型（目前使用彩色球体）
- 实现音效和触觉反馈
- 添加每个器官的教育信息
- 多个难度级别
- 多人游戏模式
- 排行榜系统

## 故障排除

### AR无法工作：
- 确保设备支持ARCore（Android）或ARKit（iOS）
- 检查AR权限已授予
- 验证参考图配置正确
- 在良好光照条件下测试

### 器官无响应：
- 检查GameManager是否正确初始化
- 验证所有脚本引用已分配
- 确保器官对象上有碰撞器

### 性能问题：
- 减少透明效果
- 简化3D模型
- 在项目设置中降低阴影质量

## 许可证

此项目为教育目的而创建。

## 致谢

- 使用Unity 6构建
- AR Foundation框架
- 为解剖学教育开发

## 联系方式

如有问题或疑问，请参阅项目文档或联系开发团队。
