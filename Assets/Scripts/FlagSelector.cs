using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necessario per cambiare scena

public class MenuManager : MonoBehaviour
{
    // per salvare la bandiera scelta
    // stringa per semplicità
    public static string bandieraScelta;

    // immagini tutte bandiere
    public Image[] bandiere;

    // riferimento all'immagine della bandiera selezionata
    private Image bandieraSelezionata;

    // Questo metodo verrà chiamato da ogni pulsante-bandiera.
    // Passiamo il nome della texture della bandiera come parametro.
    public void selectFlag(string nomeBandiera)
    {
        bandieraScelta = nomeBandiera;
        Debug.Log("Bandiera scelta: " + bandieraScelta); // Messaggio di controllo

        foreach(Image bandiera in bandiere) {
            Outline outline = bandiera.GetComponent<Outline>();

            if (bandiera.name == nomeBandiera) {
                // atttivo outline x la bandiera
                if (outline != null) outline.enabled = true;

                bandieraSelezionata = bandiera;

            } else {
                if (outline != null) outline.enabled = false;
            }
        }

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
        Debug.Log("Caricamento scena Game con bandiera: " + bandieraScelta);
        SceneManager.LoadScene("Game");
    }
}
