using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    Dictionary<int, Player> playerMap;
    Queue<RobotMoveViewStep> viewSteps;
    int turn;
    bool playersSet;

    public Game()
    {
        turn = 0;
        playersSet = false;
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

    public bool HasPlayersSet()
    {
        return playersSet;
    }

    public void SetStartInfo(StartInfo startInfo)
    {
        playerMap = new Dictionary<int, Player>();
        for (int i = 0; i < startInfo.playerCount; i++)
        {
            Player player = new Player(new Position(startInfo.startPosition));
            playerMap.Add(i, player);
        }
        playersSet = true;
    }

    public Player GetPlayer(int id)
    {
        return playerMap[id];
    }

    public List<Player> GetPlayers()
    {
        List<Player> players = new List<Player>();
        foreach(Player player in playerMap.Values)
        {
            players.Add(player);
        }
        return players;
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
