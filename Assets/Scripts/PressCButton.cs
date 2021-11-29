using UnityEngine;

public class PressCButton : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l") && !animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", true);
        }
        else if (Input.GetKeyUp("l") && animator.GetBool("is_pressed"))
        {
            animator.SetBool("is_pressed", false);
        }
    }
}
