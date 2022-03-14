using UnityEngine;

public class PressBButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
