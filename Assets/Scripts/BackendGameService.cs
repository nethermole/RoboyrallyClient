using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendGameService : MonoBehaviour
{
    GameObject scriptstore;
    RestClient restClient;

    public bool setupComplete = false;

    public bool Ready()
    {
        return setupComplete;
    }

    // Start is called before the first frame update
    void Start()
    {
        scriptstore = GameObject.Find("scriptstore");
        restClient = scriptstore.GetComponent<RestClient>();
        setupComplete = true;
    }

    public DTOBoard GetBoard()
    {
        DTOBoard callback = new DTOBoard();
        StartCoroutine(restClient.GetBoardInfo(callback));
        return callback;
    }

    public StartInfo GetStartInfo()
    {
        StartInfo callback = new StartInfo();
        StartCoroutine(restClient.GetStartInfo(callback));
        return callback;
    }

    public DTOViewUpdateList GetViewUpdateList(int turn)
    {
        DTOViewUpdateList callback = new DTOViewUpdateList();
        StartCoroutine(restClient.GetViewUpdateList(callback, turn));
        return callback;
    }

}
