# 🎯 Vérification du Sol - OnCollisionEnter

Guide complet sur l'implémentation de la détection du sol utilisant `OnCollisionEnter` pour le Laboratoire 3.

---

## 📋 Concept

La **détection du sol** est essentielle pour permettre au joueur de sauter. Sans elle, le joueur pourrait sauter infiniment en l'air.

### Objectif
Utiliser `OnCollisionEnter` pour vérifier si le joueur touche le sol, permettant de sauter à nouveau.

---

## 🔍 Comprendre OnCollisionEnter

### Qu'est-ce que OnCollisionEnter?

`OnCollisionEnter` est une méthode de callback Unity qui est appelée **quand deux objets avec des colliders commencent à se toucher**.

### Signature de la méthode

```csharp
void OnCollisionEnter(Collision collision)
{
    // Code exécuté quand une collision commence
}
```

### Paramètre: Collision

L'objet `Collision` contient des informations sur la collision:
- `collision.gameObject` - L'objet avec lequel on a collisionné
- `collision.transform` - La transformation de l'objet
- `collision.relativeVelocity` - La vélocité relative
- `collision.contacts` - Les points de contact

---

## 💻 Implémentation

### Version de Base

```csharp
void OnCollisionEnter(Collision collision)
{
    // Vérifier si le joueur touche le sol
    if (collision.gameObject.CompareTag("Ground"))
    {
        // Le joueur est maintenant au sol et peut sauter
        isGrounded = true;
    }
}

void OnCollisionExit(Collision collision)
{
    // Quand le joueur quitte le sol, il ne peut plus sauter
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = false;
    }
}
```

### Points Importants

1. **CompareTag() pour la Performance**
   ```csharp
   if (collision.gameObject.CompareTag("Ground"))
   ```
   - Plus rapide que `collision.gameObject.tag == "Ground"`
   - Évite les allocations de mémoire supplémentaires

2. **Utilisation d'un Tag**
   - Assigne le tag "Ground" à tous les objets de sol
   - Permet de distinguer le sol d'autres collisions
   - Plus flexible et maintenable

3. **OnCollisionExit pour Quitter le Sol**
   ```csharp
   void OnCollisionExit(Collision collision)
   {
       if (collision.gameObject.CompareTag("Ground"))
       {
           isGrounded = false;
       }
   }
   ```
   - S'exécute quand la collision se termine
   - Met à jour `isGrounded` quand le joueur saute

---

## 🔧 Configuration dans Unity

### Étape 1: Créer le Tag "Ground"

1. **Dans Unity Editor:**
   - Sélectionnez votre GameObject Ground
   - Inspector → Tag dropdown → Add Tag
   - Créez un nouveau tag: `Ground`

2. **Assigner le tag:**
   - Select Ground GameObject
   - Tag dropdown → Select "Ground"

### Étape 2: Vérifier les Colliders

**Pour le Player:**
- ✅ Doit avoir un Collider (Capsule, Box, etc.)
- ✅ **Ne doit PAS être en Trigger** (Is Trigger: unchecked)
- ✅ Doit avoir un Rigidbody

**Pour le Ground:**
- ✅ Doit avoir un Collider (Box, Plane, etc.)
- ✅ **Ne doit PAS être en Trigger** (Is Trigger: unchecked)
- ✅ Peut être kinematic ou dynamique

### Étape 3: Configuration du Rigidbody

**Player Rigidbody:**
```
Mass: 1
Drag: 0
Angular Drag: 0.05
Use Gravity: ✓ (Checked)
Is Kinematic: ☐ (Unchecked)
Interpolation: Interpolate (pour smooth motion)
Collision Detection: Continuous (pour éviter tunneling)
Constraints:
  - Freeze Rotation X: ✓
  - Freeze Rotation Z: ✓
```

---

## 🧪 Tests et Vérification

### Test 1: Détection du Sol

1. **Créez une scène simple:**
   - Un Cube pour le Player (avec Rigidbody)
   - Un Plane pour le Ground (avec tag "Ground")
   - Camera positionnée pour voir les deux

2. **Exécutez le jeu:**
   ```csharp
   // Ajoutez du debug dans OnCollisionEnter
   void OnCollisionEnter(Collision collision)
   {
       if (collision.gameObject.CompareTag("Ground"))
       {
           Debug.Log("Joueur touche le sol!");
           isGrounded = true;
       }
   }
   ```

3. **Vérifications:**
   - Jouez et observez la Console
   - "Joueur touche le sol!" doit s'afficher au contact
   - Le message disparaît quand le joueur saute

### Test 2: Saut

1. **Testez le saut:**
   - Appuyez sur Espace
   - Le joueur doit sauter
   - isGrounded doit être true

2. **En l'air:**
   - Après le saut, isGrounded = false
   - Espacer n'a aucun effet
   - Le joueur tombe

### Test 3: Plateformes Multiples

1. **Créez 2-3 plateformes:**
   - Assignez-les toutes le tag "Ground"
   - Placez-les à différentes hauteurs

2. **Testez:**
   - Sautez sur chaque plateforme
   - isGrounded doit se mettre à true à chaque contact
   - Saut doit être possible sur chaque plateforme

