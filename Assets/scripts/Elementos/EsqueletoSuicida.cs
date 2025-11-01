using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EsqueletoSuicida : MonoBehaviour
{
    [Header("Configurações")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 4f;
    public float visionRange = 8f;
    public float explosionRange = 1.5f;
    public float explosionDelay = 1f;
    public float patrolDistance = 4f;
    public GameObject explosionEffect;

    [Header("Patrulha")]
    [Tooltip("Direção inicial da patrulha: 1 = para frente, -1 = para trás")]
    public int initialDirection = 1; // default para frente


    [Header("Referências")]
    public Animator animator;
    public Transform nina;
    public AudioSource audioSource;
    public AudioClip detectClip;
    public AudioClip explodeClip;
    public Light glowLight;

    private Vector3 startPos;
    private int direction = 1;
    private bool isExploding = false;
    private NavMeshAgent agent;
    private bool hasDetected = false;
    private Vector3 lastDestination;

    void Start()
    {
        direction = initialDirection;
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (glowLight) glowLight.enabled = false;
    }

    void Update()
    {
        if (isExploding) return;

        float distanceToPlayer = Vector3.Distance(transform.position, nina.position);

        if (distanceToPlayer < explosionRange)
        {
            StartCoroutine(Explode());
        }
        else if (distanceToPlayer < visionRange)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("Perseguindo", false);
        animator.SetBool("Patrulha", true);
        agent.speed = patrolSpeed;

        Vector3 target = startPos + transform.forward * direction * patrolDistance;

        // Só atualiza se o destino mudou significativamente
        if (Vector3.Distance(lastDestination, target) > 0.1f)
        {
            agent.SetDestination(target);
            lastDestination = target;
        }

        if (Vector3.Distance(transform.position, target) < 0.5f)
            direction *= -1;

        if (audioSource && audioSource.isPlaying && audioSource.clip == detectClip)
            audioSource.Stop();

        hasDetected = false;
    }


    void Chase()
    {
        animator.SetBool("Patrulha", false);
        animator.SetBool("Perseguindo", true);
        agent.speed = chaseSpeed;

        if (Vector3.Distance(lastDestination, nina.position) > 0.1f)
        {
            agent.SetDestination(nina.position);
            lastDestination = nina.position;
        }

        if (!hasDetected && audioSource && detectClip)
        {
            audioSource.clip = detectClip;
            audioSource.loop = true;
            audioSource.Play();
            hasDetected = true;
        }
    }


    IEnumerator Explode()
    {
        if (isExploding) yield break;
        isExploding = true;

        agent.isStopped = true;
        animator.SetTrigger("Morte");

        // para o som de detecção
        if (audioSource && audioSource.isPlaying && audioSource.clip == detectClip)
            audioSource.Stop();

        // toca som de explosão
        if (explodeClip != null)
        {
            GameObject tempAudio = new GameObject("TempExplosionSound");
            AudioSource a = tempAudio.AddComponent<AudioSource>();

            a.clip = explodeClip;
            a.volume = 0.8f;     
            a.spatialBlend = 0f; 
            a.Play();

            Destroy(tempAudio, explodeClip.length);
        }

        if (explosionEffect)
        {
            // cria uma cópia temporária do efeito na posição do inimigo
            ParticleSystem psClone = Instantiate(explosionEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            Destroy(psClone.gameObject, 1f);

        }

        float timer = 0f;
        Vector3 originalPos = transform.position;
        while (timer < explosionDelay)
        {
            // piscando luz
            if (glowLight) glowLight.enabled = (Mathf.FloorToInt(timer * 5f) % 2 == 0);

            // tremor
            transform.position = originalPos + Random.insideUnitSphere * 0.1f;

            timer += Time.deltaTime;
            yield return null;
        }

        if (glowLight) glowLight.enabled = false;
        transform.position = originalPos;

        // dano
        nina.GetComponent<NinaVida>().TakeDamage(20);

        agent.enabled = false;
        Destroy(gameObject);
    }

}
