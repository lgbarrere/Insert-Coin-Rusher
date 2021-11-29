using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenteBonus : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] script_pieceFente maPiece;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void OnMouseEnter()
    {
        if (gameManager.coins > 0 && gameManager.isPlaying)
        {
            maPiece.apparition();
        }

    }

    void OnMouseExit()
    {
        if (!maPiece.animationIsON)
        {
            maPiece.disparition();
        }
        

    }

    void OnMouseDown()
    {
        if (!gameManager.isPlaying) return;

        if (gameManager.UseCoin())
        {
            maPiece.animationIsON = true;

            gameManager.RandomBonus();
        }
    }
}
