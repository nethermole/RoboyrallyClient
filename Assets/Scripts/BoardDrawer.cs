using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

public class BoardDrawer : MonoBehaviour
{
    GameObject scriptstore;
    BackendGameService backendGameService;

    public DTOBoard board;

    public GameObject cubePrefab;
    public GameObject tilePrefab;
    public GameObject arrowPrefab;
    public GameObject waypointPrefab;

    public Texture gearcTexture;
    public Texture gearccTexture;

    public Texture wrench1Texture;
    public Texture wrench2Texture;

    public Texture wallUpTexture;
    public Texture wallRightTexture;
    public Texture wallDownTexture;
    public Texture wallLeftTexture;

    public Texture laser_v;
    public Texture laser_h;

    public Texture conveyor1ur;
    public Texture conveyor1ud;
    public Texture conveyor1ul;
    public Texture conveyor1rd;
    public Texture conveyor1rl;
    public Texture conveyor1ru;
    public Texture conveyor1dl;
    public Texture conveyor1du;
    public Texture conveyor1dr;
    public Texture conveyor1lu;
    public Texture conveyor1lr;
    public Texture conveyor1ld;

    public Texture conveyor2ur;
    public Texture conveyor2ud;
    public Texture conveyor2ul;
    public Texture conveyor2rd;
    public Texture conveyor2rl;
    public Texture conveyor2ru;
    public Texture conveyor2dl;
    public Texture conveyor2du;
    public Texture conveyor2dr;
    public Texture conveyor2lu;
    public Texture conveyor2lr;
    public Texture conveyor2ld;

    public Texture conveyor3ur;
    public Texture conveyor3ud;
    public Texture conveyor3ul;
    public Texture conveyor3rd;
    public Texture conveyor3rl;
    public Texture conveyor3ru;
    public Texture conveyor3dl;
    public Texture conveyor3du;
    public Texture conveyor3dr;
    public Texture conveyor3lu;
    public Texture conveyor3lr;
    public Texture conveyor3ld;

    public Texture beacon;
    public Texture checkpoint1;

    public Dictionary<int, Dictionary<int, Dictionary<string, GameObject>>> gameObjectReferences;

    // Start is called before the first frame update
    void Start()
    {
        scriptstore = GameObject.Find("scriptstore");
        backendGameService = scriptstore.GetComponentInChildren<BackendGameService>();

        gameObjectReferences = new Dictionary<int, Dictionary<int, Dictionary<string, GameObject>>>();
        for(int x = 0; x < 12; x++)
        {
            gameObjectReferences.Add(x, new Dictionary<int, Dictionary<string, GameObject>>());
            for(int y = 0; y < 12; y++)
            {
                gameObjectReferences[x].Add(y, new Dictionary<string, GameObject>());
            }
        }
    }

    // Update is called once per frame
    void Update(){}

    public void DrawBoard()
    {
        foreach(int x in board.squares.Keys)
        {
            foreach(int y in board.squares[x].Keys)
            {

                if (!gameObjectReferences.ContainsKey(x))
                {
                    gameObjectReferences[x] = new Dictionary<int, Dictionary<string, GameObject>>();
                }
                if (!gameObjectReferences[x].ContainsKey(y))
                {
                    gameObjectReferences[x][y] = new Dictionary<string, GameObject>();
                }

                Tile tile = board.squares[x][y];

                bool drawFloor = true;
                foreach (Element element in tile.elements)
                {
                    if(element.elementEnum == "PIT") {
                        drawFloor = false;
                        DrawPit(x, y);
                    }
                    else
                    {
                        DrawElement(x, y, element);
                    }
                }
                if (drawFloor)
                {
                    DrawFloor(x, y);
                }
            }
        }
    }

