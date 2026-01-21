using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Impostazioni Controllo")]
    public Color coloreSelezione = Color.yellow;
    public bool isSquadraGiocabile = true; // Imposta true per la squadra di sinistra
    
    [Header("Impostazioni Freccia")]
    public float maxForzaTiro = 15f;
    public float minForzaTiro = 2f;
    public float lunghezzaFrecciaMax = 3f;
    public Color coloreFreccia = Color.white;
    public float spessoreFreccia = 0.1f;
    
    [Header("Fisica")]
    public float attrito = 0.98f;
    public float velocitaMinima = 0.1f;
    
    private SpriteRenderer spriteRenderer;
    private Color coloreOriginale;
    private Rigidbody2D rb;
    
    // Variabili per il tiro
    private bool isDragging = false;
    private Vector2 puntoInizioTrascinamento;
    private LineRenderer lineRenderer;
    private GameObject frecciaObj;
    
    // Limiti campo
    private float minX = -8.5f;
    private float maxX = 8.5f;
    private float minY = -4.5f;
    private float maxY = 4.5f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coloreOriginale = spriteRenderer.color;
        rb = GetComponent<Rigidbody2D>();
        
        // Crea la freccia (inizialmente invisibile)
        CreaFreccia();
    }
    
    void CreaFreccia()
    {
        // Crea GameObject per la freccia
        frecciaObj = new GameObject("Freccia");
        frecciaObj.transform.parent = transform;
        frecciaObj.SetActive(false);
        
        // Aggiungi LineRenderer
        lineRenderer = frecciaObj.AddComponent<LineRenderer>();
        lineRenderer.startWidth = spessoreFreccia;
        lineRenderer.endWidth = spessoreFreccia / 2f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = coloreFreccia;
        lineRenderer.endColor = coloreFreccia;
        lineRenderer.positionCount = 2;
        lineRenderer.sortingOrder = 20;
        
        // Imposta le posizioni iniziali
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }
    
    void OnMouseDown()
    {
        // Solo se è la squadra giocabile e la pedina non si sta muovendo
        if (!isSquadraGiocabile || rb.linearVelocity.magnitude > velocitaMinima)
            return;
        
        // Salva la posizione iniziale del click
        puntoInizioTrascinamento = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
        
        // Evidenzia la pedina
        spriteRenderer.color = coloreSelezione;
        
        // Mostra la freccia
        frecciaObj.SetActive(true);
    }
    
    void OnMouseDrag()
    {
        if (!isDragging)
            return;
        
        // Calcola la direzione e la forza del tiro
        Vector2 posizioneAttuale = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direzione = puntoInizioTrascinamento - posizioneAttuale;
        
        // Limita la lunghezza massima
        float lunghezzaFreccia = Mathf.Min(direzione.magnitude, lunghezzaFrecciaMax);
        Vector2 direzioneFreccia = direzione.normalized * lunghezzaFreccia;
        
        // Aggiorna la freccia
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (Vector2)transform.position + direzioneFreccia);
        
        // Cambia colore in base alla forza
        float percentualeForza = lunghezzaFreccia / lunghezzaFrecciaMax;
        lineRenderer.startColor = Color.Lerp(Color.yellow, Color.red, percentualeForza);
        lineRenderer.endColor = Color.Lerp(Color.yellow, Color.red, percentualeForza);
    }
    
    void OnMouseUp()
    {
        if (!isDragging)
            return;
        
        // Calcola la forza del tiro
        Vector2 posizioneFinale = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direzione = puntoInizioTrascinamento - posizioneFinale;
        
        // Calcola la forza in base alla lunghezza del trascinamento
        float lunghezza = Mathf.Min(direzione.magnitude, lunghezzaFrecciaMax);
        float forza = Mathf.Lerp(minForzaTiro, maxForzaTiro, lunghezza / lunghezzaFrecciaMax);
        
        // Applica la forza
        if (direzione.magnitude > 0.1f)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = direzione.normalized * forza;
        }
        
        // Nascondi la freccia
        frecciaObj.SetActive(false);
        
        // Ripristina il colore
        spriteRenderer.color = coloreOriginale;
        
        isDragging = false;
    }
    
    void FixedUpdate()
    {
        // Applica attrito
        if (rb.linearVelocity.magnitude > velocitaMinima)
        {
            rb.linearVelocity *= attrito;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        // Mantieni la pedina dentro i limiti del campo
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}