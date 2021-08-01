using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RobotMoveViewStep : ViewStep
{
    public int robotId;
    public Position startPosition;
    public Position endPosition;
    public Direction startFacing;
    public Direction endFacing;
    MovementMethod movementMethod;

    public int getRobotId()
    {
        return robotId;
    }

    public Position getEndPosition()
    {
        return endPosition;
    }
}
