# TESTING.md - Functional Testing Setup Guide

This document provides a complete step-by-step guide to set up and test the Collecte Personnage game in Unity Editor. The game features procedural maze generation, player movement with customizable controls, collectible items, and dynamic camera following.

## Prerequisites

- Unity 2022.3 or later
- Input System package installed
- Basic understanding of Unity Editor interface

## Step-by-Step Setup Guide for Functional Testing

### 1. Ensure Input System Package is Active

- [x] Open Unity Editor
- [x] Go to **Edit > Project Settings > Player**
- [x] Under **Other Settings > Configuration**, set **Active Input Handling** to **"Input System Package (New)"**
- [x] If not installed, go to **Window > Package Manager**, search for "Input System", and install it

### 2. Generate Input System C# Class (Important!)

- [x] In the **Project** window, find `Assets/InputSystem_Actions.inputactions`
- [x] Right-click it and select **"Generate C# Class"**
- [x] This creates `InputSystem_Actions.cs` in your Scripts folder (it should appear automatically)
- [x] This class allows PlayerController to read input from the configured actions

### 3. Customize Input Bindings (Optional)

- [x] Double-click `Assets/InputSystem_Actions.inputactions` to open the Input Actions editor
- [x] Under **Player > Move**, you can see bindings (likely WASD and Arrow keys)
- [x] To change keys: Click on a binding, then modify the **Path** (e.g., change `<Keyboard>/w` to another key)
- [x] For jump: Under **Player > Attack**, modify bindings (default is Space)
- [x] Save the asset after changes
- [x] This allows full customization of controls without code changes

### 4. Set Up Scene Hierarchy

Create a new scene or use an existing one. The LevelGenerator script will automatically create most elements, but you need to set up the core components.

#### Add Level Generator (Core Game Logic)

- Create empty GameObject: **GameObject > Create Empty** (name it "LevelGenerator")
- Add **LevelGenerator** script component (**Component > Scripts > LevelGenerator**)
- In Inspector, configure:
  - **Ground Material**: Drag a material asset (or leave empty for default white)
  - **Wall Material**: Drag a material asset for maze walls
  - **Coin Material/Treasure Material**: Drag materials for collectibles
  - **Player Prefab**: Drag your player prefab (or leave empty to auto-create a capsule)
  - **Coin Prefab/Treasure Prefab**: Drag prefabs (or leave empty for primitives)
  - Adjust other settings: maze rows/columns, cell size, player speed/jump force, collectible counts

#### Add Camera Rig (Camera System)

- Create empty GameObject: **GameObject > Create Empty** (name it "CameraRig")
- Add **Camera** component (**Component > Camera**)
- Add **CameraRigController** script component (**Component > Scripts > CameraRigController**)
- In Inspector, configure CameraRigController:
  - **Start Mode**: Choose **ThirdPerson** (orbit camera) or **BirdsEye** (top-down)
  - **Target**: Leave empty (LevelGenerator will automatically set this to the player)
  - **Distance**: Adjust camera distance from player (for ThirdPerson mode)
  - **Rotation Speed**: Camera rotation sensitivity
  - **Birds Eye Height**: Height for top-down view
  - Other settings: smoothing, bounds, etc.

#### Optional: Add GameManager (Score/UI Management)

- Create empty GameObject: **GameObject > Create Empty** (name it "GameManager")
- Add **GameManager** script component if you have UI elements for score display
- Configure any UI canvases or text elements for displaying collected items

### 5. Player/Character Element Integration

The LevelGenerator automatically creates the player character when the scene starts, but here's how it integrates:

#### Automatic Player Creation (via LevelGenerator)

- When you press Play, LevelGenerator.Start() calls CreatePlayer()
- This creates a GameObject named "Player" (or uses your prefab)
- Adds Rigidbody, CapsuleCollider, and PlayerController components
- Positions player at maze entrance
- Links camera target automatically

#### Manual Player Setup (Alternative)

For manual control:

