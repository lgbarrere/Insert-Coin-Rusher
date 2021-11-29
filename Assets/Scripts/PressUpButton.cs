using UnityEngine;

public class PressUpButton : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z") && !animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", true);
        }
        else if (Input.GetKeyUp("z") && animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", false);
        }
    }
}
