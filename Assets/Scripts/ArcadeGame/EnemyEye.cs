using System.Collections;
using UnityEngine;

public class EnemyEye : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject projectile;

    [SerializeField] float health = 2f;
    [SerializeField] float speed = 3f;

    Vector2 moveVector;
    GameManager gameManager;
    private bool isDying = false;
    
    void Awake()
    {
        StartCoroutine(ChooseDirection());
    }

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        tag = "Enemy";
    }

    IEnumerator ChooseDirection()
    {
        while (true)
        {
            moveVector.x = Random.Range(-1f, 1f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    void FixedUpdate()
    {
        Move(moveVector);
    }

    void Move(Vector2 movement)
    {
        rb.AddForce(movement * speed);
    }

    public void DealDamage(bool shotByDrone)
    {
        health -= gameManager.spaceship.powerShoot;

        if (health <= 0 && !isDying)
        {
            if (shotByDrone)
            {
                gameManager.successManager.UpdateAssistedSuccess();
            }
            else
            {
                gameManager.successManager.ResetAssistedSuccess();
            }
            isDying = true;
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        gameManager.AddFuel(2);
        gameManager.ReduceEnemyCount();
        gameManager.AddScore(1);

        Component[] projectiles = GetComponentsInChildren<EyeProjectile>();
        foreach(EyeProjectile ep in projectiles)
        {
            ep.transform.SetParent(null);
        }

        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        Destroy(gameObject);
    }

    void Shoot()
    {
        float value = Random.Range(0, 1f);

        if (value >= 0.5f)
        {
            animator.SetTrigger("shoot");
        }
    }

    void MakeProjectile()
    {
        Vector3 startPos = transform.position;
        startPos.y -= 0.1f;
        GameObject go = Instantiate(projectile, startPos, Quaternion.identity);
        go.transform.SetParent(transform);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
