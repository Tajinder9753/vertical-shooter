using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Click_Manager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] Button loadButton;
     void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main_Menu")
        {
            if (DataPersistanceManager.instance.HasSavedData())
            {
                loadButton.interactable = true;
            }
            else
            {
                loadButton.interactable = false;
            }
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }

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
        PlayerRuntimeStats.Instance.currentScene = SceneManager.GetActiveScene().name;
        PlayerRuntimeStats.Instance.currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(PlayerRuntimeStats.Instance.currentScene);
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

    public void NextLevel()
    {
        PlayerRuntimeStats.Instance.currentLevelIndex++;
        if (PlayerRuntimeStats.Instance.currentLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            PlayerRuntimeStats.Instance.currentScene = SceneManager.GetSceneAt(PlayerRuntimeStats.Instance.currentLevelIndex).name;
            SceneManager.LoadScene(PlayerRuntimeStats.Instance.currentLevelIndex);
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        DataPersistanceManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        DataPersistanceManager.instance.LoadGame();
        SceneManager.LoadScene(PlayerRuntimeStats.Instance.currentScene);
    }

    public void NewGame()
    {
        PlayerRuntimeStats.Instance.InitializeStats();
        DataPersistanceManager.instance.NewGame();
        PlayerRuntimeStats.Instance.currentScene = "Level_1";
        SceneManager.LoadScene("Level_1");
    }
}
