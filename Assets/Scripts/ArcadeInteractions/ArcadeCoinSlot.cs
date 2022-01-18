using UnityEngine;

public class ArcadeCoinSlot : MonoBehaviour
{
    private Vector3 startPosition;
    public float insertTime = 0.2f;
    private float currentInsertTime = 0;
    private float speed; // How fast will move the coin

    [SerializeField] SpriteRenderer coinSprite;
    public bool animationIsON = false; // true if the coin is moving, false otherwise

    void Start()
    {
        startPosition = transform.position;
        speed = Mathf.Abs(transform.localPosition.z) / insertTime; // Speed = Distance / Time

        Disparition();
    }

    public void Apparition()
    {
        coinSprite.enabled = true;
    }

    public void Disparition()
    {
        coinSprite.enabled = false;
    }


    void Update()
    {
        if(animationIsON)
        {
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            currentInsertTime += Time.deltaTime;

            if (currentInsertTime >= insertTime)
            {
                currentInsertTime = 0;
                transform.position = startPosition;
                animationIsON = false;
                Disparition();
            }
        }
    }
}
