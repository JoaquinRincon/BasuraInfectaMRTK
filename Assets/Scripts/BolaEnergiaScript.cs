using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaEnergiaScript : MonoBehaviour
{
    [SerializeField] int TiempoVida = 5; // Tiempo de vida del objeto
    [SerializeField] Collider Jugador; // Referencia al componente collider del jugador

    private void Start()
    {
        //Lanzamos la corrutina
        Jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(); // Buscamos el collider del objeto del jugador buscandolo por su tag
        StartCoroutine(Autodestruirse(TiempoVida)); //Empezar courutina destruir la bola si pasa X tiempo.
    }

    // Corrutina que destruye el objeto despues de X segundos
    IEnumerator Autodestruirse(int segundos)
    {
        // Esperamos lso segundso especificados
        yield return new WaitForSeconds(segundos);

        // Destruimos el objeto
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Comprobamos si el collider es el del jugador
        if (collision.collider == Jugador )
        {
            // Destruimos el objeto
            Destroy(this.gameObject);
        }
    }
}
