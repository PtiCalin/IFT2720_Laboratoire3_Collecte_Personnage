# Changelog

Tous les changements notables de ce projet seront documentés dans ce fichier.


## [Unreleased]

- À définir.

---

## [4.1.0] - 2025-12-08

- Renommage des scripts principaux pour simplifier : `Player`, `Level`, `Scene`, `Camera`, `UI`; suppression des anciens (`PlayerController`, `LevelGenerator`, `SceneSetup`, `CameraRigController`, `GameManager`, `CameraInputActions`).
- Caméra unifiée dans un seul script `Camera` avec gestion intégrée des entrées (Tab toggle, souris) et modes third-person / bird's-eye, plus clamp des limites configurées par le niveau.
- Contrôleur joueur refondu (accélération, vitesse max, contrôle aérien, drag sol/air, raycast de sol, pile de sauts configurable, rotation douce) et raccordement des propriétés via le niveau.
- Générateur de niveau (`Level`) mis à jour : configuration directe des collectibles sans réflexion, exposition des bornes caméra, spawns et liaisons joueur/caméra/UI harmonisés.
- Gestion du score/UI déplacée dans `UI` (singleton) avec mise à jour coins/trésors/score en temps réel.
- Nettoyage et réalignement de la documentation (`README.md`, `TESTING.md`, `CONTRIBUTING.md`) sur les nouveaux noms et flux ; corrections de la table des contrôles et de l'architecture du projet.
- Suppression des fichiers obsolètes et meta associées.

---

## [4.0.0] - 2025-12-07

- Génération procédurale du labyrinthe avec entrée et sortie uniques.
- Support des prefabs de joueur et de collectibles depuis l'Inspector.
- Placement des collectibles basé sur une grille de cellules réservées.
- Matériaux auto-gérés via `Assets/Materials` et recherche de textures Poliigon.
- Caméra unifiée (`Camera`) gérant l'orbite third-person et la vue aérienne.

---

## [3.3.0] - 2025-12-06

- Passage complet à l'Input System pour les actions joueur/caméra ; configuration des bindings clavier/souris et axes souris.
- Ajout du basculement de caméra (3e personne ↔ vue aérienne) avec verrouillage/déverrouillage du curseur.
- UI centralisée en singleton pour suivre pièces, trésors et score total ; mise à jour en temps réel lors des triggers de collectibles.
- Ajustement des prefabs (joueur, collectibles) pour exposer les paramètres dans l'Inspector.

---

## [3.2.0] - 2025-12-04

- Enrichissement du générateur de niveau avec réservations de cellules pour l'entrée/sortie et les collectibles.
- Ajout de types de collectibles distincts (coins, treasures) avec valeurs configurables et animations de rotation/bobbing.
- Optimisation des matériaux : détection automatique des matériaux manquants et fallback Poliigon.
- Documentation initiale de la structure du projet et des contrôles dans le `README.md`.

---

## [3.1.0] - 2025-12-02

- Première version du labyrinthe procédural (backtracking) avec murs générés en grille.
- Contrôleur joueur physique simple (Rigidbody) : déplacement caméra-relatif et saut basique.
- Première version de la caméra orbitale autour du joueur.
- Mise en place des prefabs de base pour le joueur et les collectibles.

---

## [3.0.0] - 2025-11-28

- Prototype initial : scène de collecte avec contrôles clavier, saut et collecte de pièces.
- UI TextMeshPro affichant le nombre de pièces et le score.
- Configuration URP et assets de base importés.
- Ajout du squelette de documentation (licence MIT, contribution, TODO tests).

---

## Types de changements

- `Added` – nouvelles fonctionnalités.
- `Changed` – ajustements de fonctionnalités existantes.
- `Removed` – suppressions.
- `Fixed` – corrections de bugs.
