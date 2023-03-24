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
        webRequest.url = "localhost:8080/board/0";

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
        webRequest.url = "localhost:8080/viewupdate/0/turn/0";

        webRequest.SendWebRequest();
        while (!webRequest.isDone) { }

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            print("webrequestConnectionError - GetStartInfo");
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
                StartInfo startInfo = JsonConvert.DeserializeObject<StartInfo>(rawJson);

                callback.players = startInfo.players;
                callback.startPosition = startInfo.startPosition;
            } else
            {
                print("localhost:8080/viewupdate/turn/" + " responded with no content");
            }

            yield return callback;
        }
    }

    public IEnumerator GetViewUpdateList(DTOViewUpdateList callback, int turn)
    {
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/viewupdate/0/turn/" + turn;

        webRequest.SendWebRequest();
        while (!webRequest.isDone) { }

        string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
        DTOViewUpdateList dtoViewUpdateList = JsonConvert.DeserializeObject<DTOViewUpdateList>(rawJson);

        callback.viewSteps = dtoViewUpdateList.viewSteps;
        callback.startInfo = dtoViewUpdateList.startInfo;

        yield return callback;
    }

}