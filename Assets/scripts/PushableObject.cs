using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody rb;
    public float pesoEmpurrao = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        // Dire��o do empurr�o
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Aplicar for�a
        body.AddForce(pushDir * pesoEmpurrao, ForceMode.Impulse);
    }
}
