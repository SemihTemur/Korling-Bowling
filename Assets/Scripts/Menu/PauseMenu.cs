using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void MainMenu(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void RestartGame(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

}
