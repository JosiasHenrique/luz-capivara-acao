using UnityEngine;
using UnityEngine.Audio;

public class DragaoVoador : MonoBehaviour
{
    [Header("Configurações de voo")]
    public Transform[] pontos; 
    public float velocidade = 5f;
    public float distanciaMinima = 1f;
    public bool loop = true;

    public AudioSource audioSource;

    private int pontoAtual = 0;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
            animator.SetBool("FlyingFWD", true);
    }

    void Update()
    {
        if (pontos.Length == 0) return;

        Transform alvo = pontos[pontoAtual];
        Vector3 direcao = (alvo.position - transform.position).normalized;

       
        Quaternion rot = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 2f);

       
        transform.position += direcao * velocidade * Time.deltaTime;

        if (Vector3.Distance(transform.position, alvo.position) < distanciaMinima)
        {
            audioSource.Play();
            pontoAtual++;
            pontoAtual++;

            if (pontoAtual >= pontos.Length)
            {
                if (loop)
                    pontoAtual = 0;
                else
                    enabled = false;
            }
        }
    }
}
