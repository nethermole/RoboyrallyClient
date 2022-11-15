using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    Game game;  //the game actually has the queue
    Utilities utilities;
    public GameObject arrowPrefab;
    BoardDrawer boardDrawer;

    GameObject scriptstore;
    BackendGameService backendGameService;

    // Start is called before the first frame update
    void Start()
    {
        scriptstore = GameObject.Find("scriptstore");
        backendGameService = scriptstore.GetComponentInChildren<BackendGameService>();

        utilities = new Utilities();

        boardDrawer = GameObject.Find("BoardDrawer").GetComponent<BoardDrawer>();
    }

    public void setGame(Game game)
    {
        this.game = game;
    }

    public void startPolling()
    {
        InvokeRepeating("GetViewUpdateList", 1, 1);
        InvokeRepeating("DoNextAction", 1f, .7f);
    }

    private void Update() { }

    // Update is called once per frame
    void DoNextAction()
    {
        if(game == null) {
            return;
        }
        if (game.HasNextAction())
        {
            ViewStep viewstep = game.GetNextViewstep();
            RobotMoveViewStep robotMoveViewStep = (RobotMoveViewStep)viewstep;
            Debug.Log("Doing Action: Moving from " + robotMoveViewStep.startPosition.x + "," + robotMoveViewStep.startPosition.y + " to " + robotMoveViewStep.endPosition.x + "," + robotMoveViewStep.endPosition.y);

            DoAction(robotMoveViewStep);
        }
    }

    void GetViewUpdateList()
    {
        if (game == null) { return; }
        DTOViewUpdateList dtoViewUpdateList = backendGameService.GetViewUpdateList(game.GetTurn());

        StartInfo startInfo = dtoViewUpdateList.startInfo;
        if (startInfo != null && !game.HasPlayersSet())
        {
            game.SetStartInfo(dtoViewUpdateList.startInfo);
            boardDrawer.DrawPlayers(game.GetPlayers());
        }

        List<RobotMoveViewStep> viewSteps = dtoViewUpdateList.viewSteps;
        if(viewSteps == null)
        {
            print("viewSteps was null in GetViewUpdateList()");
        } else if (viewSteps.Count > 0)
        {
            List<RobotMoveViewStep> gameSteps = new List<RobotMoveViewStep>();
            for (int i = 0; i < viewSteps.Count; i++)
            {
                gameSteps.Add(viewSteps[i]);
            }
            game.addRobotMoveViewSteps(gameSteps);
        }
        else
        {
            print("no viewSteps in GetViewUpdateList()");
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
