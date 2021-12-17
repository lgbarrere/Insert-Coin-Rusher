using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float zoomedOutRotationSpeed = 150f;
    private const float zoomedInRotationSpeed = 200f;
    public float cameraRotationSpeed = zoomedOutRotationSpeed;
    public const float maxAngle = 45f;
    public const float minAngle = -45f;
    private float yRotation = 0;
    public CameraZoom cameraZoom;

    void Start()
    {
        if (cameraZoom == null) cameraZoom = GetComponent<CameraZoom>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(cameraZoom.zoomState)
        {
            case CameraZoom.ZoomState.ZOOMED_IN:
                cameraRotationSpeed = zoomedInRotationSpeed;
                break;
            case CameraZoom.ZoomState.ZOOMED_OUT:
                cameraRotationSpeed = zoomedOutRotationSpeed;
                break;
        }

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
