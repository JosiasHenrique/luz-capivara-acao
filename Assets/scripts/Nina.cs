using UnityEngine;

public class Nina : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Transform myCamera;

    [Header("Velocidades")]
    public float velocidadeAndando = 1f;
    public float velocidadeCorrendo = 2f;

    [Header("Pulo")]
    public float forcaPulo = 5f;
    public float gravidade = -9.81f;
    public float areaEsferaPe = 0.1f;

    [Header("Chão")]
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask colisaoLayer;

    private bool estaNoChao;
    private float velocidadeY;

    public GameObject painelGameOver;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myCamera = Camera.main.transform;
    }

    void Update()
    {

        // --- MOVIMENTO ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0, vertical);

        // Converte direção relativa à câmera
        movimento = myCamera.TransformDirection(movimento);
        movimento.y = 0;

        // Define velocidade dependendo do Shift
        bool estaCorrendo = Input.GetKey(KeyCode.LeftShift) && movimento != Vector3.zero;
        float speed = estaCorrendo ? velocidadeCorrendo : velocidadeAndando;

        // Move a personagem horizontalmente
        controller.Move(movimento * speed * Time.deltaTime);

        // Rotação suave na direção do movimento
        if (movimento != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10f);
        }

        // Atualiza animações
        animator.SetBool("Mover", movimento != Vector3.zero);
        animator.SetBool("Correr", estaCorrendo);

        // --- PULO E GRAVIDADE ---
        estaNoChao = Physics.CheckSphere(peDoPersonagem.position, areaEsferaPe, colisaoLayer);
        animator.SetBool("EstaNoChao", estaNoChao);

        if (estaNoChao && Input.GetKeyDown(KeyCode.Space))
        {
            velocidadeY = forcaPulo;
            animator.SetTrigger("Pular");
        }

        // Aplica gravidade
        if (velocidadeY > gravidade)
        {
            velocidadeY += gravidade * Time.deltaTime;
        }

        controller.Move(new Vector3(0, velocidadeY, 0) * Time.deltaTime);
    }

    // Método chamado quando Nina é capturada pela Luisa
    public void OnCaptured(Transform luisa)
    {

        if (animator != null)
        {
            animator.SetTrigger("Capturada");
            animator.SetBool("Capturou", true);
        }

        if (GameOverMenu.Instance != null)
        {
            GameOverMenu.Instance.MostrarGameOver("A Luisa te capturou!");
        }

    }
}

