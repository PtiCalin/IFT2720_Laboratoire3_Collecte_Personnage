# ⚙️ Guide Complet: Physique Unity et Rigidbody

Explication détaillée de tous les concepts de physique utilisés dans le script PlayerController pour le Laboratoire 3.

---

## 📚 Table des Matières

1. [Rigidbody (rb)](#1-rigidbody-rb)
2. [AddForce()](#2-addforce)
3. [Vector3](#3-vector3)
4. [jumpForce](#4-jumpforce)
5. [ForceMode](#5-forcemode)
6. [Concepts Connexes](#6-concepts-connexes)

---

## 1. Rigidbody (rb)

### Qu'est-ce que le Rigidbody?

**rb** est une variable représentant le composant **Rigidbody** attaché à l'objet.

### Importance du Rigidbody

Le **Rigidbody** est ce qui permet à l'objet d'être influencé par la physique dans Unity, comme:
- **La gravité** - L'objet tombe vers le bas
- **Les collisions** - L'objet rebondit sur d'autres objets
- **Les forces** - On peut appliquer des forces avec AddForce()
- **La vélocité** - L'objet a une vitesse de mouvement

### Sans Rigidbody
```csharp
// SANS Rigidbody, le personnage:
// ❌ Ne tombe pas avec la gravité
// ❌ Traverse les murs
// ❌ Ne peut pas sauter avec AddForce
// ❌ N'a pas de physique réaliste
```

### Avec Rigidbody
```csharp
// AVEC Rigidbody, le personnage:
// ✅ Tombe avec la gravité
// ✅ Rebondit sur les collisions
// ✅ Peut sauter avec AddForce
// ✅ A une physique réaliste
```

### Comment Récupérer le Rigidbody

```csharp
// Dans la méthode Start(), on récupère le Rigidbody
private Rigidbody rb;

void Start()
{
    // GetComponent<Rigidbody>() récupère le composant Rigidbody
    // attaché au même GameObject que ce script
    rb = GetComponent<Rigidbody>();
}
```

### Utilisation du Rigidbody

Une fois obtenu, on peut utiliser rb pour:

```csharp
// Appliquer une force
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

// Accéder à la vélocité
float vitesseActuelle = rb.velocity.magnitude;

// Modifier directement la vélocité
rb.velocity = new Vector3(vitesseX, vitesseY, vitesseZ);

// Ajouter une rotation
rb.angularVelocity = new Vector3(0, rotationSpeed, 0);

// Geler la position ou rotation
rb.freezeRotation = true;
```

### Configuration du Rigidbody dans l'Inspector

```
Mass: 1                          // Poids de l'objet
Drag: 0                          // Résistance à l'air
Angular Drag: 0.05               // Résistance de rotation
Use Gravity: ✓ (Checked)         // La gravité affecte cet objet
Is Kinematic: ☐ (Unchecked)      // L'objet est dynamique (bouge)
Interpolation: Interpolate       // Mouvement lisse entre frames
Collision Detection: Continuous  // Détection précise des collisions
Constraints:
  - Freeze Rotation X: ✓         // Pas de rotation sur X
  - Freeze Rotation Z: ✓         // Pas de rotation sur Z
```

---

## 2. AddForce()

### Qu'est-ce que AddForce()?

`AddForce()` est une **méthode** du Rigidbody qui applique une force à l'objet.

### Signature

```csharp
rb.AddForce(Vector3 force, ForceMode mode = ForceMode.Force);
```

### Parameters

| Paramètre | Type | Description |
|-----------|------|-------------|
| `force` | Vector3 | Le vecteur représentant la force à appliquer |
| `mode` | ForceMode | Comment la force est appliquée (par défaut: Force) |

### Utilité

AddForce() peut être utilisée pour:

```csharp
// Faire sauter un personnage
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

// Pousser un objet
rb.AddForce(Vector3.right * pushForce, ForceMode.Impulse);

// Appliquer une force continue (par exemple, vent)
rb.AddForce(windDirection * windForce, ForceMode.Force);

// Ajouter une accélération
rb.AddForce(accelerationVector, ForceMode.Acceleration);
```

### Exemples Pratiques

```csharp
// Exemple 1: Saut Vertical (comme dans PlayerController)
rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

// Exemple 2: Explosion (force dans toutes les directions)
Vector3 explosionDirection = (transform.position - explosionCenter).normalized;
rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);

// Exemple 3: Poussée par projectile
rb.AddForce(projectileDirection * projectileForce, ForceMode.Impulse);
```

### Différence avec transform.Translate()

```csharp
// ❌ transform.Translate() - Pas de physique
transform.Translate(movement * Time.deltaTime);
// Problèmes:
// - Traverse les objets
// - Pas de gravité
// - Pas de collisions réalistes

// ✅ rb.AddForce() - Avec physique
rb.AddForce(direction * force, ForceMode.Impulse);
// Avantages:
// - Respecte les collisions
// - Affecté par la gravité
// - Mouvement réaliste
```

---

## 3. Vector3

### Qu'est-ce que Vector3?

Un **Vector3** représente une position ou une direction en 3D avec 3 composantes: **X**, **Y**, **Z**.

### Composantes

```csharp
Vector3 v = new Vector3(x, y, z);

// X : Axe Horizontal Gauche/Droite
// Y : Axe Vertical Haut/Bas
// Z : Axe Profondeur Avant/Arrière
```

### Visualisation

```
       Y (Haut)
       |
       |  Z (Avant)
       | /
-------+-------- X (Droite)
      /|
     / |
(Bas) (Arrière)
```

### Vecteurs Prédéfinis

```csharp
// Directions unitaires prédéfinies
Vector3.up        // (0, 1, 0)  - Vers le haut
Vector3.down      // (0, -1, 0) - Vers le bas
Vector3.right     // (1, 0, 0)  - Vers la droite
Vector3.left      // (-1, 0, 0) - Vers la gauche
Vector3.forward   // (0, 0, 1)  - Vers l'avant
Vector3.back      // (0, 0, -1) - Vers l'arrière
Vector3.zero      // (0, 0, 0)  - Pas de direction
Vector3.one       // (1, 1, 1)  - Tous les axes à 1

// Exemple d'utilisation
Vector3 up = Vector3.up;  // Équivalent à (0, 1, 0)
```

### Vector3 pour le Saut

```csharp
// Pour le saut, on utilise Vector3.up
// Cela signifie une force dirigée vers le haut (Y positif)

// Saut faible
rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);  // (0, 2, 0)

// Saut moyen
rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);  // (0, 5, 0)

// Saut puissant
rb.AddForce(Vector3.up * 10f, ForceMode.Impulse); // (0, 10, 0)
```

### Création de Vecteurs Personnalisés

```csharp
// Saut diagonal
Vector3 diagonalJump = new Vector3(2f, 5f, 0f);
rb.AddForce(diagonalJump, ForceMode.Impulse);

// Saut en arc
Vector3 arcJump = new Vector3(0f, 8f, 5f);
rb.AddForce(arcJump, ForceMode.Impulse);
```

### Opérations sur Vector3

```csharp
// Multiplication
Vector3 doubleForce = Vector3.up * 2f;  // (0, 2, 0)

// Addition
Vector3 combined = Vector3.up + Vector3.right;  // (1, 1, 0)

// Normalisation (direction unitaire)
Vector3 normalized = directionVector.normalized;

// Magnitude (longueur du vecteur)
float longueur = directionVector.magnitude;

// Distance entre deux points
float distance = Vector3.Distance(pointA, pointB);
```

---

## 4. jumpForce

### Qu'est-ce que jumpForce?

`jumpForce` est une **variable flottante (float)** qui détermine la **magnitude de la force** appliquée lors du saut.

### Déclaration

```csharp
[Tooltip("Force du saut")]
public float jumpForce = 5f;
```

### Comment Cela Fonctionne

```csharp
// Avec jumpForce = 5f
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
// Résultat: rb.AddForce((0, 5, 0), ForceMode.Impulse);

// Avec jumpForce = 10f
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
// Résultat: rb.AddForce((0, 10, 0), ForceMode.Impulse);
```

### Impact sur le Jeu

| jumpForce | Hauteur du Saut | Impression |
|-----------|-----------------|-----------|
| 2f | Très court | Petit saut |
| 5f | Moyen | Saut naturel (par défaut) |
| 8f | Haut | Bon saut |
| 12f | Très haut | Super-héros |
| 20f | Excessif | Bug (trop puissant) |

### Ajustement du jumpForce

```csharp
// AVANT: Saut trop faible
public float jumpForce = 2f;
// Le joueur peut à peine sauter sur une petite caisse

// APRÈS: Saut bon
public float jumpForce = 5f;
// Le joueur peut sauter naturellement et atteindre des platfomes

// OPTIMAL: Saut précis
public float jumpForce = 7f;
// Équilibre parfait pour le gameplay
```

### Relation avec la Masse (Mass)

La hauteur du saut dépend aussi de la **masse** du Rigidbody:

```csharp
// Même jumpForce, masse différente:

// Objet léger (Mass = 0.5)
// Le saut est plus haut (moins de poids à soulever)

// Objet lourd (Mass = 2)
// Le saut est moins haut (plus de poids à soulever)
```

### Formule de Hauteur (Approximative)

```
Hauteur ≈ (jumpForce / 2) / 9.81  (en secondes)
Hauteur ≈ jumpForce² / (2 * 9.81) (en unités)

Exemple avec jumpForce = 5:
Hauteur ≈ 25 / (2 * 9.81) ≈ 1.27 unités
```

---

## 5. ForceMode

### Qu'est-ce que ForceMode?

`ForceMode` est un **paramètre** qui définit **comment** la force est appliquée au Rigidbody.

### Types de ForceMode

#### A. ForceMode.Impulse (Utilisé pour le Saut)

```csharp
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
```

**Caractéristiques:**
- ✅ Applique une force **instantanée**
- ✅ Tient compte de la **masse** de l'objet
- ✅ Idéal pour les **changements soudains** de vitesse
- ✅ Parfait pour les **sauts**, **explosions**, **impacts**

**Formule:**
```
Accélération = Force / Masse
Impulsion = Force * Δt
```

**Exemple:**
```csharp
// Saut - Force instantanée
rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
// Le personnage saute immédiatement
```

#### B. ForceMode.Force (Force Continue)

```csharp
rb.AddForce(Vector3.up * gravityForce, ForceMode.Force);
```

**Caractéristiques:**
- Applique une force **continue** (chaque frame)
- Tient compte de la **masse**
- Idéal pour les **accélérations progressives**
- Utilisé pour les **mouvements continus**

**Exemple:**
```csharp
// Poussée progressive (à appeler à chaque frame)
rb.AddForce(Vector3.right * pushForce, ForceMode.Force);
// L'objet accélère progressivement vers la droite
```

#### C. ForceMode.Acceleration

```csharp
rb.AddForce(windVector, ForceMode.Acceleration);
```

**Caractéristiques:**
- Ajoute directement à **l'accélération**
- **Ignore la masse** de l'objet
- Tous les objets accélèrent de la même façon
- Utile pour des effets **indépendants de la masse**

**Exemple:**
```csharp
// Vent qui affecte tous les objets pareil
rb.AddForce(windDirection, ForceMode.Acceleration);
```

#### D. ForceMode.VelocityChange

```csharp
rb.AddForce(new Vector3(5, 0, 0), ForceMode.VelocityChange);
```

**Caractéristiques:**
- Change directement la **vélocité**
- **Ignore la masse** complètement
- Effet instantané et direct
- Rarement utilisé (peut causer des bugs)

**Exemple:**
```csharp
// Changement direct de vélocité (rare)
rb.AddForce(new Vector3(10, 0, 0), ForceMode.VelocityChange);
```

### Comparaison des ForceMode

| Mode | Tient Compte Masse | Continu | Idéal Pour |
|------|------------------|---------|-----------|
| **Impulse** | ✅ Oui | ❌ Non | Sauts, impacts instantanés |
| **Force** | ✅ Oui | ✅ Oui | Accélérations progressives |
| **Acceleration** | ❌ Non | ✅ Oui | Effets environnementaux |
| **VelocityChange** | ❌ Non | ❌ Non | Changements de vitesse directs |

### Pourquoi Utiliser ForceMode.Impulse pour le Saut?

```csharp
// ✅ CORRECT: ForceMode.Impulse
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
// Avantages:
// - Saut instantané et réactif
// - Tient compte de la masse (réaliste)
// - Parfait pour un contrôle de platformer
// - Le saut n'est pas affecté par les frames précédentes

// ❌ INCORRECT: ForceMode.Force (pour un saut)
rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
// Problèmes:
// - S'applique chaque frame (augmente pendant plusieurs frames)
// - Le joueur s'envole trop haut
// - Difficile à contrôler
```

---

## 6. Concepts Connexes

### 6.1 Vélocité vs Force

```csharp
// FORCE - Acceleration progressive
rb.AddForce(direction * force, ForceMode.Impulse);
// La force s'ajoute à la vélocité actuelle
// Résultat: Changement de vitesse graduel

// VÉLOCITÉ - Changement direct
rb.velocity = new Vector3(5, 0, 0);
// Change directement la vitesse
// Résultat: Le personnage se déplace à exactement 5 unités/s

// Exemple de différence:
// Avec Force: 0 → 3 → 5 → 5 (accélère progressivement)
// Avec Velocity: 0 → 5 (changement immédiat)
```

### 6.2 Gravité

```csharp
// La gravité est appliquée automatiquement par Unity
// Par défaut: -9.81 unités/s² (comme dans la vie réelle)

// Si Use Gravity est coché dans le Rigidbody:
// rb.velocity.y diminue de 9.81 m/s chaque seconde
// Cela fait tomber automatiquement l'objet

// Quand on saute avec AddForce:
// 1. AddForce applique une vélocité Y positive
// 2. La gravité commence à réduire cette vélocité
// 3. L'objet monte jusqu'à Y = 0
// 4. Puis il tombe
```

### 6.3 Masse et Poids

```csharp
// La masse affecte comment les forces agissent:
// F = m * a (Force = Masse * Accélération)

// Objet léger (Mass = 1)
// a = F / m = 5 / 1 = 5 m/s²
rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

// Objet lourd (Mass = 2)
// a = F / m = 5 / 2 = 2.5 m/s²
// Même force, mais accélération plus faible
// Donc saut moins haut

// Solution: Augmenter la force pour objet lourd
rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);  // Compense la masse
```

### 6.4 Drag

```csharp
// Le Drag est la résistance à l'air

// Drag bas (0-1)
// Objet accélère et continue facilement

// Drag haut (10+)
// Objet ralentit rapidement (comme marcher dans l'eau)

// Exemple pour personnage:
private float groundDrag = 5f;   // Au sol: plus de friction
private float airDrag = 2f;      // En l'air: moins de friction
```

---

## 📊 Résumé Complet

```csharp
void Update()
{
    // Détecte l'entrée (Espace)
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        // 1. rb : Le composant Rigidbody du personnage
        // 2. AddForce() : Applique une force à cet objet
        // 3. Vector3.up : Direction (0, 1, 0) = vers le haut
        // 4. * jumpForce : Magnitude (5 par défaut)
        // 5. ForceMode.Impulse : Force instantanée (idéale pour saut)
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        // Résultat: Le personnage saute vers le haut!
    }
}
```

---

## 🧪 Tests et Expérimentations

### Test 1: Modifier jumpForce

```csharp
// Dans l'Inspector, essayez:
// jumpForce = 2f   → Saut très bas
// jumpForce = 5f   → Saut normal (par défaut)
// jumpForce = 10f  → Saut très haut
// jumpForce = 20f  → Saut excessif (trop puissant)
```

### Test 2: Modifier la Masse

```csharp
// Dans le Rigidbody de l'Inspector:
// Mass = 0.5  → Saute plus haut (léger)
// Mass = 1    → Saut normal
// Mass = 2    → Saute moins haut (lourd)

// Conclusion: Même jumpForce, masse différente = hauteur différente
```

### Test 3: Changer ForceMode

```csharp
// Remplacez ForceMode.Impulse par ForceMode.Force
// ❌ Le personnage s'envole incontrôlablement
// Cela montre pourquoi Impulse est mieux pour les sauts
```

---

## ✅ Checklist de Compréhension

- [ ] Je comprends ce qu'est un Rigidbody
- [ ] Je sais pourquoi on utilise rb.AddForce()
- [ ] Je comprends les composantes X, Y, Z de Vector3
- [ ] Je sais que Vector3.up = (0, 1, 0)
- [ ] Je comprends l'impact du jumpForce sur la hauteur
- [ ] Je sais pourquoi on utilise ForceMode.Impulse
- [ ] Je peux expliquer la différence Force vs Impulse
- [ ] Je comprends le lien entre masse et accélération
- [ ] Je peux modifier jumpForce pour ajuster le gameplay
- [ ] Je peux debugger les problèmes de physique

---

## 🎓 Ressources Unity Officielles

- [Rigidbody Documentation](https://docs.unity3d.com/ScriptReference/Rigidbody.html)
- [AddForce Documentation](https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html)
- [Vector3 Documentation](https://docs.unity3d.com/ScriptReference/Vector3.html)
- [ForceMode Documentation](https://docs.unity3d.com/ScriptReference/ForceMode.html)
- [Physics Tutorial](https://docs.unity3d.com/Manual/PhysicsSection.html)

---

**Vous maîtrisez maintenant la physique Unity! 🚀**
