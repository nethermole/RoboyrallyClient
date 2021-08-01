using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerGetter : MonoBehaviour
{
    bool needsUpdate;
    bool canDrawPlayers;
    List<Player> players;
    public GameObject arrowPrefab;

    // Start is called before the first frame update
    void Start()
    {
        needsUpdate = true;
        canDrawPlayers = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (needsUpdate)
        {
            StartCoroutine(GetPlayerInfo());
            needsUpdate = false;
            canDrawPlayers = false;
        }

        if (canDrawPlayers)
        {
            canDrawPlayers = false;
        }
    }

    IEnumerator GetPlayerInfo()
    {
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/player";
        yield return webRequest.SendWebRequest();

        string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
        players = JsonConvert.DeserializeObject<List<Player>>(rawJson);
        canDrawPlayers = true;
    }

}
