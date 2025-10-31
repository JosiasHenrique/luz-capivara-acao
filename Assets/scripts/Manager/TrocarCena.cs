using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TrocarCena : MonoBehaviour
{
    [Header("Nome da próxima cena")]
    [Tooltip("Nome exato da cena para onde o jogo vai ao entrar no gatilho")]
    public string nomeCenaDestino;

    [Header("Configurações de transição")]
    [Tooltip("Tempo de espera antes de iniciar o fade (opcional)")]
    public float delayAntesDoFade = 0.5f;

    [Tooltip("Referência opcional para o SceneFader na cena")]
    public SceneFader sceneFader;

    private bool trocandoCena = false;

    private void OnTriggerEnter(Collider other)
    {
        if (trocandoCena) return; // evita duplas trocas
        if (!other.CompareTag("Player")) return;

        trocandoCena = true;
        StartCoroutine(TrocarCenaComFade());
    }

    private IEnumerator TrocarCenaComFade()
    {
        // Espera um pequeno delay antes do fade (opcional)
        yield return new WaitForSeconds(delayAntesDoFade);

        // Se tiver SceneFader na cena, usa ele
        if (sceneFader != null)
        {
            sceneFader.FadeToScene(nomeCenaDestino);
        }
        else
        {
            // Tenta encontrar um automaticamente na cena atual
            sceneFader = FindFirstObjectByType<SceneFader>();

            if (sceneFader != null)
            {
                sceneFader.FadeToScene(nomeCenaDestino);
            }
            else
            {
                Debug.LogWarning("⚠️ Nenhum SceneFader encontrado! Carregando cena diretamente...");
                SceneManager.LoadScene(nomeCenaDestino);
            }
        }
    }
}
