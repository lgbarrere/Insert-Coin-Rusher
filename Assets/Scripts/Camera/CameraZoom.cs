using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    //param�trer une touche du clavier
    public KeyCode test;

    //Zoom :
    //Initialisation : SphereCollider
    private SphereCollider maHitBox;
    //Initialisation : Position de la cam
    private Transform maposition;
    private Vector3 flag;

    public Transform HitBoxZoom;
    public Transform HitBoxDezoom;

    //Variable vitesse (float)
    public float vitesse = 5;

    //Variable etat : 0 (dezoom), 1(en train de zoomer), 2(zoom�), 3(en train de d�zoomer), 4 (en positionnement final)
    public int etat = 4;

    //ETAT DEZOOM
    public const float POSITION_DZ_Y = 8.22F;
    public const float POSITION_DZ_Z = -10.0F;

    //ETAT ZOOM
    public const float POSITION_ZOOM_Y = 8.91F;
    public const float POSITION_ZOOM_Z = -5.0F;

    public bool zoom_initial = false;

    private float deltaY = 0;
    private float deltaZ = 0;



    // Start is called before the first frame update
    void Start()
    {
        maposition = this.GetComponent(typeof(Transform)) as Transform;
        maHitBox = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;

        flag = maposition.position;

        HitBoxZoom.position = new Vector3(flag.x, POSITION_ZOOM_Y, POSITION_ZOOM_Z);
        HitBoxDezoom.position = new Vector3(flag.x, POSITION_DZ_Y, POSITION_DZ_Z);

        if (zoom_initial)
        {
            //position zoom
            maposition.position = new Vector3(flag.x, POSITION_ZOOM_Y, POSITION_ZOOM_Z);
            etat = 2;
            //maposition.position.z = POSITION_Z_Z;
        }
        else
        {
            //position dezoom
            
            maposition.position = new Vector3(flag.x, POSITION_DZ_Y, POSITION_DZ_Z);
            etat = 0;
            //maposition.position.y = POSITION_DZ_Y;
            //maposition.position.z = POSITION_DZ_Z;

        }

        deltaY = POSITION_ZOOM_Y - POSITION_DZ_Y;
        deltaZ = POSITION_ZOOM_Y - POSITION_DZ_Z;
    }


    private void toggleZoom()
    {
        Debug.Log("toggle le zoom");
        
        if(etat%2 == 0)
        {
            etat++;
            Debug.Log("Etat = " + etat);
        }
    }



    // Update is called once per frame
    void Update()
    {

        //lorsque cette touche est appuy�e, si la cam n'est pas en mouvement : toggle le zoom
        if (Input.GetKeyDown(test))
        {
            toggleZoom();
        }

        //Lorsque le zoom est activ�,
        // Si etat=1  Avancer vers la destination � la vitesse pr�d�finie
        // Si etat=3  Reculer vers la position initiale

        if (etat == 1)
        {
            flag = maposition.position;
            maposition.position = new Vector3(0, flag.y + vitesse / 35 * (deltaY / (deltaY + deltaZ)) , flag.z + vitesse * (deltaZ / (deltaY + deltaZ)) / 100);
        }
        else if (etat == 3)
        {
            flag = maposition.position;
            maposition.position = new Vector3(0, flag.y - vitesse / 35 * (deltaY / (deltaY + deltaZ)) , flag.z - vitesse * (deltaZ / (deltaY + deltaZ)) / 100);
        }
    }



    void OnTriggerEnter(Collider finDeCourses)
    {
        //Lorsque la cam rencontre un obstacle :
        // Si �tat = 1 : etat=4 -> position = position destination -> etat = 2
        // Si �tat = 3 : etat=4 -> position = position arriv� -> etat = 0
        //Debug.Log("collision detect�e");
        if(etat == 1)
        {
            etat = 4;
            maposition.position = new Vector3(flag.x, POSITION_ZOOM_Y, POSITION_ZOOM_Z);
            etat = 2;
        } else if (etat == 3)
        {
            etat = 4;
            maposition.position = new Vector3(flag.x, POSITION_DZ_Y, POSITION_DZ_Z);
            etat = 0;
        }
    }
}
