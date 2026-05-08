using UnityEngine;

public class Pickup : MonoBehaviour
{

    public enum  PickupType
    {
        Healing,
        Score
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float rewardAmount;

    public void GetReward()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        switch (pickupType)
        {
            case PickupType.Healing:
                player.GetHealth(rewardAmount);
                break;
            case PickupType.Score:
                player.GetScore(rewardAmount);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetReward();
            Destroy(gameObject);
        }
    }
}
