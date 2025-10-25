using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public static GameOverMenu Instance;

    public GameObject painelGameOver;
    public TextMeshProUGUI textoMensagem;

    [Header("Áudio")]
    public AudioSource musicaCenario;
    public AudioSource musicaGameOver;  

    void Awake()
    {
        Instance = this;

        if (painelGameOver != null)
            painelGameOver.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MostrarGameOver(string mensagem)
    {
        Time.timeScale = 0f;
        textoMensagem.text = mensagem;
        painelGameOver.SetActive(true);

        // Para a música de cenário
        if (musicaCenario != null && musicaCenario.isPlaying)
            musicaCenario.Stop();

        // Toca a música de Game Over
        if (musicaGameOver != null && !musicaGameOver.isPlaying)
            musicaGameOver.Play();
    }

    public void ReiniciarCena()
    {
        Time.timeScale = 1f;
        StartCoroutine(RecarregarCena());
    }

    private IEnumerator RecarregarCena()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void VoltarMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
