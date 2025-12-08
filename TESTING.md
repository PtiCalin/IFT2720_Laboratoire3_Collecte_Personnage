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
- [x] This class allows Player to read input from the configured actions

### 3. Customize Input Bindings (Optional)

- [x] Double-click `Assets/InputSystem_Actions.inputactions` to open the Input Actions editor
- [x] Under **Player > Move**, you can see bindings (likely WASD and Arrow keys)
- [x] To change keys: Click on a binding, then modify the **Path** (e.g., change `<Keyboard>/w` to another key)
- [x] For jump: Under **Player > Attack**, modify bindings (default is Space)
- [x] Save the asset after changes
- [x] This allows full customization of controls without code changes

### 4. Set Up Scene Hierarchy

Create a new scene or use an existing one. The Level script will automatically create most elements, but you need to set up the core components.

#### Add Scene

- [x] Create empty GameObject: **GameObject > Create Empty** (name it "Scene")
- [x] Add **Scene** script component (**Component > Scripts > Scene**)

#### Add Level (Core Game Logic)

- [x] Create empty GameObject: **GameObject > Create Empty** (name it "Level")
- [x] Add **Level** script component (**Component > Scripts > Level**)
In Inspector, configure:
- [x] **Ground Material**: Drag a material asset (or leave empty for default white)
- [x] **Wall Material**: Drag a material asset for maze walls
- [x] **Coin Material/Treasure Material**: Drag materials for collectibles
- [x] **Player Prefab**: Drag your player prefab (or leave empty to auto-create a capsule)
- [x] **Coin Prefab/Treasure Prefab**: Drag prefabs (or leave empty for primitives)
- [x] Adjust other settings: maze rows/columns, cell size, player speed/jump force, collectible counts

#### Add Camera (Camera System)

- [x] Create empty GameObject: **GameObject > Create Empty** (name it "CameraRig")
- [x] Add **Camera** component (**Component > Camera**)
- [x] Add **Camera** script component (**Component > Scripts > Camera**) on the same object
- [x] In Inspector, configure Camera:
  - **Start Mode**: Choose **ThirdPerson** (orbit camera) or **BirdsEye** (top-down)
  - **Target**: Leave empty (Level will automatically set this to the player)
  - **Distance**: Adjust camera distance from player (for ThirdPerson mode)
  - **Rotation Speed**: Camera rotation sensitivity
  - **Birds Eye Height**: Height for top-down view
  - Other settings: smoothing, bounds, etc.

#### Optional: Add UI (Score/UI Management)

- [x] Create empty GameObject: **GameObject > Create Empty** (name it "UI")
- [x] Add **UI** script component if you have UI elements for score display
- [x] Configure any UI canvases or text elements for displaying collected items

### 5. Player/Character Element Integration

The Level script automatically creates the player character when the scene starts, but here's how it integrates:

#### Automatic Player Creation (via Level)

- [x] When you press Play, `Level.Start()` calls `CreatePlayer()`
- [x] This creates a GameObject named "Player" (or uses your prefab)
- [x] Adds Rigidbody, CapsuleCollider, and Player components
- [x] Positions player at maze entrance
- [x] Links camera target automatically

#### Manual Player Setup (Alternative)

For manual control:

- [x] Create empty GameObject: **GameObject > Create Empty** (name it "Player")
- [x] Add **Rigidbody** component (**Component > Physics > Rigidbody**)
- [x] Add **Capsule Collider** (**Component > Physics > Capsule Collider**)
- [x] Add **Player** script (**Component > Scripts > Player**)
- [x] In Player Inspector:
  - Set **Move Speed** and **Jump Force**
  - Assign **Character** transform if using a child model
- [x] Position manually or let `Level` handle it

#### Camera Linking to Character

- The Camera automatically finds and follows the player via `FindFirstObjectByType<Camera>()`
- Level calls `cameraRig.SetTarget(player.transform)` and `cameraRig.ConfigureBounds()`
- For ThirdPerson mode: Camera orbits around player with mouse look
- For BirdsEye mode: Camera stays above player with orthographic projection
- Camera bounds are set to match maze size for proper framing

### 6. Collectible Elements Integration

LevelGenerator automatically spawns collectibles:

- [x] **Coins**: Spherical objects with rotation/bobbing animation
- [x] **Treasures**: Cubical objects with higher point value
- [x] Placed randomly in maze cells, avoiding walls and player spawn
- [x] Each has Collectible component with configurable properties
- [x] Trigger colliders for collection detection

### 7. Full Element Integration Steps

1. **Scene Setup**: Add Level and Camera as described
2. **Material Assignment**: Drag materials into LevelGenerator Inspector slots
3. **Prefab Assignment**: Drag player/collectible prefabs if available
4. **Input Configuration**: Customize keys in Input Actions editor
5. **Camera Configuration**: Choose mode and adjust settings in Camera
6. **Play Mode**: Press Play to generate level and test

### 8. Test in Play Mode

- [x] Press **Play** in Unity Editor
- [x] The Level will automatically:
  - [x] Create ground plane with assigned material
  - [x] Generate procedural maze with walls
  - [x] Spawn player at entrance with physics components
  - [x] Place collectibles randomly throughout maze
  - [x] Position camera and link it to player
  - [x] Configure camera bounds to fit maze
**Controls should now work:**
  - [x] Use configured keys (WASD/Arrows by default) to move player
  - [x] Space (or configured jump key) to jump when grounded
  - [x] Camera follows player automatically (orbit in ThirdPerson, top-down in BirdsEye)
  - [x] Collect items by walking into them
**Verify Integration:**
  - Player moves relative to camera direction
  - Camera stays within maze bounds
  - Collectibles disappear on contact and update score (if UI present)
  - Maze has clear entrance/exit paths

### 9. Troubleshooting

- **No movement:** Check that `InputSystem_Actions.cs` was generated and `Player` has no compilation errors
- **Camera not following:** Ensure `Camera` script is in scene and `Level` finds it (check Console for warnings)
- **Materials not applying:** Assign them manually in `Level` Inspector; ensure materials exist in Assets
- **Input not customizable:** Make sure you saved changes in Input Actions editor; restart Unity if needed
- **Player not spawning:** Check `Level` Inspector for prefab assignments; ensure no compilation errors
- **Collectibles not spawning:** Verify maze generation completes; check Console for placement warnings
- **Physics issues:** Ensure Ground has "Ground" tag; check Rigidbody settings on player
- **Input System errors:** Confirm package is installed and Active Input Handling is set correctly

### 10. Advanced Testing

- **Change Camera Modes:** Press Tab (configurable) to switch between ThirdPerson and BirdsEye
- **Test Different Mazes:** Modify rows/columns in Level and restart scene
- **Custom Controls:** Add new bindings in Input Actions (e.g., gamepad support)
- **Performance:** Monitor frame rate with larger mazes; adjust cell size if needed
- **Edge Cases:** Test with no materials assigned, no prefabs, extreme maze sizes

The `Player` now uses the Input System, allowing full control customization in the editor without code changes. The `Level` handles all procedural generation and integration automatically. If you encounter issues, check the Unity Console for detailed error messages and ensure all scripts compile successfully.
