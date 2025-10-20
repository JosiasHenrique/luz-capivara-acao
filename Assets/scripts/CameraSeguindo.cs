using UnityEngine;

public class CameraSeguindo : MonoBehaviour
{
    public Transform target;      // O personagem que a câmera vai seguir
    public float smoothSpeed = 0.125f; // Velocidade de suavização
    public Vector3 offset;        // Distância fixa da câmera em relação ao personagem 0,1, 1.5
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Posição desejada da câmera
        Vector3 desiredPosition = target.position + offset;

        // Suaviza o movimento da câmera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;
    }
}
