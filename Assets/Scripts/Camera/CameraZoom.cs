using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public enum ZoomState
    {
        ZOOMED_OUT,
        ZOOMED_IN,
        ZOOMING_IN,
        ZOOMING_OUT
    }

    public float moveSpeed = 0.5f;
    private Vector3 zoomOutPosition;
    private Vector3 zoomInPosition;
    public ZoomState zoomState;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        zoomOutPosition = transform.position;
        zoomInPosition = new(transform.position.x, transform.position.y, transform.position.z + 5);
        zoomState = ZoomState.ZOOMED_OUT;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Menu.pause)
        {
            float step = moveSpeed * Time.deltaTime;

            switch (zoomState)
            {
                // Try to zoom in
                case ZoomState.ZOOMED_OUT:
                case ZoomState.ZOOMING_OUT:
                    if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                    {
                        targetPosition = zoomInPosition;
                        zoomState = ZoomState.ZOOMING_IN;
                    }
                    break;
                // Try to zoom out
                case ZoomState.ZOOMED_IN:
                case ZoomState.ZOOMING_IN:
                    if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                    {
                        targetPosition = zoomOutPosition;
                        zoomState = ZoomState.ZOOMING_OUT;
                    }
                    break;
            }
            // If zooming in or out, update camera's position
            if (zoomState == ZoomState.ZOOMING_IN || zoomState == ZoomState.ZOOMING_OUT)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                // If the camera is close enough to the target, consider the target is reached
                if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
                {
                    transform.position = targetPosition;
                    if (zoomState == ZoomState.ZOOMING_IN)
                    {
                        zoomState = ZoomState.ZOOMED_IN;
                    }
                    else
                    {
                        zoomState = ZoomState.ZOOMED_OUT;
                    }
                }
            }
        }
    }
}