    public void DrawElement(int x, int y, Element element)
    {
        if (element.elementEnum == "BEACON")
        {
            Debug.Log("drawing beacon at " + x + ", " + y);
            //GameObject waypoint = Instantiate(waypointPrefab, new Vector3(x, 2, y), Quaternion.Euler(new Vector3(Quaternion.identity.eulerAngles.x, Quaternion.identity.eulerAngles.y, Quaternion.identity.eulerAngles.z + 180f)));
            //waypoint.GetComponent<MeshRenderer>().materials[0].color = Color.red;
        }

        if (element.elementEnum == "CHECKPOINT1")
        {
            Debug.Log("drawing checkpoint at " + x + ", " + y);
            //GameObject waypoint = Instantiate(waypointPrefab, new Vector3(x, 2, y), Quaternion.Euler(new Vector3(Quaternion.identity.eulerAngles.x, Quaternion.identity.eulerAngles.y, Quaternion.identity.eulerAngles.z + 180f)));
            //waypoint.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        }

        //walls
        if (element.elementEnum == "WALL_UP" ||
            element.elementEnum == "WALL_RIGHT" ||
            element.elementEnum == "WALL_DOWN" ||
            element.elementEnum == "WALL_LEFT")
        {
            DrawWall(element);
            return;
        }

        //things that arent walls
        Texture texture = getTexture(element.elementEnum);
        if (texture != null) {
            //float height = 0.5f + (0.01f * gameObjectReferences[x][y].Count - 1);   //block + tiles
            Vector3 position = new Vector3(x, 0.55f, y);
            GameObject elementGameObject = Instantiate(tilePrefab, position, Quaternion.Euler(new Vector3(Quaternion.identity.eulerAngles.x, Quaternion.identity.eulerAngles.y, Quaternion.identity.eulerAngles.z + 180f)));

            gameObjectReferences[x][y].Add(element.elementEnum, elementGameObject);
       

            elementGameObject.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", texture);
        } else
        {
            Debug.Log("Unable to load texture for: " + element.elementEnum);
        }
    }

    public void DrawWall(Element element)
    {
        //TODO
    }

    public void DrawFloor(int x, int y)
    {
        Vector3 position = new Vector3(x, 0, y);
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        if (!gameObjectReferences[x][y].ContainsKey("floor")){
            gameObjectReferences[x][y].Add("floor", cube);

            float randomFloat = Random.Range(.3f, 0.7f);
            Color cubeColor = new Color(randomFloat, randomFloat, randomFloat);
            cube.GetComponent<Renderer>().material.SetColor("_Color", cubeColor);

            cube.GetComponentInChildren<TextMesh>().text = "" + x + "," + y;
        }


    }

    public void DrawPit(int x, int y)
    {
        Vector3 position = new Vector3(x, 0, y);
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        if (gameObjectReferences[x][y] == null)
        {
            gameObjectReferences[x][y].Add("pit", cube);
        }

        cube.GetComponent<Renderer>().material.color = Color.red;

        cube.GetComponentInChildren<TextMesh>().text = "" + x + "," + y;
        cube.GetComponentInChildren<TextMesh>().color = Color.black;
    }


    public List<Player> DrawPlayers(List<Player> players)
    {
        foreach (Player player in players)
        {
            Vector3 position = new Vector3(player.position.x, 1, player.position.y);
            GameObject robot = Instantiate(arrowPrefab, position, Quaternion.identity);
            player.robotPiece = robot;
        }

        return players;
    }

    private Texture getTexture(string elementEnum)
    {
        switch (elementEnum)
        {
            case "GEAR_C":
                return gearcTexture;
            case "GEAR_CC":
                return gearccTexture;

            case "WRENCH_1":
                return wrench1Texture;
            case "WRENCH_2":
                return wrench2Texture;

            case "LASER_V":
                return laser_v;
            case "LASER_H":
                return laser_h;

            case "WALL_UP":
                return wallUpTexture;
            case "WALL_RIGHT":
                return wallRightTexture;
            case "WALL_DOWN":
                return wallDownTexture;
            case "WALL_LEFT":
                return wallLeftTexture;

            case "CONVEYOR_1_UP_RIGHT":
                return conveyor1ur;
            case "CONVEYOR_1_UP_DOWN":
                return conveyor1ud;
            case "CONVEYOR_1_UP_LEFT":
                return conveyor1ul;

            case "CONVEYOR_1_RIGHT_DOWN":
                return conveyor1rd;
            case "CONVEYOR_1_RIGHT_LEFT":
                return conveyor1rl;
            case "CONVEYOR_1_RIGHT_UP":
                return conveyor1ru;

            case "CONVEYOR_1_DOWN_LEFT":
                return conveyor1dl;
            case "CONVEYOR_1_DOWN_UP":
                return conveyor1du;
            case "CONVEYOR_1_DOWN_RIGHT":
                return conveyor1dr;

            case "CONVEYOR_1_LEFT_UP":
                return conveyor1lu;
            case "CONVEYOR_1_LEFT_RIGHT":
                return conveyor1lr;
            case "CONVEYOR_1_LEFT_DOWN":
                return conveyor1ld;

            case "CONVEYOR_2_UP_RIGHT":
                return conveyor2ur;
            case "CONVEYOR_2_UP_DOWN":
                return conveyor2ud;
            case "CONVEYOR_2_UP_LEFT":
                return conveyor2ul;

            case "CONVEYOR_2_RIGHT_DOWN":
                return conveyor2rd;
            case "CONVEYOR_2_RIGHT_LEFT":
                return conveyor2rl;
            case "CONVEYOR_2_RIGHT_UP":
                return conveyor2ru;

            case "CONVEYOR_2_DOWN_LEFT":
                return conveyor2dl;
            case "CONVEYOR_2_DOWN_UP":
                return conveyor2du;
            case "CONVEYOR_2_DOWN_RIGHT":
                return conveyor2dr;

            case "CONVEYOR_2_LEFT_UP":
                return conveyor2lu;
            case "CONVEYOR_2_LEFT_RIGHT":
                return conveyor2lr;
            case "CONVEYOR_2_LEFT_DOWN":
                return conveyor2ld;

            case "CONVEYOR_3_UP_RIGHT":
                return conveyor3ur;
            case "CONVEYOR_3_UP_DOWN":
                return conveyor3ud;
            case "CONVEYOR_3_UP_LEFT":
                return conveyor3ul;

            case "CONVEYOR_3_RIGHT_DOWN":
                return conveyor3rd;
            case "CONVEYOR_3_RIGHT_LEFT":
                return conveyor3rl;
            case "CONVEYOR_3_RIGHT_UP":
                return conveyor3ru;

            case "CONVEYOR_3_DOWN_LEFT":
                return conveyor3dl;
            case "CONVEYOR_3_DOWN_UP":
                return conveyor3du;
            case "CONVEYOR_3_DOWN_RIGHT":
                return conveyor3dr;

            case "CONVEYOR_3_LEFT_UP":
                return conveyor3lu;
            case "CONVEYOR_3_LEFT_RIGHT":
                return conveyor3lr;
            case "CONVEYOR_3_LEFT_DOWN":
                return conveyor3ld;

            case "CHECKPOINT1":
                return checkpoint1;

            case "BEACON":
                return beacon;

            default:
                Debug.Log("Unknown element: " + elementEnum);
                return null;
        }
    }
}
