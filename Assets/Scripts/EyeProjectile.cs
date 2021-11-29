using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeProjectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Spaceship")
        {
            col.transform.GetComponent<Spaceship>().DealDamage();
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "ProjectileDestroyer")
        {
            Destroy(gameObject);
        }
    }
}
