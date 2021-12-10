using System.Collections;
using System.Collections.Generic;
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
    
    void Awake()
    {
        StartCoroutine(ChooseDirection());
    }

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    IEnumerator ChooseDirection()
    {
        while (true)
        {
            float rnd = Random.Range(-1f, 1f);
            moveVector = new Vector2(rnd, 0);

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

    public void DealDamage()
    {
        health -= gameManager.spaceship.powerShoot;

        if (health <= 0)
        {
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        gameManager.AddFuel(2);
        gameManager.ReduceEnemyCount();

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
