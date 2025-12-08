# Laboratoire 3 - Collecte de Personnage

Projet Unity 3D pour le cours IFT2720. Le générateur de niveau construit un labyrinthe procédural, instancie le joueur à l'entrée, répartit les collectibles et configure la caméra dynamique.

## Aperçu rapide

- Génération procédurale de labyrinthe uniques avec entrée et sortie uniques.
- Joueur contrôlé par un `Rigidbody` simple (déplacements caméra-relatifs et saut).
- Pièces et trésors animés qui attribuent des points au `GameManager`.
- Caméra unique basculant entre orbite 3e personne et vue aérienne.
- Matériaux recherchés automatiquement dans `Assets/Materials` (support Poliigon).

## Licence

Ce projet est sous licence MIT - voir le fichier [LICENSE](LICENSE) pour plus de détails.

## ✨ Fonctionnalités

### Contrôle du Personnage avec Rigidbody: Mouvement Physique Réaliste

- ⚡ **Mouvement horizontal fluide** avec système d'accélération progressive
- 🎯 **Vitesse maximale limitée** pour un contrôle prévisible
- 🏃 **Accélération configurable** pour ajuster la réactivité
- 🌪️ **Contrôle aérien réduit** - facteur de contrôle en l'air (50% par défaut)
- 🎯 **Détection de sol précise** avec raycast configurable
- 🔄 **Rotation visuelle** du personnage selon la direction du mouvement

#### Mécaniques de Saut

- 🦘 **Saut basique** avec force d'impulsion configurable
- 🎯 **Double saut** avec support multi-sauts configurable (extension)

### Système de Collecte d'Objets: Collectibles Interactifs

- 💰 **Pièces (Coins)** - objets de base avec animation de rotation et flottement
- 💎 **Trésors (Treasures)** - objets spéciaux avec valeur en points plus élevée
- 🎨 **Animations procédurales** - rotation continue et mouvement sinusoïdal
- ✅ **Détection par Trigger** - collision précise sans impact physique
- 💥 **Effet de collecte** - destruction instantanée de l'objet et feedback visuel

#### Gestion du Score

- 📊 **Suivi des scores** - compteurs séparés pour pièces et trésors
- 🔢 **Points configurables** - valeur assignable par type d'objet
- 📈 **Affichage temps réel** - mise à jour immédiate de l'UI
- 🎯 **GameManager Singleton** - gestion centralisée du score global

### Optimisations et Améliorations Techniques

- 🎛️ **Paramètres exposés** - tous les réglages accessibles via l'Inspector Unity
- 📊 **Headers organisés** - interface Inspector claire avec sections (Movement, Advanced, Ground Check, etc.)

#### Option de Caméra

- 🎥 **Caméra principale (3e Personne)** orbitale verrouillée sur le joueur avec distance, offset et lissages configurables.
- 🦅 **Vue aérienne** orthographique centrée automatiquement sur le labyrinthe pour une supervision rapide.
- 🔁 **Basculer en un clic** (`Tab`) entre les deux angles pour analyser la progression ou explorer en détail.
- 🖱️ **Commandes souris** pour pivoter autour du personnage tout en conservant des limites de pitch configurables.

---

## 🚀 Installation

### Prérequis

