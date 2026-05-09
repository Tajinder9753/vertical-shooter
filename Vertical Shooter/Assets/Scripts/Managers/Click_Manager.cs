using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Click_Manager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;


    public void OnPause(InputAction.CallbackContext context)
    {

        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            return;
        } else
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }

    public void NewGame()
    {
        PlayerRuntimeStats.Instance.InitializeStats();
        SceneManager.LoadScene("Level_1");
    }
}
