using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public float currentHealth;
    public float acceleration;
    public float deceleration;
    public float moveSpeed;
    public float firingRate;
    public float bulletSpeed;
    public float maxHealth;
    public Vector2 playerPos;
    public string currentScene;
    public Serializable_Dictionary<string, bool> zonesCleared;
    public string currentCameraZone;
    public GameData()
    {
        this.currentHealth = 100;
        this.acceleration = 12;
        this.deceleration = 8;
        this.moveSpeed = 25;
        this.firingRate = 5;
        this.bulletSpeed = 8;
        this.maxHealth = 100;
        this.playerPos = Vector2.zero;
        this.currentScene = "Level_1";
        this.zonesCleared = new Serializable_Dictionary<string, bool>();
    }
}
