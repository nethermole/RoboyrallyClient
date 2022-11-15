using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrypoint : MonoBehaviour
{
    Game game;
    Utilities utilities;
    BoardDrawer boardDrawer;

    GameObject scriptstore;
    BackendGameService backendGameService;

    bool setupComplete;

    int interval = 1;
    float nextTime = 0;


    GameObject actionQueueGameObject;
    ActionQueue actionQueue;


    void Start()
    {
        game = new Game(this);

        setupComplete = false;
        scriptstore = GameObject.Find("scriptstore");
        backendGameService = scriptstore.GetComponentInChildren<BackendGameService>();
        boardDrawer = GameObject.Find("BoardDrawer").GetComponentInChildren<BoardDrawer>();

        actionQueueGameObject = GameObject.Find("ActionQueue");
        actionQueue = actionQueueGameObject.GetComponentInChildren<ActionQueue>();

        utilities = new Utilities();


        Invoke("IOSetup", 2);

        Invoke("ActionQueueStart", 3);
    }

    public void Log(string output)
    {
        print(output);
    }

    void IOSetup()
    {
        actionQueue.setGame(game);

        DTOBoard board = backendGameService.GetBoard();
        boardDrawer.board = board;
        boardDrawer.DrawBoard();

        setupComplete = true;
    }

    void ActionQueueStart()
    {
        actionQueue.startPolling();
    }
}
