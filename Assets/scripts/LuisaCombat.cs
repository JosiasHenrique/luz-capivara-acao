using UnityEngine;

public class LuisaCombat : MonoBehaviour
{
    [Header("Referências")]
    public Transform nina;

    [Header("Configurações de combate")]
    public float dashSpeed = 12f;      // velocidade da investida da Luísa
    public float chargeTime = 1.5f;    // tempo de carregamento antes do ataque
    public float stunDuration = 2f;    // tempo de atordoamento após bater na parede
    public float idleTime = 2f;        // tempo entre ataques quando parada

    private CharacterController controller;
    private Animator animator;

    private string state = "Parada";
    private float timer = 0f;          // contador de tempo usado para transições
    private Vector3 dashDirection;     // direção do dash (somente XZ)

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (state)
        {
            case "Parada":
                Debug.Log("Estado: Parada");
                timer += Time.deltaTime;
                if (timer >= idleTime)
                {
                    Debug.Log("Transição: Parada -> CarregandoDash");
                    StartCharge();
                }
                break;

            case "CarregandoDash":
                Debug.Log("Estado: CarregandoDash");
                FaceNina();
                timer += Time.deltaTime;
                if (timer >= chargeTime)
                {
                    Debug.Log("Transição: CarregandoDash -> Ataque");
                    Dash();
                }
                break;

            case "Ataque":
                Debug.Log("Estado: Ataque");
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);
                break;

            case "BatendoNaParede":
                Debug.Log("Estado: BatendoNaParede");
                timer += Time.deltaTime;
                if (timer >= stunDuration)
                {
                    Debug.Log("Transição: BatendoNaParede -> Recuperando");
                    Recover();
                }
                break;

            case "Recuperando":
                Debug.Log("Estado: Recuperando");
                state = "Parada"; // volta para Parada após recuperar
                break;
        }
    }

    void FaceNina()
    {
        // Se não houver referência à Nina, sai da função
        if (nina == null) return;

        // Calcula o vetor direção da Luísa até a Nina
        Vector3 dir = nina.position - transform.position;

        // Só atualiza a rotação se a distância for significativa (evita movimentos desnecessários)
        if (dir.magnitude > 0.1f)
            // Rotaciona suavemente a Luísa em direção à Nina
            // Vector3.Lerp interpola entre a direção atual (transform.forward) e a direção desejada (dir.normalized)
            // Time.deltaTime * 5f controla a velocidade da interpolação
            transform.forward = Vector3.Lerp(transform.forward, dir.normalized, Time.deltaTime * 5f);
    }

    void StartCharge()
    {
        state = "CarregandoDash";
        timer = 0f;

        if (animator)
            animator.SetTrigger("Charge");

        dashDirection = (nina.position - transform.position).normalized;

        // Calcula a direção da Luísa em direção à Nina
        // normalizada para garantir vetor unitário (apenas direção, sem afetar velocidade)
        dashDirection = (nina.position - transform.position).normalized;
    }

    void Dash()
    {
        state = "Ataque";
        timer = 0f;

        if (animator)
            animator.SetTrigger("Dash");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // bateu em uma parede enquanto ataca
        if (state == "Ataque" && hit.gameObject.CompareTag("Wall"))
        {
            Stun();
        }
    }

    void Stun()
    {
        state = "BatendoNaParede";
        timer = 0f;

        if (animator)
            animator.SetTrigger("BatendoNaParede");
    }

    void Recover()
    {
        state = "Recuperando";
        timer = 0f;

        if (animator)
            animator.SetTrigger("Recover");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}
