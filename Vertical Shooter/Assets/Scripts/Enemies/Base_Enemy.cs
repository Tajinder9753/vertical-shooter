using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Base_Enemy : MonoBehaviour
{
    public GameObject target;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float bulletSpeed;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float health;
    Vector2 direction; //direction for looking also distance checking for shooting
    [SerializeField] float shootingDistance;
    [SerializeField] float firingRate;

    private float shootTimer = 0f;
    private void Awake()
    {
        target = FindFirstObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();

        if (direction.magnitude < shootingDistance)
        {
            shootTimer += Time.deltaTime;
            float interval = 1f / firingRate;
            if (shootTimer >= interval)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = 0f;
        }
    }

    void Move()
    {
        rb.position = Vector2.MoveTowards(rb.position, target.transform.position, speed * Time.deltaTime);
        direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void TakeDamage( float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        var b = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var bullet = b.GetComponent<Bullet>();
        bullet.isEnemyBullet = true;
        bullet.Fire(-transform.up, bulletSpeed);
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet.isPlayerBullet)
            {
                TakeDamage(bullet.damage);
                Destroy(collision.gameObject);
            }
                
        }
    }
}
