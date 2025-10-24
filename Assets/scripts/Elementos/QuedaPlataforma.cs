using UnityEngine;
using System.Collections;

public class QuedaPlataforma : MonoBehaviour
{
    public float tempoAntesDeCair = 2f;

    private Rigidbody rb;
    private bool caiu = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }


    public void IniciarQueda()
    {
        if (!caiu)
            StartCoroutine(CairDepois());
    }

    private IEnumerator CairDepois()
    {
        caiu = true;

        float tempoBalanco = tempoAntesDeCair;
        float tempo = 0f;
        float intensidade = 5f; 

        while (tempo < tempoBalanco)
        {
            float angulo = Mathf.Sin(tempo * 10f) * intensidade;
            transform.rotation = Quaternion.Euler(0, 0, angulo);
            tempo += Time.deltaTime;
            yield return null;
        }

        rb.isKinematic = false;
        Destroy(gameObject, 2f);
    }

}
