using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public bool isPlayerBullet;
    public bool isEnemyBullet;
    public float timeToLive;
    public float damage;
    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }
    public void Fire(Vector2 direction, float speed)
    {
        direction = direction.normalized;
        rb.linearVelocity = direction * speed;
    }

    //for barriers and other objects that should destroy the bullet on contact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            Destroy(gameObject);
        }
    }
}
