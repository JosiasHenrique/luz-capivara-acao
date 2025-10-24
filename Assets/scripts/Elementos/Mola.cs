using UnityEngine;
using System.Collections;

public class Mola : MonoBehaviour
{
    public float compressao = 0.5f; // quanto ela comprime
    public float duracao = 0.1f;    // tempo de compressão
    private Vector3 escalaOriginal;

    private void Start()
    {
        escalaOriginal = transform.localScale;
    }

    public void PisarNaMola()
    {
        StartCoroutine(AnimarMola());
    }

    private IEnumerator AnimarMola()
    {
        // Comprimir
        float tempo = 0f;
        Vector3 alvo = new Vector3(escalaOriginal.x, escalaOriginal.y - compressao, escalaOriginal.z);

        while (tempo < duracao)
        {
            transform.localScale = Vector3.Lerp(escalaOriginal, alvo, tempo / duracao);
            tempo += Time.deltaTime;
            yield return null;
        }

        transform.localScale = alvo;

        // Voltar
        tempo = 0f;
        while (tempo < duracao)
        {
            transform.localScale = Vector3.Lerp(alvo, escalaOriginal, tempo / duracao);
            tempo += Time.deltaTime;
            yield return null;
        }

        transform.localScale = escalaOriginal;
    }
}
