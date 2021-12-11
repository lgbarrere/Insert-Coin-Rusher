using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponentInChildren<AudioSource>();

        NextCoin();
    }

    // Update is called once per frame
    void NextCoin()
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
        audioSource.Play();
    }

    void OnMouseDown()
    {
        gameManager.AddCoin();
        animator.SetTrigger("takeCoin");

        NextCoin();
    }
}
