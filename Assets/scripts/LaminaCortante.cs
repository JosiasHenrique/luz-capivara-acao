using UnityEngine;

public class LaminaCortante : MonoBehaviour
{
    [Header("Dano da l�mina")]
    public float damage = 20f;

    [Header("Movimento da l�mina")]
    public float velocidadeRotacao = 180f;
    public Vector3 eixoRotacao = Vector3.up;

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
            Debug.Log("A l�mina atingiu a Nina!");

            var nina = other.GetComponent<NinaVida>();
            if (nina != null)
            {
                nina.TakeDamage(damage);
            }

            var animator = other.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("LevouDano");
            }
        }
    }
}
