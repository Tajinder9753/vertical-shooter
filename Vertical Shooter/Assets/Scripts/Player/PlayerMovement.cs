using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovementStats stats;
    private Rigidbody2D rb;

    //movement
    private Vector2 movement;
    private Vector2 moveVelocity;

    //shooting
    private bool isShooting = false;
    private float shootTimer = 0f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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

    #endregion

    #region Shooting Logic
    void Shoot()
    {
        var b =  Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        b.GetComponent<Bullet>().isPlayerBullet = true;
        b.GetComponent<Bullet>().Fire(Vector2.up, stats.bulletSpeed);
    }

    #endregion
}
