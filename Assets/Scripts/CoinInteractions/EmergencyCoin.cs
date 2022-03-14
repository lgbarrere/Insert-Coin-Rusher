using UnityEngine;
using UnityEngine.UI;

public class EmergencyCoin : MonoBehaviour
{
    [SerializeField] AudioSource cannotPickCoinSound;
    [SerializeField] GameManager gameManager;
    public Image coinFeedback;
    public RectTransform coinMovingPos;
    public RectTransform inventoryTransform;
    public RectTransform canvasRect;
    public SpriteRenderer emergencyCoinSprite;
    public SphereCollider coinCollider;
    public float coinMovingSpeed = 3000f;
    private bool isCoinMoving = false;
    public SuccessManager successManager;

    void Update()
    {
        if (isCoinMoving)
        {
            MoveCoinToInventory();
        }
    }

    private void MoveCoinToInventory()
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

    private void ResetMovingCoin()
    {
        coinMovingPos.position = Vector3.zero;
        coinFeedback.enabled = false;
        isCoinMoving = false;
    }

    private void TriggerCoinMoving()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out Vector2 anchoredPos);
        coinMovingPos.anchoredPosition = anchoredPos;
        coinFeedback.enabled = true;
        isCoinMoving = true;
    }

    public void ResetEmergencyCoin()
    {
        coinCollider.enabled = true;
        emergencyCoinSprite.enabled = true;
    }

    void OnMouseDown()
    {
        if (!gameManager.MaxCoinReached())
        {
            TriggerCoinMoving();
            successManager.StartPacifistSuccess();
            coinCollider.enabled = false;
            emergencyCoinSprite.enabled = false;
        }
        else
        {
            cannotPickCoinSound.Play();
        }
    }
}
