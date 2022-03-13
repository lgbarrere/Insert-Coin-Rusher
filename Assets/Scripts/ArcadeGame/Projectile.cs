using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            bool droneShoot = false;
            if (name == "DroneProjectile(Clone)")
            {
                droneShoot = true;
            }
            col.transform.GetComponent<EnemyEye>().DealDamage(droneShoot);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("ProjectileDestroyer"))
        {
            Destroy(gameObject);
        }
    }
}

