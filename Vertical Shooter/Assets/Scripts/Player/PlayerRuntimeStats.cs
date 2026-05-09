using UnityEngine;


public class PlayerRuntimeStats : MonoBehaviour
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
