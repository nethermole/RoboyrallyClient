using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int id;
    public int health;
    public string name;
    public Direction facing;
    public Position position;
    public GameObject robotPiece;
    public Color color;

    public Player()
    {
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public void SetRobotPiece(GameObject robotPiece)
    {
        this.robotPiece = robotPiece;
        robotPiece.GetComponentInChildren<Renderer>().material.color = color;
    }

    public Direction GetFacing()
    {
        return facing;
    }

    public Color getColor()
    {
        return color;
    }
}
