using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeSlot : MonoBehaviour
{
    public bool fentePourFuel = true;
        

    public ArcadeCoinSlot maPiece;

    

    //Lors du survol de la souris : afficher la pi�ce

    //Lors du clique de la souris : lancer l'animation

    //Lorsque la souris quitte la fente SANS AVOIR CLIQUE  : faire disparaitre la pi�ce (si l'animation est d�j� lanc� ne pas faire disparaitre la pi�ce)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnMouseEnter()
    {
        maPiece.Apparition();

    }

    void OnMouseExit()
    {
        if (!maPiece.animationIsON)
        {
            maPiece.Disparition();
        }
        

    }

    void OnMouseDown()
    {
        maPiece.animationIsON = true;

    }
}
