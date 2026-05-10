using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Click_Manager : MonoBehaviour, IDataPersistance
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] string currentScene;
    private int currentLevelIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

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
        currentScene = SceneManager.GetActiveScene().name;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
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
        currentLevelIndex++;
        if (currentLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            currentScene = SceneManager.GetSceneAt(currentLevelIndex).name;
            SceneManager.LoadScene(currentLevelIndex);
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
        SceneManager.LoadScene(currentScene);
    }

    public void NewGame()
    {
        PlayerRuntimeStats.Instance.InitializeStats();
        DataPersistanceManager.instance.NewGame();
        currentScene = "Level_1";
        SceneManager.LoadScene("Level_1");
    }

    public void LoadData(GameData data)
    {
        this.currentScene = data.currentScene;
    }

    public void SaveData(ref GameData data)
    {
        data.currentScene = this.currentScene;
    }
}
