# Guide de Contribution

Merci de votre intérêt pour contribuer au projet **Laboratoire 3 - Collecte de Personnage** ! 🎉

Ce document fournit des lignes directrices pour contribuer au projet.

## 📋 Table des Matières

- [Code de Conduite](#code-de-conduite)
- [Comment Contribuer](#comment-contribuer)
- [Processus de Développement](#processus-de-développement)
- [Standards de Code](#standards-de-code)
- [Structure des Commits](#structure-des-commits)
- [Pull Requests](#pull-requests)
- [Rapporter des Bugs](#rapporter-des-bugs)
- [Suggérer des Fonctionnalités](#suggérer-des-fonctionnalités)

## 🤝 Code de Conduite

Ce projet adhère à un code de conduite pour assurer un environnement accueillant et respectueux pour tous.

### Nos Engagements

- Utiliser un langage accueillant et inclusif
- Respecter les points de vue et expériences différents
- Accepter gracieusement les critiques constructives
- Se concentrer sur ce qui est meilleur pour la communauté

## 🚀 Comment Contribuer

### 1. Fork et Clone

```bash
# Forkez le repo sur GitHub, puis clonez votre fork
git clone https://github.com/VOTRE-USERNAME/IFT2720_Laboratoire3_Collecte_Personnage.git
cd IFT2720_Laboratoire3_Collecte_Personnage

# Ajoutez le repo upstream
git remote add upstream https://github.com/PtiCalin/IFT2720_Laboratoire3_Collecte_Personnage.git
```

### 2. Créer une Branche

```bash
# Mettez à jour votre main
git checkout main
git pull upstream main

# Créez une branche pour votre fonctionnalité
git checkout -b feature/ma-nouvelle-fonctionnalite

# Ou pour un bug fix
git checkout -b fix/correction-bug-specific
```

### 3. Faire vos Changements

- Écrivez du code propre et bien documenté
- Suivez les conventions de code du projet
- Ajoutez des commentaires pour les sections complexes
- Testez vos changements dans Unity

### 4. Commit

```bash
# Ajoutez vos fichiers modifiés
git add .

# Committez avec un message descriptif
git commit -m "feat: Ajoute système de particules pour collectibles"
```

### 5. Push et Pull Request

```bash
# Poussez vers votre fork
git push origin feature/ma-nouvelle-fonctionnalite
```

Ensuite, créez une Pull Request sur GitHub en utilisant notre template.

## 🔄 Processus de Développement

### Workflow Git

1. **Main Branch**: Code stable et prêt pour production
2. **Feature Branches**: Nouvelles fonctionnalités (`feature/nom-fonctionnalite`)
3. **Fix Branches**: Corrections de bugs (`fix/description-bug`)
4. **Hotfix Branches**: Corrections urgentes (`hotfix/probleme-critique`)

### Avant de Committer

- [ ] Le code compile sans erreurs
- [ ] Le code fonctionne dans Unity Editor
- [ ] Les tests passent (si applicable)
- [ ] La documentation est à jour
- [ ] Les commentaires sont clairs et en français
- [ ] Pas de warnings dans la console Unity

## 📝 Standards de Code

### Convention de Nommage C#

```csharp
// Classes: PascalCase
public class Player { }

// Méthodes: PascalCase
private void HandleMovement() { }

// Variables privées: camelCase avec underscore
private float _moveSpeed;
private Rigidbody _rb;

// Variables publiques/SerializeField: camelCase
[SerializeField] private float moveSpeed;
public int playerHealth;

// Constantes: UPPER_SNAKE_CASE
private const float MAX_SPEED = 10f;

// Properties: PascalCase
public bool IsGrounded { get; private set; }
```

### Organisation du Code

```csharp
public class ExempleScript : MonoBehaviour
{
    // 1. Serialized Fields (groupés par Header)
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    
    // 2. Variables privées
    private Rigidbody rb;
    private bool isGrounded;
    
    // 3. Properties
    public bool IsMoving => rb.velocity.magnitude > 0.1f;
    
    // 4. Unity Lifecycle Methods
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    private void FixedUpdate() { }
    
    // 5. Méthodes publiques
    public void Jump() { }
    
    // 6. Méthodes privées
    private void HandleMovement() { }
    
    // 7. Event Handlers
    private void OnCollisionEnter(Collision collision) { }
    
    // 8. Gizmos et Debug
    private void OnDrawGizmos() { }
}
```

### Commentaires

```csharp
// Commentaires sur une ligne pour des notes courtes

/// <summary>
/// Commentaires XML pour les méthodes publiques importantes
/// </summary>
/// <param name="speed">La vitesse du mouvement</param>
/// <returns>True si le mouvement a réussi</returns>
public bool Move(float speed)
{
    // Implémentation
}

/*
 * Commentaires multi-lignes pour des explications complexes
 * ou des sections importantes du code
 */
```

## 📦 Structure des Commits

Nous utilisons [Conventional Commits](https://www.conventionalcommits.org/) pour des messages de commit clairs.

### Format

```
<type>(<scope>): <description>

[corps optionnel]

[footer optionnel]
```

### Types

- `feat`: Nouvelle fonctionnalité
- `fix`: Correction de bug
- `docs`: Changements de documentation uniquement
- `style`: Formatage, points-virgules manquants, etc.
- `refactor`: Refactoring du code sans changer le comportement
- `perf`: Améliorations de performance
- `test`: Ajout ou correction de tests
- `chore`: Maintenance, configuration, etc.

### Exemples

```bash
feat(player): Ajoute système de double jump

fix(collectible): Corrige animation de rotation qui saute

docs(readme): Met à jour instructions d'installation

refactor(ui): Simplifie logique de score

perf(physics): Optimise détection de collision

test(player): Ajoute tests unitaires pour mouvement
```

## 🔍 Pull Requests

### Checklist PR

Avant de soumettre une PR, assurez-vous que:

- [ ] La branche est à jour avec `main`
- [ ] Le code suit les standards du projet
- [ ] Tous les tests passent
- [ ] La documentation est mise à jour
- [ ] Les commits sont bien structurés
- [ ] Le template de PR est rempli complètement

### Processus de Review

1. **Soumission**: Créez la PR avec le template rempli
2. **Review Automatique**: Les checks CI doivent passer
3. **Review Humaine**: Au moins 1 approbation requise
4. **Modifications**: Adressez les commentaires de review
5. **Merge**: Le mainteneur mergera votre PR

### Taille des PRs

- **Petites PRs** sont préférées (< 400 lignes)
- Une PR = Une fonctionnalité/fix
- Divisez les grandes fonctionnalités en plusieurs PRs

## 🐛 Rapporter des Bugs

### Avant de Rapporter

1. Vérifiez que le bug n'a pas déjà été rapporté
2. Assurez-vous d'utiliser la dernière version
3. Collectez les informations nécessaires

### Template de Bug Report

Utilisez le template d'issue GitHub qui inclut:

- **Description**: Que se passe-t-il?
- **Reproduction**: Étapes pour reproduire
- **Comportement attendu**: Ce qui devrait se passer
- **Captures d'écran**: Si applicable
- **Environnement**: Version Unity, OS, etc.
- **Logs**: Messages d'erreur de la console

## ✨ Suggérer des Fonctionnalités

### Template de Feature Request

- **Problème**: Quel problème cela résout-il?
- **Solution**: Comment envisagez-vous la fonctionnalité?
- **Alternatives**: Autres approches considérées?
- **Contexte**: Cas d'usage et exemples

### Priorisation

Les fonctionnalités sont priorisées selon:

1. Alignement avec les objectifs du projet
2. Impact sur l'expérience utilisateur
3. Complexité d'implémentation
4. Demande de la communauté

## 🧪 Tests

### Tests dans Unity

```csharp
// Tests de gameplay
1. Testez en mode Play dans l'Editor
2. Vérifiez les différents scénarios
3. Testez les cas limites

// Vérifications
- Pas d'erreurs dans la console
- Performance acceptable
- Comportement attendu
```

### Builds

Si possible, testez sur un build:

```bash
# Build Windows
File > Build Settings > Build

# Testez l'exécutable généré
```

## 📚 Documentation

### Quand Mettre à Jour la Documentation

- Nouvelles fonctionnalités
- Changements d'API
- Nouveaux paramètres configurables
- Changements de comportement

### Fichiers à Mettre à Jour

- `README.md`: Documentation principale
- `CHANGELOG.md`: Journal des modifications
- Commentaires de code: Documentation inline
- Wiki (si applicable): Guides détaillés

## 🎯 Domaines de Contribution

### Priorités Actuelles

1. **Gameplay**: Mécaniques de jeu, contrôles
2. **UI/UX**: Interface utilisateur, menus
3. **Audio**: Sons, musique
4. **Effets**: Particules, post-processing
5. **Optimisation**: Performance, mémoire

### Bonnes Premières Issues

Cherchez les labels:

- `good first issue`: Parfait pour débuter
- `help wanted`: Contributions bienvenues
- `documentation`: Améliorations de docs

## ❓ Questions

### Besoin d'Aide?

- **Issues**: Pour des questions spécifiques au projet
- **Discussions**: Pour des discussions générales
- **Email**: Contact direct avec les mainteneurs

### Ressources

- [Documentation Unity](https://docs.unity3d.com/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Git Documentation](https://git-scm.com/doc)

---

## 🙏 Remerciements

Merci à tous les contributeurs qui aident à améliorer ce projet!

**Contributeurs principaux:**
- [PtiCalin](https://github.com/PtiCalin) - Créateur et mainteneur principal

---

**Des questions? N'hésitez pas à ouvrir une issue ou à contacter l'équipe!**

**Bon développement! 🚀**
