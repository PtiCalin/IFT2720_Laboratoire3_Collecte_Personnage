# 🎮 Laboratoire 3 - Collecte de Personnage avec Physique Unity

[![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B-blue.svg)](https://unity.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Mac%20%7C%20Linux-lightgrey.svg)](https://unity.com/)
[![Course](https://img.shields.io/badge/Course-IFT2720-purple.svg)](https://admission.umontreal.ca/cours-et-horaires/cours/ift-2720/)

Un projet de jeu 3D développé avec Unity, explorant les mécaniques de physique avancées avec **Rigidbody**, le contrôle de personnage basé sur les forces physiques, et l'implémentation d'un système de collecte d'objets interactifs.

## 📋 Table des Matières

- [Aperçu](#aperçu)
- [Objectifs du Laboratoire](#objectifs-du-laboratoire)
- [Fonctionnalités Implémentées](#fonctionnalités-implémentées)
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

Ce projet constitue le **Laboratoire 3** du cours **IFT2720 - Introduction au Multimédia** à l'Université de Montréal. Il s'agit d'une exploration approfondie des systèmes de physique Unity et de l'implémentation d'un contrôleur de personnage 3D utilisant exclusivement le composant **Rigidbody** pour le mouvement et les interactions.

Le laboratoire met l'accent sur la compréhension et l'application pratique des concepts suivants :
- **Physique Unity** : Utilisation du moteur physique pour créer des mouvements réalistes
- **Rigidbody Controller** : Contrôle de personnage basé sur les forces et vélocités
- **Système de Collecte** : Détection de collisions et interactions avec des objets
- **Game Management** : Architecture singleton et gestion d'état global
- **UI Dynamique** : Mise à jour en temps réel de l'interface utilisateur

### Contexte Académique

**Cours :** IFT2720 - Introduction au Multimédia  
**Laboratoire :** #3 - Collecte de Personnage  
**Objectif Principal :** Maîtriser les composants physiques Unity (Rigidbody, Collider, Forces) et créer un système de gameplay interactif complet.

## 🎓 Objectifs du Laboratoire

### Objectifs Pédagogiques Principaux

1. **Maîtrise du Rigidbody**
   - Comprendre les propriétés du Rigidbody (masse, drag, gravité)
   - Appliquer des forces et impulses pour le mouvement
   - Configurer l'interpolation et la détection de collision
   - Implémenter des contraintes de rotation et de position

2. **Contrôle de Personnage Basé sur la Physique**
   - Créer un mouvement fluide et réactif avec AddForce
   - Gérer le saut avec des impulses verticales
   - Implémenter un système de détection du sol robuste
   - Optimiser le drag pour différents états (sol/air)

3. **Système de Collecte Interactif**
   - Configurer des Colliders en mode Trigger
   - Détecter les collisions entre joueur et objets collectibles
   - Gérer la destruction d'objets et la mise à jour du score
   - Animer les collectibles pour améliorer l'expérience

4. **Architecture et Gestion d'État**
   - Implémenter le pattern Singleton pour le GameManager
   - Séparer les responsabilités entre différents scripts
   - Gérer la communication entre composants
   - Maintenir un code propre et bien structuré

5. **Intégration UI**
   - Utiliser TextMeshPro pour l'affichage
   - Mettre à jour dynamiquement les compteurs
   - Créer une interface informative et claire

### Compétences Développées

- ✅ Configuration et manipulation de composants physiques Unity
- ✅ Programmation orientée objet en C#
- ✅ Debug et optimisation de systèmes physiques
- ✅ Utilisation de SerializeField et l'Inspector Unity
- ✅ Gestion d'événements et callbacks Unity (OnTriggerEnter)
- ✅ Implémentation de patterns de design (Singleton)

## ✨ Fonctionnalités Implémentées

### Partie 1 : Contrôle du Personnage avec Rigidbody

#### Mouvement Physique Réaliste

- ⚡ **Mouvement horizontal fluide** avec système d'accélération progressive
- 🎯 **Vitesse maximale limitée** pour un contrôle prévisible
- 🏃 **Accélération configurable** pour ajuster la réactivité
- 🌪️ **Contrôle aérien réduit** - facteur de contrôle en l'air (50% par défaut)
- 🎯 **Détection de sol précise** avec raycast configurable
- 🔄 **Rotation visuelle** du personnage selon la direction du mouvement

#### Mécaniques de Saut Avancées
- 🦘 **Saut basique** avec force d'impulsion configurable
- 🎯 **Double saut** avec support multi-sauts configurable (extension)
- 🕐 **Coyote Time** - permet de sauter brièvement après avoir quitté le sol (extension)
- 📦 **Jump Buffering** - mémorise l'input de saut pour une réponse plus fluide (extension)
- 📏 **Hauteur de saut variable** - relâcher l'espace tôt pour des sauts plus courts (extension)

### Partie 1 : Système de Collecte d'Objets

#### Collectibles Interactifs
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

#### Configuration Rigidbody Optimale
- 🎮 **Interpolation** - mouvement fluide entre les frames physiques
- 🔍 **Collision Detection Continue** - prévention du tunneling à haute vitesse
- 🔒 **Contraintes de Rotation** - empêche les rotations indésirables sur les axes X et Z
- ⚙️ **Configuration automatique** - setup optimal au démarrage du script

#### Physique Améliorée
- ⚖️ **Multiplicateur de gravité** - gravité accrue en chute pour un meilleur feeling (1.5x)
- 🛑 **Limitation de vitesse de chute** - prévention des bugs de collision à haute vitesse
- 🌪️ **Facteur de contrôle aérien** - contrôle réduit en l'air pour plus de réalisme (50%)
- 📐 **Drag dynamique** - résistance différente au sol (5.0) et en l'air (2.0)

#### Outils de Développement
- 🔧 **Debug Gizmos** - visualisation de la détection du sol en mode Scene (ligne rouge)
- 📝 **Logs informatifs** - feedback console sur les actions importantes (sauts, collectes)
- 🎛️ **Paramètres exposés** - tous les réglages accessibles via l'Inspector Unity
- 📊 **Headers organisés** - interface Inspector claire avec sections (Movement, Advanced, Ground Check, etc.)

## 🎮 Exigences du Laboratoire

### Critères d'Évaluation (Conformité au TP)

Le projet répond aux exigences suivantes du Laboratoire 3 :

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
- ✓ Code bien structuré et commenté en anglais
- ✓ Utilisation appropriée de SerializeField
- ✓ Séparation claire des responsabilités entre scripts
- ✓ Headers pour organiser les paramètres dans l'Inspector
- ✓ Nommage cohérent et descriptif des variables

### Extensions et Améliorations (Au-delà des Exigences)

Le projet inclut également des fonctionnalités avancées :

🌟 **Double Jump System** - Permet plusieurs sauts consécutifs  
🌟 **Coyote Time** - Fenêtre de tolérance pour sauter après avoir quitté le sol  
🌟 **Jump Buffering** - Mémorisation de l'input de saut pour meilleure réactivité  
🌟 **Variable Jump Height** - Hauteur de saut modulable selon la durée de pression  
🌟 **Smooth Acceleration** - Accélération progressive au lieu de vitesse instantanée  
🌟 **Visual Rotation** - Rotation du modèle selon la direction du mouvement  
🌟 **Enhanced Gravity** - Gravité modifiée en chute pour meilleur feeling  
🌟 **Ground Check Point** - Point de vérification configurable  
🌟 **Debug Visualization** - Gizmos pour faciliter le développement

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

## 🎨 Assets & Credits

### 3D Models

**Low Poly 3D Treasure Items Game Assets**
- **Author:** [mehrasaur](https://mehrasaur.itch.io/)
- **Source:** [itch.io](https://mehrasaur.itch.io/treasure-pack)
- **License:** CC0 (Public Domain)
- **Published:** January 18, 2018
- **Updated:** March 04, 2018
- **Contents:** Chests, coins, gems (cut & uncut), keys, medals, and other treasure items
- **Formats:** .blend, .obj + .mtl, .fbx
- **Tags:** 3D, chest, coin, gem, Loot, Low-poly, treasure
- **Rating:** ⭐⭐⭐⭐⭐ (5.0/5 - 2 ratings)

These beautiful low-poly treasure models are used as collectibles throughout the game. The assets are provided without materials and are located in `Assets/Models/Collectibles/`.

## 📄 Licence

Ce projet est sous licence MIT - voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 📞 Support

Pour toute question ou problème :
- Ouvrez une [Issue](https://github.com/PtiCalin/IFT2720_Laboratoire3_Collecte_Personnage/issues)
- Contactez l'équipe de développement

## 🎓 Informations Académiques

**Cours :** IFT2720 - Introduction au Multimédia  
**Institution :** Université de Montréal  
**Laboratoire :** #3 - Collecte de Personnage  
**Année Académique :** 2024-2025  
**Professeur :** Lazhar Khelifi (lazhar.khelifi@umontreal.ca)  
**Objectif :** Maîtriser Unity Physics (Rigidbody) et créer un système de gameplay avec collecte d'objets

### Parties du Laboratoire

Ce laboratoire se compose de deux parties distinctes :

**Partie 1 - Contrôle du Personnage et Collecte (Ce Projet)**
- Implémentation d'un contrôleur Rigidbody avec physique réaliste
- Système de collecte d'objets (pièces et trésors)
- Détection du sol et mécaniques de saut avancées
- Interface utilisateur avec compteurs de score

**Partie 2 - Navigation IA avec NavMesh (Annexe Séparée)**
- Implémentation d'agents IA avec NavMesh
- Pathfinding et navigation automatique
- Comportements d'IA (patrouille, poursuite)
- Intégration IA-Joueur dans l'environnement

---

**Fait avec ❤️ et Unity**
