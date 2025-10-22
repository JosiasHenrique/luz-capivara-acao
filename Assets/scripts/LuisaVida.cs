using UnityEngine;

public class LuisaVida : MonoBehaviour
{
    [Header("Configurações de vida")]
    public float maxVida = 100f;  // vida máxima da Luísa
    private float vidaAtual;

    private void Start()
    {
        vidaAtual = maxVida;

    }

    public void TakeDamage(float amount)
    {
        vidaAtual -= amount;
       // Debug.Log("Luísa levou dano! Vida atual: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
       // Debug.Log("Luísa morreu ??");

    }
}
