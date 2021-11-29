using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float cameraRotationSpeed = 100f;
    float maxDegreRotation = 315;
    float minDegreRotation = 45;
    float initialRotationDegre = 0;
    //public float widthRatio = 0.1f;
    private void Start()
    {
        initialRotationDegre = cameraTransform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Input.mousePosition.x < Screen.width / 2 - Screen.width * widthRatio
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            
            if ((cameraTransform.rotation.eulerAngles.y -cameraRotationSpeed*Time.deltaTime >= maxDegreRotation && cameraTransform.rotation.eulerAngles.y <= 365) ^ (cameraTransform.eulerAngles.y <= minDegreRotation && cameraTransform.eulerAngles.y >= 0))
            {
                cameraTransform.Rotate(new Vector3(0, -cameraRotationSpeed * Time.deltaTime, 0));
            }
        }
        //Input.mousePosition.x > Screen.width / 2 + Screen.width * widthRatio
        if (Input.GetKey("right") || Input.GetKey("e"))
        {
            if ((cameraTransform.eulerAngles.y +cameraRotationSpeed*Time.deltaTime <= minDegreRotation && cameraTransform.eulerAngles.y >= 0) ^ (cameraTransform.eulerAngles.y >= maxDegreRotation && cameraTransform.eulerAngles.y <= 365))
            {
                cameraTransform.Rotate(new Vector3(0, cameraRotationSpeed * Time.deltaTime, 0));
            }
            else if (cameraTransform.eulerAngles.y > maxDegreRotation)
            {
                //cameraTransform.eulerAngles = new Vector3(0, initialRotationDegre + maxDegreRotation, 0);
                cameraTransform.Rotate(new Vector3(0, cameraRotationSpeed * Time.deltaTime, 0));
            }
        }


    }
}
