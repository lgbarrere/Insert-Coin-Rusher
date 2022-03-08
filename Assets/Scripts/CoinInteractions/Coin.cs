using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource coinFallSound;
    [SerializeField] AudioSource cannotPickCoinSound;
    [SerializeField] GameManager gameManager;
    public Image coinFeedback;
    public RectTransform coinMovingPos;
    public RectTransform inventoryTransform;
    public RectTransform CanvasRect;
    public float coinMovingSpeed = 3000f;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (coinFallSound == null) coinFallSound = GetComponentInChildren<AudioSource>();
        
        NextCoin();
    }

    void Update()
    {
        if (coinFeedback.enabled)
        {
            MoveCoinToInventory();
        }
    }

    // Update is called once per frame
    public void NextCoin()
    {
        int rnd = Random.Range(5, 15);
        StartCoroutine(MakeCoin(rnd));
    }

    IEnumerator MakeCoin(int wait)
    {
        yield return new WaitForSeconds(wait);

        animator.SetTrigger("makeCoin");
    }

    void PlaySound()
    {
        coinFallSound.Play();
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
        if (!Menu.pause)
        {
            if (!gameManager.MaxCoinReached())
            {
                animator.SetTrigger("takeCoin");
                TriggerCoinMoving();
                NextCoin();
            }
            else
            {
                cannotPickCoinSound.Play();
            }
        }
    }
}
