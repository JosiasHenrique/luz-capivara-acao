using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("Configurações do Fade")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    private bool isFading = false; // Evita múltiplos fades ao mesmo tempo

    private void Start()
    {
        // Começa com fade-in 
        if (fadeImage != null)
            StartCoroutine(FadeIn());
    }

    /// Faz o fade-out e depois carrega a cena indicada.
    public void FadeToScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeDuration;
            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        isFading = false;
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isFading = true;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;
            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Garante que o fade terminou antes de trocar de cena
        yield return null;
        SceneManager.LoadScene(sceneName);
    }
}
