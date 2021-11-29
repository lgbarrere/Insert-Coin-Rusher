using UnityEngine;

public class PressDownButton : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s") && !animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", true);
        }
        else if (Input.GetKeyUp("s") && animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", false);
        }
    }
}
