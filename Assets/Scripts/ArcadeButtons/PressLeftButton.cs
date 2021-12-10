using UnityEngine;

public class PressLeftButton : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("q") && !animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", true);
        }
        else if(Input.GetKeyUp("q") && animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", false);
        }
    }
}
