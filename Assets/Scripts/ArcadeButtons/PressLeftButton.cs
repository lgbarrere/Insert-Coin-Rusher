using UnityEngine;

public class PressLeftButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
