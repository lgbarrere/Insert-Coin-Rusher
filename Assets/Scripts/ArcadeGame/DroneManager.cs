using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public AudioSource laserSound;
    public float shootTimer = 2f;
    private float shootCurrentTime = 0f;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootCurrentTime += Time.deltaTime;

        if(shootCurrentTime >= shootTimer)
        {
            laserSound.Play();
            shootCurrentTime -= shootTimer;
            Shoot();
        }
    }

    // Make the drone shoot
    void Shoot()
    {
        Vector3 startPos = transform.position;
        startPos.y += 0.05f;
        Instantiate(projectile, startPos, Quaternion.identity);
    }
}
