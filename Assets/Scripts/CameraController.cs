using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform personaje;

    private float tamaņoCamara;
    private float alturaPantalla;

    // Start is called before the first frame update
    void Start()
    {
        tamaņoCamara = Camera.main.orthographicSize;
        alturaPantalla = tamaņoCamara * 2;
        
    }

    // Update is called once per frame
    void Update()
    {
        CalcularPosicionCamara();
    }

    void CalcularPosicionCamara()
    {
        int pantallaPersonaje = (int)(personaje.position.y / alturaPantalla);
        float alturaCamara = (pantallaPersonaje * alturaPantalla) + tamaņoCamara;

        transform.position = new Vector3(transform.position.x, alturaCamara, transform.position.z); 
    }
}
