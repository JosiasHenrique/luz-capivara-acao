using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    public GameObject painelPause;
    private bool estaPausado = false;

    void Awake()
    {
        Instance = this;

        if (painelPause != null)
            painelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Time.timeScale > 0 && Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (estaPausado)
            Continuar();
        else
            Pausar();
    }

    public void Pausar()
    {
        Time.timeScale = 0f;
        painelPause.SetActive(true);
        estaPausado = true;
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        painelPause.SetActive(false);
        estaPausado = false;
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
