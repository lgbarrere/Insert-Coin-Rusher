using UnityEngine;

public class CameraZoom2 : MonoBehaviour
{
    enum ZoomState
    {
        ZOOMED_OUT,
        ZOOMED_IN,
        ZOOMING_IN,
        ZOOMING_OUT
    }

    public float moveSpeed = 0.5f;
    Vector3 zoomOutPosition;
    Vector3 zoomInPosition;
    ZoomState zoomState;
    Vector3 targetPosition;

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
        float step = moveSpeed * Time.deltaTime;

        switch (zoomState)
        {
            case ZoomState.ZOOMED_OUT:
            case ZoomState.ZOOMING_OUT:
                // If pressed zoom in button
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    targetPosition = zoomInPosition;
                    Debug.Log("zoom in : " + targetPosition.x + " " + targetPosition.y + " " + targetPosition.z);
                    zoomState = ZoomState.ZOOMING_IN;
                }
                break;
            case ZoomState.ZOOMED_IN:
            case ZoomState.ZOOMING_IN:
                // If pressed zoom out button
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    targetPosition = zoomOutPosition;
                    Debug.Log("zoom out : " + targetPosition.x + " " + targetPosition.y + " " + targetPosition.z);
                    zoomState = ZoomState.ZOOMING_OUT;
                }
                break;
        }
        switch (zoomState)
        {
            case ZoomState.ZOOMING_IN:
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
                {
                    transform.position = targetPosition;
                    zoomState = ZoomState.ZOOMED_IN;
                    Debug.Log("Zoomed in !");
                }
                break;
            case ZoomState.ZOOMING_OUT:
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
                {
                    transform.position = targetPosition;
                    zoomState = ZoomState.ZOOMED_OUT;
                    Debug.Log("Zoomed out !");
                }
                break;
        }
    }
}
