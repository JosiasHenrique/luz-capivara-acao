using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

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
            director.stopped += OnCutsceneEnd;
        else
            Debug.LogWarning("⚠️ Nenhum PlayableDirector encontrado neste GameObject!");
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        StartCoroutine(GoToNextScene());
    }

    private IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(delayAfterCutscene);

        if (sceneFader != null)
        {
            sceneFader.FadeToScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ SceneFader não encontrado — carregando cena diretamente...");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
