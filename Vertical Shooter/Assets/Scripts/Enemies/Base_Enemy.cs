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
    [SerializeField] float separationRadius;
    [SerializeField] float separationForce;
    private bool isActive = false;

    private float shootTimer = 0f;

    [SerializeField] private Zone_Manager myZone;

    Animator anim;

    private void Awake()
    {
        target = FindFirstObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        myZone = GetComponentInParent<Zone_Manager>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isActive) return;

        Seperate();
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

    void Seperate()
    {
        Collider2D[] neighbours = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        foreach(var neighbour in neighbours)
        {
            if (neighbour.gameObject == gameObject) continue;
            if (!neighbour.TryGetComponent<Base_Enemy>(out _)) continue;

            Vector2 pushDirection = (Vector2)(transform.position - neighbour.transform.position);

            // The closer they are the stronger the push
            float strength = 1f - (pushDirection.magnitude / separationRadius);
            rb.AddForce(pushDirection.normalized * strength * separationForce);
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
        anim.SetTrigger("Hurt");
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        myZone.EnemyDefeated();
        Destroy(gameObject);
    }

    void Shoot()
    {
        var b = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var bullet = b.GetComponent<Bullet>();
        bullet.isEnemyBullet = true;
        bullet.Fire(-transform.up, bulletSpeed);
    }

    public void SetActive(bool active)
    {
        isActive = active;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
