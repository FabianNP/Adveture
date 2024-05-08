using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float fuerzaGolpe;
    public int saltosMaximos;
    public LayerMask capaSuelo;
    public AudioClip audioSalto;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private int saltosRestantes;
    private Animator animator;
    private bool puedeMoverse = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        saltosRestantes = saltosMaximos;
        animator = GetComponent<Animator>();
        Debug.Log(boxCollider.bounds);
    }
    void Update()
    {
        ProcesarMovimiento();
        ProcesarSalto();
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x - 0.400f, boxCollider.bounds.size.y - 0.500f), 0, Vector2.down, 0.3f, capaSuelo);
        return raycastHit.collider != null;
    }

    void ProcesarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)

        {
            saltosRestantes--;
            Debug.Log(saltosRestantes);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0f);
            rigidbody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            AudioManager.Instance.ReproducirSonido(audioSalto);
        }
        if (EstaEnSuelo())
        {
            saltosRestantes = saltosMaximos;
            Debug.Log("Suelo");
            Debug.Log(saltosRestantes);
        }
    }
    void ProcesarMovimiento()
    {
        // Si no puede moverse, salimos de la funcion
        if (!puedeMoverse) return;   

        // Logica de movimiento
        float inputMovimiento = Input.GetAxis("Horizontal");

        if(inputMovimiento != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        rigidbody.velocity = new Vector2(inputMovimiento * velocidad, rigidbody.velocity.y);
        GestionarOrientacion(inputMovimiento);
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        if (mirandoDerecha && (inputMovimiento < 0) || !mirandoDerecha && (inputMovimiento > 0))
        {
            mirandoDerecha = (inputMovimiento > 0);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    public void AplicarGolpe()
    {
        puedeMoverse = false;

        Vector2 direccionGolpe;

        if(rigidbody.velocity.x > 0f)
        {
          direccionGolpe = new Vector2(-1, 1);
        }
        else
        {
          direccionGolpe = new Vector2(1, 1);
        }
        rigidbody.AddForce(direccionGolpe * fuerzaGolpe);

        StartCoroutine(EsperarYActivarMovimiento());

    }

    IEnumerator EsperarYActivarMovimiento()
    {
        yield return new WaitForSeconds(0.1f);

        while (!EstaEnSuelo())
        {
            yield return null;
        }

        puedeMoverse = true;
    }
}