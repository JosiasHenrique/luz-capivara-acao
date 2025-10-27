using UnityEngine;
using UnityEngine.UI;

public class NinaVidaUI : MonoBehaviour
{
    [Header("Referências")]
    public NinaVida ninaVida;
    public RectTransform barraVida;
    public Image barraImagem;

    [Header("Configurações")]
    public float velocidadeLerp = 5f;
    public float velocidadePiscar = 5f; 

    private float larguraInicial;
    private float alvoLargura;
    private bool regenerando = false;
    private Color corOriginal;

    private void Start()
    {
        if (ninaVida == null)
            ninaVida = FindFirstObjectByType<NinaVida>();

        larguraInicial = barraVida.sizeDelta.x;
        alvoLargura = larguraInicial;

        if (barraImagem != null)
            corOriginal = barraImagem.color;

        if (ninaVida != null)
        {
            ninaVida.OnVidaMudou += AtualizarBarra;
            ninaVida.OnRegeneracaoMudou += AtualizarEfeitoRegeneracao;
        }
    }

    private void Update()
    {
        // Lerp suave da largura da barra
        Vector2 size = barraVida.sizeDelta;
        size.x = Mathf.Lerp(size.x, alvoLargura, Time.deltaTime * velocidadeLerp);
        barraVida.sizeDelta = size;

        // Efeito de regeneração (piscar em verde)
        if (barraImagem != null)
        {
            if (regenerando)
            {
                // Pisca entre a cor original e o verde
                float t = (Mathf.Sin(Time.time * velocidadePiscar) + 1f) / 2f;
                barraImagem.color = Color.Lerp(corOriginal, Color.green, t * 0.6f);
            }
            else
            {
                // Volta suavemente à cor original
                barraImagem.color = Color.Lerp(barraImagem.color, corOriginal, Time.deltaTime * 5f);
            }
        }
    }

    public void AtualizarBarra(float vidaAtual, float vidaMax)
    {
        alvoLargura = (vidaAtual / vidaMax) * larguraInicial;
    }

    private void AtualizarEfeitoRegeneracao(bool ativo)
    {
        regenerando = ativo;
    }
}
