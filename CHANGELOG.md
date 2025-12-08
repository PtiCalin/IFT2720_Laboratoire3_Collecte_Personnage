# Changelog

Tous les changements notables de ce projet seront documentés dans ce fichier.


## [Unreleased]

- À définir.

---

## [4.1.2] - 2025-12-08

- Renommage final des scripts principaux : `Player`, `Level`, `Scene`, `Camera`, `UI`; suppression des anciens (`PlayerController`, `LevelGenerator`, `SceneSetup`, `CameraRigController`, `GameManager`, `CameraInputActions`).
- Caméra unifiée avec bascule Tab (third-person ↔ bird's-eye), gestion des entrées souris, clamp des limites selon le niveau.
- Contrôleur joueur refondu (accélération, vitesse max, contrôle aérien, drag sol/air, raycast de sol, pile de sauts configurable, rotation douce) et exposition des propriétés via le niveau.
- Générateur de niveau (`Level`) : configuration directe des collectibles, bornes caméra, spawns et liaisons joueur/caméra/UI harmonisés.
- UI singleton : suivi temps réel des pièces/trésors/score, rafraîchi par les collectibles.
- Documentation réalignée (`README.md`, `TESTING.md`, `CONTRIBUTING.md`) et suppression des fichiers/meta obsolètes.

---

## [3.0.1] - 2025-12-07

- Génération procédurale du labyrinthe avec entrée et sortie uniques stabilisée.
- Support des prefabs joueur/collectibles depuis l'Inspector avec paramètres exposés.
- Placement des collectibles par grille de cellules réservées pour éviter les collisions.
- Matériaux auto-gérés via `Assets/Materials` et recherche de textures Poliigon.
- Caméra unifiée gérant l'orbite third-person et la vue aérienne.

---

## [3.0.0] - 2025-12-06

- Passage complet à l'Input System pour les actions joueur/caméra (bindings clavier/souris, axes souris).
- Bascule de caméra (3e personne ↔ vue aérienne) avec verrouillage/déverrouillage du curseur.
- UI centralisée en singleton pour suivre pièces, trésors et score total en temps réel.
- Ajustement des prefabs (joueur, collectibles) pour exposer les paramètres dans l'Inspector.

---

## [2.0.1] - 2025-12-05

- Affinage du placement des collectibles et équilibrage des valeurs (coins/treasures).
- Corrections de colliders et de triggers pour fiabiliser la collecte.
- Nettoyage des matériaux manquants et fallback systématique Poliigon.

---

## [2.0.0] - 2025-12-04

- Générateur de niveau enrichi avec réservations de cellules pour l'entrée, la sortie et les collectibles.
- Ajout de collectibles distincts (coins, treasures) avec valeurs configurables et animations de rotation/bobbing.
- Documentation initiale de la structure du projet et des contrôles dans le `README.md`.

---

## [1.1.2] - 2025-12-03

- Amélioration du contrôleur joueur : meilleure détection de sol et ajustement du drag.
- Première passe d'organisation des headers et paramètres dans l'Inspector.
- Corrections mineures de la scène de test et des prefabs.

---

## [1.1.1] - 2025-12-02

- Ajout de la caméra orbitale autour du joueur avec limites de pitch et distance.
- Ajustements du mouvement caméra-relatif et affinage du saut.
- Préparation du terrain pour la génération procédurale (structure de grille et murs).

---

## [1.1.0] - 2025-12-01

- Première version du labyrinthe procédural (backtracking) avec murs générés en grille.
- Contrôleur joueur physique simple (Rigidbody) : déplacement caméra-relatif et saut basique.
- Préparation des prefabs de base pour le joueur et les collectibles.

---

## [1.0.2] - 2025-11-30

- Correctifs UI (TextMeshPro) pour l'affichage des pièces et du score.
- Ajustement des touches de déplacement et du saut pour cohérence clavier AZERTY/QWERTY.
- Ajustements mineurs sur le terrain et la lumière de scène.

---

## [1.0.1] - 2025-11-29

- Raffinement du prototype : feedback visuel sur la collecte et destruction propre des pièces.
- Mise à jour de la configuration URP et des matériaux de base.
- Ajout d'un premier README et des fichiers de licence/contribution.

---

## [1.0.0] - 2025-11-28

- Prototype initial : scène de collecte simple avec contrôles clavier, saut et collecte de pièces.
- UI TextMeshPro affichant le nombre de pièces et le score.
- Import des assets de base et configuration URP minimale.

---

## Types de changements

- `Added` – nouvelles fonctionnalités.
- `Changed` – ajustements de fonctionnalités existantes.
- `Removed` – suppressions.
- `Fixed` – corrections de bugs.
