# ⚡ Quick Start Guide - Create Character GameObjects

Fast-track guide to get your game running in under 15 minutes!

## 🎯 Goal

Create a playable character that can move, jump, and collect coins in a 3D environment.

---

## 📦 Prerequisites

- Unity project open with scenes from repository
- Scripts already in `Assets/Scripts/` folder

---

## ⏱️ 10-Minute Setup

### 1️⃣ Create Player (3 minutes)

1. **Hierarchy** → Right-click → **3D Object** → **Capsule**
2. Rename to **`Player`**
3. **Add Component** → **Rigidbody**
   - Set Interpolation: **Interpolate**
   - Set Collision Detection: **Continuous**
   - Check **Freeze Rotation** X and Z
4. **Add Component** → **PlayerController** script
5. **Inspector** → Tag → **Player**

**Player Position:** `(0, 2, 0)`

✅ **Done!** Player is ready.

---

### 2️⃣ Create Ground (1 minute)

1. **Hierarchy** → Right-click → **3D Object** → **Plane**
2. Rename to **`Ground`**
3. **Scale:** `(2, 1, 2)`
4. **Layer:** Default or create **Ground** layer

**Ground Position:** `(0, 0, 0)`

✅ **Done!** Ground is ready.

---

### 3️⃣ Create Coin (3 minutes)

1. **Hierarchy** → Right-click → **3D Object** → **Cylinder**
2. Rename to **`Coin`**
3. **Transform:**
   - Position: `(5, 0.5, 0)`
   - Rotation: `(0, 0, 90)`
   - Scale: `(0.3, 0.05, 0.3)`
4. **Capsule Collider:**
   - ✅ Check **Is Trigger**
5. **Add Component** → **Collectible** script
   - Is Treasure: ☐ Unchecked
   - Points Value: **10**

✅ **Done!** Coin is ready. Duplicate for more coins!

---

### 4️⃣ Create UI (2 minutes)

1. **Hierarchy** → Right-click → **UI** → **Canvas**
2. Right-click Canvas → **UI** → **Text - TextMeshPro**
   - Rename: **`CoinsText`**
   - Text: **"Coins: 0"**
   - Position: Top-left corner
3. Duplicate → Rename: **`TreasuresText`**
   - Text: **"Treasures: 0"**

✅ **Done!** UI is ready.

---

### 5️⃣ Create GameManager (1 minute)

1. **Hierarchy** → Right-click → **Create Empty**
2. Rename to **`GameManager`**
3. **Add Component** → **GameManager** script
4. **Drag TextMeshPro objects:**
   - Coins Text → CoinsText
   - Treasures Text → TreasuresText

✅ **Done!** GameManager is ready.

---

### 6️⃣ Position Camera (30 seconds)

1. Select **Main Camera**
2. Set Transform:
   - Position: `(0, 10, -10)`
   - Rotation: `(45, 0, 0)`

✅ **Done!** Camera positioned.

---

## 🎮 Test Your Game!

Press **Play** ▶️ button

**Controls:**
- **A/D** or **←/→** : Move
- **Space** : Jump

**Expected behavior:**
- Player moves smoothly
- Player jumps when Space pressed
- Coins spin and float
- Collecting coins increases counter

---

## 🎨 Make It Pretty (Optional)

### Add Colors

**Coin Material:**
1. Right-click in Project → **Create** → **Material**
2. Name: **CoinMaterial**
3. Color: **Gold** (255, 215, 0)
4. Drag onto Coin

**Player Material:**
1. Create Material → **PlayerMaterial**
2. Color: **Blue** (50, 150, 255)
3. Drag onto Player

**Ground Material:**
1. Create Material → **GroundMaterial**
2. Color: **Green** (100, 200, 100)
3. Drag onto Ground

---

## 🚀 Next Steps

### Add More Content

**More Coins:**
- Duplicate Coin (Ctrl+D)
- Spread around at `Y = 0.5`

**Add Platforms:**
1. Create **Cube**
2. Scale: `(3, 0.5, 3)`
3. Position at different heights

**Add Treasures:**
1. Duplicate Coin
2. Rename to **Treasure**
3. Scale larger: `(0.5, 0.1, 0.5)`
4. Collectible script:
   - ✅ Check **Is Treasure**
   - Points: **50**

---

## 📋 Checklist

- [ ] Player created with Rigidbody
- [ ] PlayerController script attached
- [ ] Ground plane created
- [ ] At least one Coin created
- [ ] Coin has Collectible script
- [ ] Coin collider is Trigger
- [ ] UI Canvas with text created
- [ ] GameManager created and linked
- [ ] Camera positioned
- [ ] Game tested and working

---

## ❗ Troubleshooting

**Player falls through ground?**
→ Check Rigidbody on Player, Mesh Collider on Ground

**Can't collect coins?**
→ Coin collider must be **Is Trigger** ✅
→ Player must have **Player** tag

**Player doesn't move?**
→ Check PlayerController is attached
→ Check Rigidbody isn't Kinematic

**Coins don't spin?**
→ Check Collectible script is attached

---

## 🎯 You're Done!

You now have a working game with:
- ✅ Moving player character
- ✅ Jumping mechanics
- ✅ Collectible coins
- ✅ Score tracking
- ✅ UI display

**Time to expand your game!** 🎉

See **CHARACTER_SETUP_GUIDE.md** for advanced features like:
- Double jump
- Treasures
- Better animations
- More platforms

---

**Ready to play? Press Play! 🚀**
