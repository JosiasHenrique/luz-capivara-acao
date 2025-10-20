using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public static GameOverMenu Instance;

    public GameObject painelGameOver;
    public TextMeshProUGUI textoMensagem;

    void Awake()
    {
        Instance = this;
        painelGameOver.SetActive(false);
    }

    public void MostrarGameOver(string mensagem)
    {
        Time.timeScale = 0f;
        textoMensagem.text = mensagem;
        painelGameOver.SetActive(true);
    }

    public void ReiniciarCena()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}