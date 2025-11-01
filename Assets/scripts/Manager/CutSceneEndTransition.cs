using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneEndTransition : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("Nome da próxima cena que será carregada após o fim da cutscene")]
    public string nextSceneName;

    [Tooltip("Referência para o SceneFader (se vazio, será encontrado automaticamente)")]
    public SceneFader sceneFader;

    [Tooltip("Tempo de espera após o fim da cutscene antes de iniciar o fade")]
    public float delayAfterCutscene = 1f;

    private PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        if (director != null)
        {
            // Quando a cutscene termina, chama o método
            director.stopped += OnCutsceneEnd;
        }
        else
        {
            Debug.LogWarning("Nenhum PlayableDirector encontrado neste GameObject!");
        }
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        Invoke(nameof(GoToNextScene), delayAfterCutscene);
    }

    private void GoToNextScene()
    {
        if (sceneFader == null)
        {
            sceneFader = FindFirstObjectByType<SceneFader>();
        }

        if (sceneFader != null)
        {
            sceneFader.FadeToScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("SceneFader não encontrado — carregando cena diretamente...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
