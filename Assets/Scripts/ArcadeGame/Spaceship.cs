using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameManager gameManager;

    public int powerShoot = 1;
    public int maxPowerShoot = 2;
    
    [SerializeField] float health = 1f;
    [SerializeField] float speed; //Vitesse de déplacement du vaisseau
    float horizontalMove, verticalMove; //Stocke l'input du joueur
    Vector2 moveVector; //Vecteur de déplacement du vaisseau
    [SerializeField] GameObject projectile; //Projectile tiré par le vaisseau
    [SerializeField] Shield shield;
    bool canShoot = false; //Détermine s'il est possible de tirer
    float shootDelay = 0.25f; //Délai entre chaque tir
    
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        StartCoroutine(ShootDelay(shootDelay * 2f)); //Empêche le joueur de tirer instantanément
    }

    // Update is called once per frame
    void Update()
    {
        GetMovement();

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    // FixedUpdate is called once per time step
    void FixedUpdate()
    {
        Move(moveVector);
    }

    //Récupère l'input du joueur pour le déplacement
    void GetMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal"); //Input de l'axe horizontal
        verticalMove = Input.GetAxisRaw("Vertical"); //Input de l'axe vertical
        moveVector = new Vector2(horizontalMove, verticalMove); //Vector2D du déplacement

            animator.SetBool("left", Input.GetKey(KeyCode.Q));
            animator.SetBool("right", Input.GetKey(KeyCode.D));
    }

    //Déplace le vaisseau en fonction du mouvement donné
    void Move(Vector2 movement)
    {
        rb.AddForce(movement * speed);
    }

    public void DealDamage()
    {
        health--;

        if (health <= 0)
        {
            StartCoroutine(Kill());
        } else {
            shield.BreakShield();
        }
    }

    IEnumerator Kill()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }

    //Tire un projectile
    void Shoot()
    {
        if (canShoot)
        {
            Vector3 startPos = transform.position;
            startPos.y += 0.05f;
            Instantiate(projectile, startPos, Quaternion.identity);
            canShoot = false;
            StartCoroutine(ShootDelay(shootDelay));
        }
    }

    //Ajoute un délai entre chaque tir
    IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    public void Shield()
    {
        if(shield.isActive) return;
        health++;
        shield.ActivateShield();
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void EnableControls()
    {
        gameManager.EnableControls();
    }
}
