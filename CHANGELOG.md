# Changelog

Tous les changements notables de ce projet seront documentés dans ce fichier.


## [Unreleased]

- Nettoyage des scripts (`Collectible`, `PlayerController`, `SceneSetup`) et élimination de méthodes non utilisées.
- Factorisation de la génération des collectibles dans `LevelGenerator`.
- Réécriture du `README.md` pour refléter l'état actuel du projet.
- Mise à jour du `CHANGELOG.md` et suppression des entrées obsolètes.

---

## [4.0.0] - 2025-12-07

- Génération procédurale du labyrinthe avec entrée et sortie uniques.
- Support des prefabs de joueur et de collectibles depuis l'Inspector.
- Placement des collectibles basé sur une grille de cellules réservées.
- Matériaux auto-gérés via `Assets/Materials` et recherche de textures Poliigon.
- Caméra unifiée (`CameraRigController`) gérant l'orbite third-person et la vue aérienne.

---

## Types de changements

- `Added` – nouvelles fonctionnalités.
- `Changed` – ajustements de fonctionnalités existantes.
- `Removed` – suppressions.
- `Fixed` – corrections de bugs.
