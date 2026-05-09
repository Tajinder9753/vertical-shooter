using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

public class DataPersistanceManager : MonoBehaviour
{
    [SerializeField] private string fileName;

    private GameData gameData;
    public static DataPersistanceManager instance { get; private set; }

    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = Object
            .FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistance>();

        return dataPersistanceObjects.ToList();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data found");
            NewGame();
        }

        foreach (IDataPersistance dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(gameData);
        }

        Debug.Log("Loaded current health: " + gameData.currentHealth);
    }

    public void SaveGame()
    {
        foreach (IDataPersistance dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.SaveData(ref gameData);
        }

        Debug.Log("Saved current health: " + gameData.currentHealth);

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
