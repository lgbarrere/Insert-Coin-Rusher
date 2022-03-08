using UnityEngine;

public class PressRightButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
