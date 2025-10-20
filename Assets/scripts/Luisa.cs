using UnityEngine;
using UnityEngine.AI;

public class Luisa : MonoBehaviour
{
    [Header("Alvo")]
    public Transform nina;

    [Header("Configurações")]
    public float distanciaAtordoada = 3f;
    public float distanciaCaptura = 1f;

    private NavMeshAgent agent;
    private Animator animator;

    private bool capturou = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = distanciaCaptura;
    }

    void Update()
    {
        if (nina == null) return;

        float distancia = Vector3.Distance(transform.position, nina.position);

        // --- MOVIMENTO ---
        if (!capturou) // só move se ainda não capturou
        {
            if (distancia > distanciaCaptura)
            {
                agent.isStopped = false;
                agent.SetDestination(nina.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }

        // --- ANIMAÇÕES DE MOVIMENTO ---
        bool estaAndando = agent.velocity.magnitude > 0.1f;
        animator.SetBool("Mover", estaAndando);
        animator.SetBool("Correr", estaAndando);

        // --- ATORDOAR ---
        if (!capturou && distancia <= distanciaAtordoada)
        {
            AnimatorStateInfo estadoAtual = animator.GetCurrentAnimatorStateInfo(0);
            if (!estadoAtual.IsName("Atordoada"))
            {
                animator.SetTrigger("Atordoada");
            }
        }


        // --- CAPTURA ---
        if (!capturou && distancia <= distanciaCaptura)
        {
            capturou = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            animator.SetTrigger("Agarrar");
            animator.SetBool("Capturou", true);

            Nina ninaScript = nina.GetComponent<Nina>();
            if (ninaScript != null)
            {
                ninaScript.OnCaptured(transform);

            }
        }

    }
}