- Create empty GameObject: **GameObject > Create Empty** (name it "Player")
- Add **Rigidbody** component (**Component > Physics > Rigidbody**)
- Add **Capsule Collider** (**Component > Physics > Capsule Collider**)
- Add **PlayerController** script (**Component > Scripts > PlayerController**)
- In PlayerController Inspector:
  - Set **Move Speed** and **Jump Force**
  - Assign **Character** transform if using a child model
- Position manually or let LevelGenerator handle it

#### Camera Linking to Character

- The CameraRigController automatically finds and follows the player via `FindFirstObjectByType<CameraRigController>()`
- LevelGenerator calls `cameraRig.SetTarget(player.transform)` and `cameraRig.ConfigureBounds()`
- For ThirdPerson mode: Camera orbits around player with mouse look
- For BirdsEye mode: Camera stays above player with orthographic projection
- Camera bounds are set to match maze size for proper framing

### 6. Collectible Elements Integration

LevelGenerator automatically spawns collectibles:

- **Coins**: Spherical objects with rotation/bobbing animation
- **Treasures**: Cubical objects with higher point value
- Placed randomly in maze cells, avoiding walls and player spawn
- Each has Collectible component with configurable properties
- Trigger colliders for collection detection

### 7. Full Element Integration Steps

1. **Scene Setup**: Add LevelGenerator and CameraRig as described
2. **Material Assignment**: Drag materials into LevelGenerator Inspector slots
3. **Prefab Assignment**: Drag player/collectible prefabs if available
4. **Input Configuration**: Customize keys in Input Actions editor
5. **Camera Configuration**: Choose mode and adjust settings in CameraRigController
6. **Play Mode**: Press Play to generate level and test

### 8. Test in Play Mode

- Press **Play** in Unity Editor
- The LevelGenerator will automatically:
  - Create ground plane with assigned material
  - Generate procedural maze with walls
  - Spawn player at entrance with physics components
  - Place collectibles randomly throughout maze
  - Position camera and link it to player
  - Configure camera bounds to fit maze
- **Controls should now work:**
  - Use configured keys (WASD/Arrows by default) to move player
  - Space (or configured jump key) to jump when grounded
  - Camera follows player automatically (orbit in ThirdPerson, top-down in BirdsEye)
  - Collect items by walking into them
- **Verify Integration:**
  - Player moves relative to camera direction
  - Camera stays within maze bounds
  - Collectibles disappear on contact and update score (if GameManager present)
  - Maze has clear entrance/exit paths

### 9. Troubleshooting

- **No movement:** Check that `InputSystem_Actions.cs` was generated and PlayerController has no compilation errors
- **Camera not following:** Ensure CameraRigController is in scene and LevelGenerator finds it (check Console for "CameraRig not found" warnings)
- **Materials not applying:** Assign them manually in LevelGenerator Inspector; ensure materials exist in Assets
- **Input not customizable:** Make sure you saved changes in Input Actions editor; restart Unity if needed
- **Player not spawning:** Check LevelGenerator Inspector for prefab assignments; ensure no compilation errors
- **Collectibles not spawning:** Verify maze generation completes; check Console for placement warnings
- **Physics issues:** Ensure Ground has "Ground" tag; check Rigidbody settings on player
- **Input System errors:** Confirm package is installed and Active Input Handling is set correctly

### 10. Advanced Testing

- **Change Camera Modes:** Press Tab (configurable) to switch between ThirdPerson and BirdsEye
- **Test Different Mazes:** Modify rows/columns in LevelGenerator and restart scene
- **Custom Controls:** Add new bindings in Input Actions (e.g., gamepad support)
- **Performance:** Monitor frame rate with larger mazes; adjust cell size if needed
- **Edge Cases:** Test with no materials assigned, no prefabs, extreme maze sizes

The PlayerController now uses the Input System, allowing full control customization in the editor without code changes. The LevelGenerator handles all procedural generation and integration automatically. If you encounter issues, check the Unity Console for detailed error messages and ensure all scripts compile successfully.
