using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public int gameScene = 1;

    RestClient restClient;

    void Start()
    {
        restClient = RestClient.instance;
    }

    public void StartBotGame()
    {
        string gameUUID = "";
        StartCoroutine(restClient.StartBotGame());

        SceneManager.LoadScene(gameScene);
    }

    public void StartPlayerGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void StartBotCivGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void StartCivGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
