using UnityEngine;


public class PlayerRuntimeStats : MonoBehaviour, IDataPersistance
{
    [SerializeField] private PlayerMovementStats playerMovementStats;

    //player movement
    public float acceleration;
    public float deceleration;
    public float moveSpeed;

    //bullet firing
    public float firingRate;
    public float bulletSpeed;

    //health
    public float maxHealth;
    public float currentHealth;

    //scene information
    public string currentScene;
    public int currentLevelIndex;
    public bool completedLevel;

    //Interface implementation

    public void LoadData(GameData data)
    {
        this.currentHealth = data.currentHealth;
        this.acceleration = data.acceleration;
        this.deceleration = data.deceleration;
        this.moveSpeed = data.moveSpeed;
        this.firingRate = data.firingRate;
        this.bulletSpeed = data.bulletSpeed;
        this.maxHealth = data.maxHealth;
        this.currentScene = data.currentScene;
    }

    public void SaveData(ref GameData data)
    {
        data.currentHealth = this.currentHealth;
        data.acceleration = this.acceleration;
        data.deceleration= this.deceleration;
        data.firingRate = this.firingRate;
        data.moveSpeed = this.moveSpeed;
        data.bulletSpeed = this.bulletSpeed;
        data.maxHealth = this.maxHealth;
        data.currentScene = this.currentScene;
    }


    public void InitializeStats()
    {
        acceleration = playerMovementStats.acceleration;
        deceleration = playerMovementStats.deceleration;
        moveSpeed = playerMovementStats.moveSpeed;
        firingRate = playerMovementStats.firingRate;
        bulletSpeed = playerMovementStats.bulletSpeed;
        maxHealth = playerMovementStats.maxHealth;
        currentHealth = maxHealth;
    }
    public static PlayerRuntimeStats Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeStats();
        }
    }
}
