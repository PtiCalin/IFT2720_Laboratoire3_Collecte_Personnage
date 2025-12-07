# Changelog

Tous les changements notables de ce projet seront documentés dans ce fichier.


## [Unreleased]

### À Venir
- Système de niveaux multiples
- Menu principal et écran de pause
- Effets sonores et musique
- Système de particules pour les collectibles
- Sauvegarde du meilleur score
- Power-ups et bonus

---

## [4.0.0] - 2025-12-07

### ✅ Ajouté
- Génération procédurale du labyrinthe via un algorithme de **recursive backtracking** inspiré des travaux de Jamis Buck, avec paramètres exposés (`mazeRows`, `mazeColumns`, `cellSize`).
- Support d'un **prefab de personnage** configurable depuis l'Inspector (`playerPrefab`) pour remplacer dynamiquement le placeholder.
- Nouveau champ `playerStartCell` afin de garantir un point de spawn libre dans la grille générée.

### 🔄 Modifié
- Le personnage de secours est désormais un **GameObject vide** (au lieu d'une capsule) auquel les composants nécessaires sont ajoutés à l'exécution.
- Alignement du spawn joueur sur la cellule calculée par le générateur pour éviter toute intersection avec les murs.
- Ajustement du placement aléatoire des collectibles pour utiliser les dimensions actuelles du labyrinthe généré.
- Nettoyage du script `PlayerController` en supprimant les commentaires verbeux pour une lecture plus directe.

### 🗑️ Supprimé
- Suppression des fichiers `SAMPLE_SCENE_GUIDE.md`, `README_PREFABS.md` et `Assets/Models/Collectibles/README.md` (ainsi que leurs `.meta`) devenus redondants.

### 📝 Documentation
- Réorganisation complète du `README.md` : fusion des guides de scène, de prefabs et des licences d'assets dans un seul document.
- Ajout des crédits détaillés pour le modèle de personnage de **styloo** et pour l'implémentation de labyrinthe basée sur le gist `recursive-backtracker.rb`.
- Mise à jour de la table des matières avec les nouvelles sections « Structure de la Scène », « Bibliothèque de Prefabs » et « Crédits des Assets & Algorithmes ».

---

## [3.0.0]

### 📚 Documentation & Setup

#### Ajouté
- **Ground Detection Implementation** (OnCollisionEnter):
  - Explication complète du concept de détection du sol
  - Implémentation avec OnCollisionEnter/OnCollisionExit
  - Utilisation de CompareTag() pour performance
  - Problèmes courants et solutions
  - Optimisations avec Raycast
  - Comparaison avec OnCollisionStay
  - Code complet commenté en français

- **Character & Environment Setup**:
  - Guide complet de création du Player GameObject (Capsule + Rigidbody + Scripts)
  - Configuration des tags et layers
  - Setup des collectibles (pièces vs trésors)
  - Création de l'environnement (sol, plateformes, murs)
  - Configuration de la caméra et de l'UI TextMeshPro
  - Checklist complète de test
  - Organisation des matériaux et des paramètres

- **Prefab System**:
  - Structure de prefabs organisée (Player, Coin, Treasure, Platform)
  - Hiérarchie d'assets recommandée
  - Réutilisabilité des composants
  - Configuration recommandée pour chaque type

- **Practical Jump Examples** (5 implémentations):
  - Saut simple (base absolue avec AddForce)
  - Double saut (système de sauts multiples)
  - Saut diagonal (combinaison de vecteurs)
  - Saut ajustable (hauteur variable)
  - Push force (explosion/poussée)
  - Visualisations ASCII et code complet commenté

#### Documentation Consolidée
Toute la documentation complète est disponible dans README.md avec:
- Guide physique Rigidbody complet
- Exemples pratiques de sauts
- Guide de setup des GameObjects
- Instructions détaillées du laboratoire
- Troubleshooting et solutions

---

## [2.0.0]

### 🎉 Rigidbody

#### Ajouté
- **Double Jump System**: Support pour sauts multiples configurables
- **Coyote Time**: Permet de sauter brièvement après avoir quitté le sol (0.2s)
- **Jump Buffering**: Mémorise l'input de saut pour une réponse plus fluide (0.2s)
- **Variable Jump Height**: Hauteur de saut variable basée sur la durée de pression
- **Smooth Acceleration**: Système d'accélération progressive pour le mouvement horizontal
- **Air Control**: Contrôle réduit du personnage en l'air pour plus de réalisme
- **Max Speed Clamping**: Limitation de la vitesse maximale horizontale
- **Enhanced Gravity**: Multiplicateur de gravité pour une meilleure sensation de chute
- **Fall Speed Limiter**: Prévention de vitesses de chute excessives
- **Visual Rotation**: Rotation du modèle visuel basée sur la direction du mouvement
- **Ground Check Point**: Point de vérification configurable pour la détection du sol
- **Debug Gizmos**: Visualisation de la détection du sol en mode Scene
- **Last Grounded Position**: Stockage de la dernière position au sol

#### Modifié
- Refactorisation complète de `PlayerController.cs`
- Séparation du mouvement entre `Update()` et `FixedUpdate()`
- Amélioration de la configuration du Rigidbody (Interpolation, Continuous Collision)
- Optimisation de la détection du sol avec raycast
- Réorganisation des paramètres avec Headers dans l'Inspector

#### Paramètres Ajoutés
```
Movement:
- maxSpeed (10f)
- acceleration (10f)

Advanced Movement:
- airControlFactor (0.5f)
- maxJumps (2)
- coyoteTime (0.2f)
- jumpBufferTime (0.2f)

Physics Constraints:
- maxFallSpeed (20f)
- gravityMultiplier (1.5f)

Visual Feedback:
- visualModel (Transform)
- rotationSpeed (10f)
- groundCheckPoint (Transform)
```

#### API Publique Ajoutée
- `bool IsGrounded` - Property pour vérifier si le joueur est au sol
- `Vector3 GetVelocity()` - Obtenir la vélocité actuelle du Rigidbody
- `Vector3 GetLastGroundedPosition()` - Obtenir la dernière position au sol

---

## [1.0.0]

### 🎮 Version Initiale

#### Ajouté
- **PlayerController.cs**: Contrôle de base du personnage
  - Mouvement horizontal (gauche/droite)
  - Saut simple
  - Détection du sol basique
  - Configuration du drag (sol/air)
- **Collectible.cs**: Système de collectibles
  - Animation de rotation
  - Animation de flottement (bobbing)
  - Distinction pièces/trésors
  - Détection par trigger
  - Destruction à la collecte
- **GameManager.cs**: Gestion du jeu
  - Pattern Singleton
  - Compteur de pièces
  - Compteur de trésors
  - Mise à jour de l'UI TextMeshPro
  - Logs de debug
- **Scene JeuCollecte**: Scène principale de jeu
  - Configuration de base
  - Placement des collectibles
  - Setup de l'UI

#### Fonctionnalités de Base
- Mouvement du joueur avec physique Rigidbody
- Système de saut avec force configurable
- Collecte de pièces (10 points)
- Collecte de trésors (50 points)
- Affichage des scores en temps réel
- Universal Render Pipeline (URP) configuré

#### Configuration Initiale
- Unity 2022.3 LTS
- URP activé
- TextMeshPro intégré
- Structure de dossiers organisée
- Scripts commentés

---

## Types de Changements

- `Added` - Nouvelles fonctionnalités
- `Changed` - Changements aux fonctionnalités existantes
- `Deprecated` - Fonctionnalités bientôt supprimées
- `Removed` - Fonctionnalités supprimées
- `Fixed` - Corrections de bugs
- `Security` - Corrections de vulnérabilités

---

**Pour plus d'informations, consultez le [README.md](README.md)**
