using UnityEngine;
using UnityEngine.UI;

public class PickEmergencyCoin : MonoBehaviour
{
    [SerializeField] AudioSource cannotPickCoinSound;
    [SerializeField] GameManager gameManager;
    public Image coinFeedback;
    public RectTransform coinMovingPos;
    public RectTransform inventoryTransform;
    public RectTransform CanvasRect;
    public SpriteRenderer emergencyCoinSprite;
    public float coinMovingSpeed = 3000f;

    void Update()
    {
        if (coinFeedback.enabled)
        {
            MoveCoinToInventory();
        }
    }

    void MoveCoinToInventory()
    {
        float step = coinMovingSpeed * Time.deltaTime;
        coinMovingPos.position = Vector3.MoveTowards(coinMovingPos.position, inventoryTransform.position, step);

        // If the coin approximatively reached the inventory, reset the moving coin
        if (Vector3.Distance(coinMovingPos.position, inventoryTransform.position) < 0.001f)
        {
            ResetMovingCoin();
            gameManager.AddCoin();
        }
    }

    void ResetMovingCoin()
    {
        coinMovingPos.position = Vector3.zero;
        coinFeedback.enabled = false;
    }

    void TriggerCoinMoving()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, null, out Vector2 anchoredPos);
        coinMovingPos.anchoredPosition = anchoredPos;

        coinFeedback.enabled = true;
    }

    void OnMouseDown()
    {
        if (!gameManager.MaxCoinReached())
        {
            TriggerCoinMoving();
            emergencyCoinSprite.enabled = false;
            Destroy(this.gameObject);
        }
        else
        {
            cannotPickCoinSound.Play();
        }
    }
}
