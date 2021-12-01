using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraRotationSpeed = 100f;
    public const float maxAngle = 45f;
    public const float minAngle = -45f;
    private float yRotation = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            yRotation -= cameraRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.E))
        {
            yRotation += cameraRotationSpeed * Time.deltaTime;
        }
        yRotation = Mathf.Clamp(yRotation, minAngle, maxAngle);
        transform.eulerAngles = new Vector3(0.0f, yRotation, 0.0f);
    }
}
