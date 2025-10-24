using UnityEngine;
using UnityEngine.UI;

public class LuisaVidaUI : MonoBehaviour
{
    public LuisaVida luisaVida;
    public Image healthFill;

    void Start()
    {
        if (luisaVida != null)
        {
            luisaVida.OnVidaMudou.AddListener(UpdateHealthBar);
            UpdateHealthBar(luisaVida.VidaAtual, luisaVida.maxVida);
        }
    }

    void UpdateHealthBar(float vidaAtual, float maxVida)
    {
        if (healthFill != null)
            healthFill.fillAmount = vidaAtual / maxVida;
    }
}
