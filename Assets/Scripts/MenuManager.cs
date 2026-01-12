using UnityEngine;
using UnityEngine.SceneManagement; // Necessario per cambiare scena

public class MenuManager : MonoBehaviour
{
    // per salvare la bandiera scelta
    // stringa per semplicità
    public static string bandieraScelta;

    // Questo metodo verrà chiamato da ogni pulsante-bandiera.
    // Passiamo il nome della texture della bandiera come parametro.
    public void SelezionaBandiera(string nomeBandiera)
    {
        bandieraScelta = nomeBandiera;
        Debug.Log("Bandiera scelta: " + bandieraScelta); // Messaggio di controllo

        // TODO: feedback visivo
    }

    // metodo per play
    public void Start()
    {
        // bandiera
        if (string.IsNullOrEmpty(bandieraScelta))
        {
            Debug.LogWarning("Nessuna bandiera selezionata!");
            return;
        }

        // carica scena di gioco
        SceneManager.LoadScene("Game");
    }
}
