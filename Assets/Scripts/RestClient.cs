using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Assets.Scripts;
using System.Collections.Generic;

class RestClient : MonoBehaviour
{
    public IEnumerator GetBoardInfo(DTOBoard callback)
    {
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/board";

        webRequest.SendWebRequest();
        while (!webRequest.isDone) { }


        string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
        DTOBoard board = JsonConvert.DeserializeObject<DTOBoard>(rawJson);

        //todo: find elegant way around callback pattern, once it works
        if(board != null)
        {
            callback.squares = board.squares;
        }
        yield return callback;
    }

    public IEnumerator GetStartInfo(StartInfo callback)
    {
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/viewupdate/turn/";

        webRequest.SendWebRequest();
        while (!webRequest.isDone) { }

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            print("ugh");
        }
        else
        {
            if (webRequest.downloadHandler.data.Length != 0)
            {
                string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
                try
                {
                    JObject jobject = JObject.Parse(rawJson);
                }
                catch (System.Exception e)
                {
                    print("failed to parse json:\n" + rawJson);
                }
            } else
            {
                print("localhost:8080/viewupdate/turn/" + " responded with no content");
            }

            callback.playerCount = 2;
            callback.startPosition = new Position(5, 5);

            yield return callback;
        }
    }

    public IEnumerator GetViewUpdateList(DTOViewUpdateList callback, int turn)
    {
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/viewupdate/turn/" + turn;

        webRequest.SendWebRequest();
        while (!webRequest.isDone) { }

        string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
        DTOViewUpdateList dtoViewUpdateList = JsonConvert.DeserializeObject<DTOViewUpdateList>(rawJson);

        callback.viewSteps = dtoViewUpdateList.viewSteps;
        callback.startInfo = dtoViewUpdateList.startInfo;

        yield return callback;
    }

    public IEnumerator GetPlayerInfo(List<Player> callback)
    {
        if(callback.Count != 0)
        {
            print("callback did not start empty in RestClient.GetPlayerInfo. Get off this callback paradigm");
        }

        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/player";

        webRequest.SendWebRequest();
        while (!webRequest.isDone) { }

        if (webRequest.isError)
        {
            print("ugh)");
        }
        else
        {
            string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
            List<Player> players = JsonConvert.DeserializeObject<List<Player>>(rawJson);
            foreach(Player player in players)
            {
                callback.Add(player);
            }
        }
        yield return callback;
    }
}