using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>(); //Récupère le RigidBody du joueur s'il n'est pas attribué
    }

    // Update is called once per frame
    void Update()
    {
        if (!Menu.pause)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(Vector2.left * moveSpeed);
                Debug.Log("Gauche");
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector2.right * moveSpeed);
                Debug.Log("Droite");
            }
        }
    }
}
