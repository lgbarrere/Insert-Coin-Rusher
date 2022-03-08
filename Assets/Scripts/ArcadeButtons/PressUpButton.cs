using UnityEngine;

public class PressUpButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
