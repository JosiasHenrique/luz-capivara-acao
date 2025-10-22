using UnityEngine;

public class CameraTerceiraPessoa : MonoBehaviour
{
    [Header("Referência à personagem")]
    public Transform target;

    [Header("Configurações da câmera")]
    public Vector3 shoulderOffset = new Vector3(0.5f, 2f, -3f); // deslocamento relativo ao ombro
    public float rotationSpeed = 8f; // velocidade de rotação da câmera
    public float smoothSpeed = 15f;   // suavização da posição

    private float yaw = 0f;   // rotação horizontal
    private float pitch = 10f; // rotação vertical inicial

    void Start()
    {
        // trava o cursor no centro da tela e o deixa invisível
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // captura entrada do mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f); // limita rotação vertical

        // cria rotação a partir da entrada do mouse
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // calcula a posição desejada da câmera usando rotação + offset
        Vector3 desiredPosition = target.position + rotation * shoulderOffset;

        // suaviza movimento da câmera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // sempre olhar para a Nina (aprox. tronco/cabeça)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
