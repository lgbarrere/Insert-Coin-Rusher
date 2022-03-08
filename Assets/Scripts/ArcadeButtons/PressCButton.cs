using UnityEngine;

public class PressCButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
