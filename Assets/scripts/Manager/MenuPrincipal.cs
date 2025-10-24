using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void IniciarJogo()
    {
        SceneManager.LoadScene("PlatformScene");
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
