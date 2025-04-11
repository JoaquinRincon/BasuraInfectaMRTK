using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DronScript : MonoBehaviour
{
    [SerializeField] GameObject Jugador; // Referencia al objeto jugado
    [SerializeField] GameObject Proyectil; // Referencia al objeto que dispara
    [SerializeField] float VelocidadDron; // Variable para asignar velocidad al dron
    [SerializeField] GameObject PuntoSpawn; // Lugar del Spawn del rayo
    [SerializeField] float VelocidadProyectil; // Velocidad de movimiento de la bola
    [SerializeField] float RangoDeteccion = 5f; // Rango circular para detectar al jugador
    [SerializeField] float DistanciaSeguimiento = 3f; // Distancia m�nima para dejar de seguir al jugador
    [SerializeField] int CadenciaDisparo = 3; // Tiempo entre disparos 
    private bool jugadorDetectado = false; // Booleano para ver si lo ha detectado o no
    private float tiempoUltimoDisparo = 0f; //

    // Variables usadas para mantener el objeto flotando respecto al Terrain y Para sus Patrulla
    [SerializeField] private float radio = 5f; // Este valor afecta al tama�o del circulo de la patrulla
    [SerializeField] private float alturaSobreTerrain = 1f; // La altura deseada sobre la superficie del Terrain
    [SerializeField] private Terrain terreno; // Referencia al objeto Terrain
    private float angulo = 0f;
    private Vector3 centroHorizontalInicial; // La posici�n horizontal inicial del dron (ser� el centro)
    [SerializeField] private float velocidadRotacion = 5f; // Velocidad de rotaci�n del dron
    [SerializeField] float VelocidadPatrulla; // Variable para asignar velocidad al dron

    private void Start()
    {
        // Guarda la posici�n horizontal inicial del dron
        centroHorizontalInicial = new Vector3(transform.position.x, 0f, transform.position.z);

        // Establece la posici�n inicial correcta sobre el terreno
        float alturaTerrainInicial = terreno.SampleHeight(transform.position);
        transform.position = new Vector3(transform.position.x, alturaTerrainInicial + alturaSobreTerrain, transform.position.z);
    }

    private void Update()
    {
        
        DetectarJugador(); // Comprobar si est� en rango de detecci�n (controla el booleano) 

        if (jugadorDetectado) // Si ha detectado al jugador
        {
            SeguirJugador(); // Lo empieza a perseguir
            DispararPeriodicamente(); // Lo empieza a disparar
        } else {
            angulo += VelocidadPatrulla * Time.deltaTime;

            // Calcula el desplazamiento desde el centro horizontal inicial
            float xOffset = Mathf.Cos(angulo) * radio;
            float zOffset = Mathf.Sin(angulo) * radio;

            // Calcula la nueva posici�n horizontal
            Vector3 nuevaPosicionHorizontal = centroHorizontalInicial + new Vector3(xOffset, 0f, zOffset);

            // Obtiene la altura del terreno en la nueva posici�n horizontal
            float alturaTerrain = terreno.SampleHeight(nuevaPosicionHorizontal);

            // Calcula la direcci�n del movimiento
            Vector3 direccionMovimiento = nuevaPosicionHorizontal - new Vector3(transform.position.x, 0f, transform.position.z);
            direccionMovimiento.Normalize();

            // Evita rotaciones innecesarias si el dron no se ha movido (primer frame o velocidad muy baja)
            if (direccionMovimiento != Vector3.zero)
            {
                // Crea la rotaci�n objetivo basada en la direcci�n del movimiento
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);

                // Aplica la rotaci�n gradualmente usando Slerp para que sea suave
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadRotacion);
            }

            // Establece la nueva posici�n del dron
            transform.position = new Vector3(nuevaPosicionHorizontal.x, alturaTerrain + alturaSobreTerrain, nuevaPosicionHorizontal.z);
        }
    }

    private void DetectarJugador()
    {
        // Comprueba en un area circular alrededor de el tranform que le pasas por primer parametro si se encuentra el transform del objeto que le pasas por segundo parametro
        float distanciaAlJugador = Vector3.Distance(transform.position, Jugador.transform.position);

        if (distanciaAlJugador <= RangoDeteccion)
        {
            jugadorDetectado = true;
        }
        else
        {
            // Puedes comentar o descomentar la linea de "jugadorDetectado = false;" para que tenga ciertos comportamiento
            // Si la comentas el dron una vez que te detecta te seguir� para siempre
            // Si la descomentas el dron una vez te alejes de su rango de detecci�n dejar� de seguirte

            // jugadorDetectado = false;
        }
    }

    private void SeguirJugador()
    {

        float distanciaAlJugador = Vector3.Distance(transform.position, Jugador.transform.position);

        if (distanciaAlJugador > DistanciaSeguimiento)
        {
            // Calcular la direcci�n hacia el jugador
            Vector3 direccion = (Jugador.transform.position - transform.position).normalized;

            // Mover el dron hacia el jugador
            transform.position += direccion * VelocidadDron * Time.deltaTime;

            // Rotar el dron para que mire al jugador
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f); // Ajusta la velocidad de rotaci�n si es necesario
        }
    }

    private void DispararPeriodicamente()
    {
        // Comprobamos si ha disparado recientemente o no.
        if (Time.time >= tiempoUltimoDisparo + CadenciaDisparo)
        {
            DispararProyectil(); 
            tiempoUltimoDisparo = Time.time; // Nuevo valor para el tiempo del ultimo disparo
        }
    }

    private void DispararProyectil()
    {
        // Instanciamos el proyectil
        GameObject BolaEnergia = Instantiate(Proyectil, PuntoSpawn.transform.position, PuntoSpawn.transform.rotation);
        Rigidbody rb = BolaEnergia.GetComponent<Rigidbody>();
        // le decimos que vaya para adelante y le aplicamos su velocidad respecto a los valores puestos
        if (rb != null)
        {
            Vector3 direccion = transform.forward;
            rb.velocity = direccion * VelocidadProyectil;
        }
    }

    // Opcional: Para visualizar el rango de detecci�n en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, RangoDeteccion);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DistanciaSeguimiento);
    }
}
