using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Position
{

    public Position()
    {
        this.x = 0;
        this.y = 0;
    }

    //copy constructor
    public Position(Position originalPosition)
    {
        this.x = originalPosition.x;
        this.y = originalPosition.y;
    }

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x;
    public int y;

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

}
