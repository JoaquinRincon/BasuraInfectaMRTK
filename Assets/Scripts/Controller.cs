using UnityEngine;


public class Controller : MonoBehaviour
{
    // Referencia al CharacterController
    private CharacterController characterController;
    // Velocidad del movimiento
    [SerializeField] float speed = 20f;
    [SerializeField] float rotationSpeed = 40f;
    // Start es llamado antes del primer frame
    void Start()
    {
        // Obtenemos la referencia
        characterController = GetComponent<CharacterController>();

    }
    // Update es llamado una vez en cada frame
    void Update()
    {
        // Obtenemos el movimiento de los ejes
        float vMotion = Input.GetAxis("Vertical");
        float hMotion = Input.GetAxis("Horizontal");
        // Movimiento con gravedad y sin uso del Time.deltaTime
        characterController.SimpleMove(transform.forward * vMotion * speed);
        // Rotación con Time.deltaTime
        transform.Rotate(transform.up * hMotion * rotationSpeed * Time.deltaTime);
        

    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Acciones a realizar cuando se detecta una colisión
        Debug.Log("¡¡COLISIÓN!!");
        Debug.Log(hit.collider);
    }
}
