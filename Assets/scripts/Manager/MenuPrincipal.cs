using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public SceneFader fader;
    public void IniciarJogo()
    {
        fader.FadeToScene("PlatformCutScene");
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
