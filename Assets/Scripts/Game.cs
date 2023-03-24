using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    Dictionary<int, Player> playerMap;
    Queue<RobotMoveViewStep> viewSteps;
    Entrypoint logger;
    int turn;
    bool playersSet;

    public Game(Entrypoint entrypoint)
    {
        logger = entrypoint;
        turn = 0;
        playersSet = false;
        viewSteps = new Queue<RobotMoveViewStep>();
    }

    private void print(string output)
    {
        logger.Log(output);
    }

    public bool HasNextAction()
    {
        if (!playersSet)
        {
            return false;
        }
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
       print("Start coordinate: " + startInfo.startPosition.GetX() + ", " + startInfo.startPosition.GetY());
        playerMap = new Dictionary<int, Player>();
        foreach(Player player in startInfo.players)
        {
            playerMap[player.id] = player;
        }
        /*for (int i = 0; i < startInfo.players.Count; i++)
        {
            Player player = new Player(new Position(startInfo.startPosition));
            playerMap.Add(i, player);
        }*/
        playersSet = true;
        turn = 0;
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
