using UnityEngine;

public class LuisaHitbox : MonoBehaviour
{
    [Header("Referência à Luísa")]
    public LuisaCombat luisaCombat; // para pegar estado e evitar dano fora do ataque

    [Header("Configuração de dano")]
    public float damage = 20f; // dano da investida

    private void OnTriggerEnter(Collider other)
    {
        // checa se colidiu com a Nina
        if (other.CompareTag("Player")) // certifique-se que a Nina está com tag "Player"
        {
            // só causa dano se a Luísa estiver atacando
            if (luisaCombat != null && luisaCombat.IsAttacking())
            {
                var ninaVida = other.GetComponent<NinaVida>();
                if (ninaVida != null)
                {
                    ninaVida.TakeDamage(damage);
                    Debug.Log("Luísa acertou a Nina e causou " + damage + " de dano!");
                }
            }
        }
    }
}
