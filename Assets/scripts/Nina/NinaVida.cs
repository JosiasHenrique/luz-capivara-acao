using System;
using UnityEngine;

public class NinaVida : MonoBehaviour
{
    public float maxVida = 100f;
    private float vidaAtual;

    [Header("Áudio")]
    public AudioSource danoAudioSource;
    public AudioClip somDano;

    public DanoTela danoTela;

    public Animator animator;

    public event Action<float, float> OnVidaMudou;

    private void Start()
    {
        vidaAtual = maxVida;
    }

    public void TakeDamage(float amount)
    {
        vidaAtual -= amount;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, maxVida);

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
        Debug.Log("Nina morreu");

        if (GameOverMenu.Instance != null)
        {
            GameOverMenu.Instance.MostrarGameOver("A Nina morreu!");
        }
    }
}
