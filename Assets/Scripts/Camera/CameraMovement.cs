using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float zoomedOutRotationSpeed = 150f;
    private const float zoomedInRotationSpeed = 200f;
    private const float zoomingRotationSpeed = (zoomedOutRotationSpeed + zoomedInRotationSpeed) / 2;
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
        if (!Menu.pause)
        {
            // Change the rotation speed
            switch (cameraZoom.zoomState)
            {
                case CameraZoom.ZoomState.ZOOMED_IN:
                    cameraRotationSpeed = zoomedInRotationSpeed;
                    break;
                case CameraZoom.ZoomState.ZOOMING_IN:
                    break;
                case CameraZoom.ZoomState.ZOOMED_OUT:
                    cameraRotationSpeed = zoomedOutRotationSpeed;
                    break;
                case CameraZoom.ZoomState.ZOOMING_OUT:
                    break;
                default:
                    cameraRotationSpeed = zoomingRotationSpeed;
                    break;
            }
            // Right or left rotation
            if (Input.GetKey(KeyCode.A) || Input.GetMouseButton(1) && Input.mousePosition.x < Screen.width / 2.0f)
            {
                yRotation -= cameraRotationSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E) || Input.GetMouseButton(1) && Input.mousePosition.x >= Screen.width / 2.0f)
            {
                yRotation += cameraRotationSpeed * Time.deltaTime;
            }
            yRotation = Mathf.Clamp(yRotation, minAngle, maxAngle);
            transform.eulerAngles = new Vector3(0.0f, yRotation, 0.0f);
        }
    }
}
