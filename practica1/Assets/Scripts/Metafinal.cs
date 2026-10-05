using UnityEngine;

public class Metafinal : MonoBehaviour
{
    public GameObject mensajeFinal;

    private void Start()
    {
        mensajeFinal.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            mensajeFinal.SetActive(true);
            Debug.Log("¡Práctica No. 1 realizada!");
        }
    }
}