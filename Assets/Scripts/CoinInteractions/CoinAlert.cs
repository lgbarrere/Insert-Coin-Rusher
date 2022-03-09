using UnityEngine;
using UnityEngine.UI;

public class CoinAlert : MonoBehaviour
{
    public GameObject dude;
    public Image alert;
    private SphereCollider CoinCollision;
    private float alertTimer = 0;
    public const float ALERT_BLINK_TIME = 0.3f;
    private int nbBlinks = 0;
    public const int NB_MAX_BLINKS = 4;

    void Start()
    {
        CoinCollision = dude.GetComponent<SphereCollider>();
    }

    private void ResetAlert()
    {
        alert.enabled = false;
        alertTimer = 0;
    }

    void Update()
    {
        if(CoinCollision.enabled)
        {
            if(!alert.enabled && nbBlinks == 0)
            {
                alert.enabled = true;
            }
            if(nbBlinks < NB_MAX_BLINKS)
            {
                alertTimer += Time.deltaTime;

                if (alertTimer >= ALERT_BLINK_TIME)
                {
                    alert.enabled = !alert.enabled;
                    alertTimer -= ALERT_BLINK_TIME;
                    nbBlinks++;
                    if (nbBlinks >= NB_MAX_BLINKS)
                    {
                        ResetAlert();
                    }
                }
            }
        }
        else
        {
            if (nbBlinks > 0)
            {
                nbBlinks = 0;
            }
            if (alert.enabled)
            {
                ResetAlert();
            }
        }
    }
}
