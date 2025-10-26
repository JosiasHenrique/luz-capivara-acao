using UnityEngine;

public class LaminaCortante : MonoBehaviour
{
    [Header("Dano da lâmina")]
    public float damage = 20f;

    [Header("Movimento da lâmina")]
    public float velocidadeRotacao = 180f;
    public Vector3 eixoRotacao = Vector3.up;

    [Header("Áudio de ataque")]
    public AudioSource audioSourceLamina; // som da lâmina cortando
    public AudioClip somLamina;


    [Header("Direção da rotação")]
    public bool sentidoHorario = true; // true = horário, false = anti-horário

    private void Update()
    {
        // Define o multiplicador da direção
        float direcao = sentidoHorario ? 1f : -1f;
        transform.Rotate(eixoRotacao * velocidadeRotacao * direcao * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplica dano
            var nina = other.GetComponent<NinaVida>();
            if (nina != null)
            {
                nina.TakeDamage(damage);
            }

            // Som da lâmina cortando
            if (audioSourceLamina != null && somLamina != null)
            {
                audioSourceLamina.PlayOneShot(somLamina);
            }

        }

    }

}
