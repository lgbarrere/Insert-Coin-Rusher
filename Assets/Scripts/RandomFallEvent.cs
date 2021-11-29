using UnityEngine;

public class RandomFallEvent : MonoBehaviour
{
    public float minTimeEvent = 10f;
    public float maxTimeEvent = 20f;
    float timeEvent = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeEvent = Random.Range(minTimeEvent, maxTimeEvent);
    }
}
