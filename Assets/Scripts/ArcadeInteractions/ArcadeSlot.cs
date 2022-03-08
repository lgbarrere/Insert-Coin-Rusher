using UnityEngine;

public class ArcadeSlot : MonoBehaviour
{
    public bool fentePourFuel = true;
    public ArcadeCoinSlot maPiece;
    
    void OnMouseEnter()
    {
        if (!Menu.pause)
        {
            maPiece.Apparition();
        }
    }

    void OnMouseExit()
    {
        if (!Menu.pause)
        {
            if (!maPiece.animationIsON)
            {
                maPiece.Disparition();
            }
        }
    }

    void OnMouseDown()
    {
        if (!Menu.pause)
        {
            maPiece.animationIsON = true;
        }
    }
}
