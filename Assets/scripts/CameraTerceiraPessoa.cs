using UnityEngine;

public class CameraTerceiraPessoa : MonoBehaviour
{
    [Header("Refer�ncia � personagem")]
    public Transform target;

    [Header("Configura��es da c�mera")]
    public Vector3 shoulderOffset = new Vector3(0.5f, 2f, -3f); // deslocamento relativo ao ombro
    public float rotationSpeed = 8f; // velocidade de rota��o da c�mera
    public float smoothSpeed = 15f;   // suaviza��o da posi��o

    private float yaw = 0f;   // rota��o horizontal
    private float pitch = 10f; // rota��o vertical inicial

    void Start()
    {
        // trava o cursor no centro da tela e o deixa invis�vel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // captura entrada do mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f); // limita rota��o vertical

        // cria rota��o a partir da entrada do mouse
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // calcula a posi��o desejada da c�mera usando rota��o + offset
        Vector3 desiredPosition = target.position + rotation * shoulderOffset;

        // suaviza movimento da c�mera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // sempre olhar para a Nina (aprox. tronco/cabe�a)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
