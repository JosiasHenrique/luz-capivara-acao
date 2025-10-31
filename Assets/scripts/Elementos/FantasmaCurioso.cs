using UnityEngine;
using UnityEngine.AI;

public class FantasmaCurioso : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    public float raioDePatrulha = 5f;
    public float tempoEspera = 2f;

    private NavMeshAgent agent;
    private Vector3 posicaoInicial;
    private float timerEspera;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        posicaoInicial = transform.position;

        agent.speed = velocidade;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        MoverParaNovoDestino();
    }

    void Update()
    {
        // Quando chega no destino
        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
        {
            timerEspera += Time.deltaTime;

            if (timerEspera >= tempoEspera)
            {
                MoverParaNovoDestino();
                timerEspera = 0f;
            }
        }

        // Espelha o sprite conforme a direção
        if (agent.velocity.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(agent.velocity.x), 1, 1);
        }
    }

    void MoverParaNovoDestino()
    {
        // Escolhe um ponto aleatório dentro do raio
        Vector2 randomCircle = Random.insideUnitCircle * raioDePatrulha;
        Vector3 destino = posicaoInicial + new Vector3(randomCircle.x, 0, randomCircle.y);

        // Garante que o ponto esteja sobre o NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destino, out hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? posicaoInicial : transform.position, raioDePatrulha);
    }
}
