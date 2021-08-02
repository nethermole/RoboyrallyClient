using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActionQueue : MonoBehaviour
{
    Game game;  //the game actually has the queue
    Utilities utilities;
    public GameObject arrowPrefab;
    BoardDrawer boardDrawer;

    // Start is called before the first frame update
    void Start()
    {
        int playerCount = 2;

        utilities = new Utilities();

        game = new Game();
        game.SetPlayerCount(playerCount);

        for(int i = 0; i < playerCount; i++)
        {
            GameObject robot = Instantiate(arrowPrefab, utilities.convertGameXYtoVector3(0, 0) + new Vector3(0, 1f, 0), Quaternion.identity);
            game.GetPlayer(i).SetRobotPiece(robot);
        }

        InvokeRepeating("StartUpdatePolling", 1f, .1f);
        InvokeRepeating("DoNextAction", 1f, .7f);

        boardDrawer = GameObject.Find("BoardGetter").GetComponent<BoardDrawer>();
    }

    private void Update() { }

    // Update is called once per frame
    void DoNextAction()
    {
        if (game.HasNextAction())
        {
            ViewStep viewstep = game.GetNextViewstep();
            RobotMoveViewStep robotMoveViewStep = (RobotMoveViewStep)viewstep;
            Debug.Log("Doing Action: Moving from " + robotMoveViewStep.startPosition.x + "," + robotMoveViewStep.startPosition.y + " to " + robotMoveViewStep.endPosition.x + "," + robotMoveViewStep.endPosition.y);

            DoAction(robotMoveViewStep);
        }
    }

    void StartUpdatePolling()
    {
        StartCoroutine(GetViewUpdateList());
    }

    IEnumerator GetViewUpdateList()
    {
        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.url = "localhost:8080/viewupdate/turn/" + game.GetTurn();
        yield return webRequest.SendWebRequest();

        string rawJson = Encoding.Default.GetString(webRequest.downloadHandler.data);
        JObject jobject = JObject.Parse(rawJson);
        if (jobject["viewSteps"].HasValues)
        {
            Debug.Log(rawJson + rawJson.Length);
            List<RobotMoveViewStep> gameSteps = new List<RobotMoveViewStep>();
            for (int i = 0; i < ((JArray)jobject["viewSteps"]).Count; i++)
            {
                string typename = (string)jobject["viewSteps"][i]["typeName"];
                switch (typename)
                {
                    case "RobotMoveViewStep":
                        gameSteps.Add(JsonConvert.DeserializeObject<RobotMoveViewStep>((string)jobject["viewSteps"][i].ToString()));
                        break;
                    default:
                        throw new System.Exception("no type found for viewstep");
                }
            }
            game.addRobotMoveViewSteps(gameSteps);
        }
    }

    void DoAction(RobotMoveViewStep viewStep)
    {
        Player player = game.GetPlayer(viewStep.getRobotId());
        GameObject robot = player.robotPiece;

        //handle moving
        StartCoroutine(
            MoveRobotTo(robot, utilities.convertGameXYtoVector3(viewStep.getEndPosition().GetX(), viewStep.getEndPosition().GetY()) + new Vector3(0, 1f, 0), .25f));

        //handle rotating
        int degreesToRotateRight = determineRotationRight(player.GetFacing(), viewStep.endFacing);
        if(degreesToRotateRight != 0)
        {
            StartCoroutine(
                RotateRight(player.robotPiece, degreesToRotateRight, .25f));
        }
        player.facing = viewStep.endFacing;
    }

    public IEnumerator MoveRobotTo(GameObject robot, Vector3 newPosition, float duration)
    {
        Vector3 currentPosition = robot.gameObject.transform.position;
        float percentComplete = 0f;
        while(percentComplete < 1)
        {
            percentComplete += Time.deltaTime / duration;
            robot.gameObject.transform.position = Vector3.Lerp(currentPosition, newPosition, percentComplete);
            yield return null;
        }
    }

    private int determineRotationRight(Direction startFacing, Direction endFacing)
    {
        int change = endFacing - startFacing;
        change *= 90;
        if(change > 180)
        {
            change -= 180;
            change *= -1;
        }
        return change;
    }

    public IEnumerator RotateRight(GameObject robot, int degrees, float duration)
    {
        Quaternion targetRotation = robot.transform.rotation * Quaternion.AngleAxis(degrees, Vector3.up);

        float percentComplete = 0f;
        while(percentComplete < 1)
        {
            percentComplete += Time.deltaTime / duration;

            robot.transform.rotation = Quaternion.Lerp(robot.transform.rotation, targetRotation, percentComplete);
            yield return null;
        }
    }
}
