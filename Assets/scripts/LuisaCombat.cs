using UnityEngine;

public class LuisaCombat : MonoBehaviour
{
    [Header("Refer�ncias")]
    public Transform nina;

    [Header("Configura��es de combate")]
    public float dashSpeed = 12f;      // velocidade da investida da Lu�sa
    public float chargeTime = 1.5f;    // tempo de carregamento antes do ataque
    public float stunDuration = 2f;    // tempo de atordoamento ap�s bater na parede
    public float idleTime = 2f;        // tempo entre ataques quando parada

    private CharacterController controller;
    private Animator animator;

    private string state = "Parada";
    private float timer = 0f;          // contador de tempo usado para transi��es
    private Vector3 dashDirection;     // dire��o do dash (somente XZ)

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
                    Debug.Log("Transi��o: Parada -> CarregandoDash");
                    StartCharge();
                }
                break;

            case "CarregandoDash":
                Debug.Log("Estado: CarregandoDash");
                FaceNina();
                timer += Time.deltaTime;
                if (timer >= chargeTime)
                {
                    Debug.Log("Transi��o: CarregandoDash -> Ataque");
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
                    Debug.Log("Transi��o: BatendoNaParede -> Recuperando");
                    Recover();
                }
                break;

            case "Recuperando":
                Debug.Log("Estado: Recuperando");
                state = "Parada"; // volta para Parada ap�s recuperar
                break;
        }
    }

    void FaceNina()
    {
        // Se n�o houver refer�ncia � Nina, sai da fun��o
        if (nina == null) return;

        // Calcula o vetor dire��o da Lu�sa at� a Nina
        Vector3 dir = nina.position - transform.position;

        // S� atualiza a rota��o se a dist�ncia for significativa (evita movimentos desnecess�rios)
        if (dir.magnitude > 0.1f)
            // Rotaciona suavemente a Lu�sa em dire��o � Nina
            // Vector3.Lerp interpola entre a dire��o atual (transform.forward) e a dire��o desejada (dir.normalized)
            // Time.deltaTime * 5f controla a velocidade da interpola��o
            transform.forward = Vector3.Lerp(transform.forward, dir.normalized, Time.deltaTime * 5f);
    }

    void StartCharge()
    {
        state = "CarregandoDash";
        timer = 0f;

        if (animator)
            animator.SetTrigger("Charge");

        dashDirection = (nina.position - transform.position).normalized;

        // Calcula a dire��o da Lu�sa em dire��o � Nina
        // normalizada para garantir vetor unit�rio (apenas dire��o, sem afetar velocidade)
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
