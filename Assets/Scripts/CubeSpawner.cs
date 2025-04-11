using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab del cubo
    public float spawnInterval = 2f; // Intervalo de tiempo para generar los cubos
    public float moveSpeed = 0.1f; // Velocidad de movimiento
    public float moveRange = 1f; // Rango máximo de movimiento (en cada dirección)

    void Start()
    {
        // Comienza a generar cubos de manera periódica
        InvokeRepeating("SpawnCube", 0f, spawnInterval);
    }

    void SpawnCube()
    {
        // Generar el cubo en una posición aleatoria
        Vector3 spawnPosition = new Vector3(
            Random.Range(-5f, 5f),
            1f, // Altura fija para evitar que caigan al generarse
            Random.Range(-5f, 5f)
        );
        GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        newCube.AddComponent<MoveRandomly>(); // Añadir el script de movimiento aleatorio
    }
}

public class MoveRandomly : MonoBehaviour
{
    private Vector3 targetPosition;
    public float moveSpeed = 0.1f; // Velocidad de movimiento
    public float moveRange = 1f; // Rango de movimiento

    void Start()
    {
        // Asignar una posición de destino aleatoria en un rango limitado
        targetPosition = new Vector3(
            transform.position.x + Random.Range(-moveRange, moveRange),
            transform.position.y + Random.Range(-moveRange, moveRange),
            transform.position.z + Random.Range(-moveRange, moveRange)
        );
    }

    void Update()
    {
        // Mover el cubo lentamente hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Si el cubo llega a la posición, asignar una nueva posición aleatoria
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = new Vector3(
                transform.position.x + Random.Range(-moveRange, moveRange),
                transform.position.y + Random.Range(-moveRange, moveRange),
                transform.position.z + Random.Range(-moveRange, moveRange)
            );
        }
    }
}
