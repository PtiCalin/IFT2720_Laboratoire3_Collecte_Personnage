public class PlayerController : MonoBehaviour
{
    // Paramètres de déplacement
    [Header("Paramètres de Déplacement")]
    [Tooltip("Vitesse de déplacement horizontal")]
    public float moveSpeed = 5f;
    
    [Tooltip("Force du saut")]
    public float jumpForce = 5f;
    
    // Références aux composants
    /// <summary>
    /// rb : C'est une variable représentant le composant Rigidbody attaché à l'objet.
    /// Le Rigidbody est ce qui permet à l'objet d'être influencé par la physique dans Unity,
    /// comme la gravité ou les collisions. Sans Rigidbody, le personnage n'aurait pas de physique.
    /// </summary>
    private Rigidbody rb;
    
    // Variables d'état
    /// <summary>
    /// Pour vérifier si le joueur est au sol (true) ou en l'air (false).
    /// Cette variable est modifiée par OnCollisionEnter et OnCollisionExit
    /// pour détecter si le joueur peut sauter.
    /// </summary>
    private bool isGrounded;
    
    /// <summary>
    /// Méthode appelée au démarrage
    /// Récupère le composant Rigidbody attaché au joueur
    /// </summary>
    void Start()
    {
        // Récupérer le composant Rigidbody attaché au joueur
        rb = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// Méthode appelée à chaque frame
    /// Gère les entrées utilisateur et le mouvement
    /// </summary>
    void Update()
    {
        // Déplacement Horizontal
        // Utilise Input.GetAxis("Horizontal") pour obtenir une entrée de déplacement horizontal (gauche/droite)
        // qui permet une réponse analogique, idéale pour les manettes de jeu
        float move = Input.GetAxis("Horizontal") * moveSpeed;
        
        // Applique le mouvement en modifiant la vélocité du Rigidbody
        // On conserve la vélocité verticale (Y) pour ne pas affecter la gravité
        transform.Translate(move * Time.deltaTime, 0, 0);
        
        // Saut
        // Utilise Input.GetKeyDown(KeyCode.Space) pour détecter si le joueur appuie sur la barre d'espace
        // permettant au personnage de sauter si isGrounded est vrai
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Applique une force vers le haut pour faire sauter le personnage
            // Explication détaillée de rb.AddForce():
            
            // rb.AddForce() : Cette méthode applique une force au Rigidbody.
            // Elle peut être utilisée pour pousser des objets, les faire sauter, etc.
            // C'est l'équivalent d'appliquer une force physique réelle à l'objet.
            
            // new Vector3(0, jumpForce, 0) : Le vecteur de force appliqué.
            // Il est dirigé vers le haut car la composante Y (verticale) est définie par jumpForce,
            // tandis que les composantes X (horizontale) et Z (profondeur) sont à 0.
            // Cela signifie que la force est appliquée directement vers le haut,
            // sans mouvement horizontal ou en profondeur.
            
            // jumpForce : Une variable flottante qui détermine la magnitude de la force appliquée.
            // Plus cette valeur est élevée, plus l'objet sautera haut.
            // Par défaut, jumpForce = 5f, ce qui crée un saut naturel et modéré.
            
            // ForceMode.Impulse : ForceMode est un paramètre qui définit comment la force est appliquée.
            // Impulse applique une force instantanée qui tient compte de la masse de l'objet
            // pour calculer l'accélération. Cela est souvent utilisé pour des changements soudains
            // de vitesse ou de direction, comme un saut. Il y a d'autres modes :
            // - Force : applique une force continue (pas instantanée)
            // - Acceleration : ajoute directement à l'accélération
            // - VelocityChange : change directement la vélocité
            
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    /// <summary>
    /// Vérification du Sol: Utilise OnCollisionEnter pour vérifier si le joueur touche le sol
    /// permettant de sauter à nouveau
    /// </summary>
    /// <param name="collision">Information sur la collision détectée</param>
    void OnCollisionEnter(Collision collision)
    {
        // Vérifier si le joueur touche le sol
        // On utilise CompareTag pour une meilleure performance
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Le joueur est maintenant au sol et peut sauter
            isGrounded = true;
        }
    }
    
    /// <summary>
    /// Détecte quand le joueur quitte une collision avec le sol
    /// </summary>
    /// <param name="collision">Information sur la collision quittée</param>
    void OnCollisionExit(Collision collision)
    {
        // Quand le joueur quitte le sol, il ne peut plus sauter
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
