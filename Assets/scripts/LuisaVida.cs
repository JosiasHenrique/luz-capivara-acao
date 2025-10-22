using UnityEngine;

public class LuisaVida : MonoBehaviour
{
    [Header("Configura��es de vida")]
    public float maxVida = 100f;  // vida m�xima da Lu�sa
    private float vidaAtual;

    private void Start()
    {
        vidaAtual = maxVida;

    }

    public void TakeDamage(float amount)
    {
        vidaAtual -= amount;
       // Debug.Log("Lu�sa levou dano! Vida atual: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
       // Debug.Log("Lu�sa morreu ??");

    }
}
