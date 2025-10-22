using UnityEngine;

public class LuisaCombat : MonoBehaviour
{
    [Header("Refer�ncias")]
    public Transform nina;
    public LuisaVida luisaVida; 

    [Header("Configurações de combate")]
    public float dashSpeed = 12f;
    public float chargeTime = 1.5f;
    public float stunDuration = 2f;
    public float idleTime = 2f;
    public float wallDamage = 10f;

    private CharacterController controller;
    private Animator animator;

    private string state = "Parada";
    private float timer = 0f;
    private Vector3 dashDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (luisaVida == null)
            luisaVida = GetComponent<LuisaVida>();
    }

    void Update()
    {
        switch (state)
        {
            case "Parada":
                timer += Time.deltaTime;
                if (timer >= idleTime)
                {
                    StartCharge();
                }
                break;

            case "CarregandoDash":
                FaceNina();
                timer += Time.deltaTime;
                if (timer >= chargeTime)
                {
                    Dash();
                }
                break;

            case "Ataque":
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);
                break;

            case "BatendoNaParede":
                timer += Time.deltaTime;
                if (timer >= stunDuration)
                {
                    Recover();
                }
                break;

            case "Recuperando":
                state = "Parada";
                break;
        }
    }

    void FaceNina()
    {
        if (nina == null) return;

        Vector3 dir = nina.position - transform.position;

        if (dir.magnitude > 0.1f)
            transform.forward = Vector3.Lerp(transform.forward, dir.normalized, Time.deltaTime * 5f);
    }

    void StartCharge()
    {
        state = "CarregandoDash";
        timer = 0f;

        if (animator)
            animator.SetTrigger("Charge");

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

        if (state == "Ataque" && hit.gameObject.CompareTag("Wall"))
        {
            Stun();
        }

    }

    void Stun()
    {
        state = "BatendoNaParede";
        timer = 0f;
        animator?.SetTrigger("BatendoNaParede");

        if (luisaVida != null)
        {
            luisaVida.TakeDamage(wallDamage);
        }

    }

    void Recover()
    {
        state = "Recuperando";
        timer = 0f;
        animator?.SetTrigger("Recover");

    }

    public bool IsAttacking()
    {
        return state == "Ataque";
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}