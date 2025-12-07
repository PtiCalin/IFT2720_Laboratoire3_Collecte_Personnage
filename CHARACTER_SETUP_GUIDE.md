# 🎮 Character GameObject Setup Guide

Complete guide for creating all character and object GameObjects in Unity for the Laboratoire 3 project.

## 📋 Table of Contents

1. [Player Character Setup](#1-player-character-setup)
2. [Collectible Objects Setup](#2-collectible-objects-setup)
3. [Ground and Environment Setup](#3-ground-and-environment-setup)
4. [Camera Setup](#4-camera-setup)
5. [UI Setup](#5-ui-setup)
6. [Testing Checklist](#6-testing-checklist)

---

## 1. Player Character Setup

### Step 1.1: Create Player GameObject

1. **In Unity Hierarchy:**
   - Right-click → `3D Object` → `Capsule`
   - Rename to `Player`
   - Set Position: `(0, 1, 0)`
   - Set Scale: `(1, 1, 1)`

2. **Add Tag:**
   - Select Player
   - In Inspector → Tag → Add Tag
   - Create new tag: `Player`
   - Assign tag to Player GameObject

### Step 1.2: Add Rigidbody Component

1. **Select Player GameObject**
2. **Add Component → Rigidbody**
3. **Configure Rigidbody:**
   ```
   Mass: 1
   Drag: 0
   Angular Drag: 0.05
   Use Gravity: ✓ (checked)
   Is Kinematic: ☐ (unchecked)
   Interpolation: Interpolate
   Collision Detection: Continuous
   Constraints:
     - Freeze Rotation X: ✓
     - Freeze Rotation Z: ✓
   ```

### Step 1.3: Add Capsule Collider

1. **Should already exist (comes with Capsule)**
2. **Verify settings:**
   ```
   Is Trigger: ☐ (unchecked)
   Material: None
   Center: (0, 0, 0)
   Radius: 0.5
   Height: 2
   Direction: Y-Axis
   ```

### Step 1.4: Add PlayerController Script

1. **Select Player**
2. **Add Component → Scripts → PlayerController**
3. **Configure in Inspector:**

   **Movement:**
   - Move Speed: `5`
   - Max Speed: `10`
   - Acceleration: `10`
   - Jump Force: `5`
   - Ground Drag: `5`
   - Air Drag: `2`

   **Advanced Movement:**
   - Air Control Factor: `0.5`
   - Max Jumps: `2`
   - Coyote Time: `0.2`
   - Jump Buffer Time: `0.2`

   **Ground Check:**
   - Ground Dist: `0.2`
   - Ground Layer: `Default` (or create "Ground" layer)
   - Ground Check Point: Leave empty (will use Player position)

   **Physics Constraints:**
   - Max Fall Speed: `20`
   - Gravity Multiplier: `1.5`

   **Visual Feedback:**
   - Visual Model: Drag the Capsule child here (see next step)
   - Rotation Speed: `10`

### Step 1.5: Create Visual Model Child

1. **Right-click Player → Create Empty**
2. **Rename to `VisualModel`**
3. **Right-click VisualModel → 3D Object → Capsule**
4. **Rename to `PlayerMesh`**
5. **Remove Capsule Collider from PlayerMesh** (we only need the visual)
6. **Set VisualModel Transform:**
   ```
   Position: (0, 0, 0)
   Rotation: (0, 0, 0)
   Scale: (1, 1, 1)
   ```

7. **Drag VisualModel to PlayerController's "Visual Model" field**

### Step 1.6: Create Ground Check Point

1. **Right-click Player → Create Empty**
2. **Rename to `GroundCheckPoint`**
3. **Set Position:**
   ```
   Position: (0, -1, 0)  // At the bottom of the capsule
   Rotation: (0, 0, 0)
   Scale: (1, 1, 1)
   ```

4. **Drag GroundCheckPoint to PlayerController's "Ground Check Point" field**

### Step 1.7: Add Material (Optional)

1. **Create Material:**
   - Right-click in Project → Create → Material
   - Name: `PlayerMaterial`
   - Set color (e.g., Blue: RGB 50, 150, 255)

2. **Apply to PlayerMesh:**
   - Drag material onto PlayerMesh in Hierarchy

### Step 1.8: Save as Prefab

1. **Drag Player from Hierarchy to Assets/Prefabs folder**
2. **Name: `Player.prefab`**

---

## 2. Collectible Objects Setup

### 2.1: Coin Collectible

#### Step 2.1.1: Create Coin GameObject

1. **In Hierarchy:**
   - Right-click → `3D Object` → `Cylinder`
   - Rename to `Coin`
   - Set Transform:
     ```
     Position: (0, 0.5, 0)
     Rotation: (0, 0, 90)  // Rotate to stand like a coin
     Scale: (0.3, 0.05, 0.3)  // Flat coin shape
     ```

#### Step 2.1.2: Add Tag

1. **Create Tag:**
   - Tag → Add Tag → `Collectible`
2. **Assign to Coin**

#### Step 2.1.3: Configure Collider

1. **Select Coin**
2. **Capsule Collider component:**
   ```
   Is Trigger: ✓ (MUST be checked!)
   Radius: 0.5
   Height: 2
   Direction: Y-Axis
   ```

#### Step 2.1.4: Remove Rigidbody

- **Important:** Coins should NOT have Rigidbody
- If present, remove it

#### Step 2.1.5: Add Collectible Script

1. **Add Component → Scripts → Collectible**
2. **Configure:**
   ```
   Collectible Type:
     - Is Treasure: ☐ (unchecked)
   
   Points:
     - Points Value: 10
   
   Animation:
     - Rotation Speed: 100
     - Bob Speed: 2
     - Bob Height: 0.3
   ```

#### Step 2.1.6: Add Material

1. **Create Material:**
   - Name: `CoinMaterial`
   - Color: Gold (RGB 255, 215, 0)
   - Metallic: 0.8
   - Smoothness: 0.9

2. **Apply to Coin**

#### Step 2.1.7: Save as Prefab

1. **Drag to Assets/Prefabs**
2. **Name: `Coin.prefab`**
3. **Delete from scene** (we'll place instances later)

### 2.2: Treasure Collectible

#### Step 2.2.1: Duplicate Coin

1. **Duplicate Coin prefab**
2. **Rename to `Treasure`**

#### Step 2.2.2: Modify Scale

```
Scale: (0.5, 0.1, 0.5)  // Larger than coin
```

#### Step 2.2.3: Configure Collectible Script

```
Collectible Type:
  - Is Treasure: ✓ (checked)

Points:
  - Points Value: 50

Animation:
  - Rotation Speed: 80
  - Bob Speed: 1.5
  - Bob Height: 0.5
```

#### Step 2.2.4: Change Material

1. **Create Material:**
   - Name: `TreasureMaterial`
   - Color: Purple/Pink (RGB 200, 50, 200)
   - Metallic: 1.0
   - Smoothness: 0.95

2. **Apply to Treasure**

#### Step 2.2.5: Save as Prefab

1. **Update prefab or save new**
2. **Name: `Treasure.prefab`**

### 2.3: Alternative Collectible Shapes

You can create variations using different 3D objects:

**For Coins:**
- Use Sphere scaled flat: `(0.3, 0.05, 0.3)`
- Or create custom mesh

**For Treasures:**
- Use Cube rotated 45°: `Rotation: (45, 45, 0)`
- Use Sphere: `Scale: (0.5, 0.5, 0.5)`
- Or import custom 3D models

---

## 3. Ground and Environment Setup

### 3.1: Create Ground Plane

#### Step 3.1.1: Create Ground

1. **In Hierarchy:**
   - Right-click → `3D Object` → `Plane`
   - Rename to `Ground`
   - Set Transform:
     ```
     Position: (0, 0, 0)
     Rotation: (0, 0, 0)
     Scale: (2, 1, 2)  // 20x20 units
     ```

#### Step 3.1.2: Add Layer (Optional but Recommended)

1. **Create Layer:**
   - Inspector → Layer → Add Layer
   - Name: `Ground`

2. **Assign to Ground GameObject**

3. **Update PlayerController:**
   - Set Ground Layer to `Ground` layer

#### Step 3.1.3: Add Material

1. **Create Material:**
   - Name: `GroundMaterial`
   - Color: Green (RGB 100, 200, 100)
   - Or use texture

2. **Apply to Ground**

### 3.2: Create Platform

#### Step 3.2.1: Create Platform GameObject

1. **Right-click → 3D Object → Cube**
2. **Rename to `Platform`**
3. **Set Transform:**
   ```
   Position: (5, 2, 0)
   Rotation: (0, 0, 0)
   Scale: (3, 0.5, 3)
   ```

#### Step 3.2.2: Assign Ground Layer

- Set layer to `Ground`

#### Step 3.2.3: Add Material

1. **Create Material:**
   - Name: `PlatformMaterial`
   - Color: Brown (RGB 139, 90, 43)

2. **Apply to Platform**

#### Step 3.2.4: Save as Prefab

1. **Drag to Assets/Prefabs**
2. **Name: `Platform.prefab`**

### 3.3: Create Walls (Optional)

For boundaries:

1. **Create Cube**
2. **Scale to wall size:** `(0.5, 5, 20)`
3. **Position around play area**
4. **Assign Ground layer**

---

## 4. Camera Setup

### 4.1: Position Main Camera

1. **Select Main Camera**
2. **Set Transform:**
   ```
   Position: (0, 10, -10)
   Rotation: (45, 0, 0)
   ```

3. **Adjust Field of View:**
   ```
   Field of View: 60
   ```

### 4.2: Follow Camera (Optional)

Create a simple follow script or use Cinemachine:

1. **Add Component → Scripts → CameraFollow** (create this script)
2. **Or use built-in camera parent:**
   - Make Camera child of Player
   - Adjust local position: `(0, 5, -7)`

---

## 5. UI Setup

### 5.1: Create Canvas

1. **Right-click Hierarchy → UI → Canvas**
2. **Rename to `GameUI`**
3. **Canvas settings:**
   ```
   Render Mode: Screen Space - Overlay
   ```

### 5.2: Create Coins Text

1. **Right-click Canvas → UI → Text - TextMeshPro**
2. **Rename to `CoinsText`**
3. **Set RectTransform:**
   ```
   Anchor: Top-Left
   Position: (100, -50)
   Width: 300
   Height: 50
   ```

4. **Configure Text:**
   ```
   Text: "Coins: 0"
   Font Size: 36
   Color: White
   Alignment: Left
   ```

### 5.3: Create Treasures Text

1. **Duplicate CoinsText**
2. **Rename to `TreasuresText`**
3. **Set Position: `(100, -100)`**
4. **Set Text: `"Treasures: 0"`**

### 5.4: Link to GameManager

1. **Create Empty GameObject → Rename to `GameManager`**
2. **Add Component → Scripts → GameManager**
3. **Drag CoinsText to `Coins Text` field**
4. **Drag TreasuresText to `Treasures Text` field**

---

## 6. Testing Checklist

### 6.1: Player Tests

- [ ] Player falls with gravity
- [ ] Player moves left/right with A/D or Arrow keys
- [ ] Player jumps with Space bar
- [ ] Player can double jump
- [ ] Player stops at max speed
- [ ] Player has smooth acceleration
- [ ] Visual model rotates with movement direction
- [ ] Ground detection works (can jump only when grounded)
- [ ] Coyote time allows late jumps
- [ ] Jump buffering makes jumps responsive

### 6.2: Collectible Tests

- [ ] Coins spin and bob
- [ ] Treasures spin and bob
- [ ] Player can collect coins (they disappear)
- [ ] Player can collect treasures (they disappear)
- [ ] Coin counter increases
- [ ] Treasure counter increases
- [ ] Correct points awarded (10 for coins, 50 for treasures)

### 6.3: Environment Tests

- [ ] Player lands on ground
- [ ] Player lands on platforms
- [ ] Player doesn't fall through surfaces
- [ ] Collisions feel solid
- [ ] No jittering or stuttering

### 6.4: UI Tests

- [ ] Coin counter displays correctly
- [ ] Treasure counter displays correctly
- [ ] Text is readable
- [ ] UI updates in real-time

---

## 🎨 Quick Scene Setup

For a basic playable scene:

1. **Add Ground** (20x20)
2. **Add Player** at `(0, 2, 0)`
3. **Place 5-10 Coins** around the scene at `Y = 0.5`
4. **Place 2-3 Treasures** on platforms
5. **Add 2-3 Platforms** at different heights
6. **Position Camera** at `(0, 10, -10)` looking at `(45, 0, 0)`
7. **Press Play** and test!

---

## 🔧 Common Issues and Solutions

### Player falls through ground
- **Solution:** Check ground has a collider
- Ensure Player has Rigidbody
- Check collision detection is set to Continuous

### Can't collect items
- **Solution:** Ensure Collectible collider has `Is Trigger` checked
- Check Player has `Player` tag
- Verify Collectible script is attached

### Player doesn't move
- **Solution:** Check PlayerController script is attached
- Verify Rigidbody is not kinematic
- Check input keys (A/D/Arrow keys)

### Player spins uncontrollably
- **Solution:** Check Rigidbody Freeze Rotation X and Z are checked
- Verify PlayerController sets `rb.freezeRotation = true`

### Collectibles don't animate
- **Solution:** Check Collectible script is attached
- Verify script has no errors (check Console)
- Check rotation/bob speed values are not zero

### UI doesn't update
- **Solution:** Check GameManager is in scene
- Verify TextMeshPro references are assigned
- Check GameManager script is attached and has no errors

---

## 📚 Additional Resources

### Prefab Organization

```
Assets/
  Prefabs/
    Player.prefab
    Coin.prefab
    Treasure.prefab
    Platform.prefab
    Environment/
      Ground.prefab
      Wall.prefab
    UI/
      GameUI.prefab
```

### Material Organization

```
Assets/
  Materials/
    Player/
      PlayerMaterial.mat
    Collectibles/
      CoinMaterial.mat
      TreasureMaterial.mat
    Environment/
      GroundMaterial.mat
      PlatformMaterial.mat
```

---

## 🎓 Learning Outcomes

By following this guide, you will have:

✅ Created a physics-based player character with Rigidbody  
✅ Implemented collectible objects with trigger collisions  
✅ Set up a functional game environment  
✅ Connected UI elements to game logic  
✅ Organized prefabs for reusability  
✅ Applied proper layers and tags  
✅ Configured materials for visual appeal  
✅ Tested all game mechanics

---

**Happy Creating! 🚀**

For questions or issues, refer to the main README.md or Unity documentation.
