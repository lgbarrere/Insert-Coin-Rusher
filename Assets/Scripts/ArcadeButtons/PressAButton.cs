using UnityEngine;

public class PressAButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
