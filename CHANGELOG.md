# Changelog

Tous les changements notables de ce projet seront documentés dans ce fichier.

Le format est basé sur [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
et ce projet adhère au [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### À Venir
- Système de niveaux multiples
- Menu principal et écran de pause
- Effets sonores et musique
- Système de particules pour les collectibles
- Sauvegarde du meilleur score
- Power-ups et bonus

---

## [2.0.0] - 2024-12-07

### 🎉 Améliorations Majeures du Rigidbody

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

### 📚 Documentation

#### Ajouté
- **README.md** complet avec:
  - Description détaillée du projet
  - Instructions d'installation
  - Guide de jeu avec contrôles
  - Architecture du projet
  - Documentation des scripts
  - Configuration des paramètres
  - Guide de contribution
- **LICENSE** (MIT License)
- **CHANGELOG.md** (ce fichier)
- **Pull Request Template** (`.github/pull_request_template.md`)
- Badges pour Unity version, licence, et plateformes

---

## [1.0.0] - 2024-12-01

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

## Notes de Migration

### De 1.0.0 à 2.0.0

**Changements Breaking:**
- Aucun changement breaking dans cette version
- Tous les paramètres existants sont conservés
- Les nouveaux paramètres ont des valeurs par défaut

**Actions Requises:**
1. Ouvrir les scènes contenant le PlayerController
2. Les nouveaux paramètres seront automatiquement ajoutés avec des valeurs par défaut
3. Ajuster les paramètres selon vos préférences dans l'Inspector
4. Optionnel: Assigner un `Ground Check Point` pour un contrôle plus précis
5. Optionnel: Assigner un `Visual Model` pour la rotation visuelle

**Recommandations:**
- Testez le `coyoteTime` et `jumpBufferTime` - peut nécessiter des ajustements
- Le `maxJumps` par défaut est 2 (double jump) - réduisez à 1 pour un saut simple
- Ajustez `airControlFactor` selon vos préférences de gameplay (0.5 = 50% de contrôle en l'air)

---

## Feuille de Route

### Version 2.1.0 (Prochaine)
- [ ] Système d'effets sonores
- [ ] Effets de particules pour collecte
- [ ] Animation du personnage
- [ ] Effets visuels pour double jump

### Version 3.0.0 (Futur)
- [ ] Système de niveaux
- [ ] Menu principal
- [ ] Écran de pause
- [ ] Système de sauvegarde
- [ ] Leaderboard local

### Version 4.0.0 (Long Terme)
- [ ] Niveaux procéduraux
- [ ] Power-ups et capacités spéciales
- [ ] Boss battles
- [ ] Mode multijoueur local

---

**Pour plus d'informations, consultez le [README.md](README.md)**
