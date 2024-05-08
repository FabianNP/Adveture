using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float cooldownAtaque;
    private bool puedeAtacar = true;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Si no puede atacar salimos de la funcion
            if (!puedeAtacar) return;

            // Desactivamos el ataque
            puedeAtacar = false;

            // Cambiamos la opacidad del sprite
            Color color = spriteRenderer.color;
            color.a = 0.5f;
            spriteRenderer.color = color;


            // Perdemos una vida
            GameManager.Instance.PerderVida();

            // Aplicamos golpe al 
            collision.gameObject.GetComponent<CharacterController>().AplicarGolpe();

            Invoke("ReactivarAtaque", cooldownAtaque);
        }
    }

    void ReactivarAtaque()
    {
        puedeAtacar = true;

        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }
}
