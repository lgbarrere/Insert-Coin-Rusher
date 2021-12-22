using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.transform.GetComponent<EnemyEye>().DealDamage();
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "ProjectileDestroyer")
        {
            Destroy(gameObject);
        }
    }
}

