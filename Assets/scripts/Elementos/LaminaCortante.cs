using UnityEngine;

public class LaminaCortante : MonoBehaviour
{
    [Header("Dano da l�mina")]
    public float damage = 20f;

    [Header("Movimento da l�mina")]
    public float velocidadeRotacao = 180f;
    public Vector3 eixoRotacao = Vector3.up;

    [Header("�udio de ataque")]
    public AudioSource audioSourceLamina; // som da l�mina cortando
    public AudioClip somLamina;


    [Header("Dire��o da rota��o")]
    public bool sentidoHorario = true; // true = hor�rio, false = anti-hor�rio

    private void Update()
    {
        // Define o multiplicador da dire��o
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

            // Som da l�mina cortando
            if (audioSourceLamina != null && somLamina != null)
            {
                audioSourceLamina.PlayOneShot(somLamina);
            }

        }

    }

}