- Unity 2022.3 LTS ou version ultérieure
- Git (pour le clonage du repository)
- Visual Studio ou VS Code (recommandé pour le développement C#)

### Étapes d'Installation

1. **Cloner le repository**

   ```bash
   git clone https://github.com/PtiCalin/IFT2720_Laboratoire3_Collecte_Personnage.git
   cd IFT2720_Laboratoire3_Collecte_Personnage
   ```

2. **Ouvrir dans Unity Hub**
   - Lancez Unity Hub
   - Cliquez sur "Add" ou "Open"
   - Sélectionnez le dossier du projet

3. **Ouvrir la scène principale**
   - Naviguez vers `Assets/Scenes/JeuCollecte.unity`
   - Double-cliquez pour ouvrir la scène

4. **Lancer le jeu**
   - Appuyez sur le bouton Play dans Unity Editor
   - Ou utilisez le raccourci `Ctrl/Cmd + P`

---

## 🎮 Comment Jouer

Voici comment jouer au jeu.

### Contrôles

| Action | Touche(s)   |
|----------------------|------------|
| Déplacement Gauche   | `A` ou `←` |
| Déplacement Devant   | `W` ou `↑` |
| Déplacement Derrière | `S` ou `↓` |
| Déplacement Droite   | `D` ou `→` |
| Sauter               | `Espace`   |
| Double Saut          | `Espace`   |
| Rotation Caméra      | Souris     |
| Changer de Vue       | `Tab`      |

### Objectif

- Collectez autant de **pièces** 💰 et de **trésors** 💎 que possible
- Explorez l'environnement en utilisant vos capacités de saut
- Battez votre meilleur score !

---

## 👥 Contributeurs

- **Développeur Principal** — Charlie Bouchard AKA [PtiCalin](https://github.com/PtiCalin)

### 🎨 Crédits des Assets & Algorithmes

| Asset | Auteur | Licence | Emplacement | Notes |
|-------|--------|---------|-------------|-------|
| [Low Poly 3D Treasure Items Game Assets](https://mehrasaur.itch.io/treasure-pack) | [mehrasaur](https://mehrasaur.itch.io/) | [CC0](https://creativecommons.org/publicdomain/zero/1.0/) | `Assets/Models/Collectibles` | Modèles FBX de pièces, trésors, gemmes et coffres. Matériaux à créer dans Unity. |
| Character Model (Visual Novel Series) | [styloo](https://styloo.itch.io/) | [CC0](https://creativecommons.org/publicdomain/zero/1.0/) | `Assets/Models/Characters` | Ressource publiée le 23 oct. 2024 (maj 22 avr. 2025). Note moyenne 4.9/5 (14 avis). Compatible Unity/Unreal/Godot. |

### Algorithmes & Inspirations

- Génération de labyrinthe : implémentation basée sur l'algorithme « Recursive Backtracker » (parcours en profondeur) popularisé par [Jamis Buck, *Maze Generation: Recursive Backtracking* (2010)](https://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking).
- Exemple d'implémentation de référence : [Jamis Buck, *recursive-backtracker.rb* gist](https://gist.github.com/jamis/756896), utilisé comme guide pour structurer la génération procédurale.
- Caméra third-person : inspiration tirée du projet open source [3rd Person Camera And Movement System](https://github.com/SunnyValleyStudio/3rd-Person-Camera-And-Movement-system-in-Unity) de **SunnyValleyStudio** (licence MIT).

## Comment Contribuer

1. Forkez le projet
2. Créez une branche pour votre fonctionnalité (`git checkout -b feature/AmazingFeature`)
3. Committez vos changements (`git commit -m 'Add some AmazingFeature'`)
4. Poussez vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrez une Pull Request (utilisez le template fourni)

### 📁 Architecture du Projet

```txt
IFT2720_Laboratoire3_Collecte_Personnage/
├── Assets/
│   ├── Scenes/
│   │   ├── JeuCollecte.unity          # Scène principale du jeu
│   │   └── SampleScene.unity          # Scène d'exemple
│   ├── Scripts/
│   │   ├── LevelGenerator             # Crée le niveau
│   │   ├── CameraControls             # Contrôle des caméras
│   │   ├── PlayerController.cs        # Contrôle du personnage
│   │   ├── Collectible.cs             # Logique des objets collectibles
│   │   └── GameManager.cs             # Gestion de l'état du jeu
│   ├── Settings/
│   │   └── *.asset                    # Configuration URP
│   └── TutorialInfo/
│       └── ...                        # Ressources tutoriel
├── ProjectSettings/
│   └── *.asset                        # Configurations Unity
├── Packages/
│   ├── manifest.json                  # Dépendances du projet
│   └── packages-lock.json             # Lock file des packages
├── README.md                          # Ce fichier
├── LICENSE                            # Licence MIT
├── CONTRIBUTING.md
├── CHANGELOG.md                       # Journal des modifications
├── CONFIGURATION.md
├── TESTING.md  
└── .github/
    └── pull_request_template.md       # Template pour les PR
```

**Paramètres configurables:**

```csharp
// Level generator

   // Sol
   [SerializeField] private Vector3 groundScale; 
   [SerializeField] private Material groundMaterial;

   // Joueur
   [SerializeField] private Vector3 playerStartPosition;
   [SerializeField] private float playerMoveSpeed;
   [SerializeField] private float playerJumpForce;
   [SerializeField] private GameObject playerPrefab;

   // Collectibles
   [SerializeField] private int numberOfCoins;
   [SerializeField] private int numberOfTreasures;
   [SerializeField] private int coinPointsValue;
   [SerializeField] private int treasurePointsValue;
   [SerializeField] private GameObject coinPrefab;
   [SerializeField] private GameObject treasurePrefab;
   [SerializeField] private Material coinMaterial;
   [SerializeField] private Material treasureMaterial;

   // Labyrinthe
   [SerializeField, Min(2)] private int mazeRows;
   [SerializeField, Min(2)] private int mazeColumns;
   [SerializeField, Min(1f)] private float cellSize;
   [SerializeField] private float wallHeight;
   [SerializeField] private float wallThickness;
   [SerializeField] private Material wallMaterial;

// PlayerController

[SerializeField] private float maxSpeed;
[SerializeField] private float acceleration;
[SerializeField] private float airControlMultiplier;
[SerializeField] private float groundedDrag;
[SerializeField] private float airDrag;
[SerializeField] private float rotationSpeed;
[SerializeField] private Transform character;
[SerializeField] private float jumpForce;
[SerializeField] private int maxJumps; // 2 = double saut
[SerializeField] private bool resetVerticalVelocityOnJump;
[SerializeField] private float groundCheckDistance;
[SerializeField] private float groundCheckRadius;
[SerializeField] private float groundCheckOffset;
[SerializeField] private LayerMask groundLayers;
[SerializeField] private Transform cameraTransform;

// Collectibles

[SerializeField] private bool isTreasure;
[SerializeField] private int pointsValue;
[SerializeField] private float rotationSpeed;
[SerializeField] private float bobSpeed;
[SerializeField] private float bobHeight;

// CameraRigController

[SerializeField] private CameraRigController.CameraMode startMode;
[SerializeField] private bool lockCursorInThirdPerson;
[SerializeField] private bool unlockCursorInBirdsEye;
[SerializeField] private Transform target;
[SerializeField] private Vector3 targetOffset;
[SerializeField, Min(0.1f)] private float distance;
[SerializeField] private float rotationSpeed;
[SerializeField] private float verticalSensitivity;
[SerializeField] private float minPitch;
[SerializeField] private float maxPitch;
[SerializeField] private float thirdPersonPositionSmoothing;
[SerializeField] private float thirdPersonRotationSmoothing;
[SerializeField] private float birdsEyeHeight;
[SerializeField] private float birdsEyeFollowSmoothing;
[SerializeField] private float birdsEyeOrthoLerpSpeed;
[SerializeField] private float birdsEyeMinOrthographicSize;

// GameManager

[SerializeField] private TextMeshProUGUI coinsText;
[SerializeField] private TextMeshProUGUI treasuresText;
[SerializeField] private TextMeshProUGUI scoreText;

```

## 🛠️ Technologies Utilisées

- **Unity Engine** 2022.3 LTS
- **C#** 9.0+
- **Universal Render Pipeline (URP)**
- **TextMeshPro** pour l'UI
- **Physics System** pour les interactions Rigidbody
- **Input System** (classique) pour les contrôles

---

## 🎓 Informations Académiques

**Cours :** IFT2720 - Introduction au Multimédia  
**Institution :** Université de Montréal  
**Laboratoire :** #3 - Collecte de Personnage  
**Trimestre :** Automne 2025  
**Professeur :** Lazhar Khelifi (<lazhar.khelifi@umontreal.ca>)  
**Objectif :** Maîtriser Unity Physics (Rigidbody) et créer un système de gameplay avec collecte d'objets

### Instructions du Laboratoire

**Partie 1 - Contrôle du Personnage et Collecte (Ce Projet):**

- Implémentation d'un contrôleur Rigidbody avec physique réaliste
- Système de collecte d'objets (pièces et trésors)
- Détection du sol et mécaniques de saut avancées
- Interface utilisateur avec compteurs de score

**Partie 2 - Navigation IA avec NavMesh (Annexe Séparée):**

- Implémentation d'agents IA avec NavMesh
- Pathfinding et navigation automatique
- Comportements d'IA (patrouille, poursuite)
- Intégration IA-Joueur dans l'environnement

### 🎮 Exigences de réussite

✅ **Contrôle de Personnage avec Rigidbody**

- ✓ Mouvement horizontal fluide utilisant des forces physiques
- ✓ Saut vertical avec impulsion (AddForce en mode Impulse)
- ✓ Détection du sol fonctionnelle avec Raycast
- ✓ Gestion appropriée du drag (différent au sol et en l'air)
- ✓ Configuration optimale du Rigidbody (interpolation, collision continue)

✅ **Système de Collecte d'Objets**

- ✓ Objets collectibles avec Colliders configurés en mode Trigger
- ✓ Détection de collision avec le joueur (OnTriggerEnter)
- ✓ Destruction des objets à la collecte (Destroy)
- ✓ Attribution de points selon le type d'objet
- ✓ Animations visuelles des collectibles (rotation, flottement)

✅ **GameManager et Architecture**

- ✓ Pattern Singleton correctement implémenté
- ✓ Gestion centralisée du score global
- ✓ Communication efficace avec les autres scripts
- ✓ Méthodes publiques pour ajouter des points
- ✓ Getters pour consulter les scores

✅ **Interface Utilisateur**

- ✓ Affichage des scores avec TextMeshPro
- ✓ Mise à jour dynamique en temps réel
- ✓ Compteurs séparés pour différents types d'objets
- ✓ Interface claire et lisible

✅ **Qualité du Code et Bonnes Pratiques**

- ✓ Code bien structuré et commentéc (en français)
- ✓ Utilisation appropriée de SerializeField
- ✓ Séparation claire des responsabilités entre scripts
- ✓ Headers pour organiser les paramètres dans l'Inspector
- ✓ Nommage cohérent et descriptif des variables

### Extensions et Améliorations (Au-delà des Exigences)

Le projet inclut également des fonctionnalités avancées :

🌟 **Double Jump System** - Permet plusieurs sauts consécutifs
🌟 **Visual Rotation** - Rotation du modèle selon la direction du mouvement.

---

#### Fait avec ❤️, Unity et le savoir infini de la communauté en ligne
