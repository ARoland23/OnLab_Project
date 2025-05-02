using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage;
    public int Damage { get { return damage; } }

    private float timer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootBullet(Transform barrelEnd)
    {
        timer = 0;
        rb.linearVelocity = Vector2.zero;
        transform.position = barrelEnd.position;
        transform.rotation = barrelEnd.rotation;

        gameObject.SetActive(true);

        rb.AddForce(-transform.up * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyBase eb = collision.gameObject.GetComponent<EnemyBase>();
        if (eb != null)
        {
            eb.RecieveDamage(damage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
            gameObject.SetActive(false);
    }
}
