using UnityEngine;
using UnityEngine.Events;

public class LuisaVida : MonoBehaviour
{
    [Header("Configurações de vida")]
    public float maxVida = 100f;      // Vida máxima da Luísa
    [SerializeField] private float vidaAtual; // Vida atual (protegida no Inspector)

    [Header("Eventos")]
    public UnityEvent<float, float> OnVidaMudou; // evento (vidaAtual, maxVida)

    void Start()
    {
        vidaAtual = maxVida;
        OnVidaMudou?.Invoke(vidaAtual, maxVida);
    }

    public void TakeDamage(float amount)
    {
        vidaAtual -= amount;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, maxVida); // evita valores negativos
        OnVidaMudou?.Invoke(vidaAtual, maxVida);

        if (vidaAtual <= 0)
            Die();
    }

    public void Curar(float amount)
    {
        vidaAtual += amount;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, maxVida);
        OnVidaMudou?.Invoke(vidaAtual, maxVida);
    }

    private void Die()
    {
        Debug.Log("💀 Luísa morreu!");
        // Aqui no futuro podemos disparar animação, som ou reiniciar o combate
    }

    // Getter público (para leitura externa)
    public float VidaAtual => vidaAtual;
}
