using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCena : MonoBehaviour
{
    [Header("Nome da pr�xima cena")]
    public string nomeCenaDestino;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Nina entrou no portal, carregando pr�xima cena...");
            SceneManager.LoadScene(nomeCenaDestino);
        }
    }
}
