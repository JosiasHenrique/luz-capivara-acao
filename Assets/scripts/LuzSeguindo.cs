using UnityEngine;

public class LuzSeguindo : MonoBehaviour
{
    public Transform personagem;
    public Vector3 offset = new Vector3(0, 3, 0);
    public float delay = 3f;

    private Vector3 velocidade = Vector3.zero;

    void Update()
    {
        if (personagem != null)
        {
            Vector3 destino = personagem.position + offset;

            // Movimento suave com "delay"
            transform.position = Vector3.SmoothDamp(
                transform.position,
                destino,
                ref velocidade,
                1f / delay
            );
        }
    }
}
