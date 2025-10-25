using UnityEngine;

public class Nina : MonoBehaviour
{
    // --- COMPONENTES ---
    private CharacterController controller;
    private Animator animator;
    private Transform myCamera;

    // --- MOVIMENTO ---
    [Header("Velocidades")]
    public float velocidadeAndando = 1.5f;
    public float velocidadeCorrendo = 3.5f;

    // --- PULO ---
    [Header("Pulo")]
    public float forcaPulo = 5f;
    public float gravidade = -9.81f;
    public float areaEsferaPe = 0.1f;

    // --- PULO COM MOLA ---
    [Header("Pulo Mola")]
    public float impulso = 10f;

    // --- CHÃO ---
    [Header("Chão")]
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask colisaoLayer;

    // --- ÁUDIO ---
    [Header("Som Corrida")]
    public AudioSource runAudio;

    [Header("Efeitos Sonoros")]
    public AudioSource efeitosAudioSource; 
    public AudioClip somPulo;


    // --- VARIÁVEIS INTERNAS ---
    private bool estaNoChao;
    private float velocidadeY;
    private Vector3 direcaoMovimento;

    // =====================================================
    //                      CICLO DE VIDA
    // =====================================================

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myCamera = Camera.main.transform;
    }

    void Update()
    {
        AtualizarEstadoChao();
        Movimentar();
        ControlarAudioCorrida();
        ControlarPulo();
        AplicarGravidade();
    }

    // =====================================================
    //                      MOVIMENTO
    // =====================================================

    private void Movimentar()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0, vertical);
        input = myCamera.TransformDirection(input);
       
        input.Normalize();

        bool estaAndando = input != Vector3.zero && estaNoChao;
        bool estaCorrendo = Input.GetKey(KeyCode.LeftShift) && estaAndando;

        float velocidade = estaCorrendo ? velocidadeCorrendo : velocidadeAndando;


        if (estaNoChao)
        {
            direcaoMovimento = input * velocidade;
        }
        else
        {
            direcaoMovimento = input * velocidadeCorrendo;
        }

        if (input != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(input), Time.deltaTime * 10f);

        // --- ANIMAÇÕES ---
        animator.SetBool("Mover", estaAndando);
        animator.SetBool("Correr", estaCorrendo);
    }


    // =====================================================
    //                      CHÃO / PULO
    // =====================================================

    private void AtualizarEstadoChao()
    {
        estaNoChao = Physics.CheckSphere(peDoPersonagem.position, areaEsferaPe, colisaoLayer);
        animator.SetBool("EstaNoChao", estaNoChao);
    }


    private void ControlarPulo()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            velocidadeY = forcaPulo;
            animator.SetTrigger("Pular");

            if (efeitosAudioSource != null && somPulo != null)
                efeitosAudioSource.PlayOneShot(somPulo, 0.3f);
        }

    }

   

    private void AplicarGravidade()
    {
        if (estaNoChao && velocidadeY < 0)
            velocidadeY = -2f;

        velocidadeY += gravidade * Time.deltaTime;

        Vector3 movimentoFinal = direcaoMovimento;
        movimentoFinal.y = velocidadeY;
        controller.Move(movimentoFinal * Time.deltaTime);
    }

    // =====================================================
    //                      ÁUDIO
    // =====================================================

    private void ControlarAudioCorrida()
    {
        bool estaCorrendo = animator.GetBool("Correr");

        if (estaCorrendo && estaNoChao)
        {
            if (!runAudio.isPlaying)
                runAudio.Play();
        }
        else if (runAudio.isPlaying)
        {
            runAudio.Stop();
        }
    }

    // =====================================================
    //                      COLISÕES
    // =====================================================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Queda"))
        {
            animator.SetTrigger("CaindoMola");
            GameOverMenu.Instance?.MostrarGameOver("Você caiu do mapa!");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Plataforma"))
        {
            hit.collider.GetComponent<QuedaPlataforma>()?.IniciarQueda();
        }
        else if (hit.collider.CompareTag("Mola"))
        {
            velocidadeY = impulso;
            animator.SetTrigger("CaindoMola");
            hit.collider.GetComponent<Mola>()?.PisarNaMola();
        }
    }

    // =====================================================
    //                      GAME OVER
    // =====================================================

    public void OnCaptured(Transform luisa)
    {
        GameOverMenu.Instance?.MostrarGameOver("A Luisa te capturou!");
    }
}
