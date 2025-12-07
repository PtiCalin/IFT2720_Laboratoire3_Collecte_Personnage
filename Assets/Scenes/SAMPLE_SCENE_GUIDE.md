# Sample 3D Scene - Game Collecte Personnage

## Overview
This is a complete 3D game scene for the "Jeu Collecte" laboratory assignment. The scene features a player character navigating through a maze to collect coins and treasures.

## Scene Hierarchy

```
Scene Root
├── Ground (Plane)
│   └── Tag: "Ground"
│
├── Maze (Empty GameObject)
│   ├── Wall_North
│   ├── Wall_South
│   ├── Wall_East
│   ├── Wall_West
│   ├── Wall_Interior_1
│   ├── Wall_Interior_2
│   └── ... (additional interior walls)
│
├── Collectibles (Empty GameObject)
│   ├── Coin_1 (Sphere - Yellow)
│   ├── Coin_2 (Sphere - Yellow)
│   ├── Coin_3 (Sphere - Yellow)
│   ├── Treasure_1 (Cube - Orange/Gold)
│   ├── Treasure_2 (Cube - Orange/Gold)
│   └── ... (additional coins/treasures)
│
├── Main Camera
│   └── Position: (0, 20, -15)
│   └── Rotation: (45°, 0°, 0°)
│
├── Directional Light (Sun)
│   └── Rotation: (50°, -30°, 0°)
│
└── GameManager (Empty GameObject)
    └── Handles scoring and game state
```

## Game Elements

### 1. Ground (Plane)
- **Type**: Primitive Plane
- **Position**: (0, 0, 0)
- **Scale**: (5, 5, 5)
- **Tag**: "Ground"
- **Purpose**: Main walkable surface for the player
- **Components**: 
  - Mesh Renderer
  - Mesh Collider
  - Box Collider (for ground detection)

### 2. Player Character
- **Type**: Capsule
- **Position**: (0, 2, 0)
- **Tag**: "Player"
- **Purpose**: Avatar controlled by the player using keyboard input
- **Components**:
  - Mesh Renderer
  - Capsule Collider
  - Rigidbody (with gravity enabled)
  - PlayerController Script
- **Controls**:
  - Arrow Keys / A-D: Move left/right
  - Space: Jump

### 3. Maze Walls
- **Type**: Multiple Cubes arranged in a maze pattern
- **Components**:
  - Mesh Renderer
  - Box Collider (NOT a trigger)
- **Purpose**: Creates obstacles that player must navigate around
- **Exterior Walls**: Define boundaries of the play area
- **Interior Walls**: Create the maze challenge

### 4. Collectible Items

#### Coins
- **Type**: Sphere
- **Color**: Yellow
- **Scale**: (0.5, 0.5, 0.5)
- **Points Value**: 10 points each
- **Components**:
  - Mesh Renderer
  - Sphere Collider (IS Trigger)
  - Collectible Script (with isTreasure = false)
- **Animation**: 
  - Rotation: 100°/second
  - Bobbing: Sine wave oscillation
- **Quantity**: 10 coins distributed throughout maze

#### Treasures
- **Type**: Cube
- **Color**: Orange/Gold
- **Scale**: (0.7, 0.7, 0.7)
- **Points Value**: 50 points each
- **Components**:
  - Mesh Renderer
  - Box Collider (IS Trigger)
  - Collectible Script (with isTreasure = true)
- **Animation**:
  - Rotation: 80°/second
  - Bobbing: Slower oscillation
- **Quantity**: 3 treasures placed strategically

### 5. Camera (Main)
- **Type**: Perspective Camera
- **Position**: (0, 20, -15)
- **Rotation**: (45°, 0°, 0°)
- **View**: Isometric-like angle showing both ground and elevated areas
- **Background Color**: Light blue (0.2, 0.3, 0.4)

### 6. Lighting
- **Directional Light** (Sun):
  - Rotation: (50°, -30°, 0°)
  - Intensity: 1.0
  - Creates realistic shadows and depth
- **Ambient Light**:
  - Color: Gray (0.5, 0.5, 0.5)
  - Ensures no areas are completely dark

### 7. Game Manager
- **Type**: Empty GameObject
- **Components**: GameManager Script
- **Purpose**: 
  - Tracks player score (coins and treasures collected)
  - Manages UI display
  - Implements singleton pattern for game state

## How to Use This Scene

### Option 1: Using the Scene File in Editor
1. Open `Assets/Scenes/JeuCollecte.unity` (or create a new scene)
2. Attach the `LevelGenerator.cs` script to an empty GameObject
3. Click Play to automatically generate the level

### Option 2: Using SceneSetup Script
1. Create an empty GameObject named "SceneSetup"
2. Attach `SceneSetup.cs` script
3. All configurations will run automatically on Start()

### Option 3: Manual Setup (for learning)
1. Create all GameObjects manually following the Scene Hierarchy above
2. Attach the appropriate scripts to each object
3. Configure colliders and physics properties

## Physics Configuration

- **Gravity**: Standard Earth gravity (0, -9.81, 0)
- **Player Rigidbody**:
  - Mass: 1
  - Drag: 0
  - Angular Drag: 0.05
  - Constraints: Freeze Rotation X and Z
- **Wall Colliders**: Standard box colliders (NOT triggers)
- **Collectible Colliders**: Sphere/Box colliders (IS Trigger = true)

## Controls

| Input | Action |
|-------|--------|
| A / D | Move left/right |
| Arrow Keys | Move left/right |
| Space | Jump |

## Game Mechanics

1. **Movement**: Player moves horizontally with smooth acceleration
2. **Jumping**: Player can jump when grounded (on the Ground plane)
3. **Collision Detection**: 
   - Collision: Walls stop player movement
   - Trigger: Collectibles are collected on touch
4. **Scoring**:
   - Coins: +10 points
   - Treasures: +50 points
5. **Animation**:
   - Collectibles rotate and bob (move up/down) continuously
   - Coins disappear when collected

## Scripts Used

- **PlayerController.cs**: Handles player input and movement
- **Collectible.cs**: Manages collectible item behavior
- **GameManager.cs**: Manages overall game state
- **LevelGenerator.cs**: Procedurally generates the level
- **SceneSetup.cs**: Configures the scene on startup

## Customization

You can customize the scene by modifying parameters in the Inspector:

- Number and position of collectibles
- Maze layout and wall positions
- Player movement speed and jump force
- Camera angle and position
- Lighting intensity
- Collectible point values

## Performance Considerations

- The scene uses primitive shapes (Plane, Cube, Sphere, Capsule) for optimal performance
- Physics calculations are optimized with appropriate Rigidbody settings
- Trigger colliders are used for collectibles (more efficient than regular collisions)
- Scene is designed to run smoothly on most hardware

## Future Enhancements

- Add sound effects for collectibles
- Implement a timer/score UI
- Add level progression
- Create different maze layouts
- Add enemy AI
- Implement power-ups
- Add visual effects (particles, trails)
