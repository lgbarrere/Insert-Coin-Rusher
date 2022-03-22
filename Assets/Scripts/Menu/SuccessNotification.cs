using UnityEngine;

enum NotificationState
{
    HIDDEN, SHOWING, STANDING, HIDDING
}

public class SuccessNotification : MonoBehaviour
{
    private RectTransform[] successPositions;
    private Canvas[] successAppearence;
    private int notificationOrder = 0;
    private readonly NotificationState[] successNotification = new NotificationState[10];
    const float notificationSpeed = 200f;
    private float notificationStandTime = 0;
    private const float MAX_STAND_TIME = 3f;
    float notificationDistance = 0;
    const float NOTIFICATION_MAX_DISTANCE = 100;

    void Start()
    {
        successPositions = GetComponentsInChildren<RectTransform>();
        successAppearence = GetComponentsInChildren<Canvas>();
        for (int i = 0; i < 10; i++)
        {
            successNotification[i] = NotificationState.HIDDEN;
        }
    }

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            switch(successNotification[i])
            {
                case NotificationState.HIDDEN:
                    break;
                case NotificationState.SHOWING:
                    successPositions[i + 1].localPosition = new(
                        successPositions[i + 1].localPosition.x, 
                        successPositions[i + 1].localPosition.y - notificationSpeed * Time.deltaTime
                        );
                    notificationDistance += notificationSpeed * Time.deltaTime;
                    if(notificationDistance >= NOTIFICATION_MAX_DISTANCE)
                    {
                        Debug.Log(successPositions[i + 1].localPosition);
                        successPositions[i + 1].localPosition = new(
                            successPositions[i + 1].localPosition.x,
                            -NOTIFICATION_MAX_DISTANCE
                            );
                        notificationDistance = NOTIFICATION_MAX_DISTANCE;
                        successNotification[i] = NotificationState.STANDING;
                        Debug.Log(successPositions[i + 1].localPosition);
                    }
                    break;
                case NotificationState.STANDING:
                    notificationStandTime += Time.deltaTime;
                    if (notificationStandTime >= MAX_STAND_TIME)
                    {
                        notificationStandTime = 0;
                        successNotification[i] = NotificationState.HIDDING;
                    }
                    break;
                case NotificationState.HIDDING:
                    successPositions[i + 1].localPosition = new(
                        successPositions[i + 1].localPosition.x,
                        successPositions[i + 1].localPosition.y + notificationSpeed * Time.deltaTime
                        );
                    notificationDistance -= notificationSpeed * Time.deltaTime;
                    if (notificationDistance <= 0)
                    {
                        successPositions[i + 1].localPosition = new(
                            successPositions[i + 1].localPosition.x,
                            0
                            );
                        notificationDistance = 0;
                        successNotification[i] = NotificationState.HIDDEN;
                        notificationOrder--;
                        successAppearence[i].sortingOrder = notificationOrder;
                    }
                    break;
            }
        }
    }

    public void ShowSuccessNotification(int successID)
    {
        successNotification[successID] = NotificationState.SHOWING;
        notificationOrder++;
        successAppearence[successID].sortingOrder = notificationOrder;
    }
}
