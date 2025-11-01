using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void IniciarJogo()
    {
        SceneManager.LoadScene("PlatformCutScene");
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
