using UnityEngine;

public class RandomFallEvent : MonoBehaviour
{
    public float minTimeEvent = 10f;
    public float maxTimeEvent = 20f;
    float timeEvent = 0;

    // Update is called once per frame
    void Update()
    {
        timeEvent = Random.Range(minTimeEvent, maxTimeEvent);
    }
}
