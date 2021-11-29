using UnityEngine;

public class PickCoin : MonoBehaviour
{
    public Transform coinPos;
    public SpriteRenderer sprite;
    public Animator animator;
    bool keptCoin = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            sprite.enabled = true;
            keptCoin = !keptCoin;
        }
        if(keptCoin)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 3.0f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            coinPos.position = worldPosition;
        }
    }
}
