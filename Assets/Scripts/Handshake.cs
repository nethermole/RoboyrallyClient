using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Handshake : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(GetHandshake());
    }

    void GetHandshake() //IEnumerator GetHandshake()
    {
        /*
        //do this on
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/handshake/view";
        yield return webRequest.SendWebRequest();

        string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
        HandshakeResponse handshakeResponse = JsonConvert.DeserializeObject<HandshakeResponse>(rawJson);
        */
    }
}