---

## 📊 Comparaison: OnCollisionEnter vs OnCollisionStay

| Aspect | OnCollisionEnter | OnCollisionStay |
|--------|------------------|-----------------|
| Quand appelée | Quand collision **commence** | Chaque frame **pendant** la collision |
| Fréquence | Une fois | Plusieurs fois |
| Performance | Meilleure | Peut être plus lourde |
| Pour l'état | Parfait | Peut être redondant |
| Exemple | Sauter au sol | Rester au sol |

**Meilleure pratique:** 
- Utiliser `OnCollisionEnter` pour détecter **le sol**
- Utiliser `OnCollisionStay` pour détecter **rester au sol** (moins courant)

---

## ❗ Problèmes Courants et Solutions

### Problème 1: isGrounded reste false

**Symptôme:** Impossible de sauter

**Causes possibles:**
1. Le tag "Ground" n'est pas assigné au sol
2. Le Collider du sol est en Trigger
3. Le Player n'a pas de Collider
4. Le Player n'a pas de Rigidbody

**Solutions:**
```csharp
// Ajoutez du debug
void OnCollisionEnter(Collision collision)
{
    Debug.Log("Collision avec: " + collision.gameObject.name);
    Debug.Log("Tag: " + collision.gameObject.tag);
    
    if (collision.gameObject.CompareTag("Ground"))
    {
        Debug.Log("C'est le sol!");
        isGrounded = true;
    }
}
```

### Problème 2: isGrounded true mais ne saute pas

**Symptôme:** Détection fonctionne, saut ne fonctionne pas

**Causes possibles:**
1. Le Rigidbody est Kinematic
2. InputGetKeyDown ne fonctionne pas
3. jumpForce est trop faible

**Solutions:**
```csharp
// Vérifiez les conditions
if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
{
    Debug.Log("Space: " + Input.GetKeyDown(KeyCode.Space));
    Debug.Log("isGrounded: " + isGrounded);
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
}
```

### Problème 3: Le joueur tombe à travers le sol

**Symptôme:** Le joueur traverse le sol

**Causes possibles:**
1. Collision Detection n'est pas "Continuous"
2. Le sol n'a pas de Collider
3. Rigidbody trop léger ou forces trop fortes

**Solutions:**
```
Rigidbody → Collision Detection: Continuous
Ground → Add Collider si absent
Rigidbody → Mass: Augmentez si nécessaire
```

### Problème 4: OnCollisionEnter ne s'appelle pas

**Symptôme:** Aucune collision détectée

**Vérification:**
```csharp
// Ajoutez ceci au Start()
void Start()
{
    rb = GetComponent<Rigidbody>();
    
    // Debug
    if (rb == null) Debug.LogError("Rigidbody manquant!");
    if (GetComponent<Collider>() == null) Debug.LogError("Collider manquant!");
}
```

---

## 📈 Optimisations

### Utiliser Raycast pour Plus de Précision

Alternativement, vous pouvez utiliser un Raycast au lieu de collisions:

```csharp
void Update()
{
    // Raycast vers le bas
    RaycastHit hit;
    isGrounded = Physics.Raycast(
        transform.position,
        Vector3.down,
        out hit,
        1f  // Distance
    );
}
```

**Avantages du Raycast:**
- Plus de contrôle sur la détection
- Meilleure précision
- Pas affecté par les collisions
- Plus performant pour de nombreux raycast

---

## 📚 Code Complet

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Paramètres de Déplacement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    
    private Rigidbody rb;
    private bool isGrounded = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // Déplacement
        float move = Input.GetAxis("Horizontal") * moveSpeed;
        transform.Translate(move * Time.deltaTime, 0, 0);
        
        // Saut - Utilise OnCollisionEnter pour vérifier si au sol
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    // Vérification du Sol: Utilise OnCollisionEnter pour vérifier 
    // si le joueur touche le sol, permettant de sauter à nouveau
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Joueur touche le sol");
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Joueur quitte le sol");
        }
    }
}
```

---

## ✅ Checklist Implémentation

- [ ] Tag "Ground" créé et assigné au sol
- [ ] OnCollisionEnter implémenté
- [ ] OnCollisionExit implémenté
- [ ] Utilisation de CompareTag() pour performance
- [ ] Vérification du tag "Ground"
- [ ] isGrounded mis à jour correctement
- [ ] Player et Ground ont des Colliders
- [ ] Collision Detection configurée correctement
- [ ] Tests effectués et fonctionnent
- [ ] Debug temporaires ajoutés pour vérification

---

## 🎓 Ressources Additionnelles

### Documentation Unity
- [OnCollisionEnter](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter.html)
- [OnCollisionExit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit.html)
- [Collision](https://docs.unity3d.com/ScriptReference/Collision.html)
- [CompareTag](https://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html)

### Sujets Connexes
- Colliders et Rigidbodies
- Tags et Layers
- Physics Timestep
- Raycasting pour détection alternative

---

**Implémentation réussie! 🚀**

Votre détection du sol devrait maintenant fonctionner correctement avec OnCollisionEnter!
