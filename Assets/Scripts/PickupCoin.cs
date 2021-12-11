using UnityEngine;

public class PickupCoin : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 screenMousePos = new(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z));
        Vector3 newPos = Camera.main.ScreenToWorldPoint(screenMousePos);
        newPos.z = -2;
        transform.position = newPos;
    }
}
