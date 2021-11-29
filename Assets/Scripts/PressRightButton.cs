using UnityEngine;

public class PressRightButton : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("d") && !animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", true);
        }
        else if (Input.GetKeyUp("d") && animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", false);
        }
    }
}
