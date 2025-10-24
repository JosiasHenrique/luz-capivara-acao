using UnityEngine;

public class LuisaHitbox : MonoBehaviour
{
    [Header("Refer�ncia � Lu�sa")]
    public LuisaCombat luisaCombat; // para pegar estado e evitar dano fora do ataque

    [Header("Configura��o de dano")]
    public float damage = 20f; // dano da investida

    private void OnTriggerEnter(Collider other)
    {
        // checa se colidiu com a Nina
        if (other.CompareTag("Player")) // certifique-se que a Nina est� com tag "Player"
        {
            // s� causa dano se a Lu�sa estiver atacando
            if (luisaCombat != null && luisaCombat.IsAttacking())
            {
                var ninaVida = other.GetComponent<NinaVida>();
                if (ninaVida != null)
                {
                    ninaVida.TakeDamage(damage);
                    Debug.Log("Lu�sa acertou a Nina e causou " + damage + " de dano!");
                }
            }
        }
    }
}
