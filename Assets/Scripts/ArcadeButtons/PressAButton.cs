using UnityEngine;

public class PressAButton : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("is_pressed", Input.GetKey(KeyCode.Space));
    }
}
