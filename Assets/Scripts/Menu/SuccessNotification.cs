using UnityEngine;

enum NotificationState
{
    HIDDEN, SHOWING, STANDING, HIDDING
}

public class SuccessNotification : MonoBehaviour
{
    private Transform[] successPositions;
    private Canvas[] successAppearence;
    private int notificationOrder = 0;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private readonly NotificationState[] successNotification = new NotificationState[10];
    const float notificationSpeed = 2f;
    private float notificationStandTime = 0;
    private const float MAX_STAND_TIME = 3f;

    void Start()
    {
        successPositions = GetComponentsInChildren<Transform>();
        successAppearence = GetComponentsInChildren<Canvas>();
        startPosition = successPositions[0].position;
        targetPosition = new(
            successPositions[0].position.x, successPositions[0].position.y - 100
            );
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
                    successPositions[i + 1].position = Vector2.MoveTowards(
                        successPositions[i + 1].position, targetPosition, notificationSpeed
                        );
                    if (Vector2.Distance(successPositions[i + 1].position, targetPosition) < 0.001f)
                    {
                        successPositions[i + 1].position = targetPosition;
                        successNotification[i] = NotificationState.STANDING;
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
                    successPositions[i + 1].position = Vector2.MoveTowards(
                        successPositions[i + 1].position, startPosition, notificationSpeed
                        );
                    if (Vector2.Distance(successPositions[i + 1].position, startPosition) < 0.001f)
                    {
                        successPositions[i + 1].position = startPosition;
                        successNotification[i] = NotificationState.HIDDEN;
                        notificationOrder--;
                    }
                    break;
            }
        }
    }

    public void ShowSuccessNotification(int successID)
    {
        successNotification[successID] = NotificationState.SHOWING;
        successAppearence[successID].sortingOrder = notificationOrder;
        notificationOrder++;
    }
}
