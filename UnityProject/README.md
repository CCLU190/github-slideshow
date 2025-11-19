# AR Organ Puzzle Game

An educational AR (Augmented Reality) puzzle game built with Unity 6 that teaches human anatomy through interactive gameplay.

## Game Overview

This AR game allows players to learn about the human body's internal organs by:
1. Scanning a reference image (10x10cm)
2. Viewing a transparent 3D torso model with organs inside
3. Rearranging scattered organs back to their correct positions
4. Competing for the fastest completion time

## Organs Included

The game features 9 major organs:
- **Lungs (肺)** - Left and Right
- **Heart (心)** - Center-left upper chest
- **Liver (肝)** - Right side middle abdomen
- **Stomach (胃)** - Left side upper abdomen
- **Pancreas (胰)** - Center, behind stomach
- **Spleen (脾)** - Left side, behind stomach
- **Small Intestine (小腸)** - Center lower abdomen
- **Large Intestine (大腸)** - Surrounding small intestine
- **Bladder (膀胱)** - Center lowest point

## Technical Specifications

### Dimensions
- **Reference Image**: 10cm x 10cm
- **Torso Model**: 60cm (height) x 30cm (width) x 25cm (depth)
- **Note**: All dimensions are adjustable in the inspector

### AR Requirements
- Unity 6.0 or later
- AR Foundation 5.1.0
- ARCore (Android) or ARKit (iOS)
- Mobile device with AR capabilities

## Project Structure

```
UnityProject/
├── Assets/
│   ├── Scripts/
│   │   ├── ARImageTracker.cs          # Handles AR image tracking
│   │   ├── GameManager.cs             # Main game logic and state management
│   │   ├── OrganData.cs               # Data structure for organs
│   │   ├── OrganManager.cs            # Manages all organs and their positions
│   │   ├── OrganDragHandler.cs        # Handles drag and drop interactions
│   │   └── TransparentTorsoController.cs  # Controls transparent torso model
│   ├── Scenes/
│   │   └── MainScene.unity            # Main game scene (to be created)
│   ├── Prefabs/                       # Game object prefabs
│   └── Materials/                     # Materials and shaders
├── ProjectSettings/                   # Unity project settings
└── Packages/                          # Package dependencies
```

## Setup Instructions

### 1. Open Project in Unity
1. Download and install Unity 6.0 or later from [Unity Hub](https://unity.com/download)
2. Open Unity Hub
3. Click "Add" and select the `UnityProject` folder
4. Open the project

### 2. Install Required Packages
The project uses Unity Package Manager. Required packages should auto-install:
- AR Foundation 5.1.0
- ARCore XR Plugin (for Android)
- Input System 1.7.0
- UI Toolkit 2.0.0

If packages are missing:
1. Window → Package Manager
2. Search for and install the required packages listed above

### 3. Configure AR Settings

#### For Android (ARCore):
1. File → Build Settings → Android
2. Switch platform to Android
3. Player Settings → XR Plug-in Management
4. Enable "ARCore"
5. Set minimum API level to 24 or higher

#### For iOS (ARKit):
1. File → Build Settings → iOS
2. Switch platform to iOS
3. Player Settings → XR Plug-in Management
4. Enable "ARKit"
5. Set deployment target to iOS 12.0 or higher

### 4. Create AR Reference Image
1. Prepare a 10x10cm image to use as the tracking target
2. In Unity: Assets → Create → XR → Reference Image Library
3. Add your reference image to the library
4. Set physical size to 0.1m x 0.1m (10cm x 10cm)

### 5. Setup Main Scene

#### Create AR Session:
1. GameObject → XR → AR Session
2. GameObject → XR → AR Session Origin (or XR Origin)

#### Add Image Tracking:
1. Select AR Session Origin
2. Add Component → AR Tracked Image Manager
3. Assign your Reference Image Library
4. Set "Max Number of Moving Images" to 1

#### Add Game Manager:
1. Create Empty GameObject named "GameManager"
2. Add Component → Game Manager
3. Add Component → AR Image Tracker
4. Add Component → Organ Manager

#### Create UI:
1. Create Canvas (GameObject → UI → Canvas)
2. Set Canvas to Screen Space - Overlay
3. Add UI elements:
   - Text: Status Text (top center)
   - Text: Timer Text (top right)
   - Button: Start Game Button (bottom center)
   - Button: Reset Button (bottom left)
   - Text: Instruction Text (center)

4. Assign UI references in GameManager inspector

### 6. Create Torso Prefab
1. Create Empty GameObject named "TorsoPrefab"
2. Add Component → Transparent Torso Controller
3. Save as prefab in Assets/Prefabs/
4. Assign to ARImageTracker's "Torso Model Prefab" field

## How to Play

### Setup:
1. Print the reference image (10x10cm)
2. Launch the game on an AR-capable device
3. Point camera at the reference image

### Gameplay:
1. Once the image is detected, the transparent torso model appears
2. Press "Start Game" button
3. Organs scatter to random positions around the torso
4. Drag each organ back to its correct position inside the torso
5. When an organ is close enough to its correct position, it snaps into place
6. Complete all organs to finish the game
7. Your completion time is recorded

### Controls:
- **Touch/Click**: Select an organ
- **Drag**: Move the selected organ
- **Release**: Drop the organ

## Customization

### Adjust Dimensions:
- Select the TransparentTorsoController component
- Modify the "Dimensions" values in the inspector
- Default: (0.3, 0.6, 0.25) = 30cm x 60cm x 25cm

### Adjust Transparency:
- Select the TransparentTorsoController component
- Modify the "Transparency" slider (0.0 to 1.0)
- Default: 0.3 (70% transparent)

### Adjust Organ Positions:
- Edit OrganManager.cs
- Modify InitializeOrganData() method
- Change initialPosition values for each organ

### Adjust Placement Threshold:
- Edit OrganData.cs
- Change placementThreshold value
- Default: 0.05 (5cm)

## Development Notes

### Scripts Overview:

1. **ARImageTracker.cs**
   - Detects and tracks the reference image
   - Spawns the torso model when image is found
   - Maintains tracking during gameplay

2. **GameManager.cs**
   - Controls game state and flow
   - Manages timer and UI updates
   - Handles win condition

3. **OrganManager.cs**
   - Creates and initializes all organs
   - Manages organ positions and states
   - Checks placement accuracy

4. **OrganDragHandler.cs**
   - Handles touch/mouse input
   - Implements drag and drop functionality
   - Provides visual feedback

5. **TransparentTorsoController.cs**
   - Creates transparent torso visualization
   - Manages torso appearance
   - Adjustable dimensions

6. **OrganData.cs**
   - Data structure for organ properties
   - Stores position and state information

### Future Enhancements:
- Add 3D organ models (currently using colored spheres)
- Implement sound effects and haptic feedback
- Add educational information about each organ
- Multiple difficulty levels
- Multiplayer mode
- Leaderboard system

## Troubleshooting

### AR Not Working:
- Ensure device supports ARCore (Android) or ARKit (iOS)
- Check that AR permissions are granted
- Verify reference image is properly configured
- Test in good lighting conditions

### Organs Not Responding:
- Check that GameManager is properly initialized
- Verify all script references are assigned
- Ensure colliders are present on organ objects

### Performance Issues:
- Reduce transparency effects
- Simplify 3D models
- Lower shadow quality in project settings

## License

This project is created for educational purposes.

## Credits

- Built with Unity 6
- AR Foundation framework
- Developed for anatomy education

## Contact

For questions or issues, please refer to the project documentation or contact the development team.
