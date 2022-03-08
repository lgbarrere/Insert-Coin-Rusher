using UnityEngine;

public class PressDownButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
