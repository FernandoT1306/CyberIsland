using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    private bool MovIzquierda = false;
    private bool MovDerecha = false;
    private TriviaManager Manager;
    public float Speed;
    public float JumpForce;
    public GameObject BulletPrefab;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShoot;
    public int Health = 5;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Manager = FindObjectOfType<TriviaManager>();
        Manager.SetPlayer(gameObject);
    }


    private void Update()
    {
        if (!Manager.Pausa)
        {
            // Movimiento
            Horizontal = Input.GetAxisRaw("Horizontal");

            if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            if (MovIzquierda)
            {
                Horizontal = -1.0f;
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else if (MovDerecha)
            {
                Horizontal = 1.0f;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            Animator.SetBool("running", Horizontal != 0.0f);

            // Detectar Suelo
            // Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
            if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
            {
                Grounded = true;
            }
            else Grounded = false;

            // Salto
            if (Input.GetKeyDown(KeyCode.W) && Grounded || Input.GetKeyDown(KeyCode.UpArrow) && Grounded)
            {
                Jump();
            }

            // Disparar
            if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
            {
                Shoot();
                LastShoot = Time.time;
            }
        }
      
    }

    private void FixedUpdate()
    {
        if (!Manager.Pausa)
        {
            Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
        }
        else
        {
            Rigidbody2D.velocity = Vector2.zero;
        }

    }

    public void Izquierda(bool estado)
    {
        MovIzquierda = estado;
    }

    public void Derecha(bool estado)
    {
        MovDerecha = estado;
    }

    public void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    public void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que choca con la moneda es el personaje
        if (other.CompareTag("Moneda"))
        {
            Manager.RecogerMoneda();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Estrella"))
        {
            Manager.RecogerEstrella();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("LimiteMuerte"))
        {
            Manager.Morir();
        }

        if (other.CompareTag("Meta"))
        {
            Manager.NivelCompletado();
        }       
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0)
        {
            Manager.MostrarPreguntaAleatoria();
           // Destroy(gameObject);
        } 
    }
}