using UnityEngine;

public class PressCButton : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetBool("is_pressed", true);
            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                animator.SetBool("is_pressed", false);
            }
        }
    }
}
