using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementStats", menuName = "Scriptable Objects/PlayerMovementStats")]
public class PlayerMovementStats : ScriptableObject
{
    //player movement
    public float acceleration;
    public float deceleration;
    public float moveSpeed;

    //bullet firing
    public float firingRate;
    public float bulletSpeed;
}
