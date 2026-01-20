using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Bandiere Sprites")]
    public Sprite[] bandiere; // Array con tutte le bandiere disponibili
    
    [Header("Impostazioni Giocatori")]
    public int numGiocatoriPerSquadra = 4;
    public float playerScale = 0.2f;
    public int playerSortingOrder = 10;
    
    [Header("Posizioni Squadra Francia (Sinistra)")]
    public Vector2[] posizioniSquadraFrancia;
    
    [Header("Posizioni Squadra Avversaria (Destra)")]
    public Vector2[] posizioniSquadraAvversaria;
    
    [Header("Collider Settings")]
    public float colliderRadius = 0.5f;
    
    void Start()
    {
        // Recupera la bandiera scelta dal menu
        string bandieraScelta = MenuManager.bandieraScelta;
        
        if (string.IsNullOrEmpty(bandieraScelta))
        {
            Debug.LogWarning("Nessuna bandiera selezionata! Uso bandiera default.");
            bandieraScelta = "France_0"; // Default
        }
        
        Debug.Log("Creazione giocatori con bandiera: " + bandieraScelta);
        
        // Trova lo sprite della bandiera scelta
        Sprite spriteBandieraScelta = TrovaBandiera(bandieraScelta);
        Sprite spriteBandieraFrancia = TrovaBandiera("France");
        
        // Crea squadra Francia (sempre a sinistra)
        CreaSquadra("France_0", spriteBandieraFrancia, posizioniSquadraFrancia);
        
        // Crea squadra avversaria (a destra)
        CreaSquadra(bandieraScelta, spriteBandieraScelta, posizioniSquadraAvversaria);
    }
    
    Sprite TrovaBandiera(string nomeBandiera)
    {
        nomeBandiera = nomeBandiera + "_0";
        foreach (Sprite sprite in bandiere)
        {
            if (sprite.name == nomeBandiera)
            {
                return sprite;
            }
        }
        
        Debug.LogWarning("Bandiera " + nomeBandiera + " non trovata! Uso la prima disponibile.");
        return bandiere.Length > 0 ? bandiere[0] : null;
    }
    
    void CreaSquadra(string nomeSquadra, Sprite bandiera, Vector2[] posizioni)
    {
        // Crea un GameObject contenitore per la squadra
        GameObject squadraContainer = new GameObject("Team_" + nomeSquadra);
        squadraContainer.transform.parent = transform;
        
        // Crea ogni giocatore
        for (int i = 0; i < Mathf.Min(numGiocatoriPerSquadra, posizioni.Length); i++)
        {
            CreaPedina(nomeSquadra, i + 1, bandiera, posizioni[i], squadraContainer.transform);
        }
    }
    
    void CreaPedina(string squadra, int numero, Sprite bandiera, Vector2 posizione, Transform parent)
    {
        // Crea GameObject
        GameObject pedina = new GameObject("Player_" + squadra + "_" + numero);
        pedina.transform.parent = parent;
        pedina.transform.position = new Vector3(posizione.x, posizione.y, 0);
        pedina.transform.localScale = new Vector3(playerScale, playerScale, 1);
        
        // Aggiungi Sprite Renderer
        SpriteRenderer sr = pedina.AddComponent<SpriteRenderer>();
        sr.sprite = bandiera;
        sr.sortingOrder = playerSortingOrder;
        
        // Aggiungi Circle Collider 2D
        CircleCollider2D collider = pedina.AddComponent<CircleCollider2D>();
        collider.radius = colliderRadius;
        
        // Aggiungi Rigidbody2D (Kinematic!)
        Rigidbody2D rb = pedina.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}