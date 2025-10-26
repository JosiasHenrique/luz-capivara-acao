using UnityEngine;

public class TriggerBomba : MonoBehaviour
{
    [Header("Bombas a ativar")]
    public Rigidbody[] bombas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Rigidbody rb in bombas)
            {
                if (rb != null)
                    rb.isKinematic = false; 
            }
        }
    }
}
