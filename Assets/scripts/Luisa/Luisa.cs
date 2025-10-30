using System.Collections;
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
    private bool pulando = false;

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
        agent.autoTraverseOffMeshLink = false;
    }

    void Update()
    {
        if (nina == null) return;

        float distancia = Vector3.Distance(transform.position, nina.position);

        AtualizarMovimento(distancia);
        AtualizarAnimacoes();
        ChecarAtordoamento(distancia);
        ChecarCaptura(distancia);

        if (agent.isOnOffMeshLink && !pulando)
        {
            StartCoroutine(FazerPulo());
        }
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

    // =====================================================
    //                 PULO SOBRE NAVMESH LINK
    // =====================================================
    private IEnumerator FazerPulo()
    {
        pulando = true;
        agent.isStopped = true;

        animator.SetTrigger("Pular");

        // dados do link
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float duracao = 1.0f;
        float tempo = 0f;
        float altura = 2f; 

        while (tempo < duracao)
        {
            float t = tempo / duracao;
            float alturaPulo = Mathf.Sin(Mathf.PI * t) * altura;
            agent.transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * alturaPulo;
            tempo += Time.deltaTime;
            yield return null;
        }

        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        pulando = false;
    }
}
