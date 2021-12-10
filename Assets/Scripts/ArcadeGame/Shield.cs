using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Animator animator;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void ActivateShield()
    {
        animator.SetTrigger("activate");
        isActive = true;
    }

    public void BreakShield()
    {
        animator.SetTrigger("damage");
        isActive = false;
    }
}
