using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int CurrentX{set;get;}
    public int CurrentY{set;get;}
    public byte maxHeight{set;get;}
    public byte maxWidth{set;get;}
    public byte speed{set;get;}
    public byte Team;
    //public bool turn;
    public bool [,] moves{set;get;}


    //public void Character

    public void SetPosition(int x, int y)
    {
        CurrentX = x; CurrentY = y;
    }
    
    public void SetDimensions(byte H, byte W)
    {
        maxHeight = H; maxWidth = W;
    }

    public void SetSpeed(byte s)
    {
        speed = s;
    }

    public void CheckMoves()
    {
        moves = PossibleMoves();
    }

    public virtual bool[,] PossibleMoves ()
    {//check for up down left right etc, later CHECK for speed 2 movement possibilities
        //if (Mathf.Abs(CurrentX - x) <= speed && Mathf.Abs(CurrentY - y) <= speed) // if destination coordinate is within speed limitations && is not out of bounds
        moves = new bool[maxWidth,maxHeight];
        for(int i = 0; i < maxWidth; i++)
        {
            for (int j = 0; j < maxHeight; j++)
            {
                moves[i,j] = false;
            }
        }
        for (int i = 0; i < maxWidth; i++)
        {
            for (int j = 0; j < maxHeight; j++)
            {
                //Debug.Log((Mathf.Abs(CurrentX - i) + Mathf.Abs(CurrentY - j)).ToString());
                //Debug.Log(speed.ToString() + " SPEED");
                if(Mathf.Abs(CurrentX - i) + Mathf.Abs(CurrentY - j) <= speed)
                {
                    moves[i,j] = true;
                } 
                else
                {
                    //Debug.Log("F");
                    moves[i,j] = false;
                }
            }
        }
        return moves;
    }

    public void PrintCoordinates()
    {
        //Debug.Log("x = " + CurrentX + " y = " + CurrentY);
    }

}
