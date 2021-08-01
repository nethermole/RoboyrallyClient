using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    Dictionary<int, Player> playerMap;
    Queue<RobotMoveViewStep> viewSteps;
    int turn;

    public Game()
    {
        turn = 0;
        viewSteps = new Queue<RobotMoveViewStep>();
    }

    public bool HasNextAction()
    {
        return viewSteps.Count > 0;
    }

    public ViewStep GetNextViewstep()
    {
        ViewStep viewstep = viewSteps.Dequeue();
        return viewstep;
    }

    //temporary
    public void SetPlayerCount(int number)
    {
        playerMap = new Dictionary<int, Player>();
        for(int i = 0; i < number; i++)
        {
            Player player = new Player();
            playerMap.Add(i, player);
        }
    }

    public Player GetPlayer(int id)
    {
        return playerMap[id];
    }

    public int GetTurn()
    {
        return turn;
    }

    public void addRobotMoveViewSteps(List<RobotMoveViewStep> steps)
    {
        if (steps != null)
        {
            foreach (RobotMoveViewStep robotMoveViewStep in steps)
            {
                viewSteps.Enqueue(robotMoveViewStep);
            }
            turn++;
        }
    }
}
