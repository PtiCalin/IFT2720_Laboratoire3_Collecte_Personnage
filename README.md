# 🎮 Laboratoire 3 - Collecte de Personnage

[![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B-blue.svg)](https://unity.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Mac%20%7C%20Linux-lightgrey.svg)](https://unity.com/)

Un jeu de plateforme 2D/3D développé avec Unity, mettant en œuvre des mécaniques de physique avancées avec Rigidbody et un système de collecte d'objets.

## 📋 Table des Matières

- [Aperçu](#aperçu)
- [Fonctionnalités](#fonctionnalités)
- [Installation](#installation)
- [Comment Jouer](#comment-jouer)
- [Architecture du Projet](#architecture-du-projet)
- [Scripts Principaux](#scripts-principaux)
- [Configuration](#configuration)
- [Technologies Utilisées](#technologies-utilisées)
- [Développement](#développement)
- [Contributeurs](#contributeurs)
- [Licence](#licence)

## 🎯 Aperçu

Ce projet est un laboratoire académique (IFT2720) qui explore les concepts de physique Unity, de contrôle de personnage basé sur Rigidbody, et de système de collecte d'objets. Le joueur contrôle un personnage qui peut se déplacer, sauter, et collecter des pièces et des trésors dans l'environnement.

### Objectifs Pédagogiques

- Maîtriser le système de physique Unity (Rigidbody)
- Implémenter un contrôleur de personnage responsive
- Créer un système de collecte d'objets
- Gérer l'état du jeu avec un GameManager
- Appliquer les bonnes pratiques de programmation Unity

## ✨ Fonctionnalités

### Contrôle du Personnage

- ⚡ **Mouvement fluide** avec système d'accélération
- 🦘 **Double saut** avec support multi-sauts configurable
- 🕐 **Coyote Time** - permet de sauter brièvement après avoir quitté le sol
- 📦 **Jump Buffering** - mémorise l'input de saut pour une réponse plus fluide
- 📏 **Hauteur de saut variable** - relâcher l'espace tôt pour des sauts plus courts
- 🌪️ **Contrôle aérien réduit** pour un gameplay plus réaliste
- 🎯 **Détection de sol précise** avec raycast configurable
- 🔄 **Rotation visuelle** du personnage selon la direction du mouvement

### Système de Collecte

- 💰 **Pièces** - objets de base avec animation de rotation et flottement
- 💎 **Trésors** - objets spéciaux avec valeur en points plus élevée
- 📊 **Suivi des scores** - compteurs séparés pour pièces et trésors
- 🎨 **Animations** - rotation et mouvement sinusoïdal pour tous les collectibles
- ✅ **Détection par trigger** - collision précise avec le joueur

### Interface Utilisateur

- 📈 **Affichage des scores** en temps réel
- 🎯 **TextMeshPro** pour un rendu de texte de haute qualité
- 🔄 **Mise à jour automatique** lors de la collecte

### Optimisations Physiques

- 🎮 **Interpolation** pour un mouvement fluide
- 🔍 **Collision continue** pour une détection précise
- 🔒 **Rotation figée** pour éviter les rotations indésirables
- ⚖️ **Multiplicateur de gravité** pour un meilleur feeling de saut
- 🛑 **Limitation de vitesse de chute** pour éviter les bugs

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

## 🎮 Comment Jouer

### Contrôles

| Action | Touche(s) |
|--------|-----------|
| Déplacement Gauche | `A` ou `←` |
| Déplacement Droite | `D` ou `→` |
| Sauter | `Espace` |
| Double Saut | `Espace` (dans les airs) |

### Objectif

- Collectez autant de **pièces** 💰 et de **trésors** 💎 que possible
- Explorez l'environnement en utilisant vos capacités de saut
- Battez votre meilleur score !

### Conseils

- Utilisez le **coyote time** pour sauter juste après avoir quitté une plateforme
- Le **jump buffering** permet d'appuyer sur saut un peu avant d'atterrir
- Relâchez rapidement la barre d'espace pour des sauts courts et précis
- Le double saut peut sauver d'une chute !

## 📁 Architecture du Projet

```
IFT2720_Laboratoire3_Collecte_Personnage/
├── Assets/
│   ├── Scenes/
│   │   ├── JeuCollecte.unity          # Scène principale du jeu
│   │   └── SampleScene.unity          # Scène d'exemple
│   ├── Scripts/
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
├── CHANGELOG.md                       # Journal des modifications
└── .github/
    └── pull_request_template.md       # Template pour les PR
```

## 🔧 Scripts Principaux

### PlayerController.cs

Gère tous les aspects du contrôle du personnage avec physique Rigidbody.

**Fonctionnalités clés:**
- Mouvement horizontal avec accélération
- Système de saut avancé (double saut, coyote time, jump buffer)
- Détection de sol avec raycast
- Gizmos de debug pour visualisation
- Contrôle aérien configurable

**Paramètres configurables:**
```csharp
[SerializeField] private float moveSpeed = 5f;
[SerializeField] private float maxSpeed = 10f;
[SerializeField] private float acceleration = 10f;
[SerializeField] private float jumpForce = 5f;
[SerializeField] private int maxJumps = 2;
[SerializeField] private float coyoteTime = 0.2f;
[SerializeField] private float jumpBufferTime = 0.2f;
```

### Collectible.cs

Gère le comportement des objets collectibles (pièces et trésors).

**Fonctionnalités clés:**
- Animation de rotation continue
- Mouvement de flottement sinusoïdal
- Détection de collision avec le joueur
- Distinction entre types de collectibles
- Attribution de points au GameManager

**Paramètres configurables:**
```csharp
[SerializeField] private bool isTreasure = false;
[SerializeField] private int pointsValue = 10;
[SerializeField] private float rotationSpeed = 100f;
[SerializeField] private float bobSpeed = 2f;
[SerializeField] private float bobHeight = 0.3f;
```

### GameManager.cs

Singleton qui gère l'état global du jeu et l'interface utilisateur.

**Fonctionnalités clés:**
- Pattern Singleton pour accès global
- Suivi des pièces et trésors collectés
- Mise à jour de l'UI avec TextMeshPro
- Logs de debug pour le suivi

**API publique:**
```csharp
GameManager.Instance.AddPoints(int points, bool isTreasure)
int coinsTotal = GameManager.Instance.GetTotalCoins()
int treasuresTotal = GameManager.Instance.GetTotalTreasures()
```

## ⚙️ Configuration

### Configuration du Personnage (Inspector)

Dans Unity, sélectionnez le GameObject du joueur et ajustez les paramètres dans l'Inspector :

**Movement:**
- `Move Speed`: Vitesse de déplacement (défaut: 5)
- `Max Speed`: Vitesse maximale (défaut: 10)
- `Acceleration`: Taux d'accélération (défaut: 10)
- `Jump Force`: Force du saut (défaut: 5)

**Advanced Movement:**
- `Air Control Factor`: Contrôle en l'air (défaut: 0.5)
- `Max Jumps`: Nombre de sauts (défaut: 2)
- `Coyote Time`: Temps après quitter le sol (défaut: 0.2s)
- `Jump Buffer Time`: Temps de mémorisation du saut (défaut: 0.2s)

**Ground Check:**
- `Ground Dist`: Distance de détection du sol (défaut: 0.2)
- `Ground Layer`: LayerMask pour le sol
- `Ground Check Point`: Transform pour le raycast

### Configuration des Collectibles

**Coins (Pièces):**
- `Is Treasure`: false
- `Points Value`: 10
- `Rotation Speed`: 100
- `Bob Speed`: 2
- `Bob Height`: 0.3

**Treasures (Trésors):**
- `Is Treasure`: true
- `Points Value`: 50
- `Rotation Speed`: 80
- `Bob Speed`: 1.5
- `Bob Height`: 0.5

## 🛠️ Technologies Utilisées

- **Unity Engine** 2022.3 LTS
- **C#** 9.0+
- **Universal Render Pipeline (URP)**
- **TextMeshPro** pour l'UI
- **Physics System** pour les interactions Rigidbody
- **Input System** (classique) pour les contrôles

## 👨‍💻 Développement

### Structure du Code

Le projet suit les bonnes pratiques Unity :
- Séparation des préoccupations
- Pattern Singleton pour le GameManager
- SerializeField pour l'exposition dans l'Inspector
- Commentaires clairs et documentation
- Gizmos pour le debugging visuel

### Ajouter un Nouveau Type de Collectible

1. Dupliquez un collectible existant dans la scène
2. Ajustez les paramètres dans l'Inspector
3. Créez un nouveau matériau/sprite si nécessaire
4. Testez la valeur en points et les animations

### Modifier les Contrôles

Pour changer les contrôles, modifiez les inputs dans `PlayerController.cs` :

```csharp
// Changez Input.GetAxis("Horizontal") pour d'autres touches
// Changez Input.GetKeyDown(KeyCode.Space) pour un autre bouton
```

### Debug et Visualisation

- Les **Gizmos** sont activés en mode Scene pour voir la détection du sol
- Les **Debug.Log** montrent les événements importants (sauts, collectes)
- Utilisez le **profiler Unity** pour optimiser les performances

## 🤝 Contributeurs

- **Développeur Principal** - [PtiCalin](https://github.com/PtiCalin)

### Comment Contribuer

1. Forkez le projet
2. Créez une branche pour votre fonctionnalité (`git checkout -b feature/AmazingFeature`)
3. Committez vos changements (`git commit -m 'Add some AmazingFeature'`)
4. Poussez vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrez une Pull Request (utilisez le template fourni)

## 📄 Licence

Ce projet est sous licence MIT - voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 📞 Support

Pour toute question ou problème :
- Ouvrez une [Issue](https://github.com/PtiCalin/IFT2720_Laboratoire3_Collecte_Personnage/issues)
- Contactez l'équipe de développement

## 🎓 Contexte Académique

**Cours:** IFT2720 - Laboratoire 3  
**Institution:** [Votre Institution]  
**Année Académique:** 2024-2025  
**Objectif:** Apprentissage de Unity Physics et des systèmes de gameplay

---

**Fait avec ❤️ et Unity**
