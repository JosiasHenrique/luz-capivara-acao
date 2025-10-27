using System;
using UnityEngine;

public class NinaVida : MonoBehaviour
{
    [Header("Vida")]
    public float maxVida = 100f;
    private float vidaAtual;

    [Header("Regeneração")]
    public float tempoParaRegenerar = 3f;
    public float taxaRegeneracao = 5f;
    private float tempoSemDano = 0f;
    private bool regenerando = false;

    [Header("Áudio")]
    public AudioSource danoAudioSource;
    public AudioClip somDano;

    public DanoTela danoTela;
    public Animator animator;

    public event Action<float, float> OnVidaMudou;
    public event Action<bool> OnRegeneracaoMudou;

    private void Start()
    {
        vidaAtual = maxVida;
    }

    private void Update()
    {
        if (vidaAtual < maxVida)
        {
            tempoSemDano += Time.deltaTime;

            if (tempoSemDano >= tempoParaRegenerar)
            {
                if (!regenerando)
                {
                    regenerando = true;
                    OnRegeneracaoMudou?.Invoke(true);
                }

                vidaAtual += taxaRegeneracao * Time.deltaTime;
                vidaAtual = Mathf.Clamp(vidaAtual, 0, maxVida);
                OnVidaMudou?.Invoke(vidaAtual, maxVida);

                if (vidaAtual >= maxVida)
                {
                    regenerando = false;
                    OnRegeneracaoMudou?.Invoke(false);
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        vidaAtual -= amount;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, maxVida);

        tempoSemDano = 0f;

        if (regenerando)
        {
            regenerando = false;
            OnRegeneracaoMudou?.Invoke(false);
        }

        if (animator != null)
            animator.SetTrigger("LevouDano");

        if (danoAudioSource != null && somDano != null)
            danoAudioSource.PlayOneShot(somDano);

        if (danoTela != null)
            danoTela.Flash();

        OnVidaMudou?.Invoke(vidaAtual, maxVida);

        if (vidaAtual <= 0)
            Die();
    }

    private void Die()
    {

        if (GameOverMenu.Instance != null)
        {
            GameOverMenu.Instance.MostrarGameOver("A Nina morreu!");
        }
    }
}
