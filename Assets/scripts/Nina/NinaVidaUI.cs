using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NinaVidaUI : MonoBehaviour
{
    [Header("Refer�ncias")]
    public NinaVida ninaVida;
    public RectTransform barraVida;   // RectTransform da barra vermelha

    [Header("Configura��es")]
    public float velocidadeLerp = 5f; // Suavidade da anima��o da barra

    private float larguraInicial;
    private float alvoLargura;

    private void Start()
    {
        if (ninaVida == null)
            ninaVida = FindFirstObjectByType<NinaVida>();

        larguraInicial = barraVida.sizeDelta.x;
        alvoLargura = larguraInicial;

        if (ninaVida != null)
            ninaVida.OnVidaMudou += AtualizarBarra;
    }

    private void Update()
    {
        // Lerp suave da largura
        Vector2 size = barraVida.sizeDelta;
        size.x = Mathf.Lerp(size.x, alvoLargura, Time.deltaTime * velocidadeLerp);
        barraVida.sizeDelta = size;
    }

    public void AtualizarBarra(float vidaAtual, float vidaMax)
    {
        alvoLargura = (vidaAtual / vidaMax) * larguraInicial;
    }
}
