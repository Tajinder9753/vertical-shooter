using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public bool isPlayerBullet;
    public bool isEnemyBullet;
    public float damage;
    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction, float speed)
    {
        direction = direction.normalized;
        rb.linearVelocity = direction * speed;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
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
