using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_pieceFente : MonoBehaviour
{

    private Transform position;
    private Vector3 positionDeDepart;

    [SerializeField] SpriteRenderer monSprite;
    public bool animationIsON = false; // penser � le remettre sur false � la fin de l'animation

    public float vitesse = 1;

    private SphereCollider maHitBox;

    //r�cup�rer positionDeDepart
    //creer une hitbox

    //Lorsque la souris passe devant la fente il apparait (faire un script avec le fente)
    //Lorque la souris n'est plus devant la fente il disparait


    //Lors d'un clique sur la fente (sur script de la fente)
    //m�thode pour d�clancher un bullet

    //Lorsque la pi�ce atteint la hitbox fin de course
    //Disparition de la pi�ce
    // + retour en position initiale

    public bool piecePourFuel = true;

    // Start is called before the first frame update
    void Start()
    {

        maHitBox = gameObject.AddComponent<SphereCollider>();
        maHitBox.isTrigger = true;


        position = this.GetComponent(typeof(Transform)) as Transform;
        positionDeDepart = position.position;

        disparition();
    }

    public void apparition()
    {
        monSprite.enabled = true;

    }

    public void disparition()
    {
        monSprite.enabled = false;

    }


    // Update is called once per frame
    void Update()
    {
        if(animationIsON)
        {
            position.position += new Vector3(0, 0, vitesse * Time.deltaTime);
        }
    }


    void OnTriggerEnter(Collider finDeCourses)
    {

        position.position = positionDeDepart;
        animationIsON = false;
        disparition();

    }


}
