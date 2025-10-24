using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCena : MonoBehaviour
{
    [Header("Nome da próxima cena")]
    public string nomeCenaDestino;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Nina entrou no portal, carregando próxima cena...");
            SceneManager.LoadScene(nomeCenaDestino);
        }
    }
}
