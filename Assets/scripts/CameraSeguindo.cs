using UnityEngine;

public class CameraSeguindo : MonoBehaviour
{
    public Transform target;      // O personagem que a c�mera vai seguir
    public float smoothSpeed = 0.125f; // Velocidade de suaviza��o
    public Vector3 offset;        // Dist�ncia fixa da c�mera em rela��o ao personagem 0,1, 1.5
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Posi��o desejada da c�mera
        Vector3 desiredPosition = target.position + offset;

        // Suaviza o movimento da c�mera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Atualiza a posi��o da c�mera
        transform.position = smoothedPosition;
    }
}
