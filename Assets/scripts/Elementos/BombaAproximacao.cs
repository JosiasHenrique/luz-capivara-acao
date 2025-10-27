using System.Collections;
using UnityEngine;

public class BombaAproximacao : MonoBehaviour
{
    [Header("Detecção e explosão")]
    public float detectionRadius = 3f;
    public float explosionForce = 700f;
    public float explosionRadius = 5f;
    public float delayBeforeExplode = 1f;

    [Header("Limite de queda")]
    public float yLimite = -10f;


    [Header("Feedback visual e sonoro")]
    public Renderer mineRenderer;
    public Color normalColor = Color.gray;
    public Color alertColor = Color.red;
    public AudioSource warningBeep;
    public AudioClip explosionClip;
    public GameObject explosionEffect;

    [Header("Animação de escala")]
    public float scaleMultiplier = 1.4f;
    public float pulseSpeed = 6f;

    private bool triggered = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (mineRenderer != null)
            mineRenderer.material.color = normalColor;
    }

    void Update()
    {
        if (transform.position.y < yLimite)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(ExplodeSequence());
        }
    }

    IEnumerator ExplodeSequence()
    {
        float blinkTime = 0.1f;
        float elapsed = 0f;

        while (elapsed < delayBeforeExplode)
        {
            // alterna cor
            if (mineRenderer != null)
                mineRenderer.material.color = (mineRenderer.material.color == normalColor) ? alertColor : normalColor;

            // som de aviso
            if (warningBeep != null && !warningBeep.isPlaying)
                warningBeep.Play();

            // efeito de pulsar
            float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * 0.2f;
            transform.localScale = originalScale * scaleFactor;

            elapsed += blinkTime;
            yield return new WaitForSeconds(blinkTime);
        }

        transform.localScale = originalScale * scaleMultiplier;
        yield return new WaitForSeconds(0.05f);

        Explode();
    }

    void Explode()
    {

        if (explosionClip != null)
        {
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);
        }

        if (explosionEffect)
        {
            ParticleSystem psClone = Instantiate(explosionEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            Destroy(psClone.gameObject, 1f);

        }

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var c in cols)
        {
            if (c.attachedRigidbody != null)
                c.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            if (c.CompareTag("Player"))
            {
                var vida = c.GetComponent<NinaVida>();
                if (vida != null)
                    vida.TakeDamage(20);
            }
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        transform.localScale = originalScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
