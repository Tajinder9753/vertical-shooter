using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovementStats stats;
    private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;
    private Animator anim;

    //movement
    private Vector2 movement;
    private Vector2 moveVelocity;

    //shooting
    private bool isShooting = false;
    private float shootTimer = 0f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;

    //rotation
    private Vector2 lookMovement;

    //health
    private float currentHealth;

    //UI
    [SerializeField] private Slider healthBar;
    public enum MovementState { Idle, MovingRight, MoveingLeft }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = stats.maxHealth;
        healthBar.maxValue = stats.maxHealth;
        healthBar.value = stats.maxHealth;
    }

    private void Update()
    {
        Look();
        TurnCheck(movement);
    }

    void FixedUpdate()
    {
        Move(stats.acceleration, stats.deceleration, movement);

        // Continuous firing while button is held. firingRate is shots per second.
        if (isShooting)
        {
            shootTimer += Time.fixedDeltaTime;
            float interval = 1f / stats.firingRate;
            while (shootTimer >= interval)
            {
                Shoot();
                shootTimer -= interval;
            }
        }
    }

    #region Input Callbacks


    public void onMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void onShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // start firing and shoot immediately
            isShooting = true;
            Shoot();
            shootTimer = 0f;
        }
        else if (context.canceled)
        {
            // stop firing when button released
            isShooting = false;
            shootTimer = 0f;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookMovement = context.ReadValue<Vector2>();
    }

    #endregion

    #region Movement Logic
    
    void Move(float acceleration, float deceleration, Vector2 moveInput)
    {

        if(moveInput != Vector2.zero)
        {
            Vector2 targetVelocity = moveInput * stats.moveSpeed;

            moveVelocity = Vector2.Lerp(movement, targetVelocity, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = moveVelocity;
        }
        else
        {
            moveVelocity = Vector2.Lerp(movement, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = moveVelocity;
        }
    }

    void TurnCheck(Vector2 moveInput)
    {
        Vector2 localInput = transform.InverseTransformDirection(moveInput);

        if (localInput.x > 0.1f)
        {
            SetMovementState(MovementState.MovingRight);
        }
        else if (localInput.x < -0.1f)
        {
            SetMovementState(MovementState.MoveingLeft);
        }
        else
        {
            SetMovementState(MovementState.Idle);
        }
    }

    void SetMovementState(MovementState state)
    {
        anim.SetInteger("MovementState", (int)state);
    }

    #endregion

    #region Look Logic
    void Look()
    {
        Vector3 direction = Vector3.zero;

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(lookMovement.x, lookMovement.y, -mainCamera.transform.position.z));
        direction = mouseWorld - transform.position;
        direction.z = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    #endregion

    #region Shooting Logic
    void Shoot()
    {
        var b =  Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        b.GetComponent<Bullet>().isPlayerBullet = true;
        b.GetComponent<Bullet>().Fire(transform.up, stats.bulletSpeed);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {

    }

    void Die()
    {
        // Handle player death (e.g., respawn, game over screen)
        Debug.Log("Player has died.");
    }

    #endregion

    #region Reward
    public void GetScore(float score)
    {

    }

    public void GetHealth(float health)
    {
        currentHealth = Mathf.Min(currentHealth + health, stats.maxHealth);
        healthBar.value = currentHealth;
    }

    #endregion
}
