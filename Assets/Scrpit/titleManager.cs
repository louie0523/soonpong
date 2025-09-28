using UnityEngine;
using UnityEngine.SceneManagement;

public class titleManager : MonoBehaviour
{
    public void GameGo()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
