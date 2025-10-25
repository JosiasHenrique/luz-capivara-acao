using UnityEngine;
using UnityEngine.AI;

public class Luisa : MonoBehaviour
{
    // =====================================================
    //                      REFERÊNCIAS
    // =====================================================
    [Header("Alvo")]
    public Transform nina;

    [Header("Configurações")]
    public float distanciaAtordoada = 3f;
    public float distanciaCaptura = 1f;

    private NavMeshAgent agent;
    private Animator animator;

    // =====================================================
    //                      VARIÁVEIS INTERNAS
    // =====================================================
    private bool capturou = false;

    // =====================================================
    //                      CICLO DE VIDA
    // =====================================================
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

        AtualizarMovimento(distancia);
        AtualizarAnimacoes();
        ChecarAtordoamento(distancia);
        ChecarCaptura(distancia);
    }

    // =====================================================
    //                      MOVIMENTO
    // =====================================================
    private void AtualizarMovimento(float distancia)
    {
        if (capturou) return;

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

    // =====================================================
    //                      ANIMAÇÕES
    // =====================================================
    private void AtualizarAnimacoes()
    {
        bool estaAndando = agent.velocity.magnitude > 0.1f;
        animator.SetBool("Mover", estaAndando);
        animator.SetBool("Correr", estaAndando);
    }

    private void ChecarAtordoamento(float distancia)
    {
        if (capturou || distancia > distanciaAtordoada) return;

        AnimatorStateInfo estadoAtual = animator.GetCurrentAnimatorStateInfo(0);
        if (!estadoAtual.IsName("Atordoada"))
        {
            animator.SetTrigger("Atordoada");
        }
            
    }

    // =====================================================
    //                      CAPTURA
    // =====================================================
    private void ChecarCaptura(float distancia)
    {
        if (capturou || distancia > distanciaCaptura) return;

        capturou = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        animator.SetTrigger("Agarrar");
        animator.SetBool("Capturou", true);

        Nina ninaScript = nina.GetComponent<Nina>();
        if (ninaScript != null)
            ninaScript.OnCaptured(transform);
    }
}
