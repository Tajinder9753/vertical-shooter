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
    private Vector2 movement;
    private bool isShooting;
    private Vector2 moveVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(stats.acceleration, stats.deceleration, movement);
        if (isShooting) { Shoot(); }
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
            isShooting = true;
        }
        else if (context.canceled)
        {
            isShooting = false;
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

    void Shoot()
    {
        Debug.Log("Is Shooting!!");
    }

    #endregion
}
