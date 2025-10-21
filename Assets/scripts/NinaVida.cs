using UnityEngine;

public class NinaVida : MonoBehaviour
{
    public float maxVida = 100f;
    private float vidaAtual;

    private void Start()
    {
        vidaAtual = maxVida;
    }

    public void TakeDamage(float amount)
    {
        vidaAtual -= amount;
        Debug.Log("Nina levou dano! Vida atual: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Nina morreu 😵");
    }
}
