using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public AudioSource laserSound;
    public float shootTimer = 2f;
    private float shootCurrentTime = 0f;
    public GameObject projectile;
    private const float maxDroneOffset = 0.2f;
    private float droneOffset = 0f;
    private const float moveSpeed = 0.5f;
    bool moveLeft = true;

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

    public void Move(Vector3 spaceshipPos)
    {
        Vector3 dronePos = spaceshipPos;

        // Determine if the drone must move left or right
        if(Mathf.Abs(droneOffset) >= maxDroneOffset)
        {
            moveLeft = !moveLeft;
        }

        // Apply moves
        float step = moveSpeed * Time.deltaTime;
        if (moveLeft)
        {
            droneOffset -= step;
        }
        else
        {
            droneOffset += step;
        }

        dronePos.x += droneOffset;
        dronePos.y += 0.1f;
        transform.position = dronePos;
    }
}
