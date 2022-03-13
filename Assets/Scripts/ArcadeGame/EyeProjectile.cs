using UnityEngine;

public class EyeProjectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.down);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Spaceship"))
        {
            col.transform.GetComponent<Spaceship>().DealDamage();
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("ProjectileDestroyer"))
        {
            Destroy(gameObject);
        }
    }
}
