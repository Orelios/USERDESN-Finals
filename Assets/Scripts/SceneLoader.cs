using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string TitleSceneString = "TitleScreen";
    [SerializeField] private string StartingSceneString = "Tutorial";
    [SerializeField] private string GameOverScreenString = "GameOverScreen";

    public void PlayGame()
    {
        GoToSpecifiedScene(StartingSceneString);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Player has exit the game");
    }

    public void GoToTitle()
    {
        GoToSpecifiedScene(TitleSceneString);
    }

    public void GameOver()
    {
        GoToSpecifiedScene(GameOverScreenString);
    }

    public void PlayNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void GoToSpecifiedScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReplayCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
